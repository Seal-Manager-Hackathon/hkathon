# GET /api/v1/student/criteria-templates/{templateId}

> Student xem chi tiết criteria template + danh sách criteria items.

**Controller:** [StudentCriteriaController.cs](Controllers/Student/StudentCriteriaController.cs)

## Nghiệp vụ
- Xem chi tiết template: thông tin template + các criteria items bên trong.
- Nếu template bị disable (`IsDisable = true`) → 404.
- **Chỉ lấy criteria items ko bị disable** (`IsDisable = false`).

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| templateId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "roundId": "guid",
    "title": "Tiêu chí đánh giá vòng 1",
    "description": "...",
    "isDisable": false,
    "isActive": true,
    "items": [
      {
        "id": "guid",
        "name": "Tính sáng tạo",
        "description": "...",
        "score": 10.0,
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
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Student | Ẩn chức năng |
| 404 | Resource Not Found | templateId ko tồn tại / bị disable | Báo "Không tìm thấy tiêu chí" |

> **Ref:** [Admin API tương ứng](/api/v1/admin/criteria-templates/{templateId})
