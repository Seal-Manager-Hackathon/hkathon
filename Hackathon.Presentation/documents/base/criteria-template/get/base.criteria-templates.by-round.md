# GET /api/v1/rounds/{roundId}/criteria-templates

> Xem danh sách criteria templates của một round — chỉ cần đăng nhập. Chỉ lấy các template đang active (IsDisable = false).
> **Controller:** `CriteriaTemplateController` — `GET /api/v1/rounds/{roundId}/criteria-templates`

## Nghiệp vụ

- Bất kỳ user nào đã đăng nhập đều có thể xem criteria templates của 1 round.
- Tự động lọc chỉ lấy template có `IsDisable = false`.
- Response giống hệt Admin `GET /api/v1/admin/criteria-templates/{templateId}`.

## Phân quyền
- ✅ Authenticated (chỉ cần đăng nhập)

## Request
| Parameter | Type | Bắt buộc | Ghi chú |
|-----------|------|----------|---------|
| roundId | Guid | ✅ | ID của round |

## Response (200)
```json
{
  "data": {
    "templates": [
      {
        "id": "guid",
        "roundId": "guid",
        "title": "Template đánh giá vòng 1",
        "description": "Các tiêu chí chấm điểm",
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
| 404 | Resource Not Found | roundId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/criteria-templates/{templateId})
