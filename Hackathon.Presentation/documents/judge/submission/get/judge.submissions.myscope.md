# GET /api/v1/judge/events/{eventId}/myscope

> Judge xem tất cả bài nộp trong event được phân công theo track, có lọc theo track, round, và status (graded/pending).

**Controller:** [JudgeController.cs](Controllers/Judge/JudgeController.cs)

## Nghiệp vụ

- Lấy tất cả submissions của các team trong event mà judge được phân công (assign track).
- Mỗi team chỉ hiển thị **submission cuối cùng** trong round.
- `submittedBy` = **leader của team** (`TeamDetails.IsLeader = true`).
- `status` filter: `"Graded"` = đã chấm, `"Submitted"` = chưa chấm. Nếu ko truyền → lấy hết.
- Kèm `scoreId` và `totalScore` nếu đã chấm, null nếu chưa.

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
| status | string | No | - | `Graded` hoặc `Pending`, ko truyền = all |
| pageIndex | int | No | 1 | Trang số |
| pageSize | int | No | 10 | Số item (max 100) |

## Response (200)
```json
{
  "data": {
    "items": [
      {
        "registerTeamId": "guid",
        "teamId": "guid",
        "teamName": "Team ABC",
        "eventId": "guid",
        "eventName": "Hackathon 2026",
        "roundId": "guid",
        "roundName": "Vong 1",
        "trackId": "guid",
        "trackTitle": "Tri tue nhan tao",
        "topicId": "guid",
        "topicTitle": "AI trong Y tế",
        "submittedBy": {
          "userId": "guid",
          "email": "leader@email.com",
          "firstName": "Nguyễn",
          "lastName": "Văn A"
        },
        "lastSubmission": {
          "id": "guid",
          "submittedAt": "2026-07-11T10:00:00Z",
          "url": "https://example.com/submission.pdf",
          "description": "Bài nộp cuối",
          "status": "Submitted"
        },
        "scoreId": null,
        "totalScore": null
      }
    ],
    "totalCount": 3,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-11T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You Are Not Assigned as Judge for This Event | User ko phải Judge |
| 404 | Event Not Found or You Are Not Assigned | eventId ko tồn tại |
