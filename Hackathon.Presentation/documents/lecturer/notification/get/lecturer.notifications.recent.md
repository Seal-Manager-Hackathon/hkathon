# GET /api/v1/lecturer/notifications/recent

> Lecturer lấy 10 thông báo gần nhất dành cho họ (Personal + System).

**Controller:** [LecturerNotificationController.cs](Controllers/Lecturer/LecturerNotificationController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/notifications/recent`

- **Khác Admin:** Chỉ lấy notification của Lecturer đó (Personal: UserId == currentUser) và notification System.
- Luôn filter IsDisable = false.
- Sắp xếp theo CreatedAt giảm dần, lấy 10 thông báo mới nhất.

## Phân quyền
- ✅ Lecturer

## Response (200)
```json
{
  "data": {
    "notifications": [
      {
        "id": "guid",
        "userId": "guid",
        "teamId": null,
        "title": "Thông báo mới",
        "status": "Unread",
        "description": "Nội dung thông báo...",
        "targetType": "System",
        "isDisable": false,
        "createdAt": "2026-07-07T12:00:00Z",
        "updatedAt": "2026-07-07T12:00:00Z"
      }
    ]
  },
  "message": "Fetched Successfully",
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

> **Ref:** [Admin API tương ứng](/api/v1/admin/notifications/recent)
