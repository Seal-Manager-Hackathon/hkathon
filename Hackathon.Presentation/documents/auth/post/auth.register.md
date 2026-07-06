# POST /api/v1/auth/register

> Đăng ký tài khoản mới. Role mặc định `Student`. Gửi email xác thực.

## Nghiệp vụ
- Email không trùng với user đã verify
- Nếu email đã tồn tại nhưng chưa verify → báo lỗi bảo user login để nhận lại mail
- Password được hash + pepper
- Gửi email chứa token verify (hết hạn 5 phút)

## Phân quyền
- **Public** — không cần token

## Request
```json
{ "email": "user@fpt.edu.vn", "password": "Abc@123456", "firstName": "Nguyen", "lastName": "Van A" }
```
| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| email | ✅ | Format email |
| password | ✅ | 6-100 ký tự |
| firstName | ✅ | Tối đa 50 ký tự |
| lastName | ✅ | Tối đa 50 ký tự |

## Response (201)
```json
{
  "data": { "message": "Registration Successful" },
  "message": "Registration Successful",
  "error": null,
  "isSuccess": true,
  "status": 201,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T10:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 409 | Email Already Exists | Email đã đăng ký và đã verify | Báo "Email đã được sử dụng" |
| 409 | Unverified Account Please Login To Verify | Email tồn tại nhưng chưa verify | Chuyển đến login |
| 400 | Invalid Request Data | Validation lỗi | Hiển thị từng field |
