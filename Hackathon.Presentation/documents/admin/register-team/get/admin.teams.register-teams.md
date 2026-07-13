# GET /api/v1/admin/teams/{teamId}/register-teams

> Lấy danh sách register teams của 1 team, phân trang, lọc theo status.

## Nghiệp vụ
- Lọc Status: Pending, Approved, Rejected — không truyền = lấy tất cả
- Sắp xếp gần nhất trên cùng

## Phân quyền
- ✅ Admin

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| Status | string | ❌ | `Approved` | ⚠️ Enum: Pending, Approved, Rejected, Banned |
| IsDisable | bool | ❌ | `false` | Lọc theo trạng thái disable |
| PageIndex | int | ❌ | `1` | Mặc định 1 |
| PageSize | int | ❌ | `10` | Mặc định 10 |

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
        "createdAt": "...",
        "updatedAt": "..."
      }
    ],
    "totalCount": 3,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Register Teams Fetched Successfully",
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
| 400 | Invalid Status. Must be: Pending, Approved, Rejected, Banned | Status sai | Báo "Trạng thái không hợp lệ" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
