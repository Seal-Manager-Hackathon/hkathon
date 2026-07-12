# GET /api/v1/student/events/count

> Student đếm số lượng event (Published/Closed, ko disable), có filter theo status.

**Controller:** [StudentEventController.cs](Controllers/Student/StudentEventController.cs)

## Nghiệp vụ
- Đếm tổng số event, chỉ đếm event Published/Closed, ko disable.
- Nếu truyền Status = Draft → báo lỗi (Student ko được xem Draft).
- Có filter theo status: chỉ đếm event có status tương ứng.

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| Status | string | ❌ (query) | `Published` | ⚠️ Enum: Published, Closed (Draft bị từ chối) |

## Response (200)
```json
{
  "data": {
    "total": 15
  },
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
| 400 | Invalid Status. Must be: Draft, Published, Closed | Status sai | Báo "Trạng thái không hợp lệ" |
| 400 | Invalid Status | Status = Draft | Báo "Trạng thái không hợp lệ" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Student | Ẩn chức năng |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/count)
