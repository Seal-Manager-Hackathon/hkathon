# POST /api/v1/admin/register-teams/{registerTeamId}/unban

> Admin bỏ cấm cho team tham gia lại event.

## Nghiệp vụ
- Set `IsBanned = false`, `Status = Approved`, xóa `RejectionReason`
- 400 nếu chưa bị ban

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
| Status | message | Khi nào | Token hết hạn | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Register Team Not Found | registerTeamId ko tồn tại | Báo "Không tìm thấy" |
