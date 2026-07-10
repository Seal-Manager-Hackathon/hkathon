# GET /api/v1/staff/events/{eventId}/submissions

> Lấy danh sách submission của một event (có filter).

## Nghiệp vụ
- Staff chỉ xem được submission của event mình được phân công
- Hỗ trợ filter: RoundId, TrackId, TopicId, RegisterTeamId, Keyword
- Phân trang: PageIndex, PageSize
- Mỗi item trả kèm thông tin team, round, track, topic
- Chỉ trả về team đã có ít nhất 1 submission

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

## Request

### Route Parameters

| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| eventId | Guid | Có | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | ID của event |

### Query Parameters

| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| RoundId | Guid | Không | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | Lọc theo round |
| TrackId | Guid | Không | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | Lọc theo track |
| TopicId | Guid | Không | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | Lọc theo topic |
| RegisterTeamId | Guid | Không | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | Lọc theo register team |
| Keyword | string | Không | `Team A` | Tìm theo tên team |
| PageIndex | int | Không | `1` | Mặc định = 1 |
| PageSize | int | Không | `10` | Mặc định = 10 |

## Response (200)

```json
{
  "data": {
    "items": [
      {
        "registerTeamId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "teamId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "teamName": "Team A",
        "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "eventName": "Hackathon 2026",
        "roundId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "roundName": "Vòng 1",
        "trackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "trackTitle": "AI",
        "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "topicTitle": "Chatbot",
        "submittedBy": {
          "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "email": "leader@example.com",
          "firstName": "Nguyễn",
          "lastName": "Văn A"
        },
        "lastSubmission": {
          "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "submittedAt": "2026-07-08T10:00:00Z",
          "url": "https://example.com/submission.pdf",
          "description": "Bài nộp vòng 1",
          "status": "Submitted"
        },
        "records": [
          {
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "submittedAt": "2026-07-08T10:00:00Z",
            "url": "https://example.com/submission.pdf",
            "description": "Bài nộp vòng 1",
            "status": "Submitted"
          }
        ]
      }
    ],
    "totalCount": 5,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-09T12:00:00Z"
}
```

## Lỗi

| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Staff hoặc không được phân công vào event | Ẩn chức năng |

> **Ref:** [Admin API tương ứng](/api/v1/admin/submission/get/admin.events.submissions.md)
