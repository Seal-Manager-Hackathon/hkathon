# GET /api/v1/staff/awards/{awardId}

> Staff xem chi tiết 1 award. Chỉ xem được award IsDisable = false.

## Nghiệp vụ

- Staff chỉ xem được award có `IsDisable = false`
- Staff phải được assign vào event của award mới xem được
- 404 nếu awardId không tồn tại hoặc đã bị disable

## Phân quyền
- ✅ Staff (phải được assign vào event)

## Request

| Param    | Kiểu | Bắt buộc | Ví dụ |
|----------|------|----------|-------|
| awardId  | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)

```json
{
  "data": {
    "id": "guid",
    "eventId": "guid",
    "name": "Giải Nhất",
    "description": "Trao cho đội có điểm cao nhất",
    "levelAward": 1,
    "numberOfAward": 1,
    "prize": 10000000,
    "isDisable": false,
    "createdAt": "2026-07-07T12:00:00Z",
    "updatedAt": "2026-07-07T12:00:00Z"
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Resource Not Found | awardId ko tồn tại hoặc đã disable |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | You Are Not Assigned to This Event | Staff không được assign vào event |

> **Ref:** [Admin API tương ứng](/api/v1/admin/awards/{awardId}) — [`admin/award/get/admin.awards.detail.md`](../../../admin/award/get/admin.awards.detail.md)