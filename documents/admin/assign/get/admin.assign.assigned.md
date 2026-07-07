# GET api/v1/admin/assign/events/{eventId}/assigned

Lấy danh sách user (staff/lecturer) đã được phân công vào event, kèm event role và assign tracks.

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event |

### Query Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| Keyword | string | No | Search theo email hoặc fullname |
| Role | string | No | Lọc theo role: `Staff`, `Lecturer` |
| PageIndex | int | No (default: 1) | Trang hiện tại |
| PageSize | int | No (default: 10) | Số lượng item mỗi trang |

## Response

```json
{
  "data": {
    "items": [
      {
        "assignEventId": "guid",
        "userId": "guid",
        "email": "staff@example.com",
        "firstName": "John",
        "lastName": "Doe",
        "avatarUrl": "https://robohash.org/...",
        "eventRole": "Mentor",
        "assignTracks": [
          {
            "trackId": "guid",
            "title": "AI Track"
          }
        ]
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
