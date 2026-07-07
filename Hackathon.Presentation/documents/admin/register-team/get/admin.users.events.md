# GET /api/v1/admin/users/{userId}/events

> Lấy danh sách event mà user đã tham gia (register team được approved), phân trang.

## Nghiệp vụ
- Chỉ lấy register team có status `Approved`
- Có keyword search theo tên event
- Không có event nào → trả mảng rỗng, không lỗi

## Phân quyền
- ✅ Admin

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| Keyword | string | ❌ | `Hackathon` | Search tên event |
| PageIndex | int | ❌ | `1` | Mặc định 1 |
| PageSize | int | ❌ | `10` | Mặc định 10 |

## Response (200)
```json
{
  "data": {
    "events": [
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
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | Page Index / Page Size | Pagination sai | Fix pagination |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
