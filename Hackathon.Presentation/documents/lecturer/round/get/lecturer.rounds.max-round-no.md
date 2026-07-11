# GET /api/v1/lecturer/events/{eventId}/rounds/max-round-no

> Lecturer lấy roundNo lớn nhất hiện tại của event. Dùng để FE biết round tiếp theo sẽ là số mấy.

**Controller:** [LecturerRoundController.cs](Controllers/Lecturer/LecturerRoundController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/events/{eventId}/rounds/max-round-no`

- Giống hệt Admin `GET /api/v1/admin/events/{eventId}/rounds/max-round-no`, khác auth là Lecturer.
- Trả về `roundNo` lớn nhất trong event.
- Nếu chưa có round nào → trả về `null`.
- 404 nếu eventId không tồn tại.

## Phân quyền
- ✅ Lecturer

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event |

## Response (200)
```json
{
  "data": 2,
  "message": "Fetched Successfully",
  "traceId": "..."
}
```

Nếu chưa có round nào:
```json
{
  "data": null,
  "message": "Fetched Successfully",
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |
| 404 | Event Not Found | eventId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId}/rounds/max-round-no)
