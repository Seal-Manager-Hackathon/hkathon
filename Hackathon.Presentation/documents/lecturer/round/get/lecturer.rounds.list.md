# GET /api/v1/lecturer/events/{eventId}/rounds

> Lecturer lấy danh sách rounds của event — chỉ lấy round không bị disable.
> **Controller:** `LecturerRoundController` — `GET /api/v1/lecturer/events/{eventId}/rounds?keyword=&roundNo=`

## Nghiệp vụ

- Lecturer xem danh sách vòng thi của 1 event.
- Tự động lọc chỉ lấy round có `IsDisable = false`. Lecturer không thể xem round đã bị xóa mềm.
- Không cho phép lọc theo `isDisable` — mặc định luôn là `false`.
- Hỗ trợ các filter:
  - `keyword`: tìm kiếm theo tên round (không phân biệt hoa thường, tìm chuỗi con).
  - `roundNo`: lọc chính xác theo số thứ tự round.
- Không phân trang — trả về tất cả round thỏa điều kiện.
- Sắp xếp theo thời gian tạo mới nhất.
- Response giống hệt Admin `GET /api/v1/admin/events/{eventId}/rounds`.

## Phân quyền
- ✅ Lecturer

## Request
| Parameter | Type | Bắt buộc | Ghi chú |
|-----------|------|----------|---------|
| eventId | Guid | ✅ (route) | ID của event |
| keyword | string | ❌ (query) | Tìm theo tên round |
| roundNo | int | ❌ (query) | Lọc chính xác roundNo |

## Response (200)
```json
{
  "data": {
    "rounds": [
      {
        "id": "guid",
        "eventId": "guid",
        "name": "Vòng 1",
        "description": "...",
        "roundNo": 1,
        "startTime": "2026-08-01T00:00:00Z",
        "endTime": "2026-08-03T00:00:00Z",
        "startSubmission": "2026-08-03T00:00:00Z",
        "endSubmission": "2026-08-05T00:00:00Z",
        "limitTeam": 20,
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
| 404 | Event Not Found | eventId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId}/rounds)
