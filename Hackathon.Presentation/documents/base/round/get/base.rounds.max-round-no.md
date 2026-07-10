# GET /api/v1/rounds/max-round-no?eventId={eventId}

> Lấy số round lớn nhất của 1 event — chỉ cần đăng nhập.
> **Controller:** `RoundController` — `GET /api/v1/rounds/max-round-no?eventId=`

## Nghiệp vụ

- Bất kỳ user nào đã đăng nhập đều có thể lấy max round no của event.
- Response giống hệt Admin `GET /api/v1/admin/events/{eventId}/rounds/max-round-no`.

## Phân quyền
- ✅ Authenticated (chỉ cần đăng nhập)

## Request
| Parameter | Type | Bắt buộc | Ghi chú |
|-----------|------|----------|---------|
| eventId | Guid | ✅ (query) | ID của event |

## Response (200)
```json
{
  "data": 3,
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 404 | Event Not Found | eventId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId}/rounds/max-round-no)
