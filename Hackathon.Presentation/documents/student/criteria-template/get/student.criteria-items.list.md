# GET /api/v1/student/criteria-templates/{templateId}/criteria-items

> Student lấy danh sách criteria items của 1 template (chỉ item ko disable).

**Controller:** [StudentCriteriaController.cs](Controllers/Student/StudentCriteriaController.cs)

## Nghiệp vụ
- Lấy danh sách criteria items của template.
- **Chỉ lấy items ko bị disable.**
- Hỗ trợ tìm kiếm theo tên.

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| templateId | guid | ✅ (route) | `...` |
| Keyword | string | ❌ | `Sáng tạo` |
| PageIndex | int | ❌ | `1` |
| PageSize | int | ❌ | `10` |

## Response (200)
```json
{ "data": { "items": [{ "id": "guid", "criteriaTemplateId": "guid", "name": "Tính sáng tạo", "score": 10.0, "isDisable": false }], "totalCount": 5, "pageIndex": 1, "pageSize": 10 } }
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Student |
| 404 | Not Found | templateId ko tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/criteria-templates/{templateId}/criteria-items)
