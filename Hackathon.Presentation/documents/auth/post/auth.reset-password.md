# POST /api/v1/auth/reset-password

> Đặt lại mật khẩu mới bằng token nhận được từ email.

## Nghiệp vụ
- Nhận vào token (từ email) + mật khẩu mới
- Validate token JWT → lấy UserId
- Kiểm tra token trong DB: chưa dùng (IsUsed=false), chưa hết hạn (ExpiresAt > now)
- Check mật khẩu mới không được trùng mật khẩu cũ
- Hash mật khẩu mới → cập nhật user
- Đánh dấu token đã dùng (IsUsed=true)

## Phân quyền
- ✅ Public (không cần đăng nhập, dùng token từ email)

## Request
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "newPassword": "newpassword123"
}
```

| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| token | ✅ | JWT token từ email forgot-password |
| newPassword | ✅ | 6-100 ký tự, không trùng mật khẩu cũ |

## Response (200)
```json
{
  "data": null,
  "message": "Password Reset Successfully",
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
| 400 | Invalid Or Expired Email Verification Token | Token hết hạn, sai, hoặc đã dùng rồi | Báo "Link reset password không hợp lệ hoặc đã hết hạn" |
| 400 | New Password Must Be Different From Old Password | Mật khẩu mới trùng mật khẩu cũ | Báo "Mật khẩu mới phải khác mật khẩu cũ" |
| 400 | Invalid Request Data | Validation lỗi (thiếu field, sai format) | Hiển thị từng field |
| 404 | User Not Found | Token hợp lệ nhưng user không tồn tại | Báo "Có lỗi xảy ra, vui lòng thử lại" |
