# GET /api/v1/staff/topics/{topicId}

> Xem chi tiết topic.

## Nghiệp vụ
- Topic bị disable (`isDisable = true`) sẽ trả về 404
- Staff phải được phân công vào event chứa track của topic
- Response trả thêm `trackTitle` để FE hiển thị context

## Phân quyền
- ✅ Staff (phải được assign vào event)

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| `topicId` | guid | ✅ | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | ID của topic (route) |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "trackId": "guid",
    "trackTitle": "Trí tuệ nhân tạo",
    "title": "Xây dựng Chatbot thông minh",
    "description": "Xây dựng chatbot sử dụng NLP",
    "isDisable": false,
    "createdAt": "2026-05-01T00:00:00Z",
    "updatedAt": "2026-06-01T00:00:00Z"
  },
  "message": "Topic detail fetched successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-...",
  "timestampUtc": "2026-07-09T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | User không có role Staff hoặc không được assign vào event | Ẩn chức năng |
| 404 | Not Found | Không tìm thấy topic hoặc topic đã bị disable | Chuyển về danh sách |
