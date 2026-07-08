# GET /api/v1/admin/submissions/{submissionId}/scores

> Admin xem **điểm tổng** của 1 bài nộp.

## Giải thích nghiệp vụ

API này trả về điểm tổng của 1 bài nộp — tức tổng điểm của **tất cả judge** đã chấm bài đó.

Cách tính:
```
submissionScore = SUM(Scores.TotalScore) của tất cả judge
```

VD: Bài nộp có 3 judge chấm:
- Judge A: 85
- Judge B: 90
- Judge C: 75
→ `totalScore` = 85 + 90 + 75 = 250
→ `judgeCount` = 3

Đây là **điểm cuối cùng** của bài nộp.

> Muốn xem chi tiết từng judge chấm bao nhiêu? → [GET /submissions/{submissionId}/grader-scores](admin.submissions.grader-scores.md)

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

### Field ý nghĩa

| Field | Ý nghĩa |
|-------|---------|
| `submissionId` | ID bài nộp |
| `totalScore` | **Tổng điểm** = SUM(TotalScore của tất cả judge đã chấm) |
| `judgeCount` | Số judge đã chấm bài này (có TotalScore) |

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
