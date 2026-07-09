# GET /api/v1/staff/events/{eventId}/rounds — Lấy danh sách rounds của event

## Mục đích

Staff muốn xem danh sách các vòng (round) của một event mà họ được phân công. Mỗi event có thể có nhiều round, Staff cần xem để biết tiến độ tổ chức.

## Business Context

- Round là các vòng thi trong event, mỗi round có thời gian bắt đầu/kết thúc riêng
- Staff chỉ xem được round của event họ được phân công
- Chỉ trả về round có `IsDisable = false`
- Hỗ trợ lọc keyword và số thứ tự round

## Endpoint

```
GET /api/v1/staff/events/{eventId}/rounds
```

## Controller → Service → Repository

`StaffRoundController.GetRounds()` → `IRoundService.GetRounds()` → `IRoundRepository.SearchByEventIdAsync()`. Xác thực assignment trước.

## Route Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `eventId` | Guid | Yes | ID của event |

## Request Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `Keyword` | string | No | Tìm kiếm theo tên round |
| `RoundNo` | int | No | Lọc theo số thứ tự round |
| `PageIndex` | int | No (mặc định 1) | Trang hiện tại |
| `PageSize` | int | No (mặc định 10) | Số lượng item mỗi trang |

## Response

```json
{
  "data": {
    "rounds": [
      {
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
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
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
