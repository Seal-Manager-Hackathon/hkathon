# GET /api/v1/admin/users/{userId}

> Lấy thông tin chi tiết đầy đủ của 1 user theo userId.

## Nghiệp vụ
- Trả về tất cả fields của user (kể cả thời gian tạo, cập nhật)
- 404 nếu userId không tồn tại

## Phân quyền
- ✅ Admin

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| userId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | |

## Response (200)
```json
{
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "email": "staff@fpt.edu.vn",
    "firstName": "Staff",
    "lastName": "User",
    "phoneNumber": "0123456789",
    "avatarUrl": "https://robohash.org/staff@fpt.edu.vn",
    "bio": "Staff bio...",
    "address": "FPT University",
    "dateOfBirth": "2000-01-01T00:00:00+00:00",
    "studentId": "SE123456",
    "college": "FPT University",
    "imgUrl": null,
    "linkUrl": null,
    "role": "Staff",
    "status": "Active",
    "isVerified": true,
    "isDisable": false,
    "banReason": null,
    "bannedAt": null,
    "verifyEmailAt": "2026-07-07T12:00:00Z",
    "createdAt": "2026-07-07T12:00:00Z",
    "updatedAt": "2026-07-07T12:00:00Z"
  },
  "message": "User Detail Fetched Successfully",
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
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | User Not Found | userId không tồn tại | Báo "Người dùng không tồn tại" |
