# GET /api/v1/lecturer/tracks/{trackId}/topics

> Lecturer lấy danh sách topic của 1 track, có phân trang và tìm kiếm.

**Controller:** [LecturerTopicController.cs](Controllers/Lecturer/LecturerTopicController.cs)

## Nghiệp vụ

- Lecturer xem tất cả topic của track.
- Hỗ trợ lọc theo IsDisable và tìm kiếm theo keyword (tên topic).
- Lecturer không cần được assign vào event để xem topics.

## Phân quyền
- ✅ Lecturer (RoleEnum.Lecturer)

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| trackId | Guid | ID của track |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| keyword | string | No | - | Tìm kiếm theo tên topic |
| isDisable | bool | No | - | Lọc theo trạng thái disable |
| pageIndex | int | No | 1 | Trang số |
| pageSize | int | No | 10 | Số item (max 100) |

## Response (200)
```json
{
  "data": {
    "topics": [
      {
        "id": "guid",
        "trackId": "guid",
        "trackTitle": "AI - Trí tuệ nhân tạo",
        "title": "Chatbot thông minh",
        "description": "Phát triển chatbot",
        "isDisable": false,
        "createdAt": "2026-07-11T00:00:00Z",
        "updatedAt": "2026-07-11T00:00:00Z"
      }
    ],
    "totalCount": 3,
    "pageIndex": 1,
    "pageSize": 10
  }
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission | Không phải Lecturer |
| 404 | Resource Not Found | TrackId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/tracks/{trackId}/topics)
