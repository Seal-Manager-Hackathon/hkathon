# PATCH /api/v1/admin/reports/{reportId}/status

> Admin cập nhật trạng thái của report (Reject / Resolved).

## Nghiệp vụ
- Chỉ admin mới được đổi status report
- Status hợp lệ: `Pending`, `Reject`, `Resolved`
- Có thể ghi lý do (reason) khi reject

## Phân quyền
- ✅ Admin

## Request
```json
{
  "status": "Resolved",
  "reason": "Đã xác minh và xử lý"
}
```
| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| status | ✅ | `Pending`, `Reject`, `Resolved` |
| reason | ❌ | - |

## Response (200)
```json
{
  "data": null,
  "message": "Updated Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Invalid Status. Must be: Pending, Reject, Resolved | status không hợp lệ |
| 404 | Resource Not Found | reportId không tồn tại |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
