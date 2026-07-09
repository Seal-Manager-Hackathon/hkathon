# GET /api/v1/staff/events/{eventId} — Xem chi tiết event được phân công

## Mục đích

Staff muốn xem thông tin chi tiết của một event cụ thể mà họ được phân công. API này chỉ hoạt động nếu Staff hiện tại thực sự có quyền truy cập vào event đó.

## Endpoint

```
GET /api/v1/staff/events/{eventId}
```

## Controller

`StaffEventController.GetMyEventDetail(Guid eventId)` gọi `IEventService.GetMyEventDetail(eventId)` → `IAssignEventRepository.GetByEventIdAndUserIdWithEventAsync()`. Nếu không tìm thấy assignment thì trả về 404.

## Route Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `eventId` | Guid | Yes | ID của event cần xem chi tiết |

## Response

```json
{
  "data": {
    "id": "guid",
    "name": "Hackathon AI 2026",
    "description": "Cuộc thi về trí tuệ nhân tạo",
    "status": "Ongoing",
    "numberRound": 3,
    "season": "Summer",
    "startTime": "2026-06-01T00:00:00Z",
    "endTime": "2026-07-01T00:00:00Z",
    "registerLimitTime": "2026-05-15T00:00:00Z",
    "limitTeam": 50,
    "minMember": 2,
    "maxMember": 5,
    "eventRoleId": "guid",
    "eventRoleName": "Mentor",
    "createdAt": "2026-05-01T00:00:00Z",
    "updatedAt": "2026-06-01T00:00:00Z"
  },
  "message": "Events fetched successfully",
  "traceId": "..."
}
```

## Business Notes

- Trả về thêm các field mở rộng so với danh sách: `RegisterLimitTime`, `LimitTeam`, `MinMember`, `MaxMember`
- Nếu Staff không được phân công vào event, API trả về 404 — không tiết lộ thông tin event
- Field `eventRoleId` và `eventRoleName` là vai trò của Staff trong event cụ thể

## Exception Handling

| Status | Meaning |
|--------|---------|
| 401 | Token không hợp lệ hoặc đã hết hạn |
| 403 | User không có role Staff |
| 404 | Không tìm thấy event hoặc Staff không được phân công vào event này |
