# GET /api/v1/admin/submissions/{submissionId}/grader-scores

> Admin xem danh sách lượt chấm (scope) của 1 bài nộp — phân trang.

## Giải thích nghiệp vụ

API này trả về danh sách các **Score** (lượt chấm) của 1 bài nộp. Mỗi score là 1 judge chấm bài đó.

Khác với [GET /submissions/{submissionId}/scores](admin.submissions.scores.md) (trả về tổng điểm), API này trả về **từng lượt chấm riêng lẻ** — chưa phải điểm cuối.

VD: Bài nộp có 3 judge:
- Judge A: `totalScore: 85` ← 1 score
- Judge B: `totalScore: 90` ← 1 score
- Judge C: `totalScore: 75` ← 1 score

→ API này trả về 3 records (phân trang).
→ API tổng (submissions/{id}/scores) trả về `totalScore: 250`.

```
Submissions (bài nộp)
  └── Score (Judge A: 85đ) ← 1 item
  └── Score (Judge B: 90đ) ← 1 item
  └── Score (Judge C: 75đ) ← 1 item
```

> Muốn xem chi tiết từng tiêu chí trong 1 score? → [GET /scores/{scoreId}/items](admin.scores.items.md)

## Phân quyền
- ✅ Admin

## Request

| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| submissionId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |
| pageIndex | int | ❌ (default=1) | `1` |
| pageSize | int | ❌ (default=10) | `10` |

## Response (200)

```json
{
  "data": {
    "submissionId": "guid",
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
    ],
    "totalCount": 3,
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
| `submissionId` | ID bài nộp |
| `scores[]` | Mảng các lượt chấm (score) |
| `scores[].scoreId` | ID lượt chấm |
| `scores[].totalScore` | Điểm judge này chấm |
| `scores[].assignTrackId` | Track judge được assign |
| `scores[].trackTitle` | Tên track |
| `scores[].isRetake` | Có phải chấm lại ko |
| `scores[].isMock` | Có phải chấm thử ko |
| `totalCount` / `pageIndex` / `pageSize` | Thông tin phân trang |

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
