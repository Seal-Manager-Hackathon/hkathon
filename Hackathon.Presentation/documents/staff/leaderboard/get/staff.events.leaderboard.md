# GET /api/v1/staff/events/{eventId}/leaderboard

> Staff xem bảng xếp hạng tổng của event được phân công.

## Nghiệp vụ

- Tính điểm event (`eventScore`) cho từng register team đã được duyệt (Approved) trong event
- `eventScore` = weighted average (weight=1) của điểm các round
  - Công thức: `SUM(scopeScore của từng round) / totalRounds`
- Mỗi round chỉ lấy submission cuối cùng của team, scopeScore = SUM(Scores.TotalScore)
- Sắp xếp theo điểm **từ cao xuống thấp**
- Phân trang
- Staff phải được assign vào event này

## Phân quyền

- ✅ Staff (phải được assign vào event)

## Request

| Param   | Kiểu | Bắt buộc | Ví dụ |
|---------|------|----------|-------|
| eventId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

| Param     | Kiểu | Bắt buộc | Mô tả |
|-----------|------|----------|-------|
| pageIndex | int  | ❌        | Mặc định 1 |
| pageSize  | int  | ❌        | Mặc định 10, tối đa 100 |

## Response (200)

```json
{
  "data": {
    "eventId": "guid",
    "eventName": "Tên event",
    "totalRounds": 3,
    "items": [
      {
        "rank": 1,
        "registerTeamId": "guid",
        "teamId": "guid",
        "teamName": "Team A",
        "trackId": "guid",
        "trackTitle": "AI - Trí tuệ nhân tạo",
        "topicId": "guid",
        "topicTitle": "Chatbot",
        "eventScore": 88.33,
        "roundScores": [
          { "roundNo": 1, "roundName": "Vòng 1", "scopeScore": 95.0 },
          { "roundNo": 2, "roundName": "Vòng 2", "scopeScore": 85.0 },
          { "roundNo": 3, "roundName": "Vòng 3", "scopeScore": 85.0 }
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

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Resource Not Found | eventId ko tồn tại |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | You Are Not Assigned to This Event | Staff không được assign vào event |

> **Ref:** [Admin API tương ứng](/api/v1/admin/leaderboard/get/admin.leaderboard.event.md)