# GET /api/v1/admin/rounds/{roundId}/register-teams/{registerTeamId}/scores

> Admin xem **điểm tổng (scopeScore)** của 1 team trong 1 round.

## Giải thích nghiệp vụ

API này trả về điểm tổng của 1 team trong 1 round — đây là điểm dùng để xếp hạng.

Cách tính:
1. Lấy **submission cuối cùng** của team trong round đó (team có thể nộp nhiều lần, lần cuối mới là chính thức)
2. Lấy tất cả **Scores** (lượt chấm của các judge) của submission đó
3. **scopeScore** = SUM(Scores.TotalScore)

```
Round → RoundDetails (team trong round)
  └── Submissions (team nộp nhiều lần)
        └── lastSubmission (bài cuối — lấy cái này)
              └── Scores (nhiều judge)
                    ├── Score: Judge A = 85
                    ├── Score: Judge B = 90
                    └── Score: Judge C = 75
              scopeScore = 85 + 90 + 75 = 250
```

## Phân quyền
- ✅ Admin

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
    "trackTitle": "Web3",
    "topicId": "guid",
    "topicTitle": "DeFi",
    "totalScore": 250.00,
    "submissionId": "guid",
    "submittedAt": "2026-07-08T10:00:00Z",
    "isLastSubmission": true
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

### Field ý nghĩa

| Field | Ý nghĩa |
|-------|---------|
| `roundId` | Round ID |
| `registerTeamId` | ID đăng ký của team trong event |
| `eventId` / `eventName` | Event |
| `trackId` / `trackTitle` | Track team đăng ký |
| `topicId` / `topicTitle` | Topic team chọn |
| `totalScore` | **scopeScore** = SUM(Scores.TotalScore) — tổng điểm submission cuối |
| `submissionId` | ID của submission cuối cùng (dùng để tính điểm) |
| `submittedAt` | Thời gian nộp submission cuối |
| `isLastSubmission` | Luôn `true` — xác nhận đây là bài cuối |

> Muốn xem điểm riêng từng judge? Dùng submissionId ở trên gọi:
> - [GET /submissions/{submissionId}/scores](admin.submissions.scores.md) — tổng điểm
> - [GET /submissions/{submissionId}/grader-scores](admin.submissions.grader-scores.md) — từng judge

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Resource Not Found | roundId + registerTeamId ko tồn tại hoặc IsDisable |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
