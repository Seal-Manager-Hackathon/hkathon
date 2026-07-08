# GET /api/v1/admin/register-teams/{registerTeamId}/submissions

> Admin lấy danh sách bài nộp của 1 team trong event, phân theo từng round.

## Giải thích nghiệp vụ

API này xem tất cả bài nộp của **1 team** (register team) xuyên suốt các round của event.

Mỗi item là 1 **RoundDetails** — team ở 1 round:
- Nếu ko truyền `roundId` → trả về tất cả round mà team đã tham gia
- Nếu truyền `roundId` → chỉ lấy round đó

```
RegisterTeam (1 team trong event)
  └── RoundDetails (round 1)
  │     ├── lastSubmission (bài cuối round 1)
  │     └── records (lịch sử nộp round 1)
  └── RoundDetails (round 2)
        ├── lastSubmission (bài cuối round 2)
        └── records (lịch sử nộp round 2)
```

## Phân quyền
- ✅ Admin

## Request

| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| registerTeamId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

| Param | Kiểu | Bắt buộc | Mô tả |
|-------|------|----------|-------|
| roundId | guid | ❌ | Lọc theo round (ko truyền = lấy tất cả round) |
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
| `lastSubmission` | Bài nộp cuối cùng trong round — dùng tính điểm |
| `records[]` | Lịch sử tất cả bài nộp trong round |

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
| 404 | Register Team Not Found | registerTeamId ko tồn tại |
