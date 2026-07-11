# GET /api/v1/lecturer/events/{eventId}/register-teams

> Lecturer lấy danh sách register teams trong 1 event, có filter keyword, status, isBanned, isDisable, thời gian, round, track, topic.

**Controller:** [LecturerRegisterTeamController.cs](Controllers/Lecturer/LecturerRegisterTeamController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/events/{eventId}/register-teams`

- Giống hệt Admin `GET /api/v1/admin/events/{eventId}/register-teams`, khác auth là Lecturer.
- Keyword search theo tên team.
- Filter Status (Pending, Approved, Rejected, Banned), IsBanned, IsDisable.
- Lọc FromDate / ToDate theo CreatedAt.
- Lọc theo RoundId, TrackId, TopicId.
- Sắp xếp gần nhất trên cùng.
- Phân trang: mặc định pageIndex=1, pageSize=10.

## Phân quyền
- ✅ Lecturer

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| Keyword | string | No | - | Tìm kiếm theo tên team |
| Status | string | No | - | Enum: Pending, Approved, Rejected, Banned |
| IsBanned | bool | No | - | Lọc theo trạng thái ban |
| IsDisable | bool | No | - | Lọc theo trạng thái disable |
| FromDate | datetime | No | - | Lọc từ ngày tạo |
| ToDate | datetime | No | - | Lọc đến ngày tạo |
| RoundId | Guid | No | - | Lọc theo round |
| TrackId | Guid | No | - | Lọc theo track |
| TopicId | Guid | No | - | Lọc theo topic |
| PageIndex | int | No | 1 | Trang hiện tại |
| PageSize | int | No | 10 | Số lượng mỗi trang |

## Response (200)
```json
{
  "data": {
    "registerTeams": [
      {
        "id": "guid",
        "registerTeamId": "guid",
        "teamId": "guid",
        "teamName": "FTeam",
        "eventId": "guid",
        "eventName": "Hackathon 2026",
        "trackId": "guid",
        "trackName": "Web3",
        "topicId": null,
        "topicName": null,
        "description": "...",
        "rejectionReason": null,
        "status": "Pending",
        "isBanned": false,
        "isDisable": false,
        "roundId": "guid",
        "roundName": "Vòng 1",
        "roundNo": 1,
        "createdAt": "2026-07-07T12:00:00Z",
        "updatedAt": "2026-07-07T12:00:00Z"
      }
    ],
    "totalCount": 42,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Register Teams Fetched Successfully",
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Invalid Status. Must be: Pending, Approved, Rejected, Banned | Status sai |
| 400 | Page Index Must Be Greater Than Zero | PageIndex < 1 |
| 400 | Page Size Must Be Between 1 And 100 | PageSize ngoài khoảng |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId}/register-teams)
