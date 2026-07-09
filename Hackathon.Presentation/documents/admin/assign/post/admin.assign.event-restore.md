# POST /api/v1/admin/assign/event-assigns/{assignEventId}/restore

> Admin khôi phục một phân công user vào event đã bị xóa trước đó, kèm tất cả track đã bị disable theo.

## Nghiệp vụ

Admin muốn khôi phục lại một phân công user đã bị xóa mềm (có `IsDisable = true`). Hệ thống sẽ đặt lại `IsDisable = false` cho bản ghi AssignEvents, đồng thời restore tất cả track mà user đó đã được phân công trong event này.

- Sau khi khôi phục, user và các track liên quan sẽ xuất hiện trở lại trong danh sách `GET /assigned`.
- Chỉ khôi phục được nếu bản ghi đang ở trạng thái bị disable.

## Phân quyền
- ✅ Admin

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| assignEventId | Guid | ID của bản ghi AssignEvents cần khôi phục |

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
| 400 | Assign Event Is Already Active | Phân công này chưa bị xóa (vẫn đang active) |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
