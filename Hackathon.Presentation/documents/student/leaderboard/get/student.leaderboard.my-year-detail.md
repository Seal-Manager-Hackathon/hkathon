# GET /api/v1/student/leaderboard/my-year-detail (bỏ)

> Student coi chi tiết thứ hạng team của mình trong năm, kèm điểm và hạng từng event.

**Controller:** [StudentLeaderboardController.cs](../../../Controllers/Student/StudentLeaderboardController.cs)

## Nghiệp vụ

Student muốn xem chi tiết thứ hạng của từng team mình tham gia trong năm:

- Lấy danh sách các team mà user đang là member active.
- Tìm các team đó trong chapter leaderboard của năm.
- **Kèm theo thứ hạng của team trong từng event** (EventRank) mà team đã tham gia trong năm.
- Không phân trang (1 năm tối đa chỉ có khoảng 4 event).

## Phân quyền

- ✅ Student (cần token để lấy user ID)

## Request

| Param | Kiểu | Bắt buộc   | Mặc định | Ví dụ  |
| ----- | ---- | ---------- | -------- | ------ |
| year  | int  | ✅ (query) | -        | `2026` |

## Response (200)

```json
{
  "data": {
    "year": 2026,
    "teams": [
      {
        "teamId": "guid",
        "teamName": "FTeam",
        "rank": 5,
        "chapterScore": 78.25,
        "eventScores": [
          {
            "eventId": "guid",
            "eventName": "Hackathon Spring 2026",
            "registerTeamId": "guid",
            "eventScore": 72.33,
            "eventRank": 3
          },
          {
            "eventId": "guid",
            "eventName": "Hackathon Summer 2026",
            "registerTeamId": "guid",
            "eventScore": 84.17,
            "eventRank": 1
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

| Status | message              | Khi nào                               |
| ------ | -------------------- | ------------------------------------- |
| 401    | Unauthorized         | Token hết hạn/thiếu                   |
| 200    | Fetched Successfully | teams rỗng nếu user ko có team active |
