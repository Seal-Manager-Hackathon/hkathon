# GET /api/v1/mentor/rounds/{roundId}/register-teams/{registerTeamId}/scores

> Mentor xem điểm cuối cùng của 1 team trong 1 round, team phải thuộc track mentor quản lý.
> **Controller:** `MentorScoreController` — `GET /api/v1/mentor/rounds/{roundId}/register-teams/{registerTeamId}/scores`

## Nghiệp vụ

- Mentor muốn xem **điểm cuối cùng** của 1 team (register team) trong 1 round.
- Điểm = `SUM(Scores.TotalScore)` của **submission cuối cùng** trong round (dùng `SubmissionHelper.GetLastSubmission`).
- **Phải check** mentor có assign vào track của register team đó không (403 nếu ko).
- Response giống hệt Admin `GET /api/v1/admin/rounds/{roundId}/register-teams/{registerTeamId}/scores`.

## Phân quyền
- ✅ Mentor — phải assign vào track tương ứng

## Request
| Parameter | Type | Bắt buộc | Ví dụ |
|-----------|------|----------|-------|
| roundId | Guid | Có | ... |
| registerTeamId | Guid | Có | ... |

## Response (200)
```json
{
  "data": {
    "roundId": "guid",
    "registerTeamId": "guid",
    "eventId": "guid",
    "eventName": "Hackathon 2026",
    "trackId": "guid",
    "trackTitle": "Tri tue nhan tao",
    "topicId": null,
    "topicTitle": null,
    "totalScore": 85.50,
    "submissionId": "guid",
    "submittedAt": "2026-07-10T10:00:00Z",
    "isLastSubmission": true
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
| 403 | You Are Not Assigned... | Mentor ko có quyền |
| 404 | Register Team Not Found | registerTeamId ko tồn tại |
| 404 | Resource Not Found | roundId + registerTeamId ko có điểm |

> **Ref:** [Admin API tương ứng](/api/v1/admin/rounds/{roundId}/register-teams/{registerTeamId}/scores)
