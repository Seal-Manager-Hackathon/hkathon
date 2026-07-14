# GET /api/v1/admin/events/{eventId}/register-teams/with-scores

> Admin lấy danh sách register teams trong event, kèm totalScopeScore của team trong round hiện tại.

## Nghiệp vụ

- Giống hệt `GET /api/v1/admin/events/{eventId}/register-teams` nhưng thêm field `totalScopeScore`.
- `totalScopeScore` = trung bình cộng (AVG) điểm của các judge cho submission cuối cùng của team trong round hiện tại.
- Cách tính: lấy `Scores.TotalScore` của submission cuối → AVG các TotalScore có giá trị.
- Nếu team chưa có submission hoặc chưa có judge nào chấm → `totalScopeScore = null`.

> **Logic tính scopeScore tham khảo từ:** `GET /api/v1/judge/submissions/{submissionId}`

## Phân quyền
- ✅ Admin

## Request

| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| Keyword | string | ❌ | `Hackathon` | Search team name |
| Status | string | ❌ | `Approved` | ⚠️ Enum: Pending, Approved, Rejected, Banned |
| IsBanned | bool | ❌ | `false` | Lọc bị ban |
| IsDisable | bool | ❌ | `false` | Lọc bị disable |
| FromDate | datetime | ❌ | `2026-07-01T00:00:00Z` | Lọc từ ngày tạo |
| ToDate | datetime | ❌ | `2026-07-13T23:59:59Z` | Lọc đến ngày tạo |
| RoundId | Guid | ❌ | `guid` | Lọc theo round |
| TrackId | Guid | ❌ | `guid` | Lọc theo track |
| TopicId | Guid | ❌ | `guid` | Lọc theo topic |
| PageIndex | int | ❌ | `1` | Mặc định 1 |
| PageSize | int | ❌ | `10` | Mặc định 10 |

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
| 403 | Forbidden | Không phải Admin |
