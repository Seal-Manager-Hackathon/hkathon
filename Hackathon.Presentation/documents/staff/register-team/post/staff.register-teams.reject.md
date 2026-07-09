# POST /api/v1/staff/register-teams/{registerTeamId}/reject — Từ chối team

## Mục đích

Staff từ chối 1 team đang ở trạng thái `Pending`.

## Business Logic

1. Team phải có status = `Pending`
2. Set status = `Rejected` + lưu rejectionReason
3. Nếu team không còn register team nào approved → mở khóa team (`CanEdit = true`)

## Endpoint

```
POST /api/v1/staff/register-teams/{registerTeamId}/reject
```

## Request Body

```json
{
  "rejectionReason": "Thiếu thông tin"
}
```