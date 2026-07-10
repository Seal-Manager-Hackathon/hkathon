# GET /api/v1/staff/criteria-templates/{criteriaTemplateId}/items

> Staff xem danh sách criteria items của một criteria template.

## Nghiệp vụ
- Staff phải được phân công vào event tương ứng.
- Chỉ trả về item có `IsDisable = false`.
- Entity `CriteriaItem` dùng field `Score` ánh xạ sang `score` trong response.

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

## Request

### Route Parameters
| Parameter | Type | Bắt buộc | Ví dụ | Ghi chú |
|-----------|------|----------|-------|---------|
| eventId | Guid | Có | 3fa85f64-5717-4562-b3fc-2c963f66afa6 | ID của event |
| criteriaTemplateId | Guid | Có | 3fa85f64-5717-4562-b3fc-2c963f66afa6 | ID của criteria template |

## Response (200)
```json
{
  "data": {
    "items": [
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "criteriaTemplateId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "name": "Tính sáng tạo",
        "description": "Ý tưởng có tính mới và sáng tạo",
        "score": 30,
        "isDisable": false,
        "createdAt": "2026-05-01T00:00:00Z",
        "updatedAt": "2026-06-01T00:00:00Z"
      }
    ],
    "totalCount": 5,
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
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Chuyển về trang login |
| 403 | You do not have permission to perform this action | Không phải Staff hoặc không được phân công vào event | Hiển thị thông báo Không có quyền |
| 404 | Resource Not Found | CriteriaTemplateId không tồn tại | Hiển thị thông báo Không tìm thấy |

> **Ref:** [Admin API tương ứng](/api/v1/admin/criteria-templates/{criteriaTemplateId}/items) — [`admin/criteria-template/get/criteria-items/admin.criteria-items.list.md`](../../../admin/criteria-template/get/criteria-items/admin.criteria-items.list.md)
