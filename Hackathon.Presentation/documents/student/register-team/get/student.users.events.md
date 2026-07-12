# GET /api/v1/student/users/{userId}/events

> Student lấy danh sách event mà user đã đăng ký team (Approved).

**Controller:** [StudentRegisterTeamController.cs](Controllers/Student/StudentRegisterTeamController.cs)

## Nghiệp vụ
- Lấy danh sách event mà user đã đăng ký team, chỉ lấy register team đã Approved.
- Tự động lấy userId từ token nếu ko truyền.
- Hỗ trợ tìm kiếm theo từ khóa (tên event).

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| userId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |
| Keyword | string | ❌ (query) | `Hackathon` |
| PageIndex | int | ❌ (query) | `1` |
| PageSize | int | ❌ (query) | `10` |

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
        "createdAt": "2026-07-07T12:00:00Z",
        "updatedAt": "2026-07-07T12:00:00Z",
        "eventId": "guid",
        "eventName": "Hackathon 2026",
        "eventDescription": "...",
        "eventStartTime": "2026-08-01T00:00:00Z",
        "eventEndTime": "2026-08-10T00:00:00Z",
        "eventStatus": "Published",
        "teamId": "guid",
        "teamName": "Fteam",
        "trackId": null,
        "trackTitle": null,
        "topicId": null,
        "topicTitle": null
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

> **Ref:** [Admin API tương ứng](/api/v1/admin/users/{userId}/events)
