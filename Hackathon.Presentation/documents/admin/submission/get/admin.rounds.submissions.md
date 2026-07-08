# GET /api/v1/admin/rounds/{roundId}/submissions

> Admin lấy danh sách bài nộp trong 1 round.

## Giải thích nghiệp vụ

Giống API submissions theo event nhưng đã filter sẵn theo 1 round cụ thể.

Mỗi item là 1 **RoundDetails** — tức 1 team tham gia round này:
- **records[]** — tất cả lần nộp bài của team trong round (nộp nháp, nộp chính thức, nộp lại...)
- **lastSubmission** — bài nộp cuối cùng (mới nhất), dùng để tính điểm scopeScore

```
Round → RoundDetails (team trong round)
            ├── lastSubmission (bài cuối → tính điểm)
            └── records (lịch sử nộp)
```

## Phân quyền
- ✅ Admin

## Request

| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| roundId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

| Param | Kiểu | Bắt buộc | Mô tả |
|-------|------|----------|-------|
| keyword | string | ❌ | Tìm kiếm theo tên team (ko dấu) |
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
| `trackId` / `trackTitle` | Track team đã đăng ký |
| `topicId` / `topicTitle` | Topic team chọn |
| `submittedBy` | Leader của team |
| `lastSubmission` | Bài nộp cuối cùng — dùng tính điểm |
| `records[]` | Lịch sử bài nộp của team trong round |

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
| 404 | Round Not Found | roundId ko tồn tại |
