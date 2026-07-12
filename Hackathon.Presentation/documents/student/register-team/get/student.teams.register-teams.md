# GET /api/v1/student/teams/{teamId}/register-teams

> Student lấy danh sách register team Approved của 1 team.

**Controller:** [StudentRegisterTeamController.cs](Controllers/Student/StudentRegisterTeamController.cs)

## Nghiệp vụ
- Lấy danh sách register team của team, chỉ lấy Approved (`Status = Approved`), ko disable (`IsDisable = false`).
- Hỗ trợ phân trang.

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| teamId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |
| PageIndex | int | ❌ (query) | `1` |
| PageSize | int | ❌ (query) | `10` |

## Response (200)
```json
{
  "data": {
    "registerTeams": [
      {
        "id": "guid",
        "registerTeamId": "guid",
        "teamId": "guid",
        "teamName": "Fteam",
        "eventId": "guid",
        "eventName": "Hackathon 2026",
        "trackId": null,
        "trackName": null,
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
        "createdAt": "2026-07-07T12:00:00Z",
        "updatedAt": "2026-07-07T12:00:00Z"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
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
| 400 | Page Index Must Be Greater Than 0 | pageIndex < 1 | Báo "Trang không hợp lệ" |
| 400 | Page Size Must Be Between 1 And 100 | pageSize < 1 hoặc > 100 | Báo "Kích thước trang không hợp lệ" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Student | Ẩn chức năng |

> **Ref:** [Admin API tương ứng](/api/v1/admin/teams/{teamId}/register-teams)
