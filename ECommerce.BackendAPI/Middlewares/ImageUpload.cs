using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;

public class UploadImagesForProductAndVariant
{
    private readonly RequestDelegate _next;
    private readonly Cloudinary _cloudinary;

    public UploadImagesForProductAndVariant(RequestDelegate next, IOptions<CloudinarySettings> cloudinaryConfig)
    {
        _next = next;
        var account = new Account(
            cloudinaryConfig.Value.CloudName,
            cloudinaryConfig.Value.ApiKey,
            cloudinaryConfig.Value.ApiSecret
        );
        _cloudinary = new Cloudinary(account);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.HasFormContentType && context.Request.Form.Files.Count > 0)
        {
            var uploadedUrls = new Dictionary<string, string>();
            foreach (var file in context.Request.Form.Files)
            {
                if (file.Length > 0)
                {
                    var fileNameParts = file.FileName.Split('!');
                    if (fileNameParts.Length < 3) 
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("Invalid file name format.");
                        return;
                    }

                    var prefix = fileNameParts[0];
                    var variantKey = fileNameParts[1];
                    var fileName = fileNameParts[2];

                    using var stream = file.OpenReadStream();
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(fileName, stream),
                        Folder = prefix 
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        if (prefix == "product")
                        {
                            uploadedUrls["product"] = uploadResult.SecureUrl.ToString();
                        }
                        else 
                        {
                            uploadedUrls[variantKey] = uploadResult.SecureUrl.ToString();
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        await context.Response.WriteAsync($"Image upload failed for file: {file.FileName}");
                        return;
                    }
                    
                }
            }
            context.Items["UploadedUrls"] = uploadedUrls;
            
        }
        await _next(context);
    }
}


public static class ImageUpload
{
    public static IApplicationBuilder UploadProducts(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UploadImagesForProductAndVariant>();
    }
}