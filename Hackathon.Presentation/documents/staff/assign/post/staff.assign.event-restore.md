# POST /api/v1/staff/event-assigns/{assignEventId}/restore

> Staff khôi phục một Lecturer đã bị xóa khỏi event, kèm tất cả track đã bị disable theo.

## Nghiệp vụ

Staff muốn khôi phục lại một phân công Lecturer đã bị xóa mềm (có `IsDisable = true`). Hệ thống sẽ đặt lại `IsDisable = false` cho bản ghi AssignEvents, đồng thời restore tất cả track mà Lecturer đó đã được phân công trong event này.

- **Chỉ tác động được lên Lecturer** — không thể khôi phục Staff
- Sau khi khôi phục, Lecturer và các track liên quan sẽ xuất hiện trở lại trong danh sách `GET /assigned`
- Chỉ khôi phục được nếu bản ghi đang ở trạng thái bị disable

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

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
| 400 | Can Only Restore Lecturer | User không phải Lecturer |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Staff / không được assign vào event |

> **Ref:** [Admin API tương ứng](/api/v1/admin/assign/post/admin.assign.event-restore.md)
