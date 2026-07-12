# GET /api/v1/lecturer/events/{eventId}/submissions

> Lecturer lấy danh sách bài nộp trong event được assign, phân trang, có filter. Mỗi item chỉ hiện **lastSubmission** (ko records).

**Controller:** [LecturerEventController.cs](Controllers/Lecturer/LecturerEventController.cs)

## Nghiệp vụ

- Giống Admin `GET /api/v1/admin/events/{eventId}/submissions` nhưng auth là Lecturer.
- **Bắt buộc** Lecturer phải được assign vào event (`AssignEvents`) — nếu không → 403 Forbidden.
- Mỗi item đại diện 1 team trong 1 round, chỉ hiển thị **lastSubmission** (bài nộp cuối cùng).
- Hỗ trợ lọc: round, track, topic, registerTeamId, keyword (tên team).
- Hỗ trợ phân trang.

## Phân quyền
- ✅ Lecturer — phải được assign vào event

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| roundId | Guid | No | - | Lọc theo round |
| trackId | Guid | No | - | Lọc theo track |
| topicId | Guid | No | - | Lọc theo topic |
| registerTeamId | Guid | No | - | Lọc theo register team |
| keyword | string | No | - | Tìm kiếm theo tên team |
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
    "totalCount": 10,
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
| 404 | Resource Not Found | eventId ko tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId}/submissions)
