namespace Hackathon.Presentation.Extentions;

public static class CookieExtensions
{
    public const string AccessTokenCookieName = "Access-Token";
    public const string RefreshTokenCookieName = "Refresh-Token";

    
    public static void WriteAuthCookies(this HttpResponse response, string accessToken, string refreshToken)
    {
      
        var accessOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Path = "/",
            Expires = DateTimeOffset.UtcNow.AddMinutes(15)
        };

        var refreshOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Path = "/",
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        };
        response.Cookies.Append(AccessTokenCookieName, accessToken, accessOptions);
        response.Cookies.Append(RefreshTokenCookieName, refreshToken, refreshOptions);
    }
    
    
    public static void DeleteAuthCookies(this HttpResponse response)
    {
        response.Cookies.Delete(AccessTokenCookieName);
        response.Cookies.Delete(RefreshTokenCookieName);
    }
}