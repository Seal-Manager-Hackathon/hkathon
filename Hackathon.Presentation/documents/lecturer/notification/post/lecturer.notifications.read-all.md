# POST /api/v1/lecturer/notifications/read-all

> Lecturer đánh dấu tất cả thông báo chưa đọc là đã đọc.

## Nghiệp vụ
- Chuyển `Status` của toàn bộ thông báo System + Personal (`Unread` → `Read`).
- Chỉ ảnh hưởng đến thông báo của chính lecturer.

## Phân quyền
- ✅ Lecturer

## Response (200)
```json
{
  "data": null,
  "message": "Updated Successfully",
  "status": 200,
  "traceId": "00-...",
  "timestampUtc": "2026-07-10T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You do not have permission | Không phải Lecturer |
