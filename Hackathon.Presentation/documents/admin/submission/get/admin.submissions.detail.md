# GET /api/v1/admin/submissions/{submissionId}

> Admin xem chi tiết 1 bài nộp (submission).

## Giải thích nghiệp vụ

Một **submission** là 1 lần team nộp bài trong 1 round. Mỗi team có thể nộp nhiều lần trong 1 round (nộp lại), lần cuối cùng mới là bài chính thức.

Submission chứa:
- Thông tin team, round, track, topic
- Danh sách **scores** — mỗi score là 1 lượt chấm của 1 judge
- Mỗi score chứa nhiều **score items** — điểm từng tiêu chí trong criteria template

Cấu trúc phân cấp:
```
Submissions (bài nộp)
  └── Scores (lượt chấm — 1 record / judge)
        └── ScoreItems (điểm từng tiêu chí)
```

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
    "id": "guid",
    "roundDetailId": "guid",
    "roundId": "guid",
    "roundName": "Vòng 1",
    "registerTeamId": "guid",
    "teamId": "guid",
    "teamName": "Tên team",
    "trackId": "guid",
    "trackTitle": "Tên track",
    "topicId": "guid",
    "topicTitle": "Tên topic",
    "url": "https://example.com/submission.pdf",
    "description": "Bài nộp cuối",
    "status": "Submitted",
    "submittedAt": "2026-07-08T10:00:00Z",
    "isRegrade": false,
    "submittedBy": {
      "userId": "guid",
      "email": "leader@email.com",
      "firstName": "Nguyễn",
      "lastName": "Văn A"
    },
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
        "items": [
          {
            "scoreItemId": "guid",
            "scoreId": "guid",
            "criteriaItemId": "guid",
            "assignTrackId": "guid",
            "assignEventId": "guid",
            "criteriaName": "Tính sáng tạo",
            "score": 20,
            "comment": "Ý tưởng tốt",
            "gradedBy": {
              "userId": "guid",
              "email": "lecturer@email.com",
              "firstName": "Nguyễn",
              "lastName": "Văn B"
            },
            "createdAt": "2026-07-07T12:00:00Z",
            "updatedAt": "2026-07-07T12:00:00Z"
          }
        ],
        "createdAt": "2026-07-07T12:00:00Z",
        "updatedAt": "2026-07-07T12:00:00Z"
      }
    ],
    "createdAt": "2026-07-07T12:00:00Z",
    "updatedAt": "2026-07-08T10:00:00Z"
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

### Field ý nghĩa

| Field | Ý nghĩa |
|-------|---------|
| `id` | ID của submission |
| `roundDetailId` | ID của RoundDetails — record trung gian (team trong round nào) |
| `roundId` / `roundName` | Round mà bài nộp này thuộc về |
| `registerTeamId` | ID đăng ký của team trong event |
| `teamId` / `teamName` | Team đã nộp bài |
| `trackId` / `trackTitle` | Track team đã đăng ký |
| `topicId` / `topicTitle` | Topic team chọn |
| `url` | Link bài nộp (file/PDF) |
| `description` | Mô tả hoặc ghi chú khi nộp |
| `status` | Trạng thái bài nộp (Submitted, ...) |
| `submittedAt` | Thời gian nộp |
| `isRegrade` | Đánh dấu bài được chấm lại |
| `submittedBy` | Người nộp (leader của team) |
| `scores[]` | Danh sách lượt chấm của bài này |

### scores[].items[].gradedBy

| Field | Ý nghĩa |
|-------|---------|
| `userId` | ID của người chấm (grader — lecturer role Judge) |
| `email` | Email người chấm |
| `firstName` / `lastName` | Tên người chấm |

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Resource Not Found | submissionId không tồn tại |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
