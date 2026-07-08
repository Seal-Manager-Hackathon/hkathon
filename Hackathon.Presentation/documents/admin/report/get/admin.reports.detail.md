# GET /api/v1/admin/reports/{reportId}

> Admin xem chi tiết 1 report.

## Phân quyền
- ✅ Admin

## Request

| Param    | Kiểu | Bắt buộc | Ví dụ |
|----------|------|----------|-------|
| reportId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)

```json
{
  "data": {
    "id": "guid",
    "userId": "guid",
    "userEmail": "user@fpt.edu.vn",
    "userFirstName": "Nguyễn",
    "userLastName": "Văn A",
    "title": "Report title",
    "description": "Mô tả chi tiết report",
    "status": "Pending",
    "reason": null,
    "typeReport": "Spam",
    "createdAt": "2026-07-07T12:00:00Z",
    "updatedAt": "2026-07-07T12:00:00Z"
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Resource Not Found | reportId ko tồn tại |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
