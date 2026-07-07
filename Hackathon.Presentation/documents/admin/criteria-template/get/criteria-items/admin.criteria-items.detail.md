# GET /api/v1/admin/criteria-items/{itemId}

> Admin xem chi tiết 1 criteria item.

## Phân quyền
- ✅ Admin

## Request

| Param  | Kiểu | Bắt buộc | Ví dụ |
|--------|------|----------|-------|
| itemId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

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
  "traceId": "00-abc123..."
}
```

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Resource Not Found | itemId ko tồn tại |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
