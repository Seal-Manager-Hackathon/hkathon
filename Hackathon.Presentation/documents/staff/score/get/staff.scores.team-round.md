# GET /api/v1/staff/rounds/{roundId}/register-teams/{registerTeamId}/scores

> Lấy tổng điểm của một team trong một round.

## Nghiệp vụ
- Staff chỉ xem được điểm thuộc event mình được phân công
- `totalScore` = tổng `TotalScore` các score của submission cuối cùng trong round
- Trả về thông tin team, track, topic kèm submission gần nhất

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

## Request

### Route Parameters
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| roundId | Guid | Có | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | ID của round |
| registerTeamId | Guid | Có | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | ID của register team |

## Response (200)
```json
{
  "data": {
    "roundId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "registerTeamId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "eventName": "Hackathon 2026",
    "trackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "trackTitle": "AI",
    "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "topicTitle": "Chatbot",
    "totalScore": 85.5,
    "submissionId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "submittedAt": "2026-07-08T10:00:00Z",
    "isLastSubmission": true
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
| 404 | Resource Not Found | registerTeamId/roundId không tồn tại | Hiển thị thông báo |
