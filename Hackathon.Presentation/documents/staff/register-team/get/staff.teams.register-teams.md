# GET /api/v1/staff/teams/{teamId}/register-teams — DS register teams của 1 team

## Mục đích

Staff xem danh sách các event mà 1 team đã đăng ký tham gia.

## Endpoint

```
GET /api/v1/staff/teams/{teamId}/register-teams
```

## Controller

`StaffRegisterTeamController.GetRegisterTeamsByTeam()` → `IRegisterTeamService.GetRegisterTeamsByTeam()`.

## Response

Giống `get-register-teams.md` — dùng `RegisterTeamCard[]`.

## Error Handling

| Status | Meaning |
|--------|---------|
| 401 | Token hết hạn/thiếu |
| 403 | Không có role Staff |