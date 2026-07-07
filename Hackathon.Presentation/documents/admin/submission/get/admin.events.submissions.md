# GET /api/v1/admin/events/{eventId}/submissions

> Admin lấy danh sách bài nộp trong event, phân trang, có filter.

## Nghiệp vụ

- Trả về danh sách bài nộp của các team trong từng round của event
- Mỗi item là 1 team trong 1 round (RoundDetails)
- Gồm thông tin team, event, round, track, topic, người nộp
- **LastSubmission:** bài nộp cuối cùng (mới nhất) của team trong round đó
- **Records:** toàn bộ lịch sử bài nộp của team trong round đó
- **SubmittedBy:** leader của team (người nộp)

## Phân quyền

- ✅ Admin

## Request

### Route Parameters

| Param   | Kiểu | Bắt buộc | Ví dụ                                  |
|---------|------|----------|----------------------------------------|
| eventId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

### Query Parameters

| Param            | Kiểu   | Bắt buộc | Mô tả                          |
|------------------|--------|----------|--------------------------------|
| roundId          | guid   | ❌        | Lọc theo round                 |
| trackId          | guid   | ❌        | Lọc theo track                 |
| topicId          | guid   | ❌        | Lọc theo topic                 |
| registerTeamId   | guid   | ❌        | Lọc theo register team         |
| keyword          | string | ❌        | Tìm kiếm theo tên team (ko dấu) |
| pageIndex        | int    | ❌        | Mặc định 1                     |
| pageSize         | int    | ❌        | Mặc định 10, tối đa 100        |

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
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-08T12:00:00Z"
}
```

## Lỗi

| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | Page Index Must Be Greater Than Zero | pageIndex < 1 | Báo "Trang không hợp lệ" |
| 400 | Page Size Must Be Between 1 And 100 | pageSize < 1 hoặc > 100 | Báo "Kích thước trang không hợp lệ" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Resource Not Found | eventId không tồn tại | Báo "Không tìm thấy sự kiện" |
