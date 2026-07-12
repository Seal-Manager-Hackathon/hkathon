# GET /api/v1/student/topics/{topicId}

> Student xem chi tiết 1 topic.

**Controller:** [StudentTopicController.cs](Controllers/Student/StudentTopicController.cs)

## Nghiệp vụ
- Xem chi tiết topic: title, track, description.
- Nếu topic bị disable → 404.

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| topicId | guid | ✅ (route) | `...` |

## Response (200)
```json
{ "data": { "id": "guid", "trackId": "guid", "trackTitle": "AI", "title": "Blockchain", "isDisable": false } }
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Student |
| 404 | Not Found | topicId ko tồn tại / bị disable |

> **Ref:** [Admin API tương ứng](/api/v1/admin/topics/{topicId})
