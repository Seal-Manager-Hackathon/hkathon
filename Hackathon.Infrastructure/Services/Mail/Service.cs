using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Exceptions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Hackathon.Infrastructure.Services.Mail;

public class Service : IMailService
{
    private readonly MailOptions _mailOption;
    private readonly ILogger<Service> _logger;

    public Service(IOptions<MailOptions> mailOption, ILogger<Service> logger)
    {
        _mailOption = mailOption.Value;
        _logger = logger;
    }

    public async Task SendMail(MailContent mailContent)
    {
        try
        {
            MimeMessage email = new();
            email.Sender = new MailboxAddress(_mailOption.DisplayName, _mailOption.Mail);
            email.From.Add(new MailboxAddress(_mailOption.DisplayName, _mailOption.Mail));
            email.To.Add(MailboxAddress.Parse(mailContent.To));
            email.Subject = mailContent.Subject;

            BodyBuilder builder = new() { HtmlBody = mailContent.Body };
            email.Body = builder.ToMessageBody();

            using SmtpClient smtp = new();
            await smtp.ConnectAsync(_mailOption.Host, _mailOption.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailOption.Mail, _mailOption.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send mail to {To}", mailContent.To);
            throw new ServerException(ErrorMessage.Mail.SendFailed);
        }
    }

    public async Task SendVerificationEmailAsync(string to, string token)
    {
        var body = MailTemplate.EmailContainToken(token, _mailOption.FrontendBaseUrl);
        await SendMail(new MailContent
        {
            To = to,
            Subject = "Account Verification - SEAL Hackathon",
            Body = body
        });
    }

    public async Task SendResetPasswordEmailAsync(string to, string token)
    {
        var body = MailTemplate.ForgotPasswordContainToken(token, _mailOption.FrontendBaseUrl);
        await SendMail(new MailContent
        {
            To = to,
            Subject = "Reset Password - SEAL Hackathon",
            Body = body
        });
    }
}
