# GET /api/v1/rounds/{roundId}

> Xem chi tiết một round — chỉ cần đăng nhập. Chỉ truyền roundId, không cần eventId.

## Nghiệp vụ

- Bất kỳ user nào đã đăng nhập đều có thể xem chi tiết round.
- Response giống hệt Admin `GET /api/v1/admin/rounds/{roundId}`.

## Phân quyền
- ✅ Authenticated (chỉ cần đăng nhập)

## Request
| Parameter | Type | Bắt buộc | Ghi chú |
|-----------|------|----------|---------|
| roundId | Guid | ✅ | ID của round |

## Response (200)
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
  "status": 200,
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 404 | Resource Not Found | roundId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/rounds/{roundId})
