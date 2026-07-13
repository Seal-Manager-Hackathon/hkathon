# POST /api/v1/admin/events/{eventId}/restore

> Admin khôi phục event — set `IsDisable = false`.

## Nghiệp vụ
- Chỉ set `IsDisable = false`
- Chỉ restore được event đang disable
- **LeaderBoard auto-creation**: Khi restore event, hệ thống tự động kiểm tra LeaderBoard của event đó. Nếu chưa có, hệ thống tạo một LeaderBoard mới với `Year` lấy từ `startTime` của event, `IsLocked = false` và `IsPublished = false`.

## Phân quyền
- ✅ Admin

## Request

| Param   | Kiểu | Bắt buộc | Ví dụ |
|---------|------|----------|-------|
| eventId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

Không cần body.

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
| Status | Message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
| 404 | Event Not Found | eventId ko tồn tại |
| 400 | Event Is Not Deleted | Event chưa disable |
