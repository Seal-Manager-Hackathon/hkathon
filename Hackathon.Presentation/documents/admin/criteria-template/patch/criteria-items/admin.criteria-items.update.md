# PATCH /api/v1/admin/criteria-items/{itemId}

> Admin chỉnh sửa thông tin criteria item.

## Phân quyền
- ✅ Admin

## Request
```json
{
  "name": "Tên tiêu chí mới",
  "description": "Mô tả mới",
  "score": 40,
  "isDisable": false
}
```
| Field | Bắt buộc |
|-------|----------|
| name | ❌ |
| description | ❌ |
| score | ❌ |
| isDisable | ❌ |

## Response (200)
```json
{
  "data": null,
  "message": "Updated Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-08T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Resource Not Found | itemId không tồn tại |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | Không phải Admin |
