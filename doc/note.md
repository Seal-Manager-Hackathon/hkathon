# Exception Message Convention

## Phân biệt ErrorMessageCode và ErrorMessage

### ErrorMessageCode (lỗi chung theo status)
- File: `Hackathon.Application/Exceptions/ErrorMessageCode.cs`
- Chứa các mã lỗi **chung, tái sử dụng** theo status code
- Chia 2 nhóm: **4xx** (Client Errors) và **5xx** (Server Errors)
- Dùng làm `messageCode` khi throw exception

```csharp
public static class ErrorMessageCode
{
    // ── 4xx Client Errors ──
    public const string BadRequest = "Bad Request";           // 400
    public const string Unauthorized = "Unauthorized";        // 401
    public const string Forbidden = "Forbidden";              // 403
    public const string NotFound = "Not Found";               // 404
    public const string Conflict = "Conflict";                // 409
    public const string TooManyRequest = "Too Many Request";  // 429

    // ── 5xx Server Errors ──
    public const string InternalServerError = "Internal Server Error";  // 500
    public const string FileUploadFailed = "File Upload Failed";        // 500
    public const string ServiceUnavailable = "Service Unavailable";     // 503
}
```

### ErrorMessage (message chi tiết theo nghiệp vụ)
- File: `Hackathon.Application/Exceptions/ErrorMessage.cs`
- Chứa message **cụ thể theo từng nghiệp vụ** (entity, use case)
- Phân nhóm theo entity/module
- Dùng làm `message` khi throw exception

```csharp
public static class ErrorMessage
{
    public static class Common { ... }
    public static class Database { ... }
    public static class Events { ... }
}
```

## Cách dùng

```csharp
// messageCode = ErrorMessageCode.TooManyRequest (chung)
// message    = ErrorMessage.Common.TooManyRequestsRetryAfter60s (cụ thể)
throw new TooManyRequestException(
    ErrorMessageCode.TooManyRequest,
    ErrorMessage.Common.TooManyRequestsRetryAfter60s);

// messageCode = ErrorMessageCode.InternalServerError (chung)
// message    = ErrorMessage.Database.SaveChangesFailed (cụ thể)
throw new ServerException(
    ErrorMessageCode.InternalServerError,
    ErrorMessage.Database.SaveChangesFailed);
```

## Khi có lỗi mới

1. Nếu là **loại lỗi mới chưa có trong ErrorMessageCode** (VD: 402 Payment Required) → thêm vào `ErrorMessageCode.cs`
2. Nếu là **message cụ thể cho 1 entity/nghiệp vụ** → thêm vào `ErrorMessage.cs` trong class tương ứng
3. Nếu chưa có class cho entity đó → tạo class mới trong `ErrorMessage.cs`

## Format

- **Title**: Capitalize Each Word (`Bad Request`, `Internal Server Error`)
- **Message**: Capitalize Each Word (`Save Changes Failed`, `Event Name Is Required`)
- **MessageCode**: Capitalize Each Word (`Bad Request`, `Conflict`)
