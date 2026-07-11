# GET /api/v1/lecturer/criteria-templates/{templateId}/criteria-items

> Lecturer xem danh sách criteria items của template (chỉ item không bị disable).

**Controller:** [LecturerCriteriaTemplateController.cs](Controllers/Lecturer/LecturerCriteriaTemplateController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/criteria-templates/{templateId}/criteria-items`

- Giống hệt Admin `GET /api/v1/admin/criteria-templates/{templateId}/criteria-items`, khác auth là Lecturer.
- **Khác Admin:** Luôn filter `IsDisable = false` — chỉ lấy item còn hoạt động.
- Hỗ trợ tìm kiếm theo tên (contains, không phân biệt hoa thường).
- Sắp xếp theo CreatedAt giảm dần.
- Phân trang: mặc định pageIndex=1, pageSize=10.
- 404 nếu templateId không tồn tại.

## Phân quyền
- ✅ Lecturer

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| templateId | Guid | ID của criteria template |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| Keyword | string | No | - | Tìm kiếm theo tên item |
| PageIndex | int | No | 1 | Trang hiện tại |
| PageSize | int | No | 10 | Số lượng item mỗi trang |

## Response (200)
```json
{
  "data": {
    "items": [
      {
        "id": "guid",
        "criteriaTemplateId": "guid",
        "name": "Tính sáng tạo",
        "description": "Mức độ mới lạ và sáng tạo",
        "score": 25,
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
  "traceId": "...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |
| 404 | Resource Not Found | templateId không tồn tại |
| 400 | PageIndex/PageSize invalid | PageIndex hoặc PageSize không hợp lệ |

> **Ref:** [Admin API tương ứng](/api/v1/admin/criteria-templates/{templateId}/criteria-items)
