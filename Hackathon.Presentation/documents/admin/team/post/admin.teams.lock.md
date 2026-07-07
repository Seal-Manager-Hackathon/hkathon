# POST api/v1/admin/teams/{teamId}/lock

Khóa team — set `CanEdit = false`, team ko thể chỉnh sửa.

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
  "message": "Team Locked Successfully",
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
| 400 | Team Is Already Locked | Team đã locked rồi |
