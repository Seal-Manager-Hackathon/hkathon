# POST api/v1/admin/teams/{teamId}/unlock

Mở khóa team — set `CanEdit = true`, team có thể chỉnh sửa lại.

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| teamId | Guid | ID của team |

### Body
Không cần body.

## Response

```json
{
  "data": null,
  "message": "Team Unlocked Successfully",
  "status": 200,
  "traceId": "..."
}
```

## Error

| Status | Message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
| 404 | Team Not Found | teamId ko tồn tại |
| 400 | Team Is Already Unlocked | Team đã unlocked rồi |
