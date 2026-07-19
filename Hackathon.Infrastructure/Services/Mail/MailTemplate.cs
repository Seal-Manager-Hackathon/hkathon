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
                    background: #0b1120;
                    font-family: 'Segoe UI', Arial, Helvetica, sans-serif;
                    color: #e2e8f0;
                }
                .wrapper {
                    width: 100%;
                    padding: 48px 16px;
                    box-sizing: border-box;
                }
                .container {
                    max-width: 580px;
                    margin: 0 auto;
                    background: linear-gradient(180deg, #151d32 0%, #0f172a 100%);
                    border: 1px solid #1e293b;
                    border-radius: 24px;
                    overflow: hidden;
                    box-shadow: 0 25px 60px rgba(0, 0, 0, 0.6);
                }
                .hero {
                    padding: 40px 36px 32px;
                    text-align: center;
                    position: relative;
                }
                .hero::before {
                    content: '';
                    position: absolute;
                    top: 0;
                    left: 0;
                    right: 0;
                    height: 4px;
                    background: linear-gradient(90deg, #6366f1, #06b6d4, #6366f1);
                    background-size: 200% 100%;
                }
                .logo {
                    width: 64px;
                    height: 64px;
                    margin: 0 auto 20px;
                    background: linear-gradient(135deg, #6366f1, #06b6d4);
                    border-radius: 18px;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    font-size: 28px;
                    font-weight: 800;
                    color: #fff;
                    box-shadow: 0 12px 32px rgba(99, 102, 241, 0.3);
                }
                .hero h1 {
                    margin: 0 0 8px;
                    font-size: 28px;
                    font-weight: 700;
                    background: linear-gradient(135deg, #e2e8f0, #94a3b8);
                    -webkit-background-clip: text;
                    -webkit-text-fill-color: transparent;
                    background-clip: text;
                }
                .hero p {
                    margin: 0;
                    color: #64748b;
                    font-size: 15px;
                }
                .content {
                    padding: 0 36px 32px;
                    line-height: 1.8;
                    font-size: 15px;
                }
                .content p {
                    margin: 0 0 18px;
                }
                .button-wrap {
                    text-align: center;
                    margin: 32px 0 28px;
                }
                .btn {
                    display: inline-block;
                    padding: 15px 36px;
                    border-radius: 14px;
                    background: linear-gradient(135deg, #6366f1, #06b6d4);
                    color: #ffffff !important;
                    text-decoration: none;
                    font-weight: 700;
                    font-size: 16px;
                    letter-spacing: 0.3px;
                    box-shadow: 0 8px 28px rgba(99, 102, 241, 0.35);
                    transition: transform 0.2s, box-shadow 0.2s;
                }
                .btn:hover {
                    transform: translateY(-1px);
                    box-shadow: 0 12px 36px rgba(99, 102, 241, 0.45);
                }
                .divider {
                    height: 1px;
                    background: linear-gradient(90deg, transparent, #1e293b, transparent);
                    margin: 24px 0;
                }
                .note {
                    padding: 16px 20px;
                    border: 1px solid #1e293b;
                    border-radius: 14px;
                    background: rgba(30, 41, 59, 0.5);
                    color: #64748b;
                    font-size: 13px;
                    line-height: 1.6;
                }
                .note strong {
                    color: #94a3b8;
                }
                .footer {
                    padding: 24px 36px;
                    border-top: 1px solid #1e293b;
                    text-align: center;
                    color: #475569;
                    font-size: 12px;
                    line-height: 1.8;
                }
                .footer a {
                    color: #6366f1;
                    text-decoration: none;
                }
                .social {
                    display: flex;
                    justify-content: center;
                    gap: 16px;
                    margin-bottom: 16px;
                }
                .social a {
                    display: inline-block;
                    width: 36px;
                    height: 36px;
                    border-radius: 50%;
                    background: #1e293b;
                    color: #64748b;
                    text-align: center;
                    line-height: 36px;
                    font-size: 14px;
                    text-decoration: none;
                    transition: background 0.2s;
                }
                .social a:hover {
                    background: #334155;
                    color: #e2e8f0;
                }
            </style>
        </head>
        <body>
            <div class="wrapper">
                <div class="container">
                    <div class="hero">
                        <div class="logo">SH</div>
                        <h1>Activate Your Account</h1>
                        <p>Just one more step to join the competition</p>
                    </div>
                    <div class="content">
                        <p>Hey there,</p>
                        <p>Thanks for signing up for <strong>SEAL Hackathon</strong>! We're thrilled to have you on board. To get started, please verify your email address by clicking the button below.</p>
                        <div class="button-wrap">
                            <a href="{{verification_link}}" class="btn">Verify Email Address</a>
                        </div>
                        <div class="note">
                            ⚡ This link expires in <strong>24 hours</strong>. If you didn't create this account, you can safely ignore this email.
                        </div>
                        <div class="divider"></div>
                        <p style="color: #64748b; font-size: 14px; margin: 0;">
                            Once verified, you'll be able to form a team, register for events, and start hacking. See you there!
                        </p>
                    </div>
                    <div class="footer">
                        <p>SEAL Hackathon &mdash; Innovate. Build. Compete.</p>
                        <p style="margin: 4px 0 0;">&copy; 2026 SEAL Hackathon. All rights reserved.</p>
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
                    background: #0b1120;
                    font-family: 'Segoe UI', Arial, Helvetica, sans-serif;
                    color: #e2e8f0;
                }
                .wrapper {
                    width: 100%;
                    padding: 48px 16px;
                    box-sizing: border-box;
                }
                .container {
                    max-width: 580px;
                    margin: 0 auto;
                    background: linear-gradient(180deg, #151d32 0%, #0f172a 100%);
                    border: 1px solid #1e293b;
                    border-radius: 24px;
                    overflow: hidden;
                    box-shadow: 0 25px 60px rgba(0, 0, 0, 0.6);
                }
                .hero {
                    padding: 40px 36px 32px;
                    text-align: center;
                    position: relative;
                }
                .hero::before {
                    content: '';
                    position: absolute;
                    top: 0;
                    left: 0;
                    right: 0;
                    height: 4px;
                    background: linear-gradient(90deg, #f59e0b, #ef4444, #f59e0b);
                    background-size: 200% 100%;
                }
                .logo {
                    width: 64px;
                    height: 64px;
                    margin: 0 auto 20px;
                    background: linear-gradient(135deg, #f59e0b, #ef4444);
                    border-radius: 18px;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    font-size: 28px;
                    font-weight: 800;
                    color: #fff;
                    box-shadow: 0 12px 32px rgba(245, 158, 11, 0.3);
                }
                .hero h1 {
                    margin: 0 0 8px;
                    font-size: 28px;
                    font-weight: 700;
                    background: linear-gradient(135deg, #e2e8f0, #94a3b8);
                    -webkit-background-clip: text;
                    -webkit-text-fill-color: transparent;
                    background-clip: text;
                }
                .hero p {
                    margin: 0;
                    color: #64748b;
                    font-size: 15px;
                }
                .content {
                    padding: 0 36px 32px;
                    line-height: 1.8;
                    font-size: 15px;
                }
                .content p {
                    margin: 0 0 18px;
                }
                .button-wrap {
                    text-align: center;
                    margin: 32px 0 28px;
                }
                .btn {
                    display: inline-block;
                    padding: 15px 36px;
                    border-radius: 14px;
                    background: linear-gradient(135deg, #f59e0b, #ef4444);
                    color: #ffffff !important;
                    text-decoration: none;
                    font-weight: 700;
                    font-size: 16px;
                    letter-spacing: 0.3px;
                    box-shadow: 0 8px 28px rgba(245, 158, 11, 0.35);
                    transition: transform 0.2s, box-shadow 0.2s;
                }
                .btn:hover {
                    transform: translateY(-1px);
                    box-shadow: 0 12px 36px rgba(245, 158, 11, 0.45);
                }
                .divider {
                    height: 1px;
                    background: linear-gradient(90deg, transparent, #1e293b, transparent);
                    margin: 24px 0;
                }
                .warning {
                    padding: 16px 20px;
                    border: 1px solid rgba(239, 68, 68, 0.2);
                    border-radius: 14px;
                    background: rgba(239, 68, 68, 0.08);
                    color: #fca5a5;
                    font-size: 13px;
                    line-height: 1.6;
                }
                .warning strong {
                    color: #f87171;
                }
                .footer {
                    padding: 24px 36px;
                    border-top: 1px solid #1e293b;
                    text-align: center;
                    color: #475569;
                    font-size: 12px;
                    line-height: 1.8;
                }
                .footer a {
                    color: #6366f1;
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
                        <p>We've got you covered</p>
                    </div>
                    <div class="content">
                        <p>Hey there,</p>
                        <p>We received a request to reset the password for your <strong>SEAL Hackathon</strong> account. If you made this request, click the button below to set a new password.</p>
                        <div class="button-wrap">
                            <a href="{{reset_password_link}}" class="btn">Reset Password</a>
                        </div>
                        <div class="warning">
                            ⚠️ This link expires in <strong>24 hours</strong>. If you didn't request this, please ignore this email — your account is safe.
                        </div>
                        <div class="divider"></div>
                        <p style="color: #64748b; font-size: 14px; margin: 0;">
                            For security reasons, never share this link with anyone. The SEAL Hackathon team will never ask for your password.
                        </p>
                    </div>
                    <div class="footer">
                        <p>SEAL Hackathon &mdash; Innovate. Build. Compete.</p>
                        <p style="margin: 4px 0 0;">&copy; 2026 SEAL Hackathon. All rights reserved.</p>
                    </div>
                </div>
            </div>
        </body>
        </html>
        """;

        return htmlBody.Replace("{{reset_password_link}}", resetPasswordLink);
    }
}
