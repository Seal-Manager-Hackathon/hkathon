# GET /api/v1/lecturer/awards/{awardId}

> Lecturer xem chi tiết 1 award.

**Controller:** [LecturerAwardController.cs](Controllers/Lecturer/LecturerAwardController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/awards/{awardId}`

- Giống hệt Admin `GET /api/v1/admin/awards/{awardId}`, khác auth là Lecturer.
- Trả về chi tiết award bao gồm cả award đã bị disable.
- 404 nếu awardId không tồn tại.

## Phân quyền
- ✅ Lecturer

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| awardId | Guid | ID của award |

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
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |
| 404 | Resource Not Found | awardId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/awards/{awardId})
