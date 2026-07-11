# GET /api/v1/lecturer/users/recent

> Lecturer lấy 10 users được tạo gần nhất.

**Controller:** [LecturerUserController.cs](Controllers/Lecturer/LecturerUserController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/users/recent`

- Giống hệt Admin `GET /api/v1/admin/users/recent`, khác auth là Lecturer.
- Sắp xếp theo CreatedAt giảm dần, lấy 10 users mới nhất.

## Phân quyền
- ✅ Lecturer

## Response (200)
```json
{
  "data": {
    "users": [
      {
        "id": "guid",
        "email": "lecturer@fpt.edu.vn",
        "firstName": "Lecturer",
        "lastName": "User",
        "avatarUrl": "https://robohash.org/lecturer@fpt.edu.vn",
        "role": "Lecturer",
        "createdAt": "2026-07-07T12:00:00Z"
      }
    ]
  },
  "message": "Recent Users Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |

> **Ref:** [Admin API tương ứng](/api/v1/admin/users/recent)
