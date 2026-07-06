# PATCH /api/v1/admin/teams/{teamId}/restore

> Admin khôi phục team đã xoá mềm (set IsDisable = false).

## Nghiệp vụ
- Ngược lại với DELETE, set `IsDisable = false`
- Báo lỗi nếu team chưa bị disable

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
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | Team Is Not Disabled | Chưa bị xoá mềm | Báo "Team chưa bị vô hiệu hoá" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Team Not Found | teamId không tồn tại | Báo "Team không tồn tại" |
