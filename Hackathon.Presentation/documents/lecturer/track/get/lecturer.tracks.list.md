# GET /api/v1/lecturer/events/{eventId}/tracks

> Lecturer lấy danh sách tracks của 1 event (chỉ track đang hoạt động), phân trang, có search keyword.

**Controller:** [LecturerTrackController.cs](Controllers/Lecturer/LecturerTrackController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/events/{eventId}/tracks`

- Giống Admin `GET /api/v1/admin/events/{eventId}/tracks` về request/response, khác auth là Lecturer.
- **Khác Admin:** Luôn filter `IsDisable = false` — chỉ lấy track đang hoạt động.
- Keyword search theo title (contains, không phân biệt hoa thường).
- Sắp xếp theo CreatedAt giảm dần.
- Phân trang: mặc định pageIndex=1, pageSize=10.

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
| Keyword | string | No | - | Tìm kiếm theo title |
| PageIndex | int | No | 1 | Trang hiện tại |
| PageSize | int | No | 10 | Số lượng mỗi trang |

## Response (200)
```json
{
  "data": {
    "tracks": [
      {
        "id": "guid",
        "eventId": "guid",
        "title": "Web3",
        "description": "...",
        "maxTeam": 20,
        "isDisable": false,
        "createdAt": "2026-07-07T12:00:00Z",
        "updatedAt": "2026-07-07T12:00:00Z"
      }
    ],
    "totalCount": 3,
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

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId}/tracks)
