# POST /api/v1/admin/criteria-templates/{templateId}/activate

> Admin active 1 criteria template của 1 round. Mỗi round chỉ có 1 template được active (IsDisable = false) tại 1 thời điểm.

## Nghiệp vụ
- Khi active 1 template, tất cả template khác trong cùng round sẽ bị disable (IsDisable = true)
- Template được active sẽ set IsDisable = false
- Chỉ disable template, không ảnh hưởng đến các criteria items bên trong

## Phân quyền
- ✅ Admin

## Request

| Param      | Kiểu | Bắt buộc | Ví dụ |
|------------|------|----------|-------|
| templateId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)
```json
{
  "data": null,
  "message": "Operation Successful",
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 400 | This Template Is Already Active | template đã active rồi |
| 404 | Resource Not Found | templateId ko tồn tại |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
