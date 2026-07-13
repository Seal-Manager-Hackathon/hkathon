# POST /api/v1/student/notifications/read-all

> Student đánh dấu tất cả notification chưa đọc thành đã đọc.

## Nghiệp vụ

API này cho phép student đánh dấu tất cả notification có status `Unread` trong list của họ thành `Read`.

- Chỉ tác động lên notification mà student có quyền xem.
- Personal (UserId trùng) + System + Team (thành viên team) được tính.
- Notification đã `Read` rồi không bị ảnh hưởng.
- Nếu không có notification Unread nào → vẫn trả về thành công (idempotent).

## Phân quyền
- ✅ Student (đã đăng nhập, token hợp lệ)

## Request
Không body, không query params.

## Response (200)
```json
{
  "data": null,
  "message": "Operation Successful",
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
