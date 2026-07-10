# GET /api/v1/lecturer/tracks/{trackId}

> Lecturer xem chi tiết một track. Response giống hệt Admin.

## Nghiệp vụ

- Chỉ truyền `trackId`, không cần eventId.
- Response giống hệt Admin `GET /api/v1/admin/tracks/{trackId}`.

## Phân quyền
- ✅ Lecturer

## Request
| Parameter | Type | Bắt buộc | Ghi chú |
|-----------|------|----------|---------|
| trackId | Guid | ✅ | ID của track |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "eventId": "guid",
    "title": "AI - Trí tuệ nhân tạo",
    "description": "Các giải pháp ứng dụng AI",
    "maxTeam": 2,
    "isDisable": false,
    "registerTeamCount": 0,
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
| 404 | Resource Not Found | trackId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/tracks/{trackId})
