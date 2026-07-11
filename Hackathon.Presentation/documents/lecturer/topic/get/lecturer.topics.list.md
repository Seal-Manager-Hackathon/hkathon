# GET /api/v1/lecturer/tracks/{trackId}/topics

> Lecturer lấy danh sách topics của 1 track (chỉ topic đang hoạt động), phân trang, có search keyword.

**Controller:** [LecturerTopicController.cs](Controllers/Lecturer/LecturerTopicController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/tracks/{trackId}/topics`

- Giống Admin `GET /api/v1/admin/tracks/{trackId}/topics` về request/response, khác auth là Lecturer.
- **Khác Admin:** Luôn filter `IsDisable = false` — chỉ lấy topic đang hoạt động.
- Keyword search theo title (contains, không phân biệt hoa thường).
- Sắp xếp theo CreatedAt giảm dần.
- Phân trang: mặc định pageIndex=1, pageSize=10.
- 404 nếu trackId không tồn tại.

## Phân quyền
- ✅ Lecturer

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| trackId | Guid | ID của track |

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
    "topics": [
      {
        "id": "guid",
        "trackId": "guid",
        "trackTitle": "Web3",
        "title": "DeFi",
        "description": "...",
        "isDisable": false,
        "createdAt": "2026-07-07T12:00:00Z",
        "updatedAt": "2026-07-07T12:00:00Z"
      }
    ],
    "totalCount": 5,
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
| 404 | Resource Not Found | trackId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/tracks/{trackId}/topics)
