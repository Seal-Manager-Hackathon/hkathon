# GET /api/v1/mentor/register-teams/{registerTeamId}/scores

> Mentor xem điểm của 1 register team qua tất cả các round — mỗi round có submission cuối kèm tổng điểm.

**Controller:** [MentorScoreController.cs](Controllers/Mentor/MentorScoreController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/mentor/register-teams/{registerTeamId}/scores`

- Mentor muốn xem điểm của 1 team qua tất cả các round trong event.
- Mỗi round trả về submission cuối cùng (theo `SubmittedAt` giảm dần) và tổng điểm của submission đó (= SUM Scores.TotalScore).
- **Phải check** mentor có assign vào track của register team đó không (403 nếu ko).
- Không phân trang — trả về tất cả round của register team.

## Phân quyền
- ✅ Mentor — phải assign vào track tương ứng

## Request

### Route Parameters
| Parameter | Type | Bắt buộc | Ví dụ |
|-----------|------|----------|-------|
| registerTeamId | Guid | Có | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)
```json
{
  "data": {
    "registerTeamId": "guid",
    "eventId": "guid",
    "eventName": "Hackathon 2026",
    "trackId": "guid",
    "trackTitle": "AI - Trí tuệ nhân tạo",
    "topicId": "guid",
    "topicTitle": "Chatbot",
    "rounds": [
      {
        "roundId": "guid",
        "roundNo": 1,
        "roundName": "Vòng 1",
        "totalScore": 250.00,
        "submissionId": "guid",
        "submissionUrl": "https://...",
        "submittedAt": "2026-07-08T10:00:00Z",
        "judgeCount": 3
      },
      {
        "roundId": "guid",
        "roundNo": 2,
        "roundName": "Vòng 2",
        "totalScore": null,
        "submissionId": null,
        "submissionUrl": null,
        "submittedAt": null,
        "judgeCount": 0
      }
    ]
  },
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "...",
  "timestampUtc": "2026-07-11T00:00:00Z"
}
```

### Field ý nghĩa

| Field | Ý nghĩa |
|-------|---------|
| `registerTeamId` | ID đăng ký của team trong event |
| `eventId` / `eventName` | Event |
| `trackId` / `trackTitle` | Track team đăng ký |
| `topicId` / `topicTitle` | Topic team chọn |
| `rounds[].roundNo` | Số thứ tự round |
| `rounds[].totalScore` | scopeScore = SUM(Scores.TotalScore) — tổng điểm submission cuối. null nếu chưa có submission |
| `rounds[].submissionId` | ID của submission cuối cùng trong round |
| `rounds[].submissionUrl` | URL bài nộp cuối cùng |
| `rounds[].submittedAt` | Thời gian nộp submission cuối |
| `rounds[].judgeCount` | Số judge đã chấm bài này |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn |
| 403 | You Are Not Assigned to This Track | Mentor ko có quyền với track này |
| 404 | Register Team Not Found | registerTeamId ko tồn tại |

> **Ref:** API riêng của Mentor.
