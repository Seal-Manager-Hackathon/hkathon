# GET /api/v1/admin/users/recent

> Lấy 10 users được tạo gần nhất.

## Nghiệp vụ
- Sắp xếp theo `CreatedAt` giảm dần, lấy 10 users mới nhất

## Phân quyền
- ✅ Admin

## Response (200)
```json
{
  "data": {
    "users": [
      {
        "id": "guid",
        "email": "staff@fpt.edu.vn",
        "firstName": "Staff",
        "lastName": "User",
        "avatarUrl": "https://robohash.org/staff@fpt.edu.vn",
        "role": "Staff",
        "createdAt": "2026-07-07T12:00:00Z"
      }
    ]
  },
  "message": "Recent Users Fetched Successfully",
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
