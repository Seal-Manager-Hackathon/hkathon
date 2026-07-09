# POST /api/v1/staff/register-teams/{registerTeamId}/unban — Bỏ cấm team

## Mục đích

Staff bỏ cấm 1 team, khôi phục trạng thái Approved.

## Business Logic

1. Kiểm tra staff có assign vào event không
2. Set `IsBanned = false`, status = `Approved`, xóa rejectionReason
3. Không cho phép unban nếu chưa bị banned

## Endpoint

```
POST /api/v1/staff/register-teams/{registerTeamId}/unban
```

## Error Handling

| Status | Meaning |
|--------|---------|
| 400 | Team chưa bị banned |
| 401 | Token hết hạn/thiếu |
| 403 | Staff không được phân công vào event |
| 404 | Register team không tồn tại |