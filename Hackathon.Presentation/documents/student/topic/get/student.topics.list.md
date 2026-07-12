# GET /api/v1/student/tracks/{trackId}/topics

> Student lấy danh sách topic của 1 track (chỉ topic ko disable).

**Controller:** [StudentTopicController.cs](Controllers/Student/StudentTopicController.cs)

## Nghiệp vụ
- Lấy danh sách topic của track.
- **Tự động filter** chỉ lấy topic có `IsDisable = false`.
- Hỗ trợ tìm kiếm theo title (contains, ko phân biệt hoa thường).
- Sắp xếp theo CreatedAt giảm dần.
- 404 nếu trackId ko tồn tại.

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| trackId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |
| Keyword | string | ❌ (query) | `Blockchain` |
| PageIndex | int | ❌ (query) | `1` |
| PageSize | int | ❌ (query) | `10` |

## Response (200)
```json
{
  "data": {
    "topics": [
      {
        "id": "guid",
        "trackId": "guid",
        "trackTitle": "AI",
        "title": "Blockchain",
        "description": "...",
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
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Student | Ẩn chức năng |
| 404 | Resource Not Found | trackId ko tồn tại | Báo "Không tìm thấy track" |

> **Ref:** [Admin API tương ứng](/api/v1/admin/tracks/{trackId}/topics)
