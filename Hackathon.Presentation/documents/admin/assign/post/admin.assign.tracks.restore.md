# POST /api/v1/admin/assign/event-assigns/{assignEventId}/tracks/{trackId}/restore

> Admin khôi phục một phân công track đã bị xóa mềm trước đó.

## Nghiệp vụ

Admin muốn khôi phục lại một track đã bị xóa khỏi phân công của user. Hệ thống sẽ đặt lại `IsDisable = false` trên bản ghi `AssignTracks`.

- Sau khi khôi phục, track sẽ xuất hiện trở lại trong danh sách track của user đó.
- Chỉ khôi phục được nếu track đang ở trạng thái bị disable.

Ngược lại với API [remove track](admin.assign.tracks.remove.md).

## Phân quyền
- ✅ Admin

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| assignEventId | Guid | ID của bản ghi AssignEvents |
| trackId | Guid | ID của track cần khôi phục |

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
| 404 | Assign Track Not Found | Không tìm thấy bản ghi assign track (có thể chưa từng được gán) |
| 400 | Assign Track Is Already Active | Track này chưa bị xóa (vẫn đang active) |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
