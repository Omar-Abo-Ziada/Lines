using Lines.Application.Features.Chat;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Lines.Application.Features.Common.Commands
{
    public record UploadImageOrchestrator(IFormFile Image, string? FolderName = null) : IRequest<Result<string>>;

    public class UploadImageOrchestratorHandler : RequestHandlerBase<UploadImageOrchestrator, Result<string>>
    {
        private readonly ILogger<UploadImageOrchestratorHandler> _logger;
        private readonly string _uploadsPath;
        private readonly string _uploadsFolder;
        private readonly string _imagesFolder;
        private readonly long _maxFileSize = 100 * 1024 * 1024; // 100MB
        private readonly string[] _allowedExtensions = { ".png", ".jpg", ".jpeg", ".gif" };
        private readonly IConfiguration _configuration;
        public UploadImageOrchestratorHandler(RequestHandlerBaseParameters parameters, ILogger<UploadImageOrchestratorHandler> logger,
             IConfiguration configuration) : base(parameters)
        {
            _logger = logger;
            _configuration = configuration;

             _uploadsFolder = _configuration["FileStorage:RootFolder"] ?? "uploads";
            _imagesFolder = _configuration["FileStorage:ImageFolder"] ?? "images";
            _uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), _uploadsFolder);
            
            // Ensure uploads directory exists
            try
            {
                if (!Directory.Exists(_uploadsPath))
                {
                    Directory.CreateDirectory(_uploadsPath);
                    _logger.LogInformation("Created uploads directory: {UploadsPath}", _uploadsPath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create uploads directory: {UploadsPath}", _uploadsPath);
                throw;
            }
        }

        public override async Task<Result<string>> Handle(UploadImageOrchestrator request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate input parameters
                if (request.Image == null)
                {
                    return Result<string>.Failure(ChatErrors.InvalidImage("Image file is required"));
                }

                // Use provided folder name or fall back to configured folder
                var folderName = !string.IsNullOrWhiteSpace(request.FolderName) ? request.FolderName : _imagesFolder;
                
                if (string.IsNullOrWhiteSpace(folderName))
                {
                    return Result<string>.Failure(ChatErrors.InvalidImage("Folder name is required"));
                }

                // Validate image
                var validationResult = ValidateImage(request.Image);
                if (!validationResult.IsSuccess)
                {
                    return validationResult;
                }

                // Create folder if it doesn't exist
                var folderPath = Path.Combine(_uploadsPath, folderName);
                if (!Directory.Exists(folderPath))
                {
                    try
                    {
                        Directory.CreateDirectory(folderPath);
                        _logger.LogInformation("Created folder: {FolderPath}", folderPath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to create folder: {FolderPath}", folderPath);
                        return Result<string>.Failure(ChatErrors.FailedToUploadImage("Failed to create upload folder"));
                    }
                }

                // Generate unique filename
                var fileExtension = Path.GetExtension(request.Image.FileName).ToLowerInvariant();
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(folderPath, fileName);

                // Save file
                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await request.Image.CopyToAsync(stream, cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to save file: {FilePath}", filePath);
                    return Result<string>.Failure(ChatErrors.FailedToUploadImage("Failed to save image file"));
                }

                var relativePath = Path.Combine(_uploadsFolder, folderName, fileName).Replace("\\", "/");
                _logger.LogInformation("Image uploaded successfully: {FilePath}", relativePath);
                
                return Result<string>.Success(relativePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image: {FileName}", request.Image.FileName);
                return Result<string>.Failure(ChatErrors.FailedToUploadImage("Failed to upload image"));
            }
        }

        private Result<string> ValidateImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return Result<string>.Failure(ChatErrors.InvalidImage("Image is required"));
            }

            // Check file size
            if (image.Length > _maxFileSize)
            {
                return Result<string>.Failure(ChatErrors.InvalidImage($"Image size exceeds maximum allowed size: {image.Length / (1024 * 1024)}MB"));
            }

            // Check file extension
            var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(fileExtension))
            {
                return Result<string>.Failure(ChatErrors.InvalidImage($"Invalid file extension: {fileExtension}. Allowed extensions: {string.Join(", ", _allowedExtensions)}"));
            }

            // Check MIME type
            var allowedMimeTypes = new[] { "image/png", "image/jpeg", "image/jpg", "image/gif" };
            if (!allowedMimeTypes.Contains(image.ContentType.ToLowerInvariant()))
            {
                return Result<string>.Failure(ChatErrors.InvalidImage($"Invalid MIME type: {image.ContentType}"));
            }

            return Result<string>.Success(string.Empty);
        }
    }
}
