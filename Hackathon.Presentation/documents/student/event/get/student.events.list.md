# GET /api/v1/student/events

> Student lấy danh sách các event Published/Closed, có filter và phân trang.

**Controller:** [StudentEventController.cs](Controllers/Student/StudentEventController.cs)

## Nghiệp vụ
- Student xem danh sách event — chỉ thấy event **Published** hoặc **Closed**.
- **KHÔNG** thấy event Draft hoặc IsDisable = true.
- Hỗ trợ tìm kiếm theo tên, lọc theo status, khoảng thời gian.
- Sắp xếp theo CreatedAt giảm dần.

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| Keyword | string | ❌ | `FPT` | Search tên event |
| Status | string | ❌ | `Published` | ⚠️ Enum: Draft, Published, Closed |
| FromDate | datetime | ❌ | `2026-07-01T00:00:00Z` | |
| ToDate | datetime | ❌ | `2026-07-07T23:59:59Z` | |
| PageIndex | int | ❌ | `1` | Mặc định 1 |
| PageSize | int | ❌ | `10` | Mặc định 10 |

## Response (200)
```json
{
  "data": {
    "events": [
      {
        "id": "guid",
        "name": "Hackathon 2026",
        "description": "Event description",
        "status": "Published",
        "startTime": "2026-07-01T00:00:00Z",
        "endTime": "2026-07-10T00:00:00Z",
        "isDisable": false,
        "createdAt": "2026-07-01T00:00:00Z",
        "updatedAt": "2026-07-01T00:00:00Z"
      }
    ],
    "totalCount": 10,
    "pageIndex": 1,
    "pageSize": 10
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
| 400 | Page Index/Size | Pagination sai |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Student |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events)
