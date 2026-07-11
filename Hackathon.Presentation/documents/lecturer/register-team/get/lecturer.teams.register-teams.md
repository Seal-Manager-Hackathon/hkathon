# GET /api/v1/lecturer/teams/{teamId}/register-teams

> Lecturer lấy danh sách register teams của 1 team, phân trang, lọc theo status.

**Controller:** [LecturerRegisterTeamController.cs](Controllers/Lecturer/LecturerRegisterTeamController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/teams/{teamId}/register-teams`

- Giống hệt Admin `GET /api/v1/admin/teams/{teamId}/register-teams`, khác auth là Lecturer.
- Lọc Status: Pending, Approved, Rejected, Banned — không truyền = lấy tất cả.
- Sắp xếp gần nhất trên cùng.
- Phân trang: mặc định pageIndex=1, pageSize=10.

## Phân quyền
- ✅ Lecturer

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| teamId | Guid | ID của team |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| Status | string | No | - | Enum: Pending, Approved, Rejected, Banned |
| IsDisable | bool | No | - | Lọc theo trạng thái disable |
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
        "status": "Approved",
        "isBanned": false,
        "isDisable": false,
        "roundId": "guid",
        "roundName": "Vòng 1",
        "roundNo": 1,
        "createdAt": "...",
        "updatedAt": "..."
      }
    ],
    "totalCount": 3,
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
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |

> **Ref:** [Admin API tương ứng](/api/v1/admin/teams/{teamId}/register-teams)
