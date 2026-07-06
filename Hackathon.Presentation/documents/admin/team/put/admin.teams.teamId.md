# PUT /api/v1/admin/teams/{teamId}

> Admin chỉnh sửa thông tin team.

## Nghiệp vụ
- Chỉ gửi field nào cần sửa, field null giữ nguyên

## Phân quyền
- ✅ Admin

## Request
```json
{
  "name": "Tên mới",
  "canEdit": true,
  "isDisable": false
}
```
| Field | Bắt buộc | Ghi chú |
|-------|----------|---------|
| name | ❌ | null = giữ nguyên |
| canEdit | ❌ | null = giữ nguyên |
| isDisable | ❌ | null = giữ nguyên |

## Response (200)
```json
{
  "data": null,
  "message": "Team Updated Successfully",
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
| 404 | Team Not Found | teamId không tồn tại | Báo "Team không tồn tại" |
