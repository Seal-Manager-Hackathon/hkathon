# GET /api/v1/lecturer/criteria-templates/{templateId}

> Lecturer xem chi tiết 1 criteria template (kèm danh sách criteria items không bị disable).
> **Controller:** `LecturerCriteriaTemplateController` — `GET /api/v1/lecturer/criteria-templates/{templateId}`

## Nghiệp vụ

- Lecturer xem chi tiết criteria template.
- Chỉ lấy items có `IsDisable = false`.
- Response giống hệt Admin `GET /api/v1/admin/criteria-templates/{templateId}`.

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
    "id": "guid",
    "roundId": "guid",
    "title": "Template đánh giá vòng 1",
    "description": "Các tiêu chí chấm điểm",
    "isDisable": false,
    "isActive": true,
    "items": [
      {
        "id": "guid",
        "name": "Tính sáng tạo",
        "description": "Mức độ sáng tạo của ý tưởng",
        "score": 30,
        "isDisable": false,
        "createdAt": "2026-07-07T12:00:00Z"
      }
    ],
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
| 404 | Resource Not Found | templateId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/criteria-templates/{templateId})
