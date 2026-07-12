# GET /api/v1/judge/submissions/{submissionId}

> Judge lấy thông tin chi tiết của 1 bài nộp.

## Nghiệp vụ

Judge muốn xem chi tiết 1 bài nộp cụ thể:
- Response giống hệt Admin API `GET /api/v1/admin/submissions/{submissionId}`.
- Judge phải được assign vào event chứa bài nộp đó.
- Trả về: thông tin submission, round, team, track, topic, người nộp, tổng điểm, số lượng judge đã chấm.
- `status` dựa trên `SubmissionStatusEnum`: `Submitted` = mới nộp, `Graded` = đã được chấm điểm.

> **Ref:** [Admin API tương ứng](/api/v1/admin/submissions/{submissionId})

## Phân quyền
- ✅ Judge (RoleEnum = Lecturer, được assign làm Judge trong event)

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
| 403 | You Are Not Assigned as Judge for This Event | Judge không được assign vào event |
| 404 | Resource Not Found | submissionId không tồn tại |
