# POST /api/v1/auth/forgot-password

> Gửi email reset password cho user. Token có thời hạn 2 phút.

## Nghiệp vụ
- Nhận email → tạo reset token (JWT, 2 phút) → lưu vào DB → gửi email
- **Luôn trả về thành công** dù email có tồn tại hay không — để tránh lộ thông tin user (email enumeration)
- Nếu email không tồn tại → không gửi mail, nhưng vẫn trả 200

## Phân quyền
- ✅ Public (không cần đăng nhập)

## Request
```json
{
  "email": "user@example.com"
}
```

| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| email | ✅ | Format email hợp lệ |

## Response (200)
```json
{
  "data": null,
  "message": "Forgot Password Request Accepted",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | Invalid Request Data | Email sai format | Báo "Email không hợp lệ" |
