using System.ComponentModel.DataAnnotations;

namespace Hackathon.Service.MailServices;

public class MailOption
{
    [Required]public string Mail { get; set; }
    [Required]public string DisplayName { get; set; }
    [Required]public string Password { get; set; }
    [Required]public string Host { get; set; }
    [Required]public int Port { get; set; }
}
