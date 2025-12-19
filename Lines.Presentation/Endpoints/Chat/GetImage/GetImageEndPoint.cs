using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Chat.GetImage
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetImageEndPoint : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _uploadsFolder;
        private readonly string _imagesFolder;
        private readonly IConfiguration _configuration;

        public GetImageEndPoint(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _configuration = configuration;
            _uploadsFolder = _configuration["FileStorage:RootFolder"] ?? "uploads";
            _imagesFolder = _configuration["FileStorage:ImageFolder"] ?? "images";
        }

        [HttpGet("images/{*imageName}")]  
        public IActionResult GetImage(string imageName)
        {
            try
            {
                var fullPath = Path.Combine(_environment.ContentRootPath, _uploadsFolder, _imagesFolder, imageName);
                
                if (!System.IO.File.Exists(fullPath))
                {
                    return NotFound("Image not found");
                }

                var fileBytes = System.IO.File.ReadAllBytes(fullPath);
                var contentType = GetContentType(imageName);
                
                return File(fileBytes, contentType);
            }
            catch (Exception)
            {
                return BadRequest("Error retrieving image");
            }
        }

        private string GetContentType(string imagePath)
        {
            var extension = Path.GetExtension(imagePath).ToLowerInvariant();
            return extension switch
            {
                ".png" => "image/png",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }
    }
}
