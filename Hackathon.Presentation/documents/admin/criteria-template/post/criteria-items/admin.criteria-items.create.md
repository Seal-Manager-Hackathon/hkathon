# POST /api/v1/admin/criteria-templates/{templateId}/criteria-items

> Admin tạo mới criteria item trong 1 template.

## Phân quyền
- ✅ Admin

## Request
Body (JSON):
```json
{
  "name": "Tính sáng tạo",
  "description": "Mức độ sáng tạo của ý tưởng",
  "score": 30
}
```
| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| name | ✅ | Tối đa 200 ký tự |
| description | ❌ | - |
| score | ❌ | Mặc định 0, >= 0 |

## Response (201)
```json
{
  "data": null,
  "message": "Created Successfully",
  "error": null,
  "isSuccess": true,
  "status": 201,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-08T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Resource Not Found | templateId không tồn tại |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | Không phải Admin |
