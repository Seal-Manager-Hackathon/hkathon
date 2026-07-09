# GET /api/v1/staff/events/{eventId}/register-teams

> Lấy danh sách các đội đã đăng ký tham gia một event cụ thể, hỗ trợ phân trang và filter đa chiều.

## Nghiệp vụ
- Chỉ trả về các register team thuộc event mà staff được phân công
- Hỗ trợ filter theo: keyword (tên team), status, isBanned, isDisable, fromDate/toDate, roundId, trackId, topicId
- Mỗi item trả kèm round hiện tại của team (round có RoundNo cao nhất)
- Dữ liệu trả về dạng phân trang (PageIndex, PageSize, TotalCount)

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

## Request

### Route Parameters

| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| eventId | Guid | Có | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | ID của event |

### Query Parameters

| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| Keyword | string | Không | `Team A` | Tìm kiếm theo tên team |
| Status | string | Không | `Pending` | Enum: `Pending`, `Approved`, `Rejected`, `Banned` |
| IsBanned | bool | Không | `false` | Lọc team bị ban |
| IsDisable | bool | Không | `false` | Lọc team bị disable |
| FromDate | datetime | Không | `2026-07-01T00:00:00Z` | Ngày bắt đầu |
| ToDate | datetime | Không | `2026-07-08T00:00:00Z` | Ngày kết thúc |
| RoundId | Guid | Không | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | Lọc theo round |
| TrackId | Guid | Không | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | Lọc theo track |
| TopicId | Guid | Không | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | Lọc theo topic |
| PageIndex | int | Không | `1` | Mặc định = 1 |
| PageSize | int | Không | `10` | Mặc định = 10 |

## Response (200)

```json
{
  "data": {
    "registerTeams": [
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "teamId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "teamName": "Team A",
        "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "eventName": "Hackathon 2026",
        "trackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "trackName": "AI",
        "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "topicName": "Chatbot",
        "description": "Mô tả dự án",
        "rejectionReason": null,
        "status": "Approved",
        "isBanned": false,
        "isDisable": false,
        "roundId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "roundName": "Vòng 1",
        "roundNo": 1,
        "createdAt": "2026-07-01T00:00:00Z",
        "updatedAt": "2026-07-08T00:00:00Z"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Lấy danh sách register teams thành công",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-0b4e4e4b7b8c4d4f8f9a0b1c2d3e4f5a",
  "timestampUtc": "2026-07-09T10:00:00Z"
}
```

## Lỗi

| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | `Invalid Status value` | Giá trị Status không hợp lệ (không thuộc enum) | Hiển thị thông báo lỗi |
| 401 | `Unauthorized` | Token hết hạn hoặc thiếu | Redirect sang trang login |
| 403 | `Forbidden` | Không có role Staff hoặc không được phân công vào event | Hiển thị thông báo không có quyền |
