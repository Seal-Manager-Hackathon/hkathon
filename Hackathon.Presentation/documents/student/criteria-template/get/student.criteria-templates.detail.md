# GET /api/v1/student/criteria-templates/{templateId}

> Student xem chi tiết criteria template + criteria items.

**Controller:** [StudentCriteriaController.cs](Controllers/Student/StudentCriteriaController.cs)

## Nghiệp vụ
- Xem chi tiết template + các criteria items.
- Nếu template bị disable → 404.
- **Chỉ lấy criteria items ko bị disable.**

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| templateId | guid | ✅ (route) | `...` |

## Response (200)
```json
{ "data": { "id": "guid", "roundId": "guid", "title": "...", "isActive": true, "isDisable": false, "items": [{ "id": "guid", "name": "...", "score": 10.0, "isDisable": false }] } }
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Student |
| 404 | Not Found | templateId ko tồn tại / bị disable |

> **Ref:** [Admin API tương ứng](/api/v1/admin/criteria-templates/{templateId})
