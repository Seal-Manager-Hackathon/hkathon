# GET /api/v1/staff/events/{eventId}/register-teams/with-scores

> Staff lấy danh sách register teams trong event (phải được phân công vào event đó), kèm totalScopeScore, sort theo điểm giảm dần.

## Nghiệp vụ

- Giống hệt `GET /api/v1/staff/events/{eventId}/register-teams` nhưng thêm field `totalScopeScore`.
- `totalScopeScore` = AVG(Scores.TotalScore) của submission cuối trong round hiện tại.
- Sort theo `totalScopeScore` giảm dần (cao nhất lên đầu).
- **Staff phải được assign vào event đó** — nếu không → 403 Forbidden.
- Nếu team chưa có submission hoặc chưa có judge chấm → `totalScopeScore = null`.

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId}/register-teams/with-scores)

## Phân quyền
- ✅ Staff (phải được assign vào event)

## Request

| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| Keyword | string | ❌ | `Hackathon` | Search team name |
| Status | string | ❌ | `Approved` | ⚠️ Enum |
| IsBanned | bool | ❌ | `false` | Lọc bị ban |
| IsDisable | bool | ❌ | `false` | Lọc bị disable |
| FromDate | datetime | ❌ | - | Lọc từ ngày tạo |
| ToDate | datetime | ❌ | - | Lọc đến ngày tạo |
| RoundId | Guid | ❌ | - | Lọc theo round |
| TrackId | Guid | ❌ | - | Lọc theo track |
| TopicId | Guid | ❌ | - | Lọc theo topic |
| PageIndex | int | ❌ | 1 | Mặc định 1 |
| PageSize | int | ❌ | 10 | Mặc định 10 |

## Response (200)
```json
{
  "data": {
    "registerTeams": [
      {
        "id": "guid",
        "registerTeamId": "guid",
        "teamId": "guid",
        "teamName": "Team Alpha",
        "eventId": "guid",
        "eventName": "Hackathon 2026",
        "trackId": "guid",
        "trackName": "AI/ML",
        "topicId": "guid",
        "topicName": "Chatbot",
        "description": "Mô tả",
        "rejectionReason": null,
        "status": "Approved",
        "isBanned": false,
        "isDisable": false,
        "roundId": "guid",
        "roundName": "Vòng 1",
        "roundNo": 1,
        "totalScopeScore": 85.50,
        "createdAt": "2026-07-01T00:00:00Z",
        "updatedAt": "2026-07-10T00:00:00Z"
      }
    ],
    "totalCount": 10,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Register Teams Fetched Successfully",
  "status": 200,
  "traceId": "00-..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Invalid Status | Status sai |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Staff hoặc không được assign vào event |
