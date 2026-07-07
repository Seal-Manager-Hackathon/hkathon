# PATCH /api/v1/admin/notifications/{notificationId}

> Admin chỉnh sửa thông báo. Chỉ được sửa title và description.

## Nghiệp vụ
- Chỉ cập nhật Title và Description — các field khác không đổi
- Chỉ gửi field nào cần sửa, field null giữ nguyên

## Phân quyền
- ✅ Admin

## Request
```json
{
  "title": "Tiêu đề mới",
  "description": "Nội dung mới"
}
```
| Field | Bắt buộc | Ghi chú |
|-------|----------|---------|
| title | ❌ | null = giữ nguyên |
| description | ❌ | null = giữ nguyên |

## Response (200)
```json
{
  "data": null,
  "message": "Notification Updated Successfully",
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
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Notification Not Found | notificationId không tồn tại | Báo "Không tìm thấy thông báo" |
