# GET /api/v1/topics/{topicId}

> Xem chi tiết một topic — chỉ cần đăng nhập.

## Nghiệp vụ

- Bất kỳ user nào đã đăng nhập đều có thể xem chi tiết topic.
- Response giống hệt Admin `GET /api/v1/admin/topics/{topicId}`.

## Phân quyền
- ✅ Authenticated (chỉ cần đăng nhập)

## Request
| Parameter | Type | Bắt buộc | Ghi chú |
|-----------|------|----------|---------|
| topicId | Guid | ✅ | ID của topic |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "trackId": "guid",
    "trackTitle": "AI",
    "title": "Xử lý ảnh",
    "description": "Các giải pháp về thị giác máy tính",
    "isDisable": false,
    "createdAt": "...",
    "updatedAt": "..."
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
| 404 | Resource Not Found | topicId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/topics/{topicId})
