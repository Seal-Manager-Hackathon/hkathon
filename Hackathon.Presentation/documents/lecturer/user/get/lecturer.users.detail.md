# GET /api/v1/lecturer/users/{userId}

> Lecturer xem chi tiết user — copy y chang từ Admin `GET /api/v1/admin/users/{userId}`, không sửa gì.

**Controller:** [LecturerUserController.cs](Controllers/Lecturer/LecturerUserController.cs)

## Nghiệp vụ

- Giống Admin `GET /api/v1/admin/users/{userId}` — request, response giống hệt.
- Chi tiết user: email, tên, role, trạng thái, isVerified, isDisable, ban, avatar...
- Nếu user bị disable (`IsDisable = true`) → 404 (ko cho xem).
- Nếu user bị ban vẫn xem được bình thường.

## Phân quyền
- ✅ Judge

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| userId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "email": "user@email.com",
    "firstName": "Nguyễn",
    "lastName": "Văn A",
    "phoneNumber": "0123456789",
    "avatarUrl": "https://robohash.org/...",
    "bio": "Sinh viên",
    "address": "Hà Nội",
    "dateOfBirth": "2000-01-01T00:00:00Z",
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
    "verifyEmailAt": "2026-07-01T00:00:00Z",
    "createdAt": "2026-07-01T00:00:00Z",
    "updatedAt": "2026-07-01T00:00:00Z"
  },
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-12T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Judge |
| 404 | User Not Found | userId ko tồn tại hoặc bị disable |

> **Ref:** [Admin API tương ứng](/api/v1/admin/users/{userId})
