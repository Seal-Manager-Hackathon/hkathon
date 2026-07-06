# POST /api/v1/auth/login

> Đăng nhập bằng email + password. Trả về access token + refresh token.

## Nghiệp vụ
- Tìm user theo email, check Banned
- Verify password (có pepper)
- Nếu email chưa verify → gửi lại mail → báo lỗi
- Tạo JWT (chỉ chứa **UserId**) + refresh token
- Lưu refresh token

## Phân quyền
- **Public** — không cần token

## Request
```json
{ "email": "user@fpt.edu.vn", "password": "Abc@123456" }
```
| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| email | ✅ | Format email |
| password | ✅ | |

## Response (200)
```json
{
  "data": { "accessToken": "...", "refreshToken": "..." },
  "message": "Login Successful",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T10:10:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 404 | User Not Found | Email không tồn tại | Báo "Email không tồn tại" |
| 403 | User Is Banned | User bị ban | Báo "Tài khoản đã bị khóa" |
| 401 | Invalid Email Or Password | Sai mật khẩu | Báo "Email hoặc mật khẩu không đúng" |
| 401 | Email Unverified Otp Sent | Email chưa verify | Báo "Kiểm tra email để xác thực" |
