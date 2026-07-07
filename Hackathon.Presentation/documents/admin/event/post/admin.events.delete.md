# POST /api/v1/admin/events/{eventId}/delete

> Admin xóa mềm event — set `IsDisable = true`.

## Nghiệp vụ
- Chỉ set `IsDisable = true`, ko ảnh hưởng đến các dữ liệu khác
- Event đã disable rồi thì ko delete được nữa

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
  "message": "Event Deleted Successfully",
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
| 400 | Event Is Already Deleted | Event đã disable rồi |
