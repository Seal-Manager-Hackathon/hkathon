# GET /api/v1/staff/notifications/{notificationId}

> Staff xem chi tiết một thông báo.

## Nghiệp vụ

Lấy thông tin chi tiết của một notification theo ID.

## Phân quyền
- ✅ Staff

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "userId": "guid",
    "teamId": "guid",
    "title": "Notification Title",
    "status": "Unread",
    "description": "Chi tiết thông báo",
    "targetType": "System",
    "createdAt": "2026-07-01T00:00:00Z",
    "updatedAt": "2026-07-01T00:00:00Z"
  },
  "message": "Notification Detail Fetched Successfully",
  "status": 200
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | User không có role Staff |
| 404 | Notification Not Found | ID không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/notifications/{notificationId}) — [`admin/notification/get/admin.notifications.detail.md`](../../../admin/notification/get/admin.notifications.detail.md)
