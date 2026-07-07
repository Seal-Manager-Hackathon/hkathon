# POST api/v1/admin/assign/events/{eventId}/assign/users

Phân công 1 user (Staff hoặc Lecturer) vào event.

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event |

### Body (JSON)
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| UserId | Guid | Yes | ID của user |

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
- `400` — Cannot Assign Student/Admin To Event

## Logic

1. Check user tồn tại
2. Nếu role là **Student** hoặc **Admin** → throw lỗi
3. Check user chưa được assign vào event này
4. Nếu role là **Staff**:
   - Lấy EventRole có Name = Staff
   - Tạo AssignEvents với EventRoleId = Staff
5. Nếu role là **Lecturer**:
   - Tạo AssignEvents với EventRoleId = **null** (ko gán event role)
