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
                body {
                    margin: 0;
                    padding: 0;
                    background: #f4f6fb;
                    font-family: 'Segoe UI', -apple-system, BlinkMacSystemFont, 'Helvetica Neue', Arial, sans-serif;
                    color: #1a1a2e;
                }
                .wrapper {
                    width: 100%;
                    padding: 40px 16px;
                    box-sizing: border-box;
                }
                .container {
                    max-width: 560px;
                    margin: 0 auto;
                    background: #ffffff;
                    border-radius: 16px;
                    overflow: hidden;
                    box-shadow: 0 4px 24px rgba(0, 0, 0, 0.06);
                }
                .hero {
                    background: linear-gradient(135deg, #1a1a2e 0%, #16213e 100%);
                    padding: 48px 40px 36px;
                    text-align: center;
                }
                .logo {
                    width: 56px;
                    height: 56px;
                    margin: 0 auto 20px;
                    background: linear-gradient(135deg, #0f3460, #533483);
                    border-radius: 14px;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    font-size: 24px;
                    font-weight: 800;
                    color: #ffffff;
                }
                .hero h1 {
                    margin: 0 0 6px;
                    font-size: 26px;
                    font-weight: 700;
                    color: #ffffff;
                    letter-spacing: -0.3px;
                }
                .hero p {
                    margin: 0;
                    color: #a8b2d1;
                    font-size: 15px;
                }
                .body-content {
                    padding: 40px;
                }
                .body-content p {
                    margin: 0 0 16px;
                    font-size: 15px;
                    line-height: 1.7;
                    color: #334155;
                }
                .button-wrap {
                    text-align: center;
                    margin: 32px 0;
                }
                .btn {
                    display: inline-block;
                    padding: 14px 40px;
                    border-radius: 10px;
                    background: linear-gradient(135deg, #0f3460, #533483);
                    color: #ffffff !important;
                    text-decoration: none;
                    font-weight: 600;
                    font-size: 15px;
                    letter-spacing: 0.3px;
                    box-shadow: 0 4px 14px rgba(83, 52, 131, 0.3);
                }
                .divider {
                    height: 1px;
                    background: #e2e8f0;
                    margin: 28px 0;
                }
                .note {
                    background: #f8fafc;
                    border: 1px solid #e2e8f0;
                    border-radius: 10px;
                    padding: 16px 20px;
                    font-size: 13px;
                    color: #64748b;
                    line-height: 1.6;
                }
                .note strong {
                    color: #334155;
                }
                .footer {
                    padding: 24px 40px;
                    background: #f8fafc;
                    border-top: 1px solid #e2e8f0;
                    text-align: center;
                }
                .footer p {
                    margin: 0;
                    color: #94a3b8;
                    font-size: 12px;
                    line-height: 1.8;
                }
                .footer a {
                    color: #533483;
                    text-decoration: none;
                }
            </style>
        </head>
        <body>
            <div class="wrapper">
                <div class="container">
                    <div class="hero">
                        <div class="logo">SH</div>
                        <h1>Activate Your Account</h1>
                        <p>You're almost there!</p>
                    </div>
                    <div class="body-content">
                        <p>Hi there,</p>
                        <p>Thanks for signing up for <strong>SEAL Hackathon</strong>. We're excited to have you on board! Please verify your email address by clicking the button below to unlock all features.</p>
                        <div class="button-wrap">
                            <a href="{{verification_link}}" class="btn">Verify Email</a>
                        </div>
                        <div class="note">
                            🔒 This link expires in <strong>24 hours</strong>. If you didn't create this account, feel free to ignore this email.
                        </div>
                        <div class="divider"></div>
                        <p style="font-size: 14px; margin: 0;">
                            Once you're verified, you can form a team, register for events, and start your hacking journey.
                        </p>
                    </div>
                    <div class="footer">
                        <p>SEAL Hackathon &mdash; Innovate. Build. Compete.</p>
                        <p>&copy; 2026 SEAL Hackathon. All rights reserved.</p>
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
                body {
                    margin: 0;
                    padding: 0;
                    background: #f4f6fb;
                    font-family: 'Segoe UI', -apple-system, BlinkMacSystemFont, 'Helvetica Neue', Arial, sans-serif;
                    color: #1a1a2e;
                }
                .wrapper {
                    width: 100%;
                    padding: 40px 16px;
                    box-sizing: border-box;
                }
                .container {
                    max-width: 560px;
                    margin: 0 auto;
                    background: #ffffff;
                    border-radius: 16px;
                    overflow: hidden;
                    box-shadow: 0 4px 24px rgba(0, 0, 0, 0.06);
                }
                .hero {
                    background: linear-gradient(135deg, #1a1a2e 0%, #16213e 100%);
                    padding: 48px 40px 36px;
                    text-align: center;
                }
                .logo {
                    width: 56px;
                    height: 56px;
                    margin: 0 auto 20px;
                    background: linear-gradient(135deg, #e94560, #533483);
                    border-radius: 14px;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    font-size: 24px;
                    font-weight: 800;
                    color: #ffffff;
                }
                .hero h1 {
                    margin: 0 0 6px;
                    font-size: 26px;
                    font-weight: 700;
                    color: #ffffff;
                    letter-spacing: -0.3px;
                }
                .hero p {
                    margin: 0;
                    color: #a8b2d1;
                    font-size: 15px;
                }
                .body-content {
                    padding: 40px;
                }
                .body-content p {
                    margin: 0 0 16px;
                    font-size: 15px;
                    line-height: 1.7;
                    color: #334155;
                }
                .button-wrap {
                    text-align: center;
                    margin: 32px 0;
                }
                .btn {
                    display: inline-block;
                    padding: 14px 40px;
                    border-radius: 10px;
                    background: linear-gradient(135deg, #e94560, #533483);
                    color: #ffffff !important;
                    text-decoration: none;
                    font-weight: 600;
                    font-size: 15px;
                    letter-spacing: 0.3px;
                    box-shadow: 0 4px 14px rgba(233, 69, 96, 0.3);
                }
                .divider {
                    height: 1px;
                    background: #e2e8f0;
                    margin: 28px 0;
                }
                .warning {
                    background: #fff5f5;
                    border: 1px solid #fecaca;
                    border-radius: 10px;
                    padding: 16px 20px;
                    font-size: 13px;
                    color: #b91c1c;
                    line-height: 1.6;
                }
                .warning strong {
                    color: #991b1b;
                }
                .footer {
                    padding: 24px 40px;
                    background: #f8fafc;
                    border-top: 1px solid #e2e8f0;
                    text-align: center;
                }
                .footer p {
                    margin: 0;
                    color: #94a3b8;
                    font-size: 12px;
                    line-height: 1.8;
                }
                .footer a {
                    color: #533483;
                    text-decoration: none;
                }
            </style>
        </head>
        <body>
            <div class="wrapper">
                <div class="container">
                    <div class="hero">
                        <div class="logo">SH</div>
                        <h1>Reset Your Password</h1>
                        <p>No worries, we've got you covered</p>
                    </div>
                    <div class="body-content">
                        <p>Hi there,</p>
                        <p>We received a request to reset the password for your <strong>SEAL Hackathon</strong> account. If this was you, click the button below to create a new password.</p>
                        <div class="button-wrap">
                            <a href="{{reset_password_link}}" class="btn">Reset Password</a>
                        </div>
                        <div class="warning">
                            ⚠️ This link expires in <strong>24 hours</strong>. If you didn't request this, please ignore this email — your account is safe and secure.
                        </div>
                        <div class="divider"></div>
                        <p style="font-size: 14px; margin: 0;">
                            For security reasons, never share this link with anyone. The SEAL Hackathon team will never ask for your password.
                        </p>
                    </div>
                    <div class="footer">
                        <p>SEAL Hackathon &mdash; Innovate. Build. Compete.</p>
                        <p>&copy; 2026 SEAL Hackathon. All rights reserved.</p>
                    </div>
                </div>
            </div>
        </body>
        </html>
        """;

        return htmlBody.Replace("{{reset_password_link}}", resetPasswordLink);
    }
}
