using System.ComponentModel.DataAnnotations;

namespace Hackathon.Infrastructure.Services.Cloudinary;

public class CloudinaryOptions
{
    [Required] public string CloudName { get; set; } = null!;
    [Required] public string ApiKey { get; set; } = null!;
    [Required] public string ApiSecret { get; set; } = null!;
}
