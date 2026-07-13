# GET /api/v1/student/events/chapter/{year}/leaderboard

> Student xem bảng xếp hạng chapter (1 năm) — điểm chapterScore từng team, chỉ hiển thị leaderboard đã publish.

**Controller:** [StudentLeaderboardController.cs](../../../Controllers/Student/StudentLeaderboardController.cs)

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/chapter/{year}/leaderboard)

## Nghiệp vụ

Student muốn xem bảng xếp hạng tổng kết 1 năm (chapter leaderboard):
- Chỉ hiển thị các leaderboard đã được **publish** (IsPublished = true) và **không bị disable** (IsDisable = false).
- Kết quả trả về danh sách team xếp hạng theo chapterScore giảm dần (DENSE_RANK).
- `chapterScore` = AVG(eventScore) của các event team đã tham gia trong năm.
- Hỗ trợ phân trang.

## Phân quyền
- ✅ Public (không cần đăng nhập)

## Request

| Param | Kiểu | Bắt buộc | Mặc định | Ví dụ |
|-------|------|----------|----------|-------|
| year | int | ✅ (route) | - | `2026` |
| pageIndex | int | ❌ | 1 | `1` |
| pageSize | int | ❌ | 10 | `10` |

## Response (200)
```json
{
  "data": {
    "year": 2026,
    "eventCount": 3,
    "items": [
      {
        "rank": 1,
        "teamId": "guid",
        "teamName": "FTeam",
        "chapterScore": 78.25,
        "eventCount": 2,
        "eventScores": [
          {
            "eventId": "guid",
            "eventName": "Hackathon Spring 2026",
            "registerTeamId": "guid",
            "eventScore": 72.33
          },
          {
            "eventId": "guid",
            "eventName": "Hackathon Summer 2026",
            "registerTeamId": "guid",
            "eventScore": 84.17
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
- `chapterScore` = AVG(eventScore) của các event team đã tham gia trong năm
- `eventScores[].eventScore` = eventScore của team trong từng event (weighted avg scopeScores)

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 200 | Fetched Successfully | Dữ liệu rỗng nếu chưa có leaderboard nào publish |
