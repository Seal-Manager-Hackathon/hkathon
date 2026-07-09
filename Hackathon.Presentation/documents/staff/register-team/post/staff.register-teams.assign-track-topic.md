# POST /api/v1/staff/register-teams/{registerTeamId}/assign-track-topic — Gán track/topic

## Mục đích

Staff gán track và topic cho 1 team. Track và topic phải thuộc cùng event.

## Business Logic

1. Kiểm tra staff có assign vào event không
2. Track phải tồn tại và thuộc event của register team
3. Nếu có topic → topic phải thuộc track được gán
4. Set `TrackId` và `TopicId` trên register team

## Endpoint

```
POST /api/v1/staff/register-teams/{registerTeamId}/assign-track-topic
```

## Request Body

```json
{
  "trackId": "guid",
  "topicId": "guid"
}
```

## Error Handling

| Status | Meaning |
|--------|---------|
| 400 | Track không thuộc event, hoặc topic không thuộc track |
| 401 | Token hết hạn/thiếu |
| 403 | Staff không được phân công vào event |
| 404 | Register team/track/topic không tồn tại |