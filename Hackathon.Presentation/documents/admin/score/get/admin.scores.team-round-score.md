# GET /api/v1/admin/rounds/{roundId}/register-teams/{registerTeamId}/scores

> Admin xem điểm của 1 team trong 1 round — điểm tổng (scopeScore), điểm từng judge, điểm trung bình từng tiêu chí.

Dữ liệu được tính on-the-fly từ ScoreItems, **không lưu** vào DB scope riêng.

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
    "totalScore": 72.33,
    "graderScores": [
      {
        "scoreId": "guid",
        "assignTrackId": "guid",
        "trackTitle": "Web3",
        "totalScore": 75.00,
        "isRetake": false,
        "isMock": false,
        "graderName": "Nguyễn Văn B",
        "graderEmail": "lecturer@email.com",
        "createdAt": "2026-07-07T12:00:00Z"
      }
    ],
    "criteriaAverages": [
      {
        "criteriaItemId": "guid",
        "criteriaName": "Tính sáng tạo",
        "averageScore": 24.5,
        "judgeCount": 2
      }
    ]
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

### Field ý nghĩa

| Field | Ý nghĩa |
|-------|---------|
| `totalScore` | **scopeScore** = SUM(criteriaAvg) — tổng điểm team trong round |
| `graderScores[]` | Điểm từng judge (1 record/Score → 1 judge) |
| `criteriaAverages[]` | **criteriaAvg** = AVG(judgeScore) GROUP BY CriteriaItemId |
| `judgeCount` | Số judge đã chấm tiêu chí này (chỉ đếm judge có ScoreItem) |

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Resource Not Found | roundId + registerTeamId ko tồn tại hoặc IsDisable |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
