# POST /api/v1/admin/criteria-templates/{templateId}/restore

> Admin khôi phục criteria template đã xóa mềm (set IsDisable = false).

## Phân quyền
- ✅ Admin

## Response (200)
```json
{
  "data": null,
  "message": "Operation Successful",
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
