# PATCH /api/v1/admin/teams/{teamId}/delete

> Admin xoá mềm team (set IsDisable = true).

## Nghiệp vụ
- Xoá mềm: chỉ set `IsDisable = true`, không xoá khỏi DB
- Nếu team đã disable rồi → báo lỗi 400

## Phân quyền
- ✅ Admin

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| teamId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

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
| 400 | Team Is Already Disabled | Đã xoá mềm trước đó | Báo "Team đã bị vô hiệu hoá" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Team Not Found | teamId không tồn tại | Báo "Team không tồn tại" |
