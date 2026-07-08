# PATCH /api/v1/admin/events/{eventId}/awards/{awardId}/restore

> Admin khôi phục award đã xóa mềm. Khi restore, award được gán level = max level hiện tại trong event + 1.

## Nghiệp vụ
- Set IsDisable = false
- Gán LevelAward = (level lớn nhất của các award đang active trong cùng event) + 1
- Không ảnh hưởng đến các award khác

## Phân quyền
- ✅ Admin

## Response (200)
```json
{
  "data": null,
  "message": "Operation Successful",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-08T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Resource Not Found | awardId không tồn tại |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | Không phải Admin |
