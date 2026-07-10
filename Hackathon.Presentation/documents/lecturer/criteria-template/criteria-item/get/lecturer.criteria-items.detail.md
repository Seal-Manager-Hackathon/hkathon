# GET /api/v1/lecturer/criteria-items/{itemId}

> Lecturer xem chi tiết 1 criteria item.
> **Controller:** `LecturerCriteriaTemplateController` — `GET /api/v1/lecturer/criteria-items/{itemId}`

## Nghiệp vụ

- Lecturer xem chi tiết criteria item.
- Response giống hệt Admin `GET /api/v1/admin/criteria-items/{itemId}`.

## Phân quyền
- ✅ Lecturer

## Request
| Parameter | Type | Bắt buộc | Ghi chú |
|-----------|------|----------|---------|
| itemId | Guid | ✅ | ID của criteria item |

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
  "status": 200,
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | Forbidden | Không phải Lecturer |
| 404 | Resource Not Found | itemId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/criteria-items/{itemId})
