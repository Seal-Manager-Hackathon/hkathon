# PUT /api/v1/admin/register-teams/{registerTeamId}

> Admin chỉnh sửa thông tin register team. Chỉ sửa được các field cơ bản.

## Nghiệp vụ
- Chỉ gửi field nào cần sửa, field null giữ nguyên
- Không thể đổi TeamId, EventId, CreatedAt

## Phân quyền
- ✅ Admin

## Request
```json
{
  "description": "Mô tả mới",
  "rejectionReason": "Lý do từ chối",
  "status": "Rejected",
  "isBanned": true,
  "isDisable": false
}
```
| Field | Bắt buộc | Ghi chú |
|-------|----------|---------|
| description | ❌ | |
| rejectionReason | ❌ | |
| status | ❌ | ⚠️ Enum: Pending, Approved, Rejected |
| isBanned | ❌ | |
| isDisable | ❌ | |

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
| 400 | Invalid Status. Must be: Pending, Approved, Rejected | Status sai | Báo "Trạng thái không hợp lệ" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Register Team Not Found | registerTeamId không tồn tại | Báo "Không tìm thấy đơn đăng ký" |
