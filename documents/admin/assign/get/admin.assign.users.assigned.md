# GET api/v1/admin/assign/users/assigned

Lấy danh sách tất cả user đã được phân công vào event (toàn bộ event), kèm thông tin user, event role, assign tracks, và event.

## Request

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

## Notes

- Filter `EventRole` lọc theo **event role** (Judge/Mentor/Staff), **không phải user role**
- Nếu ko truyền EventRole → lấy tất cả
- Keyword search theo email hoặc fullname (FirstName + LastName)
