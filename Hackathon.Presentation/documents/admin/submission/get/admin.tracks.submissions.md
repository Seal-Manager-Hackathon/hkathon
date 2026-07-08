# GET /api/v1/admin/tracks/{trackId}/submissions

> Admin lấy danh sách bài nộp theo track.

## Giải thích nghiệp vụ

API này xem tất cả bài nộp của các team đã đăng ký 1 track cụ thể, trong tất cả round của event.

Mỗi item là 1 **RoundDetails** — 1 team trong 1 round:
- Lọc các team có `TrackId = trackId`
- Trả về tất cả round của các team đó

```
Track
  └── RegisterTeam (team đăng ký track)
        └── RoundDetails (round 1)
        │     ├── lastSubmission (bài cuối)
        │     └── records (lịch sử)
        └── RoundDetails (round 2)
              ├── lastSubmission (bài cuối)
              └── records (lịch sử)
```

## Phân quyền
- ✅ Admin

## Request

| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| trackId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

| Param | Kiểu | Bắt buộc | Mô tả |
|-------|------|----------|-------|
| pageIndex | int | ❌ | Mặc định 1 |
| pageSize | int | ❌ | Mặc định 10, tối đa 100 |

## Response (200)

```json
{
  "data": {
    "items": [
      {
        "registerTeamId": "guid",
        "teamId": "guid",
        "teamName": "Tên team",
        "eventId": "guid",
        "eventName": "Tên event",
        "roundId": "guid",
        "roundName": "Vòng 1",
        "trackId": "guid",
        "trackTitle": "Tên track",
        "topicId": "guid",
        "topicTitle": "Tên topic",
        "submittedBy": {
          "userId": "guid",
          "email": "leader@email.com",
          "firstName": "Nguyễn",
          "lastName": "Văn A"
        },
        "lastSubmission": {
          "id": "guid",
          "submittedAt": "2026-07-08T10:00:00Z",
          "url": "https://example.com/submission.pdf",
          "description": "Bài nộp cuối",
          "status": "Submitted"
        },
        "records": [
          {
            "id": "guid",
            "submittedAt": "2026-07-08T09:00:00Z",
            "url": "https://example.com/draft.pdf",
            "description": "Bản nháp",
            "status": "Submitted"
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
  "traceId": "00-abc123..."
}
```

### Field ý nghĩa

| Field | Ý nghĩa |
|-------|---------|
| `registerTeamId` | ID đăng ký của team trong event |
| `teamId` / `teamName` | Thông tin team |
| `eventId` / `eventName` | Event chứa round này |
| `roundId` / `roundName` | Round hiện tại |
| `trackId` / `trackTitle` | Track của team (luôn khớp với trackId filter) |
| `topicId` / `topicTitle` | Topic team chọn |
| `submittedBy` | Leader của team |
| `lastSubmission` | Bài nộp cuối cùng — dùng tính điểm |
| `records[]` | Lịch sử bài nộp trong round |

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
| 404 | Track Not Found | trackId ko tồn tại |
