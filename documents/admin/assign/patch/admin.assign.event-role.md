# PATCH api/v1/admin/assign/event-assigns/{assignEventId}/event-role

Gán event role (Judge/Mentor) cho 1 lecturer đã được phân công vào event.

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| assignEventId | Guid | ID của bản ghi phân công (AssignEvents) |

### Body (JSON)
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| EventRole | string | Yes | `Judge` hoặc `Mentor` |

## Response

```json
{
  "data": null,
  "message": "Updated Successfully",
  "traceId": "..."
}
```

## Error

- `401` — Unauthorized
- `404` — Assign Event Not Found / Event Role Not Found
- `400` — User Is Not A Lecturer / Invalid Event Role / Cannot Assign Staff Role To Lecturer

## Logic

1. Kiểm tra AssignEvent tồn tại (include User để check role)
2. Kiểm tra User phải có role = Lecturer
3. Parse EventRole: chỉ nhận `Judge` hoặc `Mentor`, ko nhận `Staff`
4. Lấy EventRole từ DB theo tên
5. Gán EventRoleId vào AssignEvents
