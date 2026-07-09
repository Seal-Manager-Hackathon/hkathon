# GET /api/v1/staff/events — Lấy danh sách event được phân công

## Mục đích

Người dùng có role Staff muốn xem danh sách các event mà họ được phân công tham gia quản lý (thông qua bảng `AssignEvents`). API này chỉ trả về các event có liên kết với Staff hiện tại, không phải tất cả event trong hệ thống.

## Business Context

- Staff là người được Admin hoặc cấp trên phân công vào một hoặc nhiều event để hỗ trợ vận hành
- Mỗi Staff có thể được phân công với một vai trò cụ thể trong event (EventRole)
- API tự động lọc **không lấy** event có status là `Draft`
- Kết quả trả về gồm thông tin event + vai trò của Staff trong event đó
- Hỗ trợ lọc theo keyword (tên event), status, và khoảng thời gian

## Endpoint

```
GET /api/v1/staff/events
```

## Controller

`StaffEventController.GetMyEvents()` gọi `IEventService.GetMyEvents()` → `IAssignEventRepository.GetEventsByStaffUserIdAsync()`. Kiểm tra assignment trước khi query.

## Request Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `Keyword` | string | No | Tìm kiếm theo tên event |
| `Status` | string | No | Lọc theo trạng thái event (VD: `Ongoing`, `Upcoming`, `Completed` — không lấy `Draft`) |
| `FromDate` | datetime | No | Lọc event bắt đầu từ ngày này |
| `ToDate` | datetime | No | Lọc event kết thúc trước ngày này |
| `PageIndex` | int | No (mặc định 1) | Trang hiện tại |
| `PageSize` | int | No (mặc định 10) | Số lượng item mỗi trang |

## Response

```json
{
  "data": {
    "items": [
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
        "eventRoleName": "Mentor",
        "createdAt": "2026-05-01T00:00:00Z",
        "updatedAt": "2026-06-01T00:00:00Z"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Events fetched successfully",
  "traceId": "..."
}
```

## Field Notes

| Field | Ý nghĩa |
|-------|---------|
| `eventRoleId` | ID của vai trò của Staff trong event này (trong bảng `EventRole`) |
| `eventRoleName` | Tên vai trò của Staff — VD: `Mentor`, `Judge`, `Staff` |
| `status` | Trạng thái của event (Enum value). Không bao gồm `Draft` |
| `season` | Mùa tổ chức event (Enum: `Spring`, `Summer`, `Fall`, `Winter`) |

## Exception Handling

| Status | Meaning |
|--------|---------|
| 401 | Token không hợp lệ hoặc đã hết hạn |
| 403 | User không có role Staff |
