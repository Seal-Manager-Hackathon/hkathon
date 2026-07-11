# GET /api/v1/lecturer/register-teams/{registerTeamId}

> Lecturer xem chi tiết register team kèm thông tin event, team, track, topic, và danh sách thành viên.

**Controller:** [LecturerRegisterTeamController.cs](Controllers/Lecturer/LecturerRegisterTeamController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/register-teams/{registerTeamId}`

- Giống hệt Admin `GET /api/v1/admin/register-teams/{registerTeamId}`, khác auth là Lecturer.
- Trả ra tất cả thông tin: register team, event, team, track, topic, members.
- 404 nếu registerTeamId không tồn tại.

## Phân quyền
- ✅ Lecturer

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| registerTeamId | Guid | ID của register team |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "description": "...",
    "rejectionReason": null,
    "status": "Pending",
    "isBanned": false,
    "isDisable": false,
    "roundId": "guid",
    "roundName": "Vòng 1",
    "roundNo": 1,
    "createdAt": "2026-07-07T12:00:00Z",
    "updatedAt": "2026-07-07T12:00:00Z",
    "eventId": "guid",
    "eventName": "Hackathon 2026",
    "eventDescription": "...",
    "eventStartDate": "2026-08-01T00:00:00Z",
    "eventEndDate": "2026-08-10T00:00:00Z",
    "teamId": "guid",
    "teamName": "FTeam",
    "teamCanEdit": true,
    "teamIsDisable": false,
    "teamCreatedAt": "2026-06-01T00:00:00Z",
    "trackId": "guid",
    "trackTitle": "Web3",
    "topicId": null,
    "topicTitle": null,
    "members": [
      {
        "userId": "guid",
        "email": "leader@fpt.edu.vn",
        "firstName": "Leader",
        "lastName": "User",
        "avatarUrl": "https://robohash.org/...",
        "isLeader": true,
        "status": "Active"
      }
    ]
  },
  "message": "Register Team Detail Fetched Successfully",
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |
| 404 | Register Team Not Found | registerTeamId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/register-teams/{registerTeamId})
