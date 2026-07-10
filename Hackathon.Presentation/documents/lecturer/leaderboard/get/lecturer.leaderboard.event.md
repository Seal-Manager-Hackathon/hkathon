# GET /api/v1/lecturer/events/{eventId}/leaderboard

> Lecturer xem bảng xếp hạng 1 event — chỉ lấy leader board có IsDisable = false.
> **Controller:** `LecturerLeaderboardController` — `GET /api/v1/lecturer/events/{eventId}/leaderboard?pageIndex=1&pageSize=10`

## Nghiệp vụ

- Lecturer xem bảng xếp hạng của 1 event.
- Kiểm tra leader board phải có `IsDisable = false`, nếu không trả về 404.
- Response giống hệt Admin `GET /api/v1/admin/events/{eventId}/leaderboard`.
- `eventScore` là weighted average của scopeScores qua các round.
- Xếp hạng theo DENSE_RANK: cùng điểm → cùng rank, rank tiếp = rank trước + 1.

## Phân quyền
- ✅ Lecturer

## Request
| Parameter | Type | Bắt buộc | Ghi chú |
|-----------|------|----------|---------|
| eventId | Guid | ✅ (route) | ID của event |
| pageIndex | int | ❌ | Mặc định 1 |
| pageSize | int | ❌ | Mặc định 10 |

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
          { "roundNo": 1, "roundName": "Vòng 1", "scopeScore": 80.50 },
          { "roundNo": 2, "roundName": "Vòng 2", "scopeScore": 75.00 },
          { "roundNo": 3, "roundName": "Vòng 3 (CK)", "scopeScore": 62.50 }
        ]
      }
    ],
    "totalCount": 10,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | Forbidden | Không phải Lecturer |
| 404 | Resource Not Found | eventId ko tồn tại hoặc leader board bị disable |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId}/leaderboard)
