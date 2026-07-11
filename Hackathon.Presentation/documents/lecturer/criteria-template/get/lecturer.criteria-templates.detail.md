# GET /api/v1/lecturer/criteria-templates/{templateId}

> Lecturer xem chi tiết criteria template (kèm danh sách criteria items đang active).

**Controller:** [LecturerCriteriaTemplateController.cs](Controllers/Lecturer/LecturerCriteriaTemplateController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/criteria-templates/{templateId}`

- Giống hệt Admin `GET /api/v1/admin/criteria-templates/{templateId}`, khác auth là Lecturer.
- Trả về thông tin template kèm danh sách criteria items.
- **Khác Admin:** Chỉ lấy items có `IsDisable = false`.
- 404 nếu templateId không tồn tại.

## Phân quyền
- ✅ Lecturer

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| templateId | Guid | ID của criteria template |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "roundId": "guid",
    "title": "Tiêu chí chấm điểm vòng 1",
    "description": "Đánh giá ý tưởng",
    "isDisable": false,
    "isActive": true,
    "items": [
      {
        "id": "guid",
        "name": "Tính sáng tạo",
        "description": "Đánh giá mức độ mới mẻ của ý tưởng",
        "score": 3.0,
        "isDisable": false,
        "createdAt": "2026-07-07T12:00:00Z"
      }
    ],
    "createdAt": "2026-07-07T12:00:00Z",
    "updatedAt": "2026-07-07T12:00:00Z"
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

> **Ref:** [Admin API tương ứng](/api/v1/admin/criteria-templates/{templateId})
