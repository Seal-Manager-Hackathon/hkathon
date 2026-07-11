# GET /api/v1/staff/users/recent

> Staff lấy danh sách 10 user mới đăng ký gần đây nhất (dashboard).

**Controller:** [StaffUserController.cs](Controllers/Staff/StaffUserController.cs)

## Nghiệp vụ

- Staff xem 10 user được tạo gần đây nhất.
- Giống hệt Admin `GET /api/v1/admin/users/recent`, khác auth là Staff.

## Phân quyền
- ✅ Staff (RoleEnum.Staff)

## Response (200)
```json
{
  "data": {
    "users": [
      {
        "id": "guid",
        "email": "user@example.com",
        "firstName": "Nguyen",
        "lastName": "Van A",
        "avatarUrl": null,
        "role": "Student",
        "createdAt": "2026-07-11T00:00:00Z"
      }
    ]
  }
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission | Không phải Staff |

> **Ref:** [Admin API tương ứng](/api/v1/admin/users/recent)
