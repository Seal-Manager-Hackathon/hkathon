# GET /api/v1/admin/scores/{scoreId}

> Admin xem chi tiết 1 lượt chấm (score/scope) của 1 người chấm.

## Nghiệp vụ

1 **score** = 1 lượt chấm của 1 judge cho 1 bài nộp (submission). API này trả về thông tin score kèm track/topic của bài nộp đó.

Khi judge chấm bài, hệ thống tạo ra:
- **Score** — record tổng, lưu `TotalScore` = tổng điểm judge đó chấm
- **ScoreItems** — chi tiết điểm từng tiêu chí (VD: Sáng tạo 40đ, Kỹ thuật 30đ...)

API này chỉ trả thông tin **Score**, ko kèm ScoreItems. ScoreItems có API riêng:
👉 [GET /scores/{scoreId}/items](admin.scores.items.md) (phân trang)

Các thông tin track và topic được lấy từ bản ghi `RegisterTeams` thông qua `Submission → RoundDetail → RegisterTeam`.

```
Submissions (bài nộp)
  └── RoundDetail
        └── RegisterTeam → TrackId, TopicId, TopicTitle
  └── Score (lượt chấm của judge A) ← API này
        └── ScoreItems (điểm từng tiêu chí) ← API /scores/{scoreId}/items
```

## Phân quyền
- ✅ Admin

## Request

| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| scoreId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)

```json
{
  "data": {
    "scoreId": "guid",
    "submissionId": "guid",
    "assignTrackId": "guid",
    "trackTitle": "Track A",
    "trackId": "guid",
    "topicId": "guid",
    "topicTitle": "AI trong Y tế",
    "totalScore": 85.5,
    "isRetake": false,
    "retakeFromScoreId": null,
    "isMock": false,
    "gradedBy": {
      "userId": "guid",
      "email": "judge@example.com",
      "firstName": "Nguyen",
      "lastName": "Van A"
    },
    "createdAt": "2026-07-07T12:00:00Z",
    "updatedAt": "2026-07-07T12:00:00Z"
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

### Field ý nghĩa

| Field | Ý nghĩa |
|-------|---------|
| `scoreId` | ID của lượt chấm này |
| `submissionId` | Bài nộp được chấm |
| `assignTrackId` | Track mà người chấm được phân công (AssignTrack) |
| `trackTitle` | Tên track (từ AssignTrack) |
| **`trackId`** | **ID của track mà team đăng ký (từ RegisterTeams)** |
| **`topicId`** | **ID của topic mà team đăng ký** |
| **`topicTitle`** | **Tên topic** |
| `totalScore` | **Tổng điểm** judge này chấm = SUM(ScoreItems.Score) |
| `isRetake` | Đánh dấu chấm lại |
| `retakeFromScoreId` | Nếu là chấm lại, ID của lượt chấm gốc |
| `isMock` | Đánh dấu điểm chấm thử (mock) |
| `gradedBy` | **Người chấm** (userId, email, firstName, lastName) |

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Resource Not Found | scoreId không tồn tại |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
