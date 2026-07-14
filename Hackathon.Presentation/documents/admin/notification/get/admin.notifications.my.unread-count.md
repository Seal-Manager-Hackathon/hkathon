# GET /api/v1/admin/notifications/my/unread-count

> Admin đếm số notification chưa đọc (status = Unread).

## Nghiệp vụ

- Chỉ đếm notification có `Status == Unread`.
- Chỉ đếm notification admin có quyền xem (Personal + System).
- Notification bị disable (IsDisable = true) không được đếm.

## Phân quyền
- ✅ Admin

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
| 403 | Forbidden | Không phải Admin |

> **Ref:** [Lecturer API tương ứng](/api/v1/lecturer/notifications/unread-count)
