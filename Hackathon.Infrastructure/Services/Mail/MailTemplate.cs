namespace Hackathon.Infrastructure.Services.Mail;

public static class MailTemplate
{
    public static string EmailContainToken(string token, string baseUrl)
    {
        var verificationLink = $"{baseUrl}/verify-email?token={token}";

        var htmlBody = """
        <!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>Verify Your Account — SEAL Hackathon</title>
            <style>
                body { margin: 0; padding: 0; background-color: #f4f7f6; font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif; }
            </style>
        </head>
        <body style="margin: 0; padding: 0; background-color: #f4f7f6; font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Arial, sans-serif; color: #1e293b; -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%;">
            <div class="wrapper" style="width: 100%; padding: 48px 16px; box-sizing: border-box; background-color: #f4f7f6;">
                <div class="container" style="max-width: 540px; margin: 0 auto; background: #ffffff; border-radius: 20px; overflow: hidden; box-shadow: 0 10px 30px -5px rgba(0, 0, 0, 0.08); border: 1px solid #e2e8f0;">

                    <div class="hero" style="background-image: url('https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRxTgNHvpfGYUZWhyjWfYZANyWy1aFpf8bvDMHsIJnrRU1M5o0iCAqs-uU&s=10'); background-repeat: no-repeat; background-position: top center; background-size: cover; height: 220px; width: 100%;"></div>
                    <div class="body-content" style="padding: 40px 48px;">
                        <h1 style="margin: 0 0 8px; font-size: 28px; font-weight: 800; color: #0f172a; letter-spacing: -0.5px; line-height: 1.2;">Activate Your Account</h1>
                        <p class="subtitle" style="margin: 0 0 32px; color: #64748b; font-size: 16px; font-weight: 500;">You're almost there!</p>

                        <p style="margin: 0 0 18px; font-size: 15px; line-height: 1.7; color: #334155;">Hi there,</p>
                        <p style="margin: 0 0 18px; font-size: 15px; line-height: 1.7; color: #334155;">Thanks for signing up for <strong style="color: #0f172a;">SEAL Hackathon</strong>. We're excited to have you on board! Please verify your email address by clicking the button below to unlock all features.</p>

                        <div class="button-wrap" style="text-align: center; margin: 36px 0;">
                            <a href="{{verification_link}}" class="btn" style="display: inline-block; padding: 15px 44px; border-radius: 12px; background-color: #16a34a; background: linear-gradient(135deg, #22c55e 0%, #15803d 100%); color: #ffffff !important; text-decoration: none; font-weight: 600; font-size: 15px; letter-spacing: 0.2px; box-shadow: 0 4px 14px rgba(22, 163, 74, 0.35);">Verify Email</a>
                        </div>

                        <div class="note" style="background-color: #f0fdf4; border-radius: 12px; padding: 16px 20px; font-size: 13.5px; color: #166534; line-height: 1.6; border-left: 4px solid #16a34a;">
                            this link expires in <strong style="color: #14532d;">10 minutes</strong>. if you didn't create this account, please ignore this email.
                        </div>

                        <div class="divider" style="height: 1px; background-color: #e2e8f0; margin: 32px 0;"></div>

                        <p style="font-size: 14px; margin: 0; color: #64748b; text-align: center;">
                            Once you're verified, you can form a team, register for events, and start your hacking journey.
                        </p>
                    </div>

                    <div class="footer" style="padding: 32px 48px; background-color: #f8fafc; border-top: 1px solid #f1f5f9; text-align: center;">
                        <p style="margin: 0 0 6px; font-weight: 600; color: #64748b; font-size: 12px; letter-spacing: 0.1px;">SEAL Hackathon &mdash; Innovate. Build. Compete.</p>
                        <p style="margin: 0; color: #cbd5e1; font-size: 12px;">&copy; 2026 SEAL Hackathon. All rights reserved.</p>
                    </div>
                </div>
            </div>
        </body>
        </html>
        """;

        return htmlBody.Replace("{{verification_link}}", verificationLink);
    }

