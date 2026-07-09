# POST /api/v1/staff/register-teams/{registerTeamId}/revert-previous-round — Quay về vòng trước

## Mục đích

Staff đưa team về vòng thi trước đó (VD: vòng 3 → vòng 2).

## Business Logic

1. Kiểm tra staff có assign vào event không
2. Lấy active rounds, sort RoundNo DESC
3. Cần ít nhất 2 rounds để revert
4. Nếu round hiện tại đã có submission → không cho revert
5. Xóa cứng RoundDetail hiện tại

## Endpoint

```
POST /api/v1/staff/register-teams/{registerTeamId}/revert-previous-round
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
    "roundName": "Vòng 1",
    "roundNo": 1
  }
}
```

## Error Handling

| Status | Meaning |
|--------|---------|
| 400 | Team chỉ có 1 round, hoặc round hiện tại có submission |
| 401 | Token hết hạn/thiếu |
| 403 | Staff không được phân công vào event |
| 404 | Register team không tồn tại |