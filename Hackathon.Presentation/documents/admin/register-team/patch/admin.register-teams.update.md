# PATCH /api/v1/admin/register-teams/{registerTeamId}

> Admin sửa thông tin register team.

## Nghiệp vụ
- Chỉ sửa các field được gửi lên (partial update)
- Không gửi field = không thay đổi

## Phân quyền
- ✅ Admin

## Request
```json
{
  "description": "Mô tả mới",
  "rejectionReason": "Lý do từ chối",
  "status": "Approved",
  "isBanned": false,
  "isDisable": false
}
```

## Response (200)
```json
{
  "data": null,
  "message": "Register Team Updated Successfully",
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
| 404 | Register Team Not Found | registerTeamId không tồn tại | Báo "Không tìm thấy" |
