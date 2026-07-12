# GET /api/v1/judge/tracks/{trackId}/submissions

> Judge lấy danh sách submissions trong 1 track được phân công. Mỗi team chỉ hiện submission cuối cùng trong round, kèm thông tin team leader (submittedBy).

**Controller:** [JudgeController.cs](Controllers/Judge/JudgeController.cs)

## Nghiệp vụ

- Lấy submissions của các team trong track judge được assign.
- Mỗi team chỉ hiển thị **submission cuối cùng** trong round.
- `submittedBy` = **leader của team** (`TeamDetails.IsLeader = true`).
- Trạng thái dựa trên `SubmissionStatusEnum`: `Submitted` = mới nộp, `Graded` = đã chấm.
- Filter `isGraded=true` → chỉ bài đã chấm, `isGraded=false` → chỉ bài chưa chấm.

## Phân quyền
- ✅ Judge — phải được assign vào track

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| trackId | Guid | ID của track |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| roundId | Guid | No | - | Lọc theo round |
| isGraded | bool | No | - | true=đã chấm, false=chưa chấm |
| pageIndex | int | No | 1 | Trang số |
| pageSize | int | No | 10 | Số item (max 100) |

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
        "roundName": "Vong 1",
        "trackId": "guid",
        "trackTitle": "Tri tue nhan tao",
        "topicId": "guid",
        "topicTitle": "AI trong Y tế",
        "submittedBy": {
          "userId": "guid",
          "email": "leader@email.com",
          "firstName": "Nguyễn",
          "lastName": "Văn A"
        },
        "lastSubmission": {
          "id": "guid",
          "submittedAt": "2026-07-11T10:00:00Z",
          "url": "https://example.com/submission.pdf",
          "description": "Bài nộp cuối",
          "status": "Submitted"
        },
        "scoreId": null,
        "totalScore": null
      }
    ],
    "totalCount": 5,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-11T12:00:00Z"
}
```

### Field ý nghĩa

| Field | Ý nghĩa |
|-------|---------|
| `registerTeamId` | ID đăng ký team trong event |
| `teamId` / `teamName` | Thông tin team |
| `eventId` / `eventName` | Event |
| `roundId` / `roundName` | Round hiện tại |
| `trackId` / `trackTitle` | Track team đăng ký |
| `topicId` / `topicTitle` | Topic team đăng ký |
| `submittedBy` | Leader của team (người nộp bài) |
| `lastSubmission` | Bài nộp cuối cùng trong round |
| `lastSubmission.status` | `Submitted` (mới nộp) / `Graded` (đã chấm) |
| `scoreId` | ID score nếu judge hiện tại đã chấm, null nếu chưa |
| `totalScore` | Điểm nếu đã chấm, null nếu chưa |

### submittedBy
| Field | Ý nghĩa |
|-------|---------|
| `userId` | ID của leader |
| `email` | Email |
| `firstName` / `lastName` | Tên leader |

### lastSubmission
| Field | Ý nghĩa |
|-------|---------|
| `id` | ID của bài nộp |
| `submittedAt` | Thời gian nộp |
| `url` | Link bài nộp |
| `description` | Mô tả |
| `status` | Trạng thái (`Submitted`, ...) |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You Are Not Assigned as Judge for This Track | Judge ko được assign |
| 404 | Track Not Found | trackId ko tồn tại |
