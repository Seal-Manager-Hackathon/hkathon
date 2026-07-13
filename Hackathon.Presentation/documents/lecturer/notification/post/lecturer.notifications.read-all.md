# POST /api/v1/lecturer/notifications/read-all

> Lecturer đánh dấu tất cả notification chưa đọc thành đã đọc.

## Nghiệp vụ

- Đánh dấu tất cả notification có `Status == Unread` trong danh sách của lecturer thành `Read`.
- Chỉ tác động lên notification lecturer có quyền xem (Personal + System).
- Nếu không có notification Unread nào → vẫn trả về thành công.

## Phân quyền
- ✅ Lecturer

## Request
Không body, không query params.

## Response (200)
```json
{
  "data": null,
  "message": "Updated Successfully",
  "status": 200,
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |
