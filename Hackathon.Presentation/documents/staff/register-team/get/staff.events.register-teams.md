# GET /api/v1/staff/events/{eventId}/register-teams — Lấy danh sách teams đăng ký

## Mục đích

Staff muốn xem danh sách các đội đã đăng ký tham gia một event cụ thể mà họ được phân công.

## Business Context

- Chỉ trả về teams trong event mà staff được phân công
- Hỗ trợ filter: keyword (tên team), status, IsBanned, IsDisable, fromDate/toDate, roundId, trackId, topicId
- Mỗi item trả kèm round hiện tại của team (round có RoundNo cao nhất)
- Response giống Admin

## Endpoint

```
GET /api/v1/staff/events/{eventId}/register-teams
```

## Controller → Service

`StaffRegisterTeamController.GetRegisterTeams()` → `IRegisterTeamService.GetRegisterTeams(eventId, request)` — kiểm tra staff có assign vào event trước.

## Route Parameters

| Param | Type | Required | Description |
|-------|------|----------|-------------|
| `eventId` | Guid | Yes | ID của event |

## Query Parameters

| Param | Type | Required | Description |
|-------|------|----------|-------------|
| `Keyword` | string | No | Tìm theo tên team |
| `Status` | string | No | `Pending`, `Approved`, `Rejected`, `Banned` |
| `IsBanned` | bool | No | Lọc team bị ban |
| `IsDisable` | bool | No | Lọc team bị disable |
| `FromDate` | datetime | No | Từ ngày |
| `ToDate` | datetime | No | Đến ngày |
| `RoundId` | Guid | No | Lọc theo round |
| `TrackId` | Guid | No | Lọc theo track |
| `TopicId` | Guid | No | Lọc theo topic |
| `PageIndex` | int | No (default=1) | Trang |
| `PageSize` | int | No (default=10) | Số item/trang |

## Response

```json
{
  "data": {
    "registerTeams": [
      {
        "id": "guid",
        "teamId": "guid",
        "teamName": "Tên team",
        "eventId": "guid",
        "eventName": "Hackathon",
        "trackId": "guid",
        "trackName": "AI",
        "topicId": "guid",
        "topicName": "Chatbot",
        "description": "Mô tả",
        "rejectionReason": null,
        "status": "Approved",
        "isBanned": false,
        "isDisable": false,
        "roundId": "guid",
        "roundName": "Vòng 1",
        "roundNo": 1,
        "createdAt": "2026-07-01T00:00:00Z",
        "updatedAt": "2026-07-08T00:00:00Z"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  }
}
```

## Error Handling

| Status | Meaning |
|--------|---------|
| 400 | Invalid Status value |
| 401 | Token hết hạn/thiếu |
| 403 | Không có role Staff hoặc không được phân công vào event |