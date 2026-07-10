# GET /api/v1/staff/submissions/{submissionId}/grader-scores

> Staff xem danh sách lượt chấm (score) của 1 bài nộp — phân trang, kèm thông tin người chấm.

## Nghiệp vụ

API này trả về danh sách các **Score** (lượt chấm) của 1 bài nộp. Mỗi score là 1 judge chấm bài đó, kèm thông tin người chấm.

Khác với API tính tổng điểm (trả về tổng số), API này trả về **từng lượt chấm riêng lẻ** kèm thông tin người chấm, track/topic của team nộp bài.

- Staff chỉ xem được điểm thuộc event mình được phân công
- Phân trang: PageIndex, PageSize
- Mỗi score là một grader chấm bài này

VD: Bài nộp có 3 judge:
- Judge A: `totalScore: 85` ← 1 score (kèm thông tin Judge A)
- Judge B: `totalScore: 90` ← 1 score (kèm thông tin Judge B)
- Judge C: `totalScore: 75` ← 1 score (kèm thông tin Judge C)

→ API này trả về 3 records (phân trang).

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

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
        "trackTitle": "AI",
        "trackId": "guid",
        "topicId": "guid",
        "topicTitle": "Chatbot",
        "totalScore": 85.5,
        "isRetake": false,
        "retakeFromScoreId": null,
        "isMock": false,
        "gradedBy": {
          "userId": "guid",
          "email": "judge@example.com",
          "firstName": "Nguyễn",
          "lastName": "Văn A"
        },
        "createdAt": "2026-07-08T12:00:00Z",
        "updatedAt": "2026-07-08T12:00:00Z"
      }
    ],
    "totalCount": 3,
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

### Field ý nghĩa

| Field | Ý nghĩa |
|-------|---------|
| `submissionId` | ID bài nộp |
| `scores[]` | Mảng các lượt chấm (score) |
| `scores[].scoreId` | ID lượt chấm |
| `scores[].totalScore` | Điểm judge này chấm |
| `scores[].assignTrackId` | Track judge được assign |
| `scores[].trackTitle` | Tên track (từ AssignTrack) |
| `scores[].trackId` | ID của track mà team đăng ký (từ RegisterTeams) |
| `scores[].topicId` | ID của topic mà team đăng ký |
| `scores[].topicTitle` | Tên topic team đăng ký |
| `scores[].isRetake` | Có phải chấm lại ko |
| `scores[].isMock` | Có phải chấm thử ko |
| `scores[].gradedBy` | Thông tin người chấm (judge): userId, email, firstName, lastName |
| `totalCount` / `pageIndex` / `pageSize` | Thông tin phân trang |

## Lỗi

| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Staff hoặc không được phân công vào event | Ẩn chức năng |
| 404 | Resource Not Found | submissionId không tồn tại | Hiển thị thông báo |

> **Ref:** [Admin API tương ứng](/api/v1/admin/submissions/{submissionId}/grader-scores) — [`admin/score/get/admin.submissions.grader-scores.md`](../../../admin/score/get/admin.submissions.grader-scores.md)