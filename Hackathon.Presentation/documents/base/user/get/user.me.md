# GET /api/v1/user/me

> Lấy thông tin profile của người dùng hiện tại.

## Nghiệp vụ
- Trả về thông tin cá nhân của user dựa trên token JWT
- Bất kỳ role nào (Student, Lecturer, Staff, Admin) đều dùng được

## Phân quyền
- ✅ Đăng nhập (Mọi role)

## Response (200)

```json
{
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "email": "user@example.com",
    "firstName": "Nguyễn",
    "lastName": "Văn A",
    "phoneNumber": "0901234567",
    "avatarUrl": "https://res.cloudinary.com/xxx/image/upload/v123/avatars/abc.jpg",
    "bio": "Sinh viên năm 3 ngành CNTT",
    "address": "Hà Nội",
    "dateOfBirth": "2000-01-15T00:00:00Z",
    "studentId": "SE123456",
    "college": "Đại học Bách Khoa",
    "imgUrl": "https://res.cloudinary.com/xxx/image/upload/v123/images/abc.jpg",
    "linkUrl": "https://facebook.com/user",
    "role": "Student",
    "status": "Active",
    "banReason": null,
    "bannedAt": null,
    "isVerified": true,
    "verifyEmailAt": "2026-01-01T00:00:00Z",
    "createdAt": "2026-01-01T00:00:00Z"
  },
  "message": "Profile Fetched Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi

| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 404 | User Not Found | User không tồn tại | Hiển thị thông báo lỗi |
