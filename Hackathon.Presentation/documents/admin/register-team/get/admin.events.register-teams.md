# GET /api/v1/admin/events/{eventId}/register-teams

> Lấy danh sách register teams phân trang, có filter keyword, status, isBanned, isDisable, thời gian tạo, round, track, topic.

## Nghiệp vụ
- Keyword search theo tên team
- Filter Status (Pending, Approved, Rejected, Banned), IsBanned, IsDisable
- Lọc FromDate / ToDate theo CreatedAt
- Lọc theo RoundId, TrackId, TopicId
- Sắp xếp gần nhất trên cùng

## Phân quyền
- ✅ Admin

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| Keyword | string | ❌ | `FTeam` | Search tên team |
| Status | string | ❌ | `Pending` | ⚠️ Enum: Pending, Approved, Rejected, Banned |
| IsBanned | bool | ❌ | `false` | |
| IsDisable | bool | ❌ | `false` | |
| FromDate | datetime | ❌ | `2026-07-01T00:00:00Z` | |
| ToDate | datetime | ❌ | `2026-07-07T23:59:59Z` | |
| RoundId | guid | ❌ | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | |
| TrackId | guid | ❌ | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | |
| TopicId | guid | ❌ | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | |
| PageIndex | int | ❌ | `1` | Mặc định 1 |
| PageSize | int | ❌ | `10` | Mặc định 10 |

## Response (200)
```json
{
  "data": {
    "registerTeams": [
      {
        "id": "guid",
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
        "createdAt": "2026-07-07T12:00:00Z",
        "updatedAt": "2026-07-07T12:00:00Z"
      }
    ],
    "totalCount": 42,
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
| 400 | Invalid Status. Must be: Pending, Approved, Rejected | Status sai | Báo "Trạng thái không hợp lệ" |
| 400 | Page Index Must Be Greater Than Zero | PageIndex < 1 | Fix pagination |
| 400 | Page Size Must Be Between 1 And 100 | PageSize ngoài khoảng | Fix pagination |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
