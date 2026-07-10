# GET /api/v1/judge/events/{eventId}/submissions/pending

> Judge lấy danh sách bài nộp chưa chấm trong 1 event.

**Controller:** `JudgeController`

## Nghiệp vụ

- Lấy các bài nộp mà judge hiện tại chưa chấm điểm.
- Duyệt qua submissions, loại bỏ những bài đã có score của judge này.
- Có filter trackId và roundId.

## Phân quyền
- ✅ Judge — phải được assign vào event

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| trackId | Guid | No | - | Lọc theo track |
| roundId | Guid | No | - | Lọc theo round |
| pageIndex | int | No | 1 | Trang số |
| pageSize | int | No | 10 | Số item (max 100) |

## Response (200)
```json
{
  "data": {
    "items": [
      {
        "submissionId": "guid",
        "roundDetailId": "guid",
        "roundId": "guid",
        "roundName": "Vong 1",
        "teamId": "guid",
        "teamName": "Team ABC",
        "url": "https://...",
        "status": "Submitted",
        "submittedAt": "2026-07-11T10:00:00Z",
        "gradingStatus": "Pending"
      }
    ],
    "totalCount": 3,
    "pageIndex": 1,
    "pageSize": 10
  }
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You Are Not Assigned as Judge for This Event | User ko phải Judge |
| 404 | Event Not Found or You Are Not Assigned | eventId ko tồn tại |
