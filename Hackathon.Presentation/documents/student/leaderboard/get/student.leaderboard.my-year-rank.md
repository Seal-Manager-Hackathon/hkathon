# GET /api/v1/student/leaderboard/my-year-rank

> Student coi thứ hạng team của mình trong bảng xếp hạng chapter năm.

**Controller:** [StudentLeaderboardController.cs](../../../Controllers/Student/StudentLeaderboardController.cs)

## Nghiệp vụ

Student muốn xem team của mình đang đứng thứ bao nhiêu trong năm (chapter):
- Lấy danh sách các team mà user đang là member active (không disable, Status = Active, team không disable).
- Tìm các team đó trong chapter leaderboard của năm được chỉ định.
- Trả về thông tin: rank, chapterScore, eventCount cho từng team của user.
- Nếu user không có team active nào → trả về mảng rỗng.

## Phân quyền
- ✅ Student (cần token để lấy user ID)

## Request

| Param | Kiểu | Bắt buộc | Mặc định | Ví dụ |
|-------|------|----------|----------|-------|
| year | int | ✅ (query) | - | `2026` |

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
        "eventCount": 2
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
| 200 | Fetched Successfully | teams rỗng nếu user ko có team active |
