# GET /api/v1/lecturer/topics/{topicId}

> Lecturer xem chi tiết một topic. Response giống hệt Admin.

## Nghiệp vụ

- Response giống hệt Admin `GET /api/v1/admin/topics/{topicId}`.
- Chỉ truyền topicId, không cần thông tin khác.

## Phân quyền
- ✅ Lecturer (RoleEnum = Lecturer)

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
| 403 | You do not have permission | Không phải Lecturer |
| 404 | Resource Not Found | topicId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/topics/{topicId})
