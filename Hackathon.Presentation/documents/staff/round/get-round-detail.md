# GET /api/v1/staff/events/{eventId}/rounds/{roundId} — Xem chi tiết round

## Mục đích

Staff muốn xem thông tin chi tiết của một round cụ thể để biết thời gian, giới hạn đội. Dùng trước khi xem submissions hoặc chấm điểm.

## Business Context

- Nếu round bị disable (`IsDisable = true`), API trả về 404
- Staff phải được phân công vào event chứa round này

## Endpoint

```
GET /api/v1/staff/events/{eventId}/rounds/{roundId}
```

## Controller → Service → Repository

`StaffRoundController.GetRoundDetail()` → `IRoundService.GetRoundDetail()` → `IRoundRepository.GetDetailByIdAsync()`. Xác thực event assignment trước khi query round.

## Route Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `eventId` | Guid | Yes | ID của event |
| `roundId` | Guid | Yes | ID của round |

## Response

```json
{
  "data": {
    "id": "guid",
    "eventId": "guid",
    "name": "Vòng loại",
    "description": "Vòng loại chọn top 20 đội",
    "roundNo": 1,
    "startTime": "2026-06-10T00:00:00Z",
    "endTime": "2026-06-15T00:00:00Z",
    "startSubmission": "2026-06-11T00:00:00Z",
    "endSubmission": "2026-06-14T00:00:00Z",
    "limitTeam": 50,
    "isDisable": false,
    "createdAt": "2026-05-01T00:00:00Z",
    "updatedAt": "2026-06-01T00:00:00Z"
  },
  "message": "Rounds fetched successfully",
  "traceId": "..."
}
```

## Exception Handling

| Status | Meaning |
|--------|---------|
| 401 | Token không hợp lệ hoặc đã hết hạn |
| 403 | User không có role Staff, hoặc không được phân công vào event |
| 404 | Không tìm thấy round (hoặc round đã bị disable) |
