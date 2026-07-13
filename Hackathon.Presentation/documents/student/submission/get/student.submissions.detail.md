# GET /api/v1/student/submissions/{submissionId}

> Student xem chi tiết 1 bài nộp.

**Controller:** [StudentSubmissionController.cs](../../../Controllers/Student/StudentSubmissionController.cs)

> **Ref:** [Judge API tương ứng](/api/v1/judge/submissions/{submissionId})

## Nghiệp vụ

Student muốn xem chi tiết 1 bài nộp cụ thể:
- Trả về thông tin đầy đủ của submission: round, team, track, topic, người nộp, tổng điểm, số judge đã chấm.
- `status` dựa trên `SubmissionStatusEnum`: `Submitted` = mới nộp, `Graded` = đã được chấm điểm.

## Phân quyền
- ✅ Student

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| submissionId | Guid | ID của bài nộp |

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
    "topicId": null,
    "topicTitle": null,
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
    "totalScore": 170.50,
    "judgeCount": 2,
    "createdAt": "2026-07-07T12:00:00Z",
    "updatedAt": "2026-07-08T10:00:00Z"
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 404 | Resource Not Found | submissionId không tồn tại |
