# GET /api/v1/staff/events

> Staff lấy danh sách tất cả event trong hệ thống (giống Admin), chỉ cần Authorize Staff.

## Nghiệp vụ
- Giống hệt [GET /api/v1/admin/events](../../../admin/event/get/admin.events.md) — lấy **tất cả event** trong hệ thống, không cần staff được phân công.
- Chỉ cần **Authorize Staff** — staff nào cũng xem được.
- Hỗ trợ search keyword theo tên, filter status, isDisable, khoảng thời gian.
- Sắp xếp gần nhất trên cùng.
- Hỗ trợ phân trang.

> API này lấy tất cả event. Để lấy event mà staff được phân công, dùng [GET /api/v1/staff/events/my-staff](staff.events.my-staff.md).

## Phân quyền
- ✅ Staff (chỉ cần role Staff, không cần assign vào event)

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| `Keyword` | string | ❌ | `Hackathon` | Tìm theo tên event |
| `Status` | string | ❌ | `Published` | Enum: `Draft`, `Published`, `Closed` |
| `IsDisable` | bool | ❌ | `false` | Lọc event bị disable |
| `FromDate` | datetime | ❌ | `2026-07-01T00:00:00Z` | Lọc theo CreatedAt |
| `ToDate` | datetime | ❌ | `2026-07-07T23:59:59Z` | Lọc theo CreatedAt |
| `PageIndex` | int | ❌ | `1` | Mặc định 1 |
| `PageSize` | int | ❌ | `10` | Mặc định 10 |

## Response (200)
```json
{
  "data": {
    "events": [
      {
        "id": "guid",
        "name": "Hackathon AI 2026",
        "description": "Cuộc thi về trí tuệ nhân tạo",
        "status": "Published",
        "numberRound": 3,
        "season": "Summer",
        "startTime": "2026-06-01T00:00:00Z",
        "endTime": "2026-07-01T00:00:00Z",
        "isDisable": false,
        "createdAt": "2026-05-01T00:00:00Z",
        "updatedAt": "2026-06-01T00:00:00Z"
      }
    ],
    "totalCount": 10,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Events fetched successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-...",
  "timestampUtc": "2026-07-09T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | Invalid Status. Must be: Draft, Published, Closed | Status sai | Báo "Trạng thái không hợp lệ" |
| 400 | Page Index Must Be Greater Than Zero | PageIndex < 1 | Fix pagination |
| 400 | Page Size Must Be Between 1 And 100 | PageSize ngoài khoảng | Fix pagination |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | User không có role Staff | Ẩn chức năng |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events) — [`admin/event/get/admin.events.md`](../../../admin/event/get/admin.events.md)
