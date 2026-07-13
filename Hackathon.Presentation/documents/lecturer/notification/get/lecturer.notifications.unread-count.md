# GET /api/v1/lecturer/notifications/unread-count

> Lecturer đếm số notification chưa đọc (status = Unread).

## Nghiệp vụ

- Chỉ đếm notification có `Status == Unread`.
- Chỉ đếm notification lecturer có quyền xem (Personal + System).
- Notification bị disable (IsDisable = true) không được đếm.

## Phân quyền
- ✅ Lecturer

## Request
Không query params, không body.

## Response (200)
```json
{
  "data": { "count": 5 },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |
