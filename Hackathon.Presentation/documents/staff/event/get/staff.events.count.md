# GET /api/v1/staff/events/count

> Staff đếm số lượng events được phân công. Có thể lọc theo trạng thái.

**Controller:** [StaffEventController.cs](Controllers/Staff/StaffEventController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/staff/events/count`

- **Khác Admin:** Chỉ đếm event mà Staff được phân công (AssignEvents với EventRole = Staff).
- Request/response format giống Admin.
- Không truyền Status → đếm tất cả event được phân công.
- Có truyền Status → đếm theo status.

## Phân quyền
- ✅ Staff

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| Status | string | ❌ | Published | ⚠️ Enum: Draft, Published, Closed |

## Response (200)
```json
{
  "data": { "total": 12 },
  "message": "Event Count Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Invalid Status. Must be: Draft, Published, Closed | Status sai |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Staff |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/count)
