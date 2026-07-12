# GET /api/v1/student/rounds/{roundId}/criteria-templates

> Student lấy danh sách criteria template đang active của 1 round.

**Controller:** [StudentCriteriaController.cs](Controllers/Student/StudentCriteriaController.cs)

## Nghiệp vụ
- Lấy danh sách criteria template của 1 round.
- **Chỉ lấy template đang active** và **ko bị disable**.

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| roundId | guid | ✅ (route) | `...` |
| Keyword | string | ❌ | `Đánh giá` |
| PageIndex | int | ❌ | `1` |
| PageSize | int | ❌ | `10` |

## Response (200)
```json
{ "data": { "templates": [{ "id": "guid", "roundId": "guid", "title": "...", "isActive": true, "isDisable": false }], "totalCount": 1, "pageIndex": 1, "pageSize": 10 } }
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Student |
| 404 | Not Found | roundId ko tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/rounds/{roundId}/criteria-templates)
