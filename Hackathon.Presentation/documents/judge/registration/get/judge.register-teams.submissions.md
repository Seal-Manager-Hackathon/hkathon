# GET /api/v1/judge/register-teams/{registerTeamId}/submissions

> Judge xem tất cả bài nộp qua các round của 1 register team (team đăng ký event) — mỗi round chỉ hiện submission cuối cùng, kèm điểm của judge hiện tại.

**Controller:** [JudgeController.cs](Controllers/Judge/JudgeController.cs)

## Nghiệp vụ

- Lấy toàn bộ round (không phân trang) của register team, mỗi round hiện **submission cuối cùng** (`lastSubmission`).
- Hiển thị trạng thái chấm của **chính judge hiện tại**: `Graded` / `Pending`.
- Nếu đã chấm → trả về `scoreId` và `totalScore`.
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
    "rounds": [
      {
        "roundId": "guid",
        "roundName": "Vòng 1",
        "roundNo": 1,
        "lastSubmission": {
          "id": "guid",
          "submittedAt": "2026-07-11T10:00:00Z",
          "url": "https://example.com/submission-v1.pdf",
          "description": "Bài nộp vòng 1",
          "status": "Submitted"
        },
        "gradingStatus": "Graded",
        "scoreId": "guid",
        "totalScore": 85.5
      },
      {
        "roundId": "guid",
        "roundName": "Vòng 2",
        "roundNo": 2,
        "lastSubmission": {
          "id": "guid",
          "submittedAt": "2026-07-12T10:00:00Z",
          "url": "https://example.com/submission-v2.pdf",
          "description": "Bài nộp vòng 2",
          "status": "Submitted"
        },
        "gradingStatus": "Pending",
        "scoreId": null,
        "totalScore": null
      }
    ],
    "totalCount": 2
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
| `roundId` / `roundName` / `roundNo` | Round và số thứ tự |
| `lastSubmission` | Bài nộp cuối cùng trong round (null nếu chưa có) |
| `gradingStatus` | `Graded` (đã chấm) / `Pending` (chưa chấm) |
| `scoreId` | ID score của judge hiện tại, null nếu chưa chấm |
| `totalScore` | Tổng điểm, null nếu chưa chấm |

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
