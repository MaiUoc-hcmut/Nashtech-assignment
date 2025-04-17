using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;

public class CloudinaryMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Cloudinary _cloudinary;

    public CloudinaryMiddleware(RequestDelegate next, IOptions<CloudinarySettings> cloudinaryConfig)
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
            var file = context.Request.Form.Files[0];
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "products" // Optional: Specify a folder in Cloudinary
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Attach the URL to the request for further processing
                    context.Items["ImageUrl"] = uploadResult.SecureUrl.ToString();
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync("Image upload failed.");
                    return;
                }
            }
        }

        await _next(context);
    }
}


public static class ImageUpload
{
    public static IApplicationBuilder UseFileUpload(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CloudinaryMiddleware>();
    }
}