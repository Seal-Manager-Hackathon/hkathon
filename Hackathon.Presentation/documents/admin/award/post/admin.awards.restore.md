# POST /api/v1/admin/events/{eventId}/awards/{awardId}/restore

> Admin khôi phục phần thưởng đã xóa mềm.

## Nghiệp vụ
- Khôi phục (IsDisable = false)
- LevelAward trả lại (set bằng max hiện tại + 1)
- 404 nếu awardId không tồn tại
- 400 nếu award chưa bị xóa

## Phân quyền
- ✅ Admin

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| eventId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |
| awardId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)
```json
{
  "data": null,
  "message": "Operation Successful",
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
| 400 | Award Is Not Deleted | Chưa bị xóa | Báo "Phần thưởng chưa bị xóa" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Resource Not Found | awardId không tồn tại | Báo "Không tìm thấy" |
