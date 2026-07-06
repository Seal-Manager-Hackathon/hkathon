using System.ComponentModel.DataAnnotations;

namespace Hackathon.Infrastructure.Services.Mail;

public class MailOption
{
    [Required] public string Mail { get; set; } = null!;
    [Required] public string DisplayName { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
    [Required] public string Host { get; set; } = null!;
    [Required] public int Port { get; set; }
    [Required] public string FrontendBaseUrl { get; set; } = null!;
}
