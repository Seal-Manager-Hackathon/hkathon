# GET api/v1/admin/assign/events/{eventId}/users/assigned

Lấy danh sách user đã được phân công trong 1 event, kèm thông tin user, event role, assign tracks, và event. Có thể lọc theo EventRole.

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event |

### Query Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| Keyword | string | No | Search theo email hoặc fullname |
| EventRole | string | No | Lọc theo event role: `Mentor`, `Judge`, `Staff` |
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
        "eventName": "Hackathon 2026",
        "eventId": "guid",
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
- `400` — Invalid EventRole / PageIndex/PageSize invalid
