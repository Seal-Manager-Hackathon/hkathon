# GET /api/v1/admin/events/chapter/{year}/leaderboard

> Admin xem bảng xếp hạng chapter (1 năm) — điểm chapterScore từng team, sắp xếp theo điểm giảm dần.

## Phân quyền
- ✅ Admin

## Request

| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| year | int | ✅ (route) | `2026` |
| PageIndex | int | ❌ | `1` |
| PageSize | int | ❌ | `10` |

## Response (200)

```json
{
  "data": {
    "year": 2026,
    "eventCount": 3,
    "items": [
      {
        "rank": 1,
        "teamId": "guid",
        "teamName": "FTeam",
        "chapterScore": 78.25,
        "eventCount": 2,
        "eventScores": [
          {
            "eventId": "guid",
            "eventName": "Hackathon Spring 2026",
            "registerTeamId": "guid",
            "eventScore": 72.33
          },
          {
            "eventId": "guid",
            "eventName": "Hackathon Summer 2026",
            "registerTeamId": "guid",
            "eventScore": 84.17
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
- `chapterScore` = AVG(eventScore) của các event team đã tham gia trong năm
- `eventScores[].eventScore` = eventScore của team trong từng event (weighted avg scopeScores)
- Dùng Average để chuẩn hóa giữa event có số round khác nhau
- Cách xếp hạng sử dụng **DENSE_RANK**: các đội có cùng tổng điểm sẽ xếp chung một hạng, và hạng tiếp theo là hạng liền kề (không nhảy hạng). Ví dụ: 2 đội cùng 250đ đều xếp hạng 1, đội 200đ xếp hạng 2.

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
