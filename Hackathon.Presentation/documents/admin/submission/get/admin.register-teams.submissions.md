# GET /api/v1/admin/register-teams/{registerTeamId}/submissions

> Admin lấy danh sách bài nộp của 1 team (register team) trong event, phân theo từng round.

## Nghiệp vụ
- Trả về bài nộp của team đó trong tất cả các round của event
- Nếu truyền `roundId` → chỉ lấy bài nộp của team trong round đó
- Mỗi item là 1 team trong 1 round
- Gồm thông tin team, event, round, track, topic, người nộp
- **LastSubmission:** bài nộp cuối cùng (mới nhất) của team trong round đó
- **Records:** toàn bộ lịch sử bài nộp của team trong round đó

## Phân quyền
- ✅ Admin

## Request

| Param          | Kiểu | Bắt buộc | Ví dụ                                  |
|----------------|------|----------|----------------------------------------|
| registerTeamId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

| Param     | Kiểu | Bắt buộc | Mô tả                                        |
|-----------|------|----------|----------------------------------------------|
| roundId   | guid | ❌        | Lọc theo round (ko truyền = lấy tất cả round) |
| pageIndex | int  | ❌        | Mặc định 1                                   |
| pageSize  | int  | ❌        | Mặc định 10, tối đa 100                      |

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

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
| 404 | Register Team Not Found | registerTeamId ko tồn tại |
