# GET /api/v1/lecturer/rounds/{roundId}/leaderboard

> Lecturer xem bảng xếp hạng 1 round (scopeScore từng team, sắp xếp theo điểm giảm dần).

**Controller:** [LecturerLeaderboardController.cs](Controllers/Lecturer/LecturerLeaderboardController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/rounds/{roundId}/leaderboard`

- Giống hệt Admin `GET /api/v1/admin/rounds/{roundId}/leaderboard`, khác auth là Lecturer.
- Xem bảng xếp hạng của 1 round, sắp xếp theo totalScore giảm dần.
- totalScore = scopeScore = SUM(AVG(judgeScore) GROUP BY CriteriaItemId).
- Chỉ tính submission cuối cùng của team trong round.
- Chỉ tính judge đã chấm thực tế (Score.HasValue = true).
- Phân trang: mặc định pageIndex=1, pageSize=10.

## Phân quyền
- ✅ Lecturer

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| roundId | Guid | ID của round |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| PageIndex | int | No | 1 | Trang hiện tại |
| PageSize | int | No | 10 | Số lượng mỗi trang |

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
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |
| 404 | Round Not Found | roundId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/rounds/{roundId}/leaderboard)
