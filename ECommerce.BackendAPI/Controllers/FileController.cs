using Microsoft.AspNetCore.Mvc;


namespace Ecommerce.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var url = HttpContext.Items["ImageUrl"] as string;

            return Ok($"File uploaded successfully. URL: {url}");
        }
    }
}