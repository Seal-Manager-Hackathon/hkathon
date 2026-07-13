# GET /api/v1/admin/events/{eventId}/leaderboard

> Admin xem bảng xếp hạng 1 event (eventScore từng team, sắp xếp theo điểm giảm dần).

## Phân quyền
- ✅ Admin

## Request

| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| eventId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |
| PageIndex | int | ❌ | `1` |
| PageSize | int | ❌ | `10` |

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
          },
          {
            "roundNo": 2,
            "roundName": "Vòng 2",
            "scopeScore": 75.00
          },
          {
            "roundNo": 3,
            "roundName": "Vòng 3 (CK)",
            "scopeScore": 62.50
          }
        ]
      }
    ],
    "totalCount": 10,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

### Scoring
- `eventScore` = weighted average scopeScores (weight_i = 1, mẫu số = số round không bị disable trong event)
- `roundScores[].scopeScore` = từng round team đã tham gia
- Team ko tham gia round = 0 điểm trong mẫu số
- Cách xếp hạng sử dụng **DENSE_RANK**: các đội có cùng tổng điểm sẽ xếp chung một hạng, và hạng tiếp theo là hạng liền kề (không nhảy hạng). Ví dụ: 2 đội cùng 250đ đều xếp hạng 1, đội 200đ xếp hạng 2.

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
