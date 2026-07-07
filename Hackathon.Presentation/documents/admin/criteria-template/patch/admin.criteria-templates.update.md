# PATCH /api/v1/admin/criteria-templates/{templateId}

> Admin chỉnh sửa thông tin criteria template.

## Phân quyền
- ✅ Admin

## Request
```json
{
  "title": "Tiêu đề mới",
  "description": "Mô tả mới",
  "isDisable": false
}
```
| Field | Bắt buộc |
|-------|----------|
| title | ❌ |
| description | ❌ |
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
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Resource Not Found | TemplateId không tồn tại |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | Không phải Admin |
