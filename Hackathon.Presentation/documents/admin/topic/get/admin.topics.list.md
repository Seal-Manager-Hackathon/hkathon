# GET /api/v1/admin/tracks/{trackId}/topics

> Admin xem danh sách topic trong track, phân trang, search, lọc.

## Phân quyền
- ✅ Admin

## Query Parameters
| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| keyword | ❌ | Tìm theo tên |
| isDisable | ❌ | true/false |
| pageIndex | ❌ | Mặc định 1 |
| pageSize | ❌ | 1-100, mặc định 10 |

## Response (200)
```json
{
  "data": {
    "topics": [
      {
        "id": "guid",
        "trackId": "guid",
        "trackTitle": "AI - Trí tuệ nhân tạo",
        "title": "Chatbot hỗ trợ học tập",
        "description": "Xây dựng chatbot AI hỗ trợ sinh viên học tập",
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
| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Resource Not Found | TrackId không tồn tại |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | Không phải Admin |
