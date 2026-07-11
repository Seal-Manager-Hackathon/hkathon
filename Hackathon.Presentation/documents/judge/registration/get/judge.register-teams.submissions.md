# GET /api/v1/judge/register-teams/{registerTeamId}/submissions

> Judge xem **bài nộp mới nhất** của 1 register team (không phân biệt round), kèm điểm của judge hiện tại. Format giống track submissions.

**Controller:** [JudgeController.cs](Controllers/Judge/JudgeController.cs)

## Nghiệp vụ

- Chỉ lấy **1 bài nộp duy nhất** — bài mới nhất (`SubmittedAt` cao nhất) trong tất cả round của register team.
- Hiển thị trạng thái chấm của **chính judge hiện tại**: `Graded` / `Pending`.
- Nếu đã chấm → trả về `scoreId` và `totalScore`.
- Trả về thông tin: team, event, round, track, topic, submittedBy (leader).
- Judge phải được assign vào event chứa register team đó.

## Phân quyền
- ✅ Judge — phải được assign vào event của register team (EventRole = Judge)

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| registerTeamId | Guid | ID của register team |

## Response (200)

```json
{
  "data": {
    "registerTeamId": "guid",
    "teamId": "guid",
    "teamName": "Team ABC",
    "eventId": "guid",
    "eventName": "Hackathon 2026",
    "roundId": "guid",
    "roundName": "Vòng 2",
    "trackId": "guid",
    "trackTitle": "Trí tuệ nhân tạo",
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
      "submittedAt": "2026-07-12T10:00:00Z",
      "url": "https://example.com/submission.pdf",
      "description": "Bài nộp cuối",
      "status": "Submitted"
    },
    "gradingStatus": "Graded",
    "scoreId": "guid",
    "totalScore": 85.5
  },
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-12T12:00:00Z"
}
```

### Field ý nghĩa

| Field | Ý nghĩa |
|-------|---------|
| `registerTeamId` / `teamId` / `teamName` | Thông tin team đăng ký |
| `eventId` / `eventName` | Event |
| `roundId` / `roundName` | Round của bài nộp mới nhất |
| `trackId` / `trackTitle` | Track team đăng ký |
| `topicId` / `topicTitle` | Topic team chọn |
| `submittedBy` | Leader của team (người nộp bài) |
| `lastSubmission` | Bài nộp mới nhất (trong tất cả round) |
| `gradingStatus` | `Graded` (đã chấm) / `Pending` (chưa chấm) |
| `scoreId` | ID score của judge hiện tại, null nếu chưa chấm |
| `totalScore` | Tổng điểm, null nếu chưa chấm |

### submittedBy

| Field | Ý nghĩa |
|-------|---------|
| `userId` | ID của leader |
| `email` | Email |
| `firstName` / `lastName` | Tên leader |

### lastSubmission

| Field | Ý nghĩa |
|-------|---------|
| `id` | ID bài nộp |
| `submittedAt` | Thời gian nộp |
| `url` | Link nộp bài |
| `description` | Mô tả |
| `status` | Trạng thái (`Submitted`, ...) |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | Forbidden / You Are Not Assigned as Judge for This Event | Judge ko được assign |
| 404 | Register Team Not Found | registerTeamId ko tồn tại |

> **Ref:** [GET /api/v1/judge/tracks/{trackId}/submissions](judge.submissions.by-track.md) — danh sách submissions theo track
