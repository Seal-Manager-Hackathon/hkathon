# POST /api/v1/admin/topics/{topicId}/delete

> Admin xóa mềm topic (IsDisable = true).

## Phân quyền
- ✅ Admin

## Request

| Param   | Kiểu | Bắt buộc | Ví dụ |
|---------|------|----------|-------|
| topicId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)
```json
{
  "data": null,
  "message": "Deleted Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Topic Is Already Deleted | topic đã disable rồi |
| 404 | Resource Not Found | topicId ko tồn tại |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
