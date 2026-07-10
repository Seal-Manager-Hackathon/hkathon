# GET /api/v1/staff/users/{userId}

> Xem chi tiết 1 user — tất cả thông tin cá nhân.

## Nghiệp vụ
- Trả về thông tin đầy đủ của user: email, tên, role, avatar, bio, college, studentId, ban, verify

## Phân quyền
- ✅ Staff

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| userId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "email": "user@example.com",
    "firstName": "Nguyễn",
    "lastName": "Văn A",
    "phoneNumber": "0123456789",
    "avatarUrl": "https://robohash.org/...",
    "bio": "Sinh viên năm 3",
    "address": "Hà Nội",
    "dateOfBirth": "2003-05-15T00:00:00Z",
    "studentId": "SE123456",
    "college": "FPT University",
    "imgUrl": null,
    "linkUrl": null,
    "role": "Student",
    "status": "Active",
    "isVerified": true,
    "isDisable": false,
    "banReason": null,
    "bannedAt": null,
    "verifyEmailAt": "2026-07-01T10:00:00Z",
    "createdAt": "2026-07-01T10:00:00Z",
    "updatedAt": "2026-07-07T12:00:00Z"
  },
  "message": "User Detail Fetched Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 404 | User Not Found | userId không tồn tại | Hiển thị thông báo |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Staff | Ẩn chức năng |

> **Ref:** [Admin API tương ứng](/api/v1/admin/user/get/admin.users.detail.md)