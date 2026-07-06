# GET /api/v1/admin/events/{eventId}

> Lấy chi tiết thông tin event.

## Nghiệp vụ
- Trả về tất cả fields của event
- 404 nếu eventId không tồn tại

## Phân quyền
- ✅ Admin

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| eventId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "name": "Hackathon 2026",
    "description": "...",
    "startTime": "2026-08-01T00:00:00Z",
    "endTime": "2026-08-10T00:00:00Z",
    "registerLimitTime": "2026-07-25T00:00:00Z",
    "limitTeam": 50,
    "minMember": 3,
    "maxMember": 5,
    "status": "Published",
    "numberRound": 3,
    "season": "Summer",
    "createdAt": "2026-07-07T12:00:00Z",
    "updatedAt": "2026-07-07T12:00:00Z"
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
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Event Not Found | eventId không tồn tại | Báo "Không tìm thấy sự kiện" |
