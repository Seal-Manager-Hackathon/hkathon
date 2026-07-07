# GET api/v1/admin/assign/events/{eventId}/staff/available

Lấy danh sách staff chưa được phân công vào event, chưa bị ban, chưa bị disable.

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event |

### Query Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| Keyword | string | No | Search theo email hoặc fullname |
| PageIndex | int | No (default: 1) | Trang hiện tại |
| PageSize | int | No (default: 10) | Số lượng item mỗi trang |

## Response

```json
{
  "data": {
    "items": [
      {
        "id": "guid",
        "email": "staff@example.com",
        "firstName": "John",
        "lastName": "Doe",
        "avatarUrl": "https://robohash.org/...",
        "college": "FPT University",
        "phoneNumber": "0123456789"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "traceId": "..."
}
```

## Error

- `401` — Unauthorized
- `400` — PageIndex/PageSize invalid
