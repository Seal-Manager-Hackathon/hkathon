# GET /api/v1/staff/notifications/recent

> Staff lấy 10 thông báo gần nhất.

## Nghiệp vụ

Lấy nhanh các thông báo mới nhất (10 bản ghi) không phân trang. Dùng cho màn hình dashboard.

## Phân quyền
- ✅ Staff

## Response (200)
```json
{
  "data": {
    "notifications": [
      {
        "id": "guid",
        "userId": "guid",
        "teamId": "guid",
        "title": "Notification Title",
        "status": "Unread",
        "description": "Chi tiết thông báo",
        "targetType": "System",
        "isDisable": false,
        "createdAt": "2026-07-01T00:00:00Z",
        "updatedAt": "2026-07-01T00:00:00Z"
      }
    ]
  },
  "message": "Notifications Fetched Successfully",
  "status": 200
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | User không có role Staff |

> **Ref:** [Admin API tương ứng](/api/v1/admin/notifications/recent) — [`admin/notification/get/admin.notifications.recent.md`](../../../admin/notification/get/admin.notifications.recent.md)
