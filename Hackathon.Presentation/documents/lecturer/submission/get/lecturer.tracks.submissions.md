# GET /api/v1/lecturer/tracks/{trackId}/submissions

> Lecturer lấy danh sách bài nộp trong 1 track, chỉ hiện **lastSubmission** (ko records). Dựa trên Admin `GET /api/v1/admin/tracks/{trackId}/submissions`.

**Controller:** [LecturerTrackController.cs](Controllers/Lecturer/LecturerTrackController.cs)

## Nghiệp vụ

- Giống Admin nhưng auth là Lecturer, **ko records**.
- **Bắt buộc** Lecturer phải được assign vào event của track — nếu không → 403 Forbidden.
- Mỗi item là 1 team trong 1 round, chỉ hiển thị `lastSubmission`.
- Hỗ trợ phân trang.

## Phân quyền
- ✅ Lecturer — phải được assign vào event chứa track

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| trackId | Guid | ID của track |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
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
| `roundId` / `roundName` | Round hiện tại |
| `trackId` / `trackTitle` | Track |
| `topicId` / `topicTitle` | Topic team chọn |
| `submittedBy` | Leader của team |
| `lastSubmission` | Bài nộp cuối cùng trong round |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Page Index/Size | Pagination sai |
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You Are Not Assigned to This Event | Lecturer ko được assign |
| 404 | Track Not Found | trackId ko tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/tracks/{trackId}/submissions)
