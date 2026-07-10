# POST /api/v1/staff/event-assigns/{assignEventId}/tracks/{trackId}/restore

> Staff khôi phục một track đã bị xóa mềm khỏi Lecturer.

## Nghiệp vụ

Staff muốn khôi phục lại một track đã bị xóa khỏi phân công của Lecturer. Hệ thống sẽ đặt lại `IsDisable = false` trên bản ghi `AssignTracks`.

- Sau khi khôi phục, track sẽ xuất hiện trở lại trong danh sách track của Lecturer đó
- Chỉ khôi phục được nếu track đang ở trạng thái bị disable
- **Chỉ tác động được lên Lecturer** — không thể khôi phục track của Staff

Ngược lại với API [remove track](staff.assign.tracks.remove.md).

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

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
| 404 | Assign Track Not Found | Không tìm thấy bản ghi assign track |
| 400 | Assign Track Is Already Active | Track này chưa bị xóa (vẫn đang active) |
| 400 | Can Only Modify Lecturer's Tracks | User không phải Lecturer |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Staff / không được assign vào event |

> **Ref:** [Admin API tương ứng](/api/v1/admin/assign/event-assigns/{assignEventId}/tracks/{trackId}/restore) — [`admin/assign/post/admin.assign.tracks.restore.md`](../../../admin/assign/post/admin.assign.tracks.restore.md)
