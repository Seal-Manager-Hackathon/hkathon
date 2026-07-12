# GET /api/v1/student/users/{userId}

> Student lấy thông tin chi tiết của 1 user khác.

## Nghiệp vụ

Student muốn xem thông tin cá nhân của user khác (thành viên trong team, leader...):
- Response giống hệt Admin API `GET /api/v1/admin/users/{userId}`.
- KHÔNG trả về user bị disable (IsDisable = true) — throw 404.
- User bị ban (BanReason != null) vẫn visible.

> **Ref:** [Admin API tương ứng](/api/v1/admin/users/{userId})

## Phân quyền
- ✅ Student (RoleEnum = Student)

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| userId | Guid | ID của user cần xem |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "email": "user@email.com",
    "firstName": "Nguyen",
    "lastName": "Van A",
    "phoneNumber": "0123456789",
    "avatarUrl": "https://example.com/avatar.jpg",
    "bio": "Some bio",
    "address": "Some address",
    "dateOfBirth": "2000-01-01T00:00:00Z",
    "studentId": "SV001",
    "college": "DH Cong Nghe",
    "imgUrl": null,
    "linkUrl": null,
    "role": "Student",
    "status": "Active",
    "isVerified": true,
    "isDisable": false,
    "banReason": null,
    "bannedAt": null,
    "verifyEmailAt": "2026-01-01T00:00:00Z",
    "createdAt": "2026-01-01T00:00:00Z",
    "updatedAt": "2026-01-01T00:00:00Z"
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 404 | User Not Found | userId không tồn tại, hoặc user bị disable |
