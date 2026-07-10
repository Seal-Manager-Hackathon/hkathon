# POST /api/v1/staff/events/{eventId}/leaderboard/hide

> Staff ẩn leader board của event.

## Nghiệp vụ

Hide (soft-delete) leader board của một event cụ thể: set IsDisable=true.

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

> **Ref:** [Admin API tương ứng](/api/v1/admin/leaderboard/post/admin.leaderboard.events.hide.md)
