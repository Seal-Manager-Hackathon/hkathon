# GET /api/v1/staff/tracks/{trackId}/topics

> Lấy danh sách topics của track.

## Nghiệp vụ
- Chỉ trả về topic có `isDisable = false`, nhưng response vẫn trả field `isDisable`
- Sắp xếp theo `createdAt` giảm dần
- Staff phải được phân công vào event chứa track
- Topic là đề tài cụ thể trong một track (VD: track AI có topics: "Chatbot", "Computer Vision")

## Phân quyền
- ✅ Staff (phải được assign vào event)

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| `trackId` | guid | ✅ | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | ID của track (route) |
| `Keyword` | string | ❌ | `Chatbot` | Tìm theo tên topic |
| `PageIndex` | int | ❌ | `1` | Mặc định 1 |
| `PageSize` | int | ❌ | `10` | Mặc định 10 |

## Response (200)
```json
{
  "data": {
    "topics": [
      {
        "id": "guid",
        "trackId": "guid",
        "trackTitle": "Trí tuệ nhân tạo",
        "title": "Xây dựng Chatbot thông minh",
        "description": "Xây dựng chatbot sử dụng NLP",
        "isDisable": false,
        "createdAt": "2026-05-01T00:00:00Z",
        "updatedAt": "2026-06-01T00:00:00Z"
      }
    ],
    "totalCount": 5,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Topics fetched successfully",
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
| 404 | Not Found | Không tìm thấy track | Chuyển về danh sách |

> **Ref:** [Admin API tương ứng](/api/v1/admin/tracks/{trackId}/topics) — [`admin/topic/get/admin.topics.list.md`](../../../admin/topic/get/admin.topics.list.md)