    public static string ForgotPasswordContainToken(string token, string baseUrl)
    {
        var resetPasswordLink = $"{baseUrl}/reset-password?token={token}";

        var htmlBody = """
        <!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>Reset Your Password — SEAL Hackathon</title>
            <style>
                body { margin: 0; padding: 0; background-color: #f4f7f6; font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif; }
            </style>
        </head>
        <body style="margin: 0; padding: 0; background-color: #f4f7f6; font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Arial, sans-serif; color: #1e293b; -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%;">
            <div class="wrapper" style="width: 100%; padding: 48px 16px; box-sizing: border-box; background-color: #f4f7f6;">
                <div class="container" style="max-width: 540px; margin: 0 auto; background: #ffffff; border-radius: 20px; overflow: hidden; box-shadow: 0 10px 30px -5px rgba(0, 0, 0, 0.08); border: 1px solid #e2e8f0;">

                    <div class="hero" style="background-image: url('https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRxTgNHvpfGYUZWhyjWfYZANyWy1aFpf8bvDMHsIJnrRU1M5o0iCAqs-uU&s=10'); background-repeat: no-repeat; background-position: top center; background-size: cover; height: 220px; width: 100%;"></div>
                    <div class="body-content" style="padding: 40px 48px;">
                        <h1 style="margin: 0 0 8px; font-size: 28px; font-weight: 800; color: #0f172a; letter-spacing: -0.5px; line-height: 1.2;">Reset Your Password</h1>
                        <p class="subtitle" style="margin: 0 0 32px; color: #64748b; font-size: 16px; font-weight: 500;">No worries, we've got you covered</p>

                        <p style="margin: 0 0 18px; font-size: 15px; line-height: 1.7; color: #334155;">Hi there,</p>
                        <p style="margin: 0 0 18px; font-size: 15px; line-height: 1.7; color: #334155;">We received a request to reset the password for your <strong style="color: #0f172a;">SEAL Hackathon</strong> account. If this was you, click the button below to create a new password.</p>

                        <div class="button-wrap" style="text-align: center; margin: 36px 0;">
                            <a href="{{reset_password_link}}" class="btn" style="display: inline-block; padding: 15px 44px; border-radius: 12px; background-color: #16a34a; background: linear-gradient(135deg, #22c55e 0%, #15803d 100%); color: #ffffff !important; text-decoration: none; font-weight: 600; font-size: 15px; letter-spacing: 0.2px; box-shadow: 0 4px 14px rgba(22, 163, 74, 0.35);">Reset Password</a>
                        </div>

                        <div class="note" style="background-color: #f0fdf4; border-radius: 12px; padding: 16px 20px; font-size: 13.5px; color: #166534; line-height: 1.6; border-left: 4px solid #16a34a;">
                            this link expires in <strong style="color: #14532d;">10 minutes</strong>. if you didn't request this, please ignore this email.
                        </div>

                        <div class="divider" style="height: 1px; background-color: #e2e8f0; margin: 32px 0;"></div>

                        <p style="font-size: 14px; margin: 0; color: #64748b; text-align: center;">
                            For security reasons, never share this link with anyone. The SEAL Hackathon team will never ask for your password.
                        </p>
                    </div>

                    <div class="footer" style="padding: 32px 48px; background-color: #f8fafc; border-top: 1px solid #f1f5f9; text-align: center;">
                        <p style="margin: 0 0 6px; font-weight: 600; color: #64748b; font-size: 12px; letter-spacing: 0.1px;">SEAL Hackathon &mdash; Innovate. Build. Compete.</p>
                        <p style="margin: 0; color: #cbd5e1; font-size: 12px;">&copy; 2026 SEAL Hackathon. All rights reserved.</p>
                    </div>
                </div>
            </div>
        </body>
        </html>
        """;

        return htmlBody.Replace("{{reset_password_link}}", resetPasswordLink);
    }
}
