# GET /api/v1/lecturer/events/chapter/{year}/leaderboard

> Lecturer xem bảng xếp hạng chapter (1 năm) — điểm chapterScore từng team, sắp xếp theo điểm giảm dần.

**Controller:** [LecturerLeaderboardController.cs](Controllers/Lecturer/LecturerLeaderboardController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/events/chapter/{year}/leaderboard`

- Giống hệt Admin `GET /api/v1/admin/events/chapter/{year}/leaderboard`, khác auth là Lecturer.
- chapterScore = AVG(eventScore) của các event team đã tham gia trong năm.
- eventScores[].eventScore = eventScore của team trong từng event (weighted avg scopeScores).
- Dùng Average để chuẩn hóa giữa event có số round khác nhau.
- Phân trang: mặc định pageIndex=1, pageSize=10.

## Phân quyền
- ✅ Lecturer

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| year | int | Năm (VD: 2026) |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| PageIndex | int | No | 1 | Trang hiện tại |
| PageSize | int | No | 10 | Số lượng mỗi trang |

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
          }
        ]
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

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/chapter/{year}/leaderboard)
