# POST /api/v1/staff/notifications/{notificationId}/delete

> Staff xóa mềm thông báo.

## Nghiệp vụ

Soft-delete: set IsDisable = true. Không xóa khỏi database.

## Phân quyền
- ✅ Staff

## Response (200)
```json
{
  "data": null,
  "message": "Deleted Successfully",
  "status": 200
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Notification Is Already Disabled | Đã bị disable trước đó |
| 404 | Notification Not Found | ID không tồn tại |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | User không có role Staff |
