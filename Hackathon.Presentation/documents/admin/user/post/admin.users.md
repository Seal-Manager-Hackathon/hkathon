# POST /api/v1/admin/users

> Admin tạo user mới. IsVerified = true ngay lập tức.

## Nghiệp vụ
- Check email trùng, password hash + pepper
- Role FE truyền string, BE parse enum
- User tạo ra có IsVerified = true

## Phân quyền
- ✅ Admin

## Request
```json
{
  "email": "staff@fpt.edu.vn",
  "password": "Abc@123456",
  "firstName": "Staff",
  "lastName": "User",
  "role": "Staff",
  "college": "FPT University"
}
```
| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| email | ✅ | Format email |
| password | ✅ | 6-100 ký tự |
| firstName | ✅ | Tối đa 50 ký tự |
| lastName | ✅ | Tối đa 50 ký tự |
| role | ✅ | ⚠️ Enum: Admin, Staff, Student, Lecturer |
| college | ❌ | Mặc định "FPT University" |

## Response (201)
```json
{
  "data": {
    "id": "guid",
    "email": "staff@fpt.edu.vn",
    "firstName": "Staff",
    "lastName": "User",
    "role": "Staff",
    "college": "FPT University"
  },
  "message": "User Created Successfully",
  "error": null,
  "isSuccess": true,
  "status": 201,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | Invalid Role. Must be: Admin, Staff, Student, Lecturer | Role sai | Báo "Vai trò không hợp lệ" |
| 400 | Invalid Request Data | Validation lỗi | Hiển thị từng field |
| 409 | Email Already Exists | Email đã tồn tại | Báo "Email đã được sử dụng" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
