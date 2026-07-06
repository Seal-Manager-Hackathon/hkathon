using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hackathon.Infrastructure.Services.Cloudinary;

public class Service : IMediaService
{
    private readonly CloudinaryDotNet.Cloudinary _cloudinary;
    private readonly ILogger<Service> _logger;

    public Service(IOptions<CloudinaryOptions> cloudinaryOptions, ILogger<Service> logger)
    {
        _logger = logger;
        var options = cloudinaryOptions.Value;
        _cloudinary = new CloudinaryDotNet.Cloudinary(
            new Account(options.CloudName, options.ApiKey, options.ApiSecret)
        );
    }

    public async Task<string> UploadImageAsync(IFormFile file, string folder)
    {
        if (file == null || file.Length == 0)
        {
            throw new BadRequestException(ErrorMessage.Media.FileEmpty);
        }

        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        if (!allowedExtensions.Contains(fileExtension))
        {
            throw new BadRequestException(ErrorMessage.Media.InvalidImageFormat);
        }

        if (file.Length > 50 * 1024 * 1024)
        {
            throw new BadRequestException(ErrorMessage.Media.FileTooLarge);
        }

        try
        {
            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folder,
                UseFilename = true,
                UniqueFilename = true
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                _logger.LogError("Cloudinary upload failed: {Error}", uploadResult.Error.Message);
                throw new ServerException(ErrorMessage.Media.UploadFailed);
            }

            return uploadResult.SecureUrl.ToString();
        }
        catch (Exception ex) when (ex is not BadRequestException and not ServerException)
        {
            _logger.LogError(ex, "Cloudinary upload failed");
            throw new ServerException(ErrorMessage.Media.UploadFailed);
        }
    }
}
