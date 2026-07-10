# PATCH /api/v1/staff/notifications/{notificationId}

> Staff cập nhật thông báo.

## Nghiệp vụ

Cập nhật tiêu đề và/hoặc nội dung thông báo. Chỉ update các field được gửi lên (partial update).

## Phân quyền
- ✅ Staff

## Request Body
```json
{
  "title": "Tiêu đề mới",
  "description": "Nội dung mới"
}
```

## Response (200)
```json
{
  "data": null,
  "message": "Notification Updated Successfully",
  "status": 200
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Notification Not Found | ID không tồn tại |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | User không có role Staff |

> **Ref:** [Admin API tương ứng](/api/v1/admin/notification/patch/admin.notifications.update.md)
