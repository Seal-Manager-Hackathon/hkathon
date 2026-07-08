# GET /api/v1/admin/register-teams/{registerTeamId}

> Lấy chi tiết register team kèm thông tin event, team, track, topic, và danh sách thành viên.

## Nghiệp vụ
- Trả ra tất cả thông tin: register team, event, team, track, topic, members
- 404 nếu registerTeamId không tồn tại

## Phân quyền
- ✅ Admin

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
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Register Team Not Found | registerTeamId không tồn tại | Báo "Không tìm thấy đơn đăng ký" |
