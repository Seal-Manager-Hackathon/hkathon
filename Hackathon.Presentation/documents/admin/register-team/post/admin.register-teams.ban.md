# POST /api/v1/admin/register-teams/{registerTeamId}/ban

> Admin cấm 1 team tham gia event. Team chỉ bị ban trong event đó — registerTeam đã xác định duy nhất event.

## Nghiệp vụ
- Set `Status = Banned`, `IsBanned = true`, ghi `RejectionReason`
- Ban team ra khỏi event — không ảnh hưởng tới các event khác
- Bị ban vẫn hiển thị trong danh sách (không xoá)
- 400 nếu đã ban rồi

## Phân quyền
- ✅ Admin

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| registerTeamId | Guid | ID của register team (đã xác định duy nhất event) |

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
  "status": 200,
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
