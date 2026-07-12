# GET /api/v1/student/events

> Student lấy danh sách các event Published/Closed, có filter và phân trang.

**Controller:** [StudentEventController.cs](Controllers/Student/StudentEventController.cs)

## Nghiệp vụ
- Student xem danh sách event — chỉ thấy event **Published** hoặc **Closed**.
- **KHÔNG** thấy event Draft hoặc IsDisable = true.
- Hỗ trợ tìm kiếm theo tên (contains, ko phân biệt hoa thường), lọc theo status, khoảng thời gian.
- Sắp xếp theo CreatedAt giảm dần.

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| Keyword | string | ❌ (query) | `FPT` | Search tên event |
| Status | string | ❌ (query) | `Published` | ⚠️ Enum: Draft, Published, Closed |
| FromDate | datetime | ❌ (query) | `2026-07-01T00:00:00Z` | |
| ToDate | datetime | ❌ (query) | `2026-07-07T23:59:59Z` | |
| PageIndex | int | ❌ (query) | `1` | Mặc định 1 |
| PageSize | int | ❌ (query) | `10` | Mặc định 10 |

## Response (200)
```json
{
  "data": {
    "events": [
      {
        "id": "guid",
        "name": "Hackathon 2026",
        "description": "...",
        "status": "Published",
        "startTime": "2026-08-01T00:00:00Z",
        "endTime": "2026-08-10T00:00:00Z",
        "isDisable": false,
        "createdAt": "2026-07-07T12:00:00Z",
        "updatedAt": "2026-07-07T12:00:00Z"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
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
| 400 | Page Index Must Be Greater Than 0 | pageIndex < 1 | Báo "Trang không hợp lệ" |
| 400 | Page Size Must Be Between 1 And 100 | pageSize < 1 hoặc > 100 | Báo "Kích thước trang không hợp lệ" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Student | Ẩn chức năng |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events)
