# GET /api/v1/awards/{awardId}

> Xem chi tiết một award — chỉ cần đăng nhập. Chỉ truyền awardId, không cần eventId.

## Nghiệp vụ

- Bất kỳ user nào đã đăng nhập đều có thể xem chi tiết award.
- Response giống hệt Admin `GET /api/v1/admin/events/{eventId}/awards/{awardId}`.

## Phân quyền
- ✅ Authenticated (chỉ cần đăng nhập)

## Request
| Parameter | Type | Bắt buộc | Ghi chú |
|-----------|------|----------|---------|
| awardId | Guid | ✅ | ID của award |

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
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 404 | Resource Not Found | awardId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId}/awards/{awardId})
