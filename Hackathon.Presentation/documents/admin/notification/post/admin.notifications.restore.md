# POST /api/v1/admin/notifications/{notificationId}/restore

> Admin khôi phục thông báo đã xoá mềm (set IsDisable = false).

## Nghiệp vụ
- Ngược lại với DELETE, set `IsDisable = false`
- Không check trạng thái trước đó — restore luôn được

## Phân quyền
- ✅ Admin

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
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Notification Not Found | notificationId không tồn tại | Báo "Không tìm thấy thông báo" |
