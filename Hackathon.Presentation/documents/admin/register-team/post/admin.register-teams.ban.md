# POST /api/v1/admin/register-teams/{registerTeamId}/ban

> Admin cấm team tham gia event (chỉ ban trong event đó).

## Nghiệp vụ
- Set `Status = Banned`, `IsBanned = true`, ghi `RejectionReason`
- Bị ban vẫn hiển thị trong danh sách (không xoá)
- 400 nếu đã ban rồi

## Phân quyền
- ✅ Admin

## Request

### Body (JSON)
```json
{
  "rejectionReason": "Lý do cấm"
}
```

| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| rejectionReason | ✅ | Lý do cấm |

## Response (200)
```json
{
  "data": null,
  "message": "Updated Successfully",
  "traceId": "00-abc123..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Register Team Is Already Banned | đã ban rồi |
| 404 | Register Team Not Found | registerTeamId ko tồn tại |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
