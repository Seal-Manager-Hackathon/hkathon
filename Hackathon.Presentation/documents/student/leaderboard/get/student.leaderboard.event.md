# GET /api/v1/student/events/{eventId}/leaderboard

> Student xem bảng xếp hạng 1 event (eventScore từng team), chỉ hiển thị leaderboard không bị disable.

**Controller:** [StudentLeaderboardController.cs](../../../Controllers/Student/StudentLeaderboardController.cs)

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId}/leaderboard)

## Nghiệp vụ

Student muốn xem bảng xếp hạng của 1 event cụ thể:
- Chỉ hiển thị nếu leaderboard của event **không bị disable** (IsDisable = false).
- Trả về danh sách team xếp hạng theo eventScore giảm dần (DENSE_RANK).
- `eventScore` = weighted average scopeScore của các round.
- Kèm chi tiết điểm từng round.
- Hỗ trợ phân trang.

## Phân quyền
- ✅ Public (không cần đăng nhập)

## Request

| Param | Kiểu | Bắt buộc | Mặc định | Ví dụ |
|-------|------|----------|----------|-------|
| eventId | guid | ✅ (route) | - | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |
| pageIndex | int | ❌ | 1 | `1` |
| pageSize | int | ❌ | 10 | `10` |

## Response (200)
```json
{
  "data": {
    "eventId": "guid",
    "eventName": "Hackathon 2026",
    "totalRounds": 3,
    "isDisable": false,
    "items": [
      {
        "rank": 1,
        "registerTeamId": "guid",
        "teamId": "guid",
        "teamName": "FTeam",
        "trackId": "guid",
        "trackTitle": "Web3",
        "topicId": null,
        "topicTitle": null,
        "eventScore": 72.33,
        "roundScores": [
          {
            "roundNo": 1,
            "roundName": "Vòng 1",
            "scopeScore": 80.50
          },
          {
            "roundNo": 2,
            "roundName": "Vòng 2",
            "scopeScore": 75.00
          },
          {
            "roundNo": 3,
            "roundName": "Vòng 3 (CK)",
            "scopeScore": 62.50
          }
        ]
      }
    ],
    "totalCount": 10,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-..."
}
```

### Scoring
- `eventScore` = weighted average scopeScores (weight_i = 1, mẫu số = tổng số round event)
- Team không tham gia round = 0 điểm trong mẫu số

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 200 | Fetched Successfully | Nếu leaderboard bị disable → response data = null |
