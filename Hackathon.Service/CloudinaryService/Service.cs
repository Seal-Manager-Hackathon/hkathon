using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Hackathon.Service.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Hackathon.Service.MediaService;

namespace Hackathon.Service.CloudinaryService;

public class Service : IService
{
    private readonly Cloudinary _cloudinary;
    private readonly CloudinaryOptions _cloudinaryOptions = new();

    public Service(IConfiguration configuration)
    {
        configuration.GetSection(nameof(CloudinaryOptions)).Bind(_cloudinaryOptions);
        _cloudinary = new Cloudinary(
            new Account(
                _cloudinaryOptions.CloudName,
                _cloudinaryOptions.ApiKey,
                _cloudinaryOptions.ApiSecret
            )
        );
    }

    public async Task<string> UploadImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new BadRequestException("FILE_EMPTY", "File is empty.");
        }

        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        if (!allowedExtensions.Contains(fileExtension))
        {
            throw new BadRequestException("INVALID_IMAGE_FORMAT", "Only .jpg, .jpeg, .png, .gif, .webp are allowed.");
        }

        if (file.Length > 50 * 1024 * 1024)
        {
            throw new BadRequestException("FILE_TOO_LARGE", "Image must be less than 50MB.");
        }

        await using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "Avatar/hackathon",
            UseFilename = true,
            UniqueFilename = true
        };
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        return uploadResult.SecureUrl.ToString();
    }
}