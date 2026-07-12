# GET /api/v1/student/criteria-items/{itemId}

> Student xem chi tiết 1 criteria item.

**Controller:** [StudentCriteriaController.cs](Controllers/Student/StudentCriteriaController.cs)

## Nghiệp vụ
- Xem chi tiết item: tên, mô tả, điểm tối đa.
- Nếu item bị disable → 404.

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| itemId | guid | ✅ (route) | `...` |

## Response (200)
```json
{ "data": { "id": "guid", "criteriaTemplateId": "guid", "name": "Tính sáng tạo", "score": 10.0, "isDisable": false } }
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Student |
| 404 | Not Found | itemId ko tồn tại / bị disable |

> **Ref:** [Admin API tương ứng](/api/v1/admin/criteria-items/{itemId})
