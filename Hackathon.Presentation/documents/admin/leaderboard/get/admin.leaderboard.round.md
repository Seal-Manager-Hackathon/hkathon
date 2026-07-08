# GET /api/v1/admin/rounds/{roundId}/leaderboard

> Admin xem bảng xếp hạng 1 round (scopeScore từng team, sắp xếp theo điểm giảm dần).

## Phân quyền
- ✅ Admin

## Request

| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| roundId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |
| PageIndex | int | ❌ | `1` |
| PageSize | int | ❌ | `10` |

## Response (200)

```json
{
  "data": {
    "roundId": "guid",
    "roundName": "Vòng 1",
    "eventId": "guid",
    "eventName": "Hackathon 2026",
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
        "lastSubmissionId": "guid",
        "totalScore": 85.50
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
- `totalScore` = **scopeScore** = SUM(AVG(judgeScore) GROUP BY CriteriaItemId)
- Chỉ tính submission cuối cùng của team trong round
- Chỉ tính judge đã chấm thực tế (Score.HasValue = true)

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Round Not Found | roundId không tồn tại |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
