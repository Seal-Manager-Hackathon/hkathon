# GET /api/v1/student/register-teams/{registerTeamId}/submissions

> Student lấy danh sách bài nộp cuối cùng của team mình trong mỗi round của event, có thể lọc theo round.

**Controller:** [StudentSubmissionController.cs](../../../Controllers/Student/StudentSubmissionController.cs)

> **Ref:** [Judge API tương ứng](/api/v1/judge/register-teams/{registerTeamId}/submissions)

## Nghiệp vụ

Student muốn xem các bài nộp của register team của mình trong event:
- Lấy tất cả các round của event mà register team đã tham gia.
- Mỗi round chỉ lấy **bài nộp cuối cùng** (submission mới nhất theo SubmittedAt).
- Có thể lọc theo **roundId** để chỉ xem bài nộp của 1 round cụ thể.
- Nếu không truyền roundId → lấy tất cả round có bài nộp.
- Chỉ lấy register team Approved, không bị disable.

## Phân quyền
- ✅ Student

## Request

### Route Parameters
| Parameter | Kiểu | Bắt buộc | Mô tả |
|-----------|------|----------|-------|
| registerTeamId | Guid | ✅ | ID đăng ký team trong event |

### Query Parameters
| Parameter | Kiểu | Bắt buộc | Mặc định | Mô tả |
|-----------|------|----------|----------|-------|
| roundId | Guid | ❌ | - | Lọc theo round cụ thể |

## Response (200)
```json
{
  "data": {
    "items": [
      {
        "registerTeamId": "guid",
        "teamId": "guid",
        "teamName": "Team ABC",
        "eventId": "guid",
        "eventName": "Hackathon 2026",
        "roundId": "guid",
        "roundName": "Vòng 1",
        "trackId": "guid",
        "trackTitle": "Tri tue nhan tao",
        "topicId": null,
        "topicTitle": null,
        "submittedBy": {
          "userId": "guid",
          "email": "leader@email.com",
          "firstName": "Nguyen",
          "lastName": "Van A"
        },
        "lastSubmission": {
          "id": "guid",
          "submittedAt": "2026-07-11T10:00:00Z",
          "url": "https://example.com/submission.pdf",
          "description": "Bai nop cuoi",
          "status": "Submitted"
        },
        "scoreId": null,
        "totalScore": null
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 2147483647
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
| 404 | Register Team Not Found | registerTeamId không tồn tại hoặc bị disable |
