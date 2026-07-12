# GET /api/v1/student/criteria-templates/{templateId}/criteria-items

> Student lấy danh sách criteria items của 1 template (chỉ item ko disable).

**Controller:** [StudentCriteriaController.cs](Controllers/Student/StudentCriteriaController.cs)

## Nghiệp vụ
- Lấy danh sách criteria items của template.
- **Chỉ lấy items ko bị disable** (`IsDisable = false`).
- Hỗ trợ tìm kiếm theo name (contains, ko phân biệt hoa thường).
- Sắp xếp theo CreatedAt giảm dần.
- 404 nếu templateId ko tồn tại.

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| templateId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |
| Keyword | string | ❌ (query) | `Sáng tạo` |
| PageIndex | int | ❌ (query) | `1` |
| PageSize | int | ❌ (query) | `10` |

## Response (200)
```json
{
  "data": {
    "items": [
      {
        "id": "guid",
        "criteriaTemplateId": "guid",
        "name": "Tính sáng tạo",
        "description": "...",
        "score": 10.0,
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
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Student | Ẩn chức năng |
| 404 | Resource Not Found | templateId ko tồn tại | Báo "Không tìm thấy template" |

> **Ref:** [Admin API tương ứng](/api/v1/admin/criteria-templates/{templateId}/criteria-items)
