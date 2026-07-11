# GET /api/v1/lecturer/tracks/{trackId}/register-teams

> Lecturer lấy danh sách register teams trong 1 track, có filter round, phân trang, kèm thông tin round hiện tại.

**Controller:** [LecturerRegisterTeamController.cs](Controllers/Lecturer/LecturerRegisterTeamController.cs)

## Nghiệp vụ

- Lấy danh sách register teams đã đăng ký vào track.
- Mỗi team hiển thị round cao nhất (max RoundNo từ RoundDetails).
- Hỗ trợ lọc theo round.
- Hỗ trợ phân trang.

## Phân quyền
- ✅ Lecturer

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| trackId | Guid | ID của track |

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
    "registerTeams": [
      {
        "id": "guid",
        "teamId": "guid",
        "teamName": "FTeam",
        "eventId": "guid",
        "eventName": "Hackathon 2026",
        "trackId": "guid",
        "trackName": "Web3",
        "topicId": null,
        "topicName": null,
        "description": "Mô tả team",
        "status": "Approved",
        "isBanned": false,
        "isDisable": false,
        "roundId": "guid",
        "roundName": "Vòng 2",
        "roundNo": 2,
        "createdAt": "2026-07-07T12:00:00Z",
        "updatedAt": "2026-07-07T12:00:00Z"
      }
    ],
    "totalCount": 42,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Register Teams Fetched Successfully",
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
| `id` | ID register team |
| `teamId` / `teamName` | Thông tin team |
| `trackId` / `trackName` | Track team đăng ký |
| `topicId` / `topicName` | Topic team chọn |
| `status` | Trạng thái: `Pending`, `Approved`, `Rejected`, `Banned` |
| `isBanned` | Team có bị banned không |
| `isDisable` | Team có bị disable không |
| `roundId` / `roundName` / `roundNo` | Round cao nhất team đang tham gia |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Page Index Must Be Greater Than Zero | PageIndex < 1 |
| 400 | Page Size Must Be Between 1 And 100 | PageSize ngoài khoảng |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | Không phải Lecturer |
| 404 | Track Not Found | trackId ko tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId}/register-teams)
