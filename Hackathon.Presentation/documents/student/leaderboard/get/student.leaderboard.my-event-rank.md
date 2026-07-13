# GET /api/v1/student/events/{eventId}/leaderboard/my-rank

> Student coi thứ hạng của team mình trong bảng xếp hạng event.

**Controller:** [StudentLeaderboardController.cs](../../../Controllers/Student/StudentLeaderboardController.cs)

## Nghiệp vụ

Student muốn xem team của mình đang đứng thứ bao nhiêu trong 1 event cụ thể:
- Lấy danh sách các team mà user đang là member active.
- Tìm các team đó trong event leaderboard (chỉ lấy leaderboard không bị disable).
- Trả về thông tin: rank, eventScore, roundScores cho từng team.
- Nếu leaderboard bị disable → response null.
- Nếu user không có team active trong event → mảng rỗng.

## Phân quyền
- ✅ Student (cần token để lấy user ID)

## Request

| Param | Kiểu | Bắt buộc | Mặc định | Ví dụ |
|-------|------|----------|----------|-------|
| eventId | guid | ✅ (route) | - | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)
```json
{
  "data": {
    "eventId": "guid",
    "eventName": "Hackathon 2026",
    "teams": [
      {
        "rank": 3,
        "registerTeamId": "guid",
        "teamId": "guid",
        "teamName": "FTeam",
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
    ]
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 200 | Fetched Successfully | teams rỗng nếu user ko có team active; response data = null nếu leaderboard bị disable |
