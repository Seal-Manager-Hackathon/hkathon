# GET /api/v1/admin/events/count

> Đếm số lượng events. Có thể lọc theo trạng thái.

## Nghiệp vụ
- Không truyền Status → đếm tất cả
- Có truyền Status → đếm theo status

## Phân quyền
- ✅ Admin

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| Status | string | ❌ | Published | ⚠️ Enum: Draft, Published, Closed |

### Ví dụ
```
GET /api/v1/admin/events/count
GET /api/v1/admin/events/count?Status=Published
```

## Response (200)
```json
{
  "data": { "total": 12 },
  "message": "Event Count Fetched Successfully",
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
| 400 | Invalid Status. Must be: Draft, Published, Closed | Status sai | Báo "Trạng thái không hợp lệ" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
