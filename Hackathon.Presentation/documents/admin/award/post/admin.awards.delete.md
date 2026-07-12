# POST /api/v1/admin/awards/{awardId}/delete

> Admin xóa mềm phần thưởng.

## Nghiệp vụ
- Xóa mềm (IsDisable = true)
- LevelAward chuyển về 0
- Các phần thưởng có level cao hơn tự động giảm 1 để đúng thứ tự
- 404 nếu awardId không tồn tại

## Phân quyền
- ✅ Admin

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| awardId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)
```json
{
  "data": null,
  "message": "Deleted Successfully",
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
| 404 | Resource Not Found | awardId không tồn tại | Báo "Không tìm thấy" |
