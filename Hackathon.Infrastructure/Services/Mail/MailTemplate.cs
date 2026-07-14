namespace Hackathon.Infrastructure.Services.Mail;

public static class MailTemplate
{
    public static string EmailContainToken(string token, string baseUrl)
    {
        var verificationLink = $"{baseUrl}/verify-email?token={token}";

        var htmlBody = """
        <!DOCTYPE html>
        <html lang="vi">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>Xác thực tài khoản - SEAL Hackathon</title>
            <style>
                body {
                    margin: 0;
                    padding: 0;
                    background: #eef4ff;
                    font-family: Arial, Helvetica, sans-serif;
                    color: #172033;
                }
                .wrapper {
                    width: 100%;
                    padding: 40px 16px;
                    box-sizing: border-box;
                }
                .container {
                    max-width: 620px;
                    margin: 0 auto;
                    background: #ffffff;
                    border-radius: 18px;
                    overflow: hidden;
                    box-shadow: 0 18px 45px rgba(28, 67, 140, 0.16);
                }
                .hero {
                    padding: 34px 32px;
                    background: linear-gradient(135deg, #1247d9 0%, #19a7ce 100%);
                    color: #ffffff;
                    text-align: center;
                }
                .badge {
                    display: inline-block;
                    padding: 7px 13px;
                    border-radius: 999px;
                    background: rgba(255, 255, 255, 0.18);
                    font-size: 12px;
                    letter-spacing: 1px;
                    text-transform: uppercase;
                    margin-bottom: 14px;
                }
                .hero h1 {
                    margin: 0;
                    font-size: 28px;
                    line-height: 1.25;
                }
                .content {
                    padding: 34px 36px 28px;
                    line-height: 1.7;
                    font-size: 15px;
                }
                .content p {
                    margin: 0 0 16px;
                }
                .button-wrap {
                    text-align: center;
                    margin: 30px 0;
                }
                .btn {
                    display: inline-block;
                    padding: 14px 28px;
                    border-radius: 12px;
                    background: #1247d9;
                    color: #ffffff !important;
                    text-decoration: none;
                    font-weight: 700;
                    box-shadow: 0 10px 22px rgba(18, 71, 217, 0.26);
                }
                .note {
                    padding: 14px 16px;
                    border-left: 4px solid #19a7ce;
                    background: #f4fbff;
                    border-radius: 10px;
                    color: #43516a;
                    font-size: 14px;
                }
                .footer {
                    padding: 22px 30px;
                    background: #f7f9fd;
                    text-align: center;
                    color: #7b879d;
                    font-size: 12px;
                }
            </style>
        </head>
        <body>
            <div class="wrapper">
                <div class="container">
                    <div class="hero">
                        <div class="badge">SEAL Hackathon 2026</div>
                        <h1>Kích hoạt tài khoản của bạn</h1>
                    </div>
                    <div class="content">
                        <p>Chào bạn,</p>
                        <p>Cảm ơn bạn đã đăng ký tham gia <strong>SEAL Hackathon</strong>. Chỉ còn một bước nữa để hoàn tất tài khoản.</p>
                        <div class="button-wrap">
                            <a href="{{verification_link}}" class="btn">Xác Thực Tài Khoản</a>
                        </div>
                        <div class="note">
                            Liên kết xác thực chỉ có hiệu lực trong thời gian ngắn. Vui lòng không chia sẻ email này cho bất kỳ ai.
                        </div>
                    </div>
                    <div class="footer">
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
        <html lang="vi">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>Đặt lại mật khẩu - SEAL Hackathon</title>
            <style>
                body {
                    margin: 0;
                    padding: 0;
                    background: #fff5f0;
                    font-family: Arial, Helvetica, sans-serif;
                    color: #201b18;
                }
                .wrapper {
                    width: 100%;
                    padding: 40px 16px;
                    box-sizing: border-box;
                }
                .container {
                    max-width: 620px;
                    margin: 0 auto;
                    background: #ffffff;
                    border-radius: 18px;
                    overflow: hidden;
                    box-shadow: 0 18px 45px rgba(186, 74, 27, 0.16);
                }
                .hero {
                    padding: 34px 32px;
                    background: linear-gradient(135deg, #f97316 0%, #ef4444 100%);
                    color: #ffffff;
                    text-align: center;
                }
                .badge {
                    display: inline-block;
                    padding: 7px 13px;
                    border-radius: 999px;
                    background: rgba(255, 255, 255, 0.18);
                    font-size: 12px;
                    letter-spacing: 1px;
                    text-transform: uppercase;
                    margin-bottom: 14px;
                }
                .hero h1 {
                    margin: 0;
                    font-size: 28px;
                    line-height: 1.25;
                }
                .content {
                    padding: 34px 36px 28px;
                    line-height: 1.7;
                    font-size: 15px;
                }
                .content p {
                    margin: 0 0 16px;
                }
                .button-wrap {
                    text-align: center;
                    margin: 30px 0;
                }
                .btn {
                    display: inline-block;
                    padding: 14px 28px;
                    border-radius: 12px;
                    background: #ef4444;
                    color: #ffffff !important;
                    text-decoration: none;
                    font-weight: 700;
                    box-shadow: 0 10px 22px rgba(239, 68, 68, 0.24);
                }
                .warning {
                    padding: 14px 16px;
                    border-left: 4px solid #f97316;
                    background: #fff7ed;
                    border-radius: 10px;
                    color: #60402a;
                    font-size: 14px;
                }
                .footer {
                    padding: 22px 30px;
                    background: #fff8f4;
                    text-align: center;
                    color: #9a8173;
                    font-size: 12px;
                }
            </style>
        </head>
        <body>
            <div class="wrapper">
                <div class="container">
                    <div class="hero">
                        <div class="badge">Bảo mật tài khoản</div>
                        <h1>Đặt lại mật khẩu</h1>
                    </div>
                    <div class="content">
                        <p>Chào bạn,</p>
                        <p>Chúng tôi nhận được yêu cầu đặt lại mật khẩu cho tài khoản <strong>SEAL Hackathon</strong> của bạn.</p>
                        <div class="button-wrap">
                            <a href="{{reset_password_link}}" class="btn">Đặt Lại Mật Khẩu</a>
                        </div>
                        <div class="warning">
                            Nếu bạn không yêu cầu đặt lại mật khẩu, hãy bỏ qua email này. Không chia sẻ liên kết này cho bất kỳ ai.
                        </div>
                    </div>
                    <div class="footer">
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
