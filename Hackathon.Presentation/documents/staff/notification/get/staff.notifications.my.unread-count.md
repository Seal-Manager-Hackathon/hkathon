# GET /api/v1/staff/notifications/my/unread-count

> Staff lấy số lượng thông báo chưa đọc.

## Nghiệp vụ
- Đếm số thông báo System + Personal có `Status = Unread`.
- Dùng để hiển thị badge.

## Phân quyền
- ✅ Staff

## Response (200)
```json
{
  "data": { "count": 5 },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-...",
  "timestampUtc": "2026-07-10T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You do not have permission | Không phải Staff |
