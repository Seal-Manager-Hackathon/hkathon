# GET /api/v1/lecturer/register-teams/{registerTeamId}/submissions

> Lecturer lấy danh sách bài nộp của 1 team trong event, phân theo từng round, chỉ hiện **lastSubmission** (ko records).

**Controller:** [LecturerRegisterTeamController.cs](Controllers/Lecturer/LecturerRegisterTeamController.cs)

## Nghiệp vụ

- Giống Admin `GET /api/v1/admin/register-teams/{registerTeamId}/submissions` nhưng auth là Lecturer.
- Mỗi item là 1 **RoundDetails** — team ở 1 round, chỉ hiển thị `lastSubmission`.
- **Bắt buộc** Lecturer phải được assign vào event — nếu không → 403 Forbidden.
- Hỗ trợ lọc theo roundId, phân trang.

## Phân quyền
- ✅ Lecturer — phải được assign vào event

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| registerTeamId | Guid | ID của register team |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| roundId | Guid | No | - | Lọc theo round cụ thể |
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
        "teamName": "FTeam",
        "eventId": "guid",
        "eventName": "Hackathon 2026",
        "roundId": "guid",
        "roundName": "Vòng 1",
        "trackId": "guid",
        "trackTitle": "Web3",
        "topicId": null,
        "topicName": null,
        "submittedBy": {
          "userId": "guid",
          "email": "leader@email.com",
          "firstName": "Nguyễn",
          "lastName": "Văn A"
        },
        "lastSubmission": {
          "id": "guid",
          "submittedAt": "2026-07-08T10:00:00Z",
          "url": "https://example.com/submission.pdf",
          "description": "Bài nộp cuối",
          "status": "Submitted"
        }
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
  "timestampUtc": "2026-07-12T12:00:00Z"
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
| `topicId` / `topicTitle` | Topic team chọn |
| `submittedBy` | Leader của team |
| `lastSubmission` | Bài nộp cuối cùng trong round |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Page Index/Size | Pagination sai |
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You Are Not Assigned to This Event | Lecturer ko được assign |
| 404 | Register Team Not Found | registerTeamId ko tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/register-teams/{registerTeamId}/submissions)
