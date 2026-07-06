# PUT /api/v1/admin/users/{userId}

> Admin chỉnh sửa thông tin của 1 user. Chỉ gửi field nào cần sửa, field null sẽ giữ nguyên.

## Nghiệp vụ
- Chỉ field được gửi lên mới cập nhật (null/bỏ trống → giữ nguyên)
- AvatarFile upload qua `IMediaService` → Cloudinary
- ImgUrl, LinkUrl là string thường, không upload file
- Role, IsVerified, BannedAt, VerifyEmailAt, CreatedAt, UpdatedAt **không được phép chỉnh sửa**

## Phân quyền
- ✅ Admin

## Request
`multipart/form-data`

| Field | Kiểu | Bắt buộc | Ghi chú |
|-------|------|----------|---------|
| FirstName | string | ❌ | |
| LastName | string | ❌ | |
| PhoneNumber | string | ❌ | |
| AvatarFile | file | ❌ | Upload ảnh lên Cloudinary |
| Bio | string | ❌ | |
| Address | string | ❌ | |
| DateOfBirth | datetime | ❌ | |
| StudentId | string | ❌ | |
| College | string | ❌ | |
| ImgUrl | string | ❌ | Chỉ là string thường |
| LinkUrl | string | ❌ | Chỉ là string thường |
| Status | string | ❌ | ⚠️ Enum: Active, Inactive |
| IsDisable | bool | ❌ | |

### Ví dụ
```
PUT /api/v1/admin/users/3fa85f64-5717-4562-b3fc-2c963f66afa6
Content-Type: multipart/form-data

{
  "firstName": "Updated",
  "lastName": "Name",
  "phoneNumber": "0123456789",
  "college": "FPT University"
}
```

## Response (200)
```json
{
  "data": null,
  "message": "User Updated Successfully",
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
| 400 | Invalid Status. Must be: Active, Inactive | Status sai | Báo "Trạng thái không hợp lệ" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | User Not Found | userId không tồn tại | Báo "Người dùng không tồn tại" |
