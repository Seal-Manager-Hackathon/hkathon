# PATCH /api/v1/staff/reports/{reportId}/status

> Staff cập nhật trạng thái báo cáo.

## Nghiệp vụ

Cập nhật trạng thái xử lý báo cáo: Pending, Reject, Resolved.

## Phân quyền
- ✅ Staff

## Request Body
```json
{
  "status": "Resolved",
  "reason": "Đã xử lý xong vấn đề"
}
```

### Field ý nghĩa
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| status | string | Yes | Pending / Reject / Resolved |
| reason | string | No | Lý do (nếu reject) |

## Response (200)
```json
{
  "data": null,
  "message": "Updated Successfully",
  "status": 200
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Invalid Status | Status không hợp lệ |
| 404 | Resource Not Found | ID không tồn tại |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | User không có role Staff |
