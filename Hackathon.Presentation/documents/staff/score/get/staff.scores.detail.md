# GET /api/v1/staff/scores/{scoreId}

> Xem chi tiết điểm (score) của một bài nộp.

## Nghiệp vụ
- Staff chỉ xem được điểm thuộc event mình được phân công
- Trả về thông tin: submission, track, topic, grader, điểm số

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

## Request

### Route Parameters
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| scoreId | Guid | Có | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | ID của score |

## Response (200)
```json
{
  "data": {
    "scoreId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "submissionId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "assignTrackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "trackTitle": "AI",
    "trackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "topicTitle": "Chatbot",
    "totalScore": 85.5,
    "isRetake": false,
    "retakeFromScoreId": null,
    "isMock": false,
    "gradedBy": {
      "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "email": "lecturer@example.com",
      "firstName": "Nguyễn",
      "lastName": "Văn B"
    },
    "createdAt": "2026-07-08T12:00:00Z",
    "updatedAt": "2026-07-08T12:00:00Z"
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
| 404 | Resource Not Found | scoreId không tồn tại | Hiển thị thông báo |
