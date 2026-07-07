# POST /api/v1/admin/tracks/{trackId}/topics

> Admin tạo topic mới trong track.

## Phân quyền
- ✅ Admin

## Request
```json
{
  "title": "Chatbot hỗ trợ học tập",
  "description": "Xây dựng chatbot AI hỗ trợ sinh viên học tập"
}
```
| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| title | ✅ | Tối đa 200 ký tự |
| description | ❌ | |

## Response (201)
```json
{
  "data": {
    "id": "guid",
    "trackId": "guid",
    "title": "Chatbot hỗ trợ học tập",
    "description": "Xây dựng chatbot AI hỗ trợ sinh viên học tập",
    "isDisable": false
  },
  "message": "Created Successfully",
  "error": null,
  "isSuccess": true,
  "status": 201,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Invalid Request Data | Validation lỗi |
| 404 | Resource Not Found | TrackId không tồn tại |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | Không phải Admin |
