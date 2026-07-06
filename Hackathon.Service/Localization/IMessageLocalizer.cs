namespace Hackathon.Service.Localization;

// Interface chung để các layer gọi localization mà không phụ thuộc trực tiếp vào ASP.NET .resx.
public interface IMessageLocalizer
{
    // Dịch message key/code sang text theo culture hiện tại; không có resource thì trả key gốc.
    string Get(string? key);

    // Dịch title lỗi; ưu tiên key dạng CODE_TITLE, fallback theo HTTP status code.
    string GetTitle(string? key, int statusCode);
}
