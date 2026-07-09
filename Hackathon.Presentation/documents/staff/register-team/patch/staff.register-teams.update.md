# PATCH /api/v1/staff/register-teams/{registerTeamId} — Cập nhật team

## Mục đích

Staff cập nhật thông tin register team: description, rejectionReason, status, IsBanned, IsDisable.

## Endpoint

```
PATCH /api/v1/staff/register-teams/{registerTeamId}
```

## Request Body

```json
{
  "description": "Mô tả mới",
  "rejectionReason": null,
  "status": "Approved",
  "isBanned": false,
  "isDisable": false
}
```

## Error Handling

| Status | Meaning |
|--------|---------|
| 400 | Status không hợp lệ |
| 401 | Token hết hạn/thiếu |
| 403 | Staff không được phân công vào event |
| 404 | Register team không tồn tại |