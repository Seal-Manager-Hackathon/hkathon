# GET /api/v1/admin/events/{eventId}/rounds/max-round-no

> Lấy roundNo lớn nhất hiện tại của event. Dùng để FE biết round tiếp theo sẽ là số mấy khi tạo round mới.

## Nghiệp vụ
- Trả về `roundNo` lớn nhất trong event
- Nếu chưa có round nào → trả về `null`
- 404 nếu eventId không tồn tại

## Phân quyền
- ✅ Admin

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| eventId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)
```json
{
  "data": 2,
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

Nếu chưa có round nào:
```json
{
  "data": null,
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Event Not Found | eventId không tồn tại | Báo "Không tìm thấy sự kiện" |
