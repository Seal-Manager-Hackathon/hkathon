using System.ComponentModel.DataAnnotations;

namespace Hackathon.Service.JwtServices;

public class JwtOption
{
    [Required] public string Issuer { get; set; } = null!;
    [Required] public string Audience { get; set; } = null!;
    [Required] public string SecretKey { get; set; } = null!;
    [Required] public int ExpireMinutes { get; set; }
}
