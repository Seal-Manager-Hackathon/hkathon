# GET /api/v1/admin/submissions/{submissionId}/scores

> Admin xem **điểm tổng** của 1 bài nộp — tổng tất cả Scores.TotalScore từ các judge.

## Phân quyền
- ✅ Admin

## Request

| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| submissionId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)

```json
{
  "data": {
    "submissionId": "guid",
    "totalScore": 170.50,
    "judgeCount": 3
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

### Giải thích
- `totalScore` = SUM(TotalScore của tất cả judge đã chấm bài này)
- `judgeCount` = số judge đã chấm (có TotalScore)

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
