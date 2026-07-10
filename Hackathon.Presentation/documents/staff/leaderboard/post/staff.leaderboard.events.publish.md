# POST /api/v1/staff/events/{eventId}/leaderboard/publish

> Staff publish leader board của event.

## Nghiệp vụ

Publish leader board của một event cụ thể: set IsPublished=true, IsDisable=false.

## Phân quyền
- ✅ Staff

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
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | User không có role Staff |
