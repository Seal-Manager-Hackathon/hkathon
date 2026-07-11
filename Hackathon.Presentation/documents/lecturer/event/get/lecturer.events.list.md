# GET /api/v1/lecturer/events

> Lecturer lấy danh sách events có phân trang, search keyword, lọc status, thời gian, isDisable.

**Controller:** [LecturerEventController.cs](Controllers/Lecturer/LecturerEventController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/events`

- Giống hệt Admin `GET /api/v1/admin/events`, khác auth là Lecturer.
- Lecturer thấy tất cả events (không giới hạn theo assign).
- Keyword search theo tên event (contains, không phân biệt hoa thường).
- Filter Status (Draft, Published, Closed).
- Filter IsDisable.
- FromDate / ToDate theo CreatedAt.
- Sắp xếp gần nhất trên cùng.
- Phân trang: mặc định pageIndex=1, pageSize=10.

## Phân quyền
- ✅ Lecturer

## Request

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| Keyword | string | No | - | Tìm kiếm theo tên event |
| Status | string | No | - | Enum: Draft, Published, Closed |
| IsDisable | bool | No | - | Lọc theo trạng thái disable |
| FromDate | datetime | No | - | Lọc từ ngày tạo |
| ToDate | datetime | No | - | Lọc đến ngày tạo |
| PageIndex | int | No | 1 | Trang hiện tại |
| PageSize | int | No | 10 | Số lượng mỗi trang |

## Response (200)
```json
{
  "data": {
    "events": [
      {
        "id": "guid",
        "name": "Hackathon 2026",
        "description": "...",
        "status": "Published",
        "startTime": "2026-08-01T00:00:00Z",
        "endTime": "2026-08-10T00:00:00Z",
        "isDisable": false,
        "createdAt": "2026-07-07T12:00:00Z",
        "updatedAt": "2026-07-07T12:00:00Z"
      }
    ],
    "totalCount": 10,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Invalid Status. Must be: Draft, Published, Closed | Status sai |
| 400 | Page Index Must Be Greater Than Zero | PageIndex < 1 |
| 400 | Page Size Must Be Between 1 And 100 | PageSize ngoài khoảng |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events)
