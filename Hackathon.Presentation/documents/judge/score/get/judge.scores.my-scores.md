# GET /api/v1/judge/scores/me

> Judge xem lại danh sách điểm mình đã chấm trong 1 event, có filter + phân trang.

**Controller:** `JudgeController`

## Nghiệp vụ

- Lấy tất cả scores của judge trong event.
- Có filter theo trackId và isGraded.
- Có phân trang.

## Phân quyền
- ✅ Judge — phải được assign vào event

## Request

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| eventId | Guid | Có | - | ID của event (query param) |
| trackId | Guid | No | - | Lọc theo track |
| isGraded | bool | No | - | Lọc đã chấm/chưa chấm |
| pageIndex | int | No | 1 | Trang số |
| pageSize | int | No | 10 | Số item (max 100) |

## Response (200)
```json
{
  "data": {
    "items": [
      {
        "scoreId": "guid",
        "submissionId": "guid",
        "trackId": "guid",
        "trackTitle": "Tri tue nhan tao",
        "teamId": "guid",
        "teamName": "Team ABC",
        "totalScore": 85.0,
        "isRetake": false,
        "isMock": false,
        "submittedAt": "2026-07-11T10:00:00Z",
        "updatedAt": "2026-07-11T12:00:00Z"
      }
    ],
    "totalCount": 10,
    "pageIndex": 1,
    "pageSize": 10
  }
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You Are Not Assigned as Judge for This Event | Ko phải Judge |
| 404 | Event Not Found or You Are Not Assigned | eventId ko tồn tại |
