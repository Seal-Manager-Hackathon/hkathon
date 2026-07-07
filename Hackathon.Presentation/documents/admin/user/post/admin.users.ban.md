# POST /api/v1/admin/users/{userId}/ban

> Admin ban user, ghi lý do. User bị ban vẫn visible với các user khác.

## Nghiệp vụ
- Ban user: ghi BanReason, BannedAt
- IsDisable vẫn **false** — user bị ban vẫn hiển thị với các user khác
- 404 nếu userId không tồn tại

## Phân quyền
- ✅ Admin

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| userId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |
| banReason | string | ✅ (body) | `Vi phạm điều khoản` |

### Ví dụ request body
```json
{
  "banReason": "Vi phạm điều khoản sử dụng"
}
```

## Response (200)
```json
{
  "data": null,
  "message": "User Banned Successfully",
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
| 400 | BanReason Is Required | Thiếu banReason | Báo "Vui lòng nhập lý do ban" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | User Not Found | userId không tồn tại | Báo "Không tìm thấy người dùng" |
