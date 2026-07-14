# POST /api/v1/admin/notifications/my/read-all

> Admin đánh dấu tất cả notification chưa đọc thành đã đọc.

## Nghiệp vụ

- Đánh dấu tất cả notification có `Status == Unread` trong danh sách của admin thành `Read`.
- Chỉ tác động lên notification admin có quyền xem (Personal + System).
- Nếu không có notification Unread nào → vẫn trả về thành công.

## Phân quyền
- ✅ Admin

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
| 403 | Forbidden | Không phải Admin |
