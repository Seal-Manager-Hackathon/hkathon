# GET /api/v1/lecturer/events

> Lecturer lấy danh sách events phân trang, có search keyword, lọc status, thời gian tạo.

**Controller:** [LecturerEventController.cs](Controllers/Lecturer/LecturerEventController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/events`

- Giống hệt Admin `GET /api/v1/admin/events`, khác auth là Lecturer.
- Lecturer thấy tất cả events (không giới hạn theo assign).
- Keyword search theo tên event.
- Filter Status (Draft, Published, Closed).
- Filter IsDisable.
- FromDate / ToDate theo CreatedAt.
- Sắp xếp gần nhất trên cùng.
- Phân trang: mặc định pageIndex=1, pageSize=10.

## Phân quyền
- ✅ Lecturer

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| Keyword | string | ❌ | `Hackathon` | Search tên event |
| Status | string | ❌ | `Published` | ⚠️ Enum: Draft, Published, Closed |
| IsDisable | bool | ❌ | `false` | Lọc theo trạng thái disable |
| FromDate | datetime | ❌ | `2026-07-01T00:00:00Z` | |
| ToDate | datetime | ❌ | `2026-07-07T23:59:59Z` | |
| PageIndex | int | ❌ | `1` | Mặc định 1 |
| PageSize | int | ❌ | `10` | Mặc định 10 |

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
| 400 | Invalid Status. Must be: Draft, Published, Closed | Status sai |
| 400 | Page Index Must Be Greater Than Zero | PageIndex < 1 |
| 400 | Page Size Must Be Between 1 And 100 | PageSize ngoài khoảng |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events)
