# POST /api/v1/staff/register-teams/{registerTeamId}/ban — Cấm team

## Mục đích

Staff cấm (ban) 1 team tham gia event. Team bị banned không thể nộp bài hay tiếp tục.

## Business Logic

1. Kiểm tra staff có assign vào event của register team không
2. Set `IsBanned = true`, status = `Banned`, lưu rejectionReason
3. Không cho phép ban nếu đã banned rồi

## Endpoint

```
POST /api/v1/staff/register-teams/{registerTeamId}/ban
```

## Request Body

```json
{
  "rejectionReason": "Vi phạm quy chế"
}
```

## Error Handling

| Status | Meaning |
|--------|---------|
| 400 | Team đã bị banned rồi |
| 401 | Token hết hạn/thiếu |
| 403 | Staff không được phân công vào event |
| 404 | Register team không tồn tại |