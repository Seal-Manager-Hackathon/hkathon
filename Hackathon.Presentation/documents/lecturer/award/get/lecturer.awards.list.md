# GET /api/v1/lecturer/events/{eventId}/awards

> Lecturer lấy danh sách awards của event — chỉ lấy award không bị disable.

## Nghiệp vụ

- Lecturer xem danh sách giải thưởng của 1 event.
- Tự động lọc chỉ lấy award có `IsDisable = false`.
- Hỗ trợ tìm kiếm theo từ khóa (`keyword`) — tìm theo tên award.
- Không cho phép lọc theo isDisable.
- Sắp xếp theo LevelAward tăng dần.
- Response giống hệt Admin `GET /api/v1/admin/events/{eventId}/awards`.

## Phân quyền
- ✅ Lecturer

## Request
| Parameter | Type | Bắt buộc | Ghi chú |
|-----------|------|----------|---------|
| eventId | Guid | ✅ (route) | ID của event |
| keyword | string | ❌ (query) | Tìm theo tên award |

## Response (200)
```json
{
  "data": {
    "awards": [
      {
        "id": "guid",
        "eventId": "guid",
        "name": "Giải Nhất",
        "description": "Phần thưởng cho đội đạt giải nhất",
        "levelAward": 1,
        "numberOfAward": 1,
        "prize": 10000000,
        "isDisable": false,
        "createdAt": "2026-07-07T12:00:00Z",
        "updatedAt": "2026-07-07T12:00:00Z"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 1
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
| 404 | Resource Not Found | eventId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId}/awards)
