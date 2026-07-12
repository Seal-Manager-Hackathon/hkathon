# GET /api/v1/student/events/count

> Student đếm số lượng event, có filter theo status.

**Controller:** [StudentEventController.cs](Controllers/Student/StudentEventController.cs)

## Nghiệp vụ
- Đếm tổng số event.
- Có filter theo status: chỉ đếm event có status tương ứng.

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| Status | string | ❌ | `Published` |

## Response (200)
```json
{
  "data": {
    "total": 15
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Invalid Status | Status sai |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Student |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/count)
