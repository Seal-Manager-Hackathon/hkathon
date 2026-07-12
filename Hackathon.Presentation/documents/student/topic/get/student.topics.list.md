# GET /api/v1/student/tracks/{trackId}/topics

> Student lấy danh sách topic của 1 track (chỉ topic ko disable).

**Controller:** [StudentTopicController.cs](Controllers/Student/StudentTopicController.cs)

## Nghiệp vụ
- Lấy danh sách topic của track.
- **Tự động filter** chỉ lấy topic có `IsDisable = false`.
- Hỗ trợ tìm kiếm theo tên.

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| trackId | guid | ✅ (route) | `...` |
| Keyword | string | ❌ | `Blockchain` |
| PageIndex | int | ❌ | `1` |
| PageSize | int | ❌ | `10` |

## Response (200)
```json
{ "data": { "topics": [{ "id": "guid", "trackId": "guid", "trackTitle": "AI", "title": "Blockchain", "isDisable": false }], "totalCount": 3, "pageIndex": 1, "pageSize": 10 } }
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Student |
| 404 | Not Found | trackId ko tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/tracks/{trackId}/topics)
