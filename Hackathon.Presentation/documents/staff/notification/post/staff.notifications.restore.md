# POST /api/v1/staff/notifications/{notificationId}/restore

> Staff khôi phục thông báo đã xóa mềm.

## Nghiệp vụ

Set IsDisable = false để hiển thị lại thông báo.

## Phân quyền
- ✅ Staff

## Response (200)
```json
{
  "data": null,
  "message": "Updated Successfully",
  "status": 200
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Notification Not Found | ID không tồn tại |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | User không có role Staff |

> **Ref:** [Admin API tương ứng](/api/v1/admin/notifications/{notificationId}/restore) — [`admin/notification/post/admin.notifications.restore.md`](../../../admin/notification/post/admin.notifications.restore.md)
