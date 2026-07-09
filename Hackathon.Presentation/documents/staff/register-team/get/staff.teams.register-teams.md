# GET /api/v1/staff/teams/{teamId}/register-teams

> Lấy danh sách register teams của 1 team (các event mà team đã đăng ký tham gia).

## Nghiệp vụ
- Trả về danh sách register team của team
- Dùng DTO `RegisterTeamCard` (giống admin `events/{eventId}/register-teams`)
- Hỗ trợ lọc theo Status và IsDisable
- Hỗ trợ phân trang

## Phân quyền
- ✅ Staff

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| teamId | Guid | ID của team |

### Query Parameters
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| Status | string | ❌ | `Approved` | Lọc theo trạng thái |
| IsDisable | bool | ❌ | `false` | Lọc disable |
| PageIndex | int | ❌ | `1` | Mặc định 1 |
| PageSize | int | ❌ | `10` | Mặc định 10 |

## Response (200)
```json
{
  "data": {
    "registerTeams": [
      {
        "registerTeamId": "guid",
        "status": "Approved",
        "isBanned": false,
        "isDisable": false,
        "createdAt": "...",
        "updatedAt": "...",
        "eventId": "guid",
        "eventName": "Hackathon 2026",
        "eventDescription": "...",
        "eventStartTime": "...",
        "eventEndTime": "...",
        "eventStatus": "Published",
        "teamId": "guid",
        "teamName": "FTeam",
        "trackId": "guid",
        "trackTitle": "Web3",
        "topicId": null,
        "topicTitle": null
      }
    ],
    "totalCount": 5,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | Page Index / Page Size | Pagination sai | Fix pagination |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Staff | Ẩn chức năng |
