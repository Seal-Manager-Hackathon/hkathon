# POST /api/v1/admin/events/chapter/{year}/leaderboard/hide

> Admin ẩn (soft-delete) leader board chapter cho một năm — set IsDisable=true cho tất cả leader board trong năm đó.

## Nghiệp vụ

Admin muốn ẩn kết quả leader board của một năm. Hệ thống sẽ:

1. Lấy tất cả leader board có `Year == year`, event liên quan có status Published/Closed và không bị disable
2. Set `IsDisable = true` cho các leader board đó
3. Sau khi ẩn, leader board sẽ không xuất hiện ở các API GET leader board

## Phân quyền
- ✅ Admin

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| year | int | Năm cần ẩn leader board |

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
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
