# GET /api/v1/staff/rounds/{roundId}/leaderboard

> Staff xem bảng xếp hạng của 1 round trong event được phân công.

## Nghiệp vụ

- Trả về danh sách tất cả register team có trong round (qua RoundDetails)
- Mỗi team chỉ lấy **bài nộp cuối cùng** (theo SubmittedAt) trong round đó
- Điểm của team = tổng `TotalScore` của tất cả Scores của bài nộp cuối cùng
- Sắp xếp theo điểm **từ cao xuống thấp**
- Phân trang — **thứ hạng (Rank) được tính theo page**
- Staff phải được assign vào event của round này

## Phân quyền

- ✅ Staff (phải được assign vào event)

## Request

| Param   | Kiểu | Bắt buộc | Ví dụ |
|---------|------|----------|-------|
| roundId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

| Param     | Kiểu | Bắt buộc | Mô tả |
|-----------|------|----------|-------|
| pageIndex | int  | ❌        | Mặc định 1 |
| pageSize  | int  | ❌        | Mặc định 10, tối đa 100 |

## Response (200)

```json
{
  "data": {
    "roundId": "guid",
    "roundName": "Vòng 1",
    "eventId": "guid",
    "eventName": "Tên event",
    "items": [
      {
        "rank": 1,
        "registerTeamId": "guid",
        "teamId": "guid",
        "teamName": "Team A",
        "trackId": "guid",
        "trackTitle": "AI - Trí tuệ nhân tạo",
        "topicId": "guid",
        "topicTitle": "Chatbot hỗ trợ học tập",
        "lastSubmissionId": "guid",
        "totalScore": 95.5
      },
      {
        "rank": 2,
        "registerTeamId": "guid",
        "teamId": "guid",
        "teamName": "Team B",
        "trackId": "guid",
        "trackTitle": "Blockchain",
        "topicId": null,
        "topicTitle": null,
        "lastSubmissionId": "guid",
        "totalScore": 80.0
      }
    ],
    "totalCount": 20,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Round Not Found | roundId ko tồn tại |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | You Are Not Assigned to This Event | Staff không được assign vào event |