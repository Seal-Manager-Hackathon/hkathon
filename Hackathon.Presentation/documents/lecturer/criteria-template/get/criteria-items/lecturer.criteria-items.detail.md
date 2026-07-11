# GET /api/v1/lecturer/criteria-items/{itemId}

> Lecturer xem chi tiết 1 criteria item.

**Controller:** [LecturerCriteriaTemplateController.cs](Controllers/Lecturer/LecturerCriteriaTemplateController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/criteria-items/{itemId}`

- Giống hệt Admin `GET /api/v1/admin/criteria-items/{itemId}`, khác auth là Lecturer.
- Trả về chi tiết criteria item bao gồm cả item đã bị disable.
- 404 nếu itemId không tồn tại.

## Phân quyền
- ✅ Lecturer

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| itemId | Guid | ID của criteria item |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "criteriaTemplateId": "guid",
    "name": "Tính sáng tạo",
    "description": "Mức độ sáng tạo của ý tưởng",
    "score": 30,
    "isDisable": false,
    "createdAt": "2026-07-07T12:00:00Z",
    "updatedAt": "2026-07-07T12:00:00Z"
  },
  "message": "Fetched Successfully",
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |
| 404 | Resource Not Found | itemId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/criteria-items/{itemId})
