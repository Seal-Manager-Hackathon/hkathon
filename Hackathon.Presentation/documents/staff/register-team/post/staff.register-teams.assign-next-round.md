# POST /api/v1/staff/register-teams/{registerTeamId}/assign-next-round — Chuyển vòng tiếp

## Mục đích

Staff chuyển team lên vòng tiếp theo.

## Business Logic

1. Kiểm tra staff có assign vào event không
2. Lấy round hiện tại của team (RoundNo cao nhất)
3. Tìm round tiếp theo: `EventId` + `RoundNo + 1`
4. Check không trùng round detail
5. Tạo RoundDetail mới

## Endpoint

```
POST /api/v1/staff/register-teams/{registerTeamId}/assign-next-round
```

## Response

```json
{
  "data": {
    "registerTeamId": "guid",
    "eventId": "guid",
    "teamId": "guid",
    "teamName": "Tên team",
    "trackId": "guid",
    "trackName": "AI",
    "topicId": "guid",
    "topicName": "Chatbot",
    "roundId": "guid",
    "roundName": "Vòng 2",
    "roundNo": 2
  }
}
```

## Error Handling

| Status | Meaning |
|--------|---------|
| 400 | Đã là vòng cuối, hoặc team đã ở round này |
| 401 | Token hết hạn/thiếu |
| 403 | Staff không được phân công vào event |
| 404 | Register team không tồn tại |