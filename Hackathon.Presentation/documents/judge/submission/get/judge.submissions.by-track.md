# GET /api/v1/judge/tracks/{trackId}/submissions

> Judge đã đăng nhập lấy danh sách bài nộp (submissions) của các team trong 1 track mà họ được phân công chấm.

**Controller:** `JudgeController` — `GET /api/v1/judge/tracks/{trackId}/submissions`

## Nghiệp vụ

- Judge muốn xem các bài nộp của teams trong track được assign.
- Chỉ lấy các team đã có ít nhất 1 lần submit (filter `Submissions.Count > 0`).
- **Chỉ lấy submission cuối cùng** của mỗi team (dùng `SubmissionHelper.GetLastSubmission`).
- Judge phải được assign vào track với EventRole = Judge.
- Hỗ trợ lọc theo round.

## Phân quyền
- ✅ Judge (RoleEnum.Lecturer + EventRoleEnum.Judge) — phải được assign vào track

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| trackId | Guid | ID của track |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| roundId | Guid | No | - | Lọc theo round |
| pageIndex | int | No | 1 | Trang số |
| pageSize | int | No | 10 | Số item trên 1 trang (max 100) |

## Response (200)
```json
{
  "data": {
    "items": [
      {
        "registerTeamId": "guid",
        "teamId": "guid",
        "teamName": "Team ABC",
        "roundId": "guid",
        "roundName": "Vong 1",
        "roundNo": 1,
        "topicId": null,
        "topicTitle": null,
        "submissionId": "guid",
        "url": "https://...",
        "description": "Bai nop vong 1",
        "status": "Submitted",
        "submittedAt": "2026-07-10T10:00:00Z",
        "gradingStatus": "Pending",
        "scoreId": null,
        "totalScore": null
      }
    ],
    "totalCount": 5,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "..."
}
```

### Field ý nghĩa

| Field | Ý nghĩa |
|-------|---------|
| registerTeamId | ID của register team |
| teamId | ID của team |
| teamName | Tên team |
| roundId | ID của round |
| roundName | Tên round |
| roundNo | Số thứ tự round |
| topicId | ID của topic (nếu có) |
| topicTitle | Tên topic (nếu có) |
| submissionId | ID của submission cuối cùng |
| url | Link bài nộp |
| description | Mô tả bài nộp |
| status | Trạng thái bài nộp (Submitted/Unsubmitted/Failed) |
| submittedAt | Thời gian nộp |
| gradingStatus | Trạng thái chấm (Pending/Graded) |
| scoreId | ID của điểm (null nếu chưa chấm) |
| totalScore | Tổng điểm (null nếu chưa chấm) |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You Are Not Assigned as Judge for This Track | Judge ko được assign track |
| 404 | Track Not Found | trackId ko tồn tại hoặc đã bị disable |
