# GET api/v1/admin/rounds/{roundId}

Lấy toàn bộ thông tin chi tiết của 1 round.

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| roundId | Guid | ID của round |

## Response

```json
{
  "data": {
    "id": "guid",
    "eventId": "guid",
    "eventName": "Hackathon 2026",
    "name": "Vòng 1",
    "description": "Vòng loại",
    "roundNo": 1,
    "startTime": "2026-07-10T00:00:00Z",
    "endTime": "2026-07-15T00:00:00Z",
    "startSubmission": "2026-07-11T00:00:00Z",
    "endSubmission": "2026-07-14T00:00:00Z",
    "limitTeam": 50,
    "isDisable": false,
    "createdAt": "2026-07-01T00:00:00Z",
    "updatedAt": "2026-07-01T00:00:00Z"
  },
  "message": "Fetched Successfully",
  "traceId": "..."
}
```

## Error

- `401` — Unauthorized
- `404` — Round Not Found
