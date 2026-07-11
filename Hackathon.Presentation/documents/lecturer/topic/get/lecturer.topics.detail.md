# GET /api/v1/lecturer/topics/{topicId}

> Lecturer xem chi tiết 1 topic.

**Controller:** [LecturerTopicController.cs](Controllers/Lecturer/LecturerTopicController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/topics/{topicId}`

- Giống hệt Admin `GET /api/v1/admin/topics/{topicId}`, khác auth là Lecturer.
- Trả về thông tin topic kèm TrackTitle.
- 404 nếu topicId không tồn tại.

## Phân quyền
- ✅ Lecturer

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| topicId | Guid | ID của topic |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "trackId": "guid",
    "trackTitle": "Web3",
    "title": "DeFi",
    "description": "Phát triển ứng dụng DeFi",
    "isDisable": false,
    "createdAt": "2026-07-07T12:00:00Z",
    "updatedAt": "2026-07-07T12:00:00Z"
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
| 404 | Resource Not Found | topicId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/topics/{topicId})
