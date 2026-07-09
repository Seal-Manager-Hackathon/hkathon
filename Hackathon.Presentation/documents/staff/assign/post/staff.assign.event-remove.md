# POST /api/v1/staff/event-assigns/{assignEventId}/remove

> Staff xóa mềm (disable) một Lecturer khỏi event, kèm tất cả track được gán cho Lecturer đó trong event.

## Nghiệp vụ

Staff muốn xóa một Lecturer đã được phân công khỏi event. Hệ thống sẽ xóa mềm: đặt `IsDisable = true` trên bản ghi AssignEvents, đồng thời disable tất cả track mà Lecturer đó đã được phân công trong event này.

- **Chỉ tác động được lên Lecturer** — không thể xóa Staff, Admin, Student khỏi event
- Sau khi xóa, Lecturer và các track liên quan sẽ không còn xuất hiện trong danh sách GET /assigned
- Dữ liệu vẫn tồn tại trong DB, có thể khôi phục lại bằng API restore

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

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
| 400 | Assign Event Is Already Removed | Phân công này đã bị xóa trước đó |
| 400 | Can Only Remove Lecturer From Event | User không phải Lecturer |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Staff / không được assign vào event |
