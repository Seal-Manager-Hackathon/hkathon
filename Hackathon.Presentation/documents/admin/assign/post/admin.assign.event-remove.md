# POST /api/v1/admin/assign/event-assigns/{assignEventId}/remove

> Admin xóa mềm (disable) một phân công user khỏi event.

## Nghiệp vụ

Admin muốn xóa một user đã được phân công khỏi event. Hệ thống sẽ xóa mềm: đặt IsDisable = true trên bản ghi AssignEvents.

Sau khi xóa, user này sẽ không còn xuất hiện trong danh sách GET /assigned. Dữ liệu vẫn tồn tại trong DB, có thể khôi phục lại bằng API restore.

## Phân quyền
- ✅ Admin

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| assignEventId | Guid | ID của bản ghi AssignEvents (phân công) |

## Response (200)
```json
{
  "data": null,
  "message": "Updated Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Assign Event Not Found | assignEventId không tồn tại |
| 400 | Assign Event Is Already Removed | Phân công này đã bị xóa trước đó rồi |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
