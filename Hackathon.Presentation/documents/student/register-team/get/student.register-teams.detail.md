# GET /api/v1/student/register-teams/{registerTeamId}

> Student xem chi tiết 1 register team (chỉ Approved).

**Controller:** [StudentRegisterTeamController.cs](Controllers/Student/StudentRegisterTeamController.cs)

## Nghiệp vụ
- Xem chi tiết register team: thông tin team, event, track, topic, members.
- **Chỉ xem được** register team có status = Approved, ko disable, ko banned.
- Nếu pending/rejected/banned/disable → 404.

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| registerTeamId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "description": "...",
    "rejectionReason": null,
    "status": "Approved",
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
    "teamName": "Fteam",
    "teamCanEdit": false,
    "teamIsDisable": false,
    "teamCreatedAt": "2026-07-07T12:00:00Z",
    "trackId": null,
    "trackTitle": null,
    "topicId": null,
    "topicTitle": null,
    "members": [
      {
        "userId": "guid",
        "email": "user@email.com",
        "firstName": "Nguyễn",
        "lastName": "Văn A",
        "avatarUrl": "https://robohash.org/...",
        "isLeader": true,
        "status": "Active"
      }
    ]
  },
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Student | Ẩn chức năng |
| 404 | Register Team Not Found | ko tồn tại / disable / banned / ko Approved | Báo "Không tìm thấy đội" |

> **Ref:** [Admin API tương ứng](/api/v1/admin/register-teams/{registerTeamId})
