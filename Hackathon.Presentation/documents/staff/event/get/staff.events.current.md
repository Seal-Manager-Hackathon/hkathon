# GET /api/v1/staff/events/current

> Staff lấy danh sách event hiện tại (đang diễn ra) mà họ được phân công với vai trò Staff.

## Nghiệp vụ

Staff cần xem nhanh các event đang diễn ra mà họ có thể làm việc. API này trả về:

- Chỉ các event mà staff được phân công với **`EventRole = Staff`** và **đang active** (`AssignEvents.IsDisable = false`).
- Chỉ các event có **thời gian hiện tại nằm trong khoảng `StartTime` → `EndTime`** (đang diễn ra).
- Tự động loại bỏ event có status `Draft`.
- Không phân trang — trả về tất cả event đang diễn ra.

## Phân quyền
- ✅ Staff (phải được phân công vào event với EventRole=Staff)

## Request
Không có request params.

## Response (200)
```json
{
  "data": [
    {
      "id": "guid",
      "name": "Hackathon AI 2026",
      "description": "Cuộc thi về trí tuệ nhân tạo",
      "status": "Published",
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

> **First** — API này là bản gốc, không có Admin tương ứng. Dùng làm chuẩn tham chiếu cho các role khác.
