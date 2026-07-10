# GET /api/v1/staff/events/my-staff

> Staff lấy danh sách event mà họ được phân công với vai trò Staff (từ `AssignEvents`, `EventRole = Staff`), không phân biệt event có bị disable hay không.

## Nghiệp vụ
- Chỉ lấy các bản ghi `AssignEvents` mà user hiện tại có **`EventRole = Staff`** và **đang active** (`AssignEvents.IsDisable = false`).
- **Trả về tất cả event**, kể cả event bị **disable** (`Event.IsDisable = true`) hay không — FE dùng field `isDisable` để hiển thị đúng trạng thái.
- Tự động **loại bỏ** event có status `Draft`.
- Mỗi item trả kèm `eventRoleId` và `eventRoleName` (luôn là `Staff`).
- Hỗ trợ lọc theo keyword, status, khoảng thời gian.
- Hỗ trợ phân trang.

> API này chỉ lấy event có EventRole=Staff. Để lấy tất cả event được phân công (mọi EventRole), dùng [GET /api/v1/staff/events](staff.events.md).

## Phân quyền
- ✅ Staff (phải có bản ghi AssignEvents với EventRole = Staff và IsDisable = false)

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| `Keyword` | string | ❌ | `Hackathon` | Tìm theo tên event |
| `Status` | string | ❌ | `Ongoing` | Enum: `Ongoing`, `Upcoming`, `Completed` |
| `FromDate` | datetime | ❌ | `2026-01-01` | Lọc event bắt đầu từ ngày này |
| `ToDate` | datetime | ❌ | `2026-12-31` | Lọc event kết thúc trước ngày này |
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
        "status": "Ongoing",
        "numberRound": 3,
        "season": "Summer",
        "startTime": "2026-06-01T00:00:00Z",
        "endTime": "2026-07-01T00:00:00Z",
        "eventRoleId": "guid",
        "eventRoleName": "Staff",
        "createdAt": "2026-05-01T00:00:00Z",
        "updatedAt": "2026-06-01T00:00:00Z",
        "isDisable": false
      }
    ],
    "totalCount": 1,
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
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | User không có role Staff | Ẩn chức năng |
