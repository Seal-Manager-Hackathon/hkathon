# POST /api/v1/admin/events/{eventId}/leaderboard/publish

> Admin publish leader board của event.

## Nghiệp vụ

Publish leader board của một event cụ thể: set IsPublished=true, IsDisable=false.

## Phân quyền
- ✅ Admin

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
| 404 | Resource Not Found | Event chưa có LeaderBoard (chưa khởi tạo) |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | User không có role Admin |
