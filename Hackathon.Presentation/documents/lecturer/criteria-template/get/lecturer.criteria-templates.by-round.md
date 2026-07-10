# GET /api/v1/lecturer/rounds/{roundId}/criteria-templates

> Lecturer lấy danh sách criteria templates đang active của 1 round.
> **Controller:** `LecturerCriteriaTemplateController` — `GET /api/v1/lecturer/rounds/{roundId}/criteria-templates?keyword=`

## Nghiệp vụ

- Lecturer xem danh sách criteria templates của round.
- Chỉ trả về template có `IsActive = true` và `IsDisable = false`.
- Không phân trang — trả về tất cả template active.
- Response giống hệt Admin `GET /api/v1/admin/rounds/{roundId}/criteria-templates`.

## Phân quyền
- ✅ Lecturer

## Request
| Parameter | Type | Bắt buộc | Ghi chú |
|-----------|------|----------|---------|
| roundId | Guid | ✅ | ID của round |
| keyword | string | ❌ | Tìm theo tên template |

## Response (200)
```json
{
  "data": {
    "templates": [
      {
        "id": "guid",
        "roundId": "guid",
        "title": "Đánh giá ý tưởng",
        "description": "Tiêu chí đánh giá ý tưởng vòng 1",
        "isDisable": false,
        "isActive": true,
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
| 404 | Resource Not Found | roundId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/rounds/{roundId}/criteria-templates)
