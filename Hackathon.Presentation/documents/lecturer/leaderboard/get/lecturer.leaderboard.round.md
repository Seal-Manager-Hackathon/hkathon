# GET /api/v1/lecturer/rounds/{roundId}/leaderboard

> Lecturer xem bảng xếp hạng 1 round — y chang Admin.
> **Controller:** `LecturerLeaderboardController` — `GET /api/v1/lecturer/rounds/{roundId}/leaderboard?pageIndex=1&pageSize=10`

## Nghiệp vụ

- Lecturer xem bảng xếp hạng của 1 round.
- `totalScore` = scopeScore = SUM(AVG(judgeScore) GROUP BY CriteriaItemId).
- Chỉ tính submission cuối cùng của team trong round.
- Chỉ tính judge đã chấm thực tế (Score.HasValue = true).
- Response giống hệt Admin `GET /api/v1/admin/rounds/{roundId}/leaderboard`.

## Phân quyền
- ✅ Lecturer

## Request
| Parameter | Type | Bắt buộc | Ghi chú |
|-----------|------|----------|---------|
| roundId | Guid | ✅ (route) | ID của round |
| pageIndex | int | ❌ | Mặc định 1 |
| pageSize | int | ❌ | Mặc định 10 |

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
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | Forbidden | Không phải Lecturer |
| 404 | Round Not Found | roundId ko tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/rounds/{roundId}/leaderboard)
