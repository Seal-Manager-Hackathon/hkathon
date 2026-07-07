# POST api/v1/admin/assign/events/{eventId}/staff

Phân công 1 staff vào event với EventRole = Staff.

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event |

### Body (JSON)
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| UserId | Guid | Yes | ID của staff |

## Response

```json
{
  "data": null,
  "message": "Created Successfully",
  "status": 201,
  "traceId": "..."
}
```

## Error

- `401` — Unauthorized
- `404` — User Not Found / Event Role Staff Not Found
- `409` — User Is Already Assigned To This Event

## Logic

1. Check user tồn tại
2. Check user chưa được assign vào event này
3. Lấy EventRole có Name = Staff
4. Tạo AssignEvents với EventId, UserId, EventRoleId = Staff
