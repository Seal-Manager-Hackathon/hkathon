# GET /api/v1/lecturer/events/count

> Lecturer đếm số lượng events được phân công. Có thể lọc theo trạng thái.

**Controller:** [LecturerEventController.cs](Controllers/Lecturer/LecturerEventController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/events/count`

- **Khác Admin:** Chỉ đếm event mà Lecturer được phân công (AssignEvents với EventRole = Judge/Mentor).
- Request/response format giống Admin.
- Không truyền Status → đếm tất cả event được phân công.
- Có truyền Status → đếm theo status.

## Phân quyền
- ✅ Lecturer

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
| 403 | Forbidden | Không phải Lecturer |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/count)
