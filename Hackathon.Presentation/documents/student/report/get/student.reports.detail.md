# GET /api/v1/student/reports/{reportId}

> Student xem chi tiết 1 report của mình.

**Controller:** [StudentReportController.cs](../../../Controllers/Student/StudentReportController.cs)

> **Ref:** [Admin API tương ứng](/api/v1/admin/reports/{reportId})

## Nghiệp vụ

Student muốn xem chi tiết 1 report mình đã gửi:
- Chỉ xem được report của chính mình (kiểm tra UserId từ token).
- Nếu reportId không tồn tại hoặc không phải của user → 404.

## Phân quyền
- ✅ Student

## Request

| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| reportId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "userId": "guid",
    "title": "Report title",
    "description": "Mô tả chi tiết report",
    "status": "Pending",
    "reason": null,
    "typeReport": "Spam",
    "createdAt": "2026-07-13T10:00:00Z",
    "updatedAt": "2026-07-13T10:00:00Z"
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Resource Not Found | reportId ko tồn tại hoặc ko phải của user |
| 401 | Unauthorized | Token hết hạn/thiếu |
