# GET /api/v1/tracks/{trackId}

> Xem chi tiết một track — chỉ cần đăng nhập. Chỉ truyền trackId, không cần eventId.

## Nghiệp vụ

- Bất kỳ user nào đã đăng nhập đều có thể xem chi tiết track.
- Response giống hệt Admin `GET /api/v1/admin/tracks/{trackId}`.

## Phân quyền
- ✅ Authenticated (chỉ cần đăng nhập)

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
| 404 | Resource Not Found | trackId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/tracks/{trackId})
