namespace Hackathon.Service.Localization;

// Nơi gom các message code ổn định; FE nên dùng code này để xử lý logic, không dùng text đã dịch.
public static class MessageKeys
{
    // Response thành công chung.
    public const string Success = "SUCCESS";
    // Lỗi validation từ model/FluentValidation.
    public const string ValidationFailed = "VALIDATION_FAILED";
    // Fallback khi input không hợp lệ nhưng không có lỗi cụ thể.
    public const string InvalidInputData = "INVALID_INPUT_DATA";
    // Fallback an toàn cho lỗi hệ thống 5xx.
    public const string UnexpectedError = "AN_UNEXPECTED_ERROR_OCCURRED";
    // Title/message mặc định cho HTTP 400.
    public const string BadRequest = "BAD_REQUEST";
    // Title/message mặc định cho HTTP 401.
    public const string Unauthorized = "UNAUTHORIZED";
    // Title/message mặc định cho HTTP 403.
    public const string Forbidden = "FORBIDDEN";
    // Title/message mặc định cho HTTP 404.
    public const string NotFound = "NOT_FOUND";
}
