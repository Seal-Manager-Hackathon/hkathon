# GET /api/v1/lecturer/criteria-templates/{templateId}/criteria-items

> Lecturer lấy danh sách criteria items của 1 template — chỉ lấy items không bị disable.

## Nghiệp vụ

- Lecturer xem danh sách criteria items của 1 criteria template.
- Tự động lọc chỉ lấy item có `IsDisable = false`.
- Không cho phép lọc theo isDisable.
- Response giống hệt Admin `GET /api/v1/admin/criteria-templates/{templateId}/criteria-items`.

## Phân quyền
- ✅ Lecturer

## Request
| Parameter | Type | Bắt buộc | Ghi chú |
|-----------|------|----------|---------|
| templateId | Guid | ✅ | ID của criteria template |

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
    "pageSize": 1
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | Forbidden | Không phải Lecturer |
| 404 | Resource Not Found | templateId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/criteria-templates/{templateId}/criteria-items)
