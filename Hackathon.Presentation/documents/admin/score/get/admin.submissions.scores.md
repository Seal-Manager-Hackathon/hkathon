# GET /api/v1/admin/submissions/{submissionId}/scores

> Admin lấy điểm của 1 bài nộp (submission), gồm tất cả lượt chấm (ko kèm chi tiết tiêu chí).

## Nghiệp vụ
- Trả về danh sách các lượt chấm (Scores) của submission
- Mỗi lượt chấm gồm tổng điểm, track chấm (không kèm score items — xem chi tiết score ở `GET /admin/scores/{scoreId}`)

## Phân quyền
- ✅ Admin

## Request

| Param       | Kiểu | Bắt buộc | Ví dụ |
|-------------|------|----------|-------|
| submissionId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)

```json
{
  "data": {
    "submissionId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "scores": [
      {
        "scoreId": "guid",
        "submissionId": "guid",
        "assignTrackId": "guid",
        "trackTitle": "Track A",
        "totalScore": 85.5,
        "isRetake": false,
        "retakeFromScoreId": null,
        "isMock": false,
        "createdAt": "2026-07-07T12:00:00Z",
        "updatedAt": "2026-07-07T12:00:00Z"
      }
    ]
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
