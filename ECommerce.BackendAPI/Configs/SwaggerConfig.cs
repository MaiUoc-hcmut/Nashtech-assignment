using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;


public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var parameters = context.ApiDescription.ParameterDescriptions
            .Where(p => p.ModelMetadata?.ContainerType != null &&
                        p.ModelMetadata.ContainerType.GetProperty(p.Name)?.PropertyType == typeof(IFormFile))
            .ToList();

        if (parameters.Any())
        {
            operation.RequestBody = new OpenApiRequestBody
            {
                Content = {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = parameters.ToDictionary(
                                p => p.Name,
                                p => new OpenApiSchema { Type = "string", Format = "binary" }
                            ),
                            Required = parameters.Select(p => p.Name).ToHashSet()
                        }
                    }
                }
            };

            // Remove the IFormFile parameters from the operation parameters list
            foreach (var param in parameters)
            {
                operation.Parameters.Remove(operation.Parameters.FirstOrDefault(p => p.Name == param.Name));
            }
        }
    }
}