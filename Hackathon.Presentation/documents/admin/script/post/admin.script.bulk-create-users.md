# POST /api/v1/script/bulk-create-users

> Admin tạo nhanh N tài khoản user cùng role, email tự động đánh số (kiểm tra trùng).

**Controller:** [ScriptController.cs](Controllers/ScriptController.cs)

## Nghiệp vụ

- Admin tạo nhiều user cùng role trong 1 lần.
- Email tự động sinh theo format: `{prefix}{số}{domain}` (vd: `stu1@gmail.com`, `stu2@gmail.com`...).
- Hệ thống check email, StudentId, PhoneNumber đã tồn tại trong DB để không tạo trùng số thứ tự (nếu đã có stu1-stu10 thì sẽ bắt đầu từ stu11).
- FirstName và LastName được thêm số thứ tự tương ứng (vd: `Student 11`, `Student 12`...).
- StudentId tự động sinh: `SE000011`, `SE000012`... (format 6 chữ số).
- PhoneNumber tự động sinh: `0900000011`, `0900000012`... (09 + 8 chữ số).
- Mật khẩu mặc định: `string`.
- Kết quả trả về phân trang (mặc định pageIndex=1, pageSize=10) — toàn bộ user đã tạo, chỉ khác trang.

## Phân quyền

- ✅ Admin

## Request

| Param | Kiểu | Bắt buộc | Mặc định | Ghi chú |
|-------|------|----------|----------|---------|
| Count | int | ✅ | 1 | Số lượng tài khoản cần tạo (1-1000) |
| Role | string | ✅ | - | ⚠️ Enum: Admin, Staff, Student, Lecturer |
| EmailPrefix | string | ✅ | - | Tiền tố email (vd: `stu` → `stu1@gmail.com`) |
| EmailDomain | string | ❌ | `@gmail.com` | Domain email, tự động thêm @ nếu thiếu |
| FirstName | string | ✅ | - | FirstName gốc, số thứ tự được append |
| LastName | string | ✅ | - | LastName gốc, số thứ tự được append |
| PageIndex | int | ❌ | 1 | Trang hiện tại |
| PageSize | int | ❌ | 10 | Số lượng mỗi trang |

### Ví dụ
```json
{
  "count": 10,
  "role": "Student",
  "emailPrefix": "stu",
  "emailDomain": "@gmail.com",
  "firstName": "Student",
  "lastName": "Nguyen",
  "pageIndex": 1,
  "pageSize": 10
}
```

## Response (201)
```json
{
  "data": {
    "users": [
      {
        "id": "guid-...",
        "email": "stu11@gmail.com",
        "firstName": "Student 11",
        "lastName": "Nguyen 11",
        "password": "string",
        "role": "Student",
        "studentId": "SE000011",
        "phoneNumber": "0900000011"
      }
    ],
    "totalCount": 10,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Users Bulk Created Successfully",
  "error": null,
  "isSuccess": true,
  "status": 201,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-24T12:00:00Z"
}
```

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Invalid Role. Must be: Admin, Staff, Student, Lecturer | Role sai |
| 400 | Count Must Be Between 1 And 1000 | Count ngoài phạm vi |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |

> **Ref:** [Admin User Create](/api/v1/admin/users)
