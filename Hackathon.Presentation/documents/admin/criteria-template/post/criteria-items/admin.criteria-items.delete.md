# POST /api/v1/admin/criteria-items/{itemId}/delete

> Admin xóa mềm criteria item (set IsDisable = true).

## Phân quyền
- ✅ Admin

## Response (200)
```json
{
  "data": null,
  "message": "Deleted Successfully",
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
