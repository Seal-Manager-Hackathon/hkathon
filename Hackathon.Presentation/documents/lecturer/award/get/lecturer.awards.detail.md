# GET /api/v1/lecturer/awards/{awardId}

> Lecturer xem chi tiết 1 award.
> **Controller:** `LecturerAwardController` — `GET /api/v1/lecturer/awards/{awardId}`

## Nghiệp vụ

- Lecturer xem chi tiết giải thưởng.
- Response giống hệt Admin `GET /api/v1/admin/events/{eventId}/awards/{awardId}`.

## Phân quyền
- ✅ Lecturer

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
| 403 | Forbidden | Không phải Lecturer |
| 404 | Resource Not Found | awardId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId}/awards/{awardId})
