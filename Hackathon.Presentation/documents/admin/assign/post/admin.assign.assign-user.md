# POST api/v1/admin/assign/events/{eventId}/assign/users

Phân công 1 user (Staff hoặc Lecturer) vào event, kèm EventRole.

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event |

### Body (JSON)
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| UserId | Guid | Yes | ID của user |
| EventRole | string | Yes | `Staff` (cho Staff), `Judge` hoặc `Mentor` (cho Lecturer) |

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
- `404` — User Not Found / Event Role Not Found
- `409` — User Is Already Assigned To This Event
- `400` — Cannot Assign Student/Admin / Staff chỉ được Staff / Lecturer ko được Staff

## Logic

1. Check user tồn tại, role phải là **Staff** hoặc **Lecturer**
2. Parse EventRole từ request
3. **Staff** → chỉ được chọn EventRole = `Staff`
4. **Lecturer** → chỉ được chọn EventRole = `Judge` hoặc `Mentor`
5. Check chưa assign
6. Lấy EventRole từ DB và tạo AssignEvents
