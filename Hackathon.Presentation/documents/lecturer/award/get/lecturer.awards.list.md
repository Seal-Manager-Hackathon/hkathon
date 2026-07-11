# GET /api/v1/lecturer/events/{eventId}/awards

> Lecturer lấy danh sách awards của event (chỉ award đang active, không bị disable).

**Controller:** [LecturerAwardController.cs](Controllers/Lecturer/LecturerAwardController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/events/{eventId}/awards`

- Giống hệt Admin `GET /api/v1/admin/events/{eventId}/awards`, khác auth là Lecturer.
- Trả về danh sách giải thưởng của 1 event.
- **Khác Admin:** Luôn filter `IsDisable = false` — Lecturer không thấy award đã bị xóa mềm.
- Sắp xếp theo LevelAward tăng dần: giải Nhất (level 1) lên trên cùng, giải thấp hơn ở dưới.
- Hỗ trợ tìm kiếm theo tên award (contains, không phân biệt hoa thường).
- Phân trang: mặc định pageIndex=1, pageSize=10.
- 404 nếu eventId không tồn tại.

## Phân quyền
- ✅ Lecturer

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| Keyword | string | No | - | Tìm kiếm theo tên award |
| PageIndex | int | No | 1 | Trang hiện tại |
| PageSize | int | No | 10 | Số lượng item mỗi trang |

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
    "pageSize": 10
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
| 404 | Resource Not Found | eventId không tồn tại |
| 400 | PageIndex/PageSize invalid | PageIndex hoặc PageSize không hợp lệ |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId}/awards)
