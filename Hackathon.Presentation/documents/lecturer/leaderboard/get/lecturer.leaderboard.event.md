# GET /api/v1/lecturer/events/{eventId}/leaderboard

> Lecturer xem bảng xếp hạng 1 event (eventScore từng team, sắp xếp theo điểm giảm dần).

**Controller:** [LecturerLeaderboardController.cs](Controllers/Lecturer/LecturerLeaderboardController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/events/{eventId}/leaderboard`

- Giống hệt Admin `GET /api/v1/admin/events/{eventId}/leaderboard`, khác auth là Lecturer.
- eventScore = weighted average scopeScores (weight_i = 1, mẫu số = tổng số round event).
- roundScores[].scopeScore = từng round team đã tham gia.
- Team không tham gia round = 0 điểm trong mẫu số.
- Phân trang: mặc định pageIndex=1, pageSize=10.

## Phân quyền
- ✅ Lecturer

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| PageIndex | int | No | 1 | Trang hiện tại |
| PageSize | int | No | 10 | Số lượng mỗi trang |

## Response (200)
```json
{
  "data": {
    "eventId": "guid",
    "eventName": "Hackathon 2026",
    "totalRounds": 3,
    "isDisable": false,
    "items": [
      {
        "rank": 1,
        "registerTeamId": "guid",
        "teamId": "guid",
        "teamName": "FTeam",
        "trackId": "guid",
        "trackTitle": "Web3",
        "topicId": null,
        "topicTitle": null,
        "eventScore": 72.33,
        "roundScores": [
          {
            "roundNo": 1,
            "roundName": "Vòng 1",
            "scopeScore": 80.50
          }
        ]
      }
    ],
    "totalCount": 10,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |
| 404 | Resource Not Found | eventId không tồn tại hoặc leaderboard bị disable |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId}/leaderboard)
