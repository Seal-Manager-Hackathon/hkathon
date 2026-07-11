# GET /api/v1/judge/events/{eventId}/scores/me

> Judge xem danh sách bài nộp của các team trong event, kèm điểm của chính judge đó (nếu đã chấm). Hỗ trợ lọc theo round, track, và trạng thái đã chấm/chưa chấm.

**Controller:** [JudgeController.cs](Controllers/Judge/JudgeController.cs)

## Nghiệp vụ

- Judge truyền **eventId trên route**, không phải query param.
- Lấy tất cả **submission cuối cùng** của các team trong event.
- Chỉ hiện các submission thuộc **track judge được phân công**.
- Mỗi item trả ra:
  - `registerTeamId`, `teamId`, `teamName`
  - `trackId`, `trackTitle`
  - `roundId`, `roundName`
  - `submissionId`, `url`, `submittedAt` — thông tin bài nộp
  - `scoreId`, `totalScore` — **nullable**: nếu judge đã chấm thì có giá trị, nếu chưa chấm để `null`
  - `gradingStatus`: `"Graded"` hoặc `"Pending"`
- Hỗ trợ lọc:
  - **roundId**: lọc theo round
  - **trackId**: lọc theo track (trong số các track judge được phân công)
  - **isGraded**: `true` = chỉ bài đã chấm, `false` = chỉ bài chưa chấm
- KHÔNG lấy điểm của judge khác — chỉ lấy score của chính judge đang request.
- Có phân trang.

## Phân quyền
- ✅ Judge — phải được assign vào event với role Judge

## Request

### Route Parameters
| Parameter | Type | Bắt buộc | Ví dụ |
|-----------|------|----------|-------|
| eventId | Guid | ✅ | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| roundId | Guid | No | - | Lọc theo round |
| trackId | Guid | No | - | Lọc theo track |
| isGraded | bool | No | - | `true` = đã chấm, `false` = chưa chấm |
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
        "trackId": "guid",
        "trackTitle": "Tri tue nhan tao",
        "roundId": "guid",
        "roundName": "Vong 1",
        "submissionId": "guid",
        "url": "https://example.com/submission.pdf",
        "submittedAt": "2026-07-11T10:00:00Z",
        "gradingStatus": "Graded",
        "scoreId": "guid",
        "totalScore": 85.0
      },
      {
        "registerTeamId": "guid",
        "teamId": "guid",
        "teamName": "Team XYZ",
        "trackId": "guid",
        "trackTitle": "Tri tue nhan tao",
        "roundId": "guid",
        "roundName": "Vong 1",
        "submissionId": "guid",
        "url": null,
        "submittedAt": "2026-07-11T09:00:00Z",
        "gradingStatus": "Pending",
        "scoreId": null,
        "totalScore": null
      }
    ],
    "totalCount": 10,
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
| 403 | You Are Not Assigned as Judge for This Event | Ko phải Judge |
| 404 | Event Not Found or You Are Not Assigned | eventId ko tồn tại |
