# POST /api/v1/staff/register-teams/{registerTeamId}/remove-track-topic — Xóa track/topic

## Mục đích

Staff xóa track và topic đã gán cho 1 team, đưa về null.

## Business Logic

1. Kiểm tra staff có assign vào event không
2. Set `TrackId = null`, `TopicId = null`
3. Không kiểm tra register team có submission hay không — cẩn thận khi dùng

## Endpoint

```
POST /api/v1/staff/register-teams/{registerTeamId}/remove-track-topic
```

## Error Handling

| Status | Meaning |
|--------|---------|
| 401 | Token hết hạn/thiếu |
| 403 | Staff không được phân công vào event |
| 404 | Register team không tồn tại |