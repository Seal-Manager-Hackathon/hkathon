# GET /api/v1/lecturer/rounds/{roundId}/register-teams/{registerTeamId}/scores

> Lecturer xem điểm tổng (scopeScore) của 1 team trong 1 round (giống Admin, auth Lecturer).

**Controller:** [LecturerScoreController.cs](Controllers/Lecturer/LecturerScoreController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/rounds/{roundId}/register-teams/{registerTeamId}/scores`

- Giống hệt Admin `GET /api/v1/admin/rounds/{roundId}/register-teams/{registerTeamId}/scores`, khác auth là Lecturer.
- scopeScore = SUM(Scores.TotalScore) của submission cuối cùng trong round.
- 404 nếu roundId + registerTeamId không có điểm.

## Phân quyền
- ✅ Lecturer

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| roundId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |
| registerTeamId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)
```json
{
  "data": {
    "roundId": "guid",
    "registerTeamId": "guid",
    "eventId": "guid",
    "eventName": "Hackathon 2026",
    "trackId": "guid",
    "trackTitle": "AI",
    "topicId": "guid",
    "topicTitle": "Chatbot",
    "totalScore": 250.00,
    "submissionId": "guid",
    "submittedAt": "2026-07-08T10:00:00Z",
    "isLastSubmission": true
  },
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Resource Not Found | roundId + registerTeamId ko có điểm |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |

> **Ref:** [Admin API tương ứng](/api/v1/admin/rounds/{roundId}/register-teams/{registerTeamId}/scores)
