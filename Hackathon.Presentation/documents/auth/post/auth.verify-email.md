# POST /api/v1/auth/verify-email

> Xác thực email bằng token. Trả luôn access token + refresh token.

## Nghiệp vụ
- Token JWT do BE cấp qua email, hết hạn 5 phút
- Validate token → lấy UserId → set IsVerified = true
- Nếu user đã verified rồi → response rỗng
- Sau verify → tạo access token + refresh token

## Phân quyền
- **Public** — cần token từ email (không cần Bearer)

## Request
```json
{ "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." }
```
| Field | Bắt buộc | Ghi chú |
|-------|----------|---------|
| token | ✅ | Token từ email BE gửi |

## Response (200)
### Verify lần đầu
```json
{
  "data": { "accessToken": "...", "refreshToken": "..." },
  "message": "Email Verified Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T10:05:00Z"
}
```
### Đã verify trước đó
```json
{
  "data": {},
  "message": null,
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T10:05:00Z"
}
```
> 💡 FE check `data.accessToken` có tồn tại không.

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | Invalid Or Expired Email Verification Token | Token sai hoặc hết hạn | Báo "Link xác thực không hợp lệ" |
| 400 | Invalid Request Data | Token rỗng hoặc null | Báo "Token không được để trống" |
| 404 | User Not Found | User không tồn tại | Báo lỗi chung |
