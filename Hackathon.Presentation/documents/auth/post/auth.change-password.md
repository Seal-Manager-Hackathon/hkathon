# POST /api/v1/auth/change-password

> Đổi mật khẩu khi đã đăng nhập. Cần cung cấp mật khẩu hiện tại.

## Nghiệp vụ
- Authenticate — lấy userId từ access token
- Load user từ userId
- Verify mật khẩu hiện tại (currentPassword) với hash trong DB
- Nếu đúng → hash mật khẩu mới → cập nhật
- Không revoke refresh token (user vẫn đăng nhập)

## Phân quyền
- ✅ Authenticated — cần Bearer token

## Request
```json
{
  "currentPassword": "oldpassword123",
  "newPassword": "newpassword123"
}
```

| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| currentPassword | ✅ | Mật khẩu hiện tại |
| newPassword | ✅ | 6-100 ký tự |

## Response (200)
```json
{
  "data": null,
  "message": "Password Changed Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 404 | User Not Found | User ko tồn tại |
| 400 | Current Password Invalid | Mật khẩu hiện tại sai |

## Logic
1. Authenticate — lấy userId từ token
2. Load user từ userId
3. Verify `currentPassword` với hash trong DB
4. Hash `newPassword` → update user
5. SaveChanges
