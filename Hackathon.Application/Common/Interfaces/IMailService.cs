namespace Hackathon.Application.Common.Interfaces;

public interface IMailService
{
    Task SendMail(MailContent mailContent);
    Task SendVerificationEmailAsync(string to, string token);
    Task SendResetPasswordEmailAsync(string to, string token);
}

public class MailContent
{
    public required string To { get; set; } 
    public required string Subject { get; set; } 
    public required string Body { get; set; } 
}