# GET /api/v1/staff/rounds/{roundId}/register-teams/{registerTeamId}/scores

> Staff xem **điểm tổng (scopeScore)** của 1 team trong 1 round.

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

- Staff phải được phân công vào event của round này

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

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
> - [GET /submissions/{submissionId}/grader-scores](staff.scores.grader-scores.md) — từng judge

## Lỗi

| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Staff hoặc không được phân công vào event | Ẩn chức năng |
| 404 | Resource Not Found | registerTeamId/roundId không tồn tại | Hiển thị thông báo |

> **Ref:** [Admin API tương ứng](/api/v1/admin/rounds/{roundId}/register-teams/{registerTeamId}/scores) — [`admin/score/get/admin.scores.team-round-score.md`](../../../admin/score/get/admin.scores.team-round-score.md)