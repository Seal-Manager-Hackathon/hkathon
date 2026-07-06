using System.ComponentModel.DataAnnotations;

namespace Hackathon.Service.Auths;

public class SecurityOption
{
    [Required]public string Pepper { get; set; } = null!;

}
