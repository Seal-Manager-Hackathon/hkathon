# GET /api/v1/lecturer/events/{eventId}/rounds

> Lecturer lấy danh sách rounds của event (chỉ round đang hoạt động), phân trang, có thể lọc theo keyword và roundNo.

**Controller:** [LecturerRoundController.cs](Controllers/Lecturer/LecturerRoundController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/events/{eventId}/rounds`

- Giống Admin `GET /api/v1/admin/events/{eventId}/rounds` về request/response, khác auth là Lecturer.
- **Khác Admin:** Luôn filter `IsDisable = false` — dù request có truyền IsDisable gì cũng chỉ lấy round đang hoạt động.
- Keyword tìm kiếm theo tên round (contains, không phân biệt hoa thường).
- Có thể lọc chính xác theo roundNo.
- Sắp xếp theo thời gian tạo mới nhất.
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
| Keyword | string | No | - | Tìm kiếm theo tên round |
| RoundNo | int | No | - | Lọc chính xác theo số thứ tự round |
| PageIndex | int | No | 1 | Trang hiện tại |
| PageSize | int | No | 10 | Số lượng mỗi trang |

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
| 400 | Page Index Must Be Greater Than 0 | PageIndex < 1 |
| 400 | Page Size Must Be Between 1 And 100 | PageSize ngoài khoảng |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |
| 404 | Event Not Found | eventId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId}/rounds)
