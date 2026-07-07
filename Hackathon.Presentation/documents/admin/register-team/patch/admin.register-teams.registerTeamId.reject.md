# POST /api/v1/admin/register-teams/{registerTeamId}/reject

> Admin từ chối register team tham gia event.

## Nghiệp vụ
- Chỉ reject được register team đang ở trạng thái `Pending`
- Có thể nhập lý do từ chối (rejectionReason)
- Nếu team còn register team khác đã approved → giữ nguyên CanEdit = false
- Nếu team không còn register team nào approved → mở khóa: CanEdit = true

## Phân quyền
- ✅ Admin

## Request
```json
{
  "rejectionReason": "Team không đủ điều kiện"
}
```
| Field | Bắt buộc | Ghi chú |
|-------|----------|---------|
| rejectionReason | ❌ | Lý do từ chối |

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
| 400 | Only Pending Register Team Can Be Rejected | Status không phải Pending | Báo "Chỉ từ chối được đơn đang chờ" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Register Team Not Found | registerTeamId không tồn tại | Báo "Không tìm thấy đơn đăng ký" |
