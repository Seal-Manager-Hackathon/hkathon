# PATCH /api/v1/admin/tracks/{trackId}

> Admin cập nhật thông tin track.

## Phân quyền
- ✅ Admin

## Request
```json
{
  "title": "AI - Trí tuệ nhân tạo mới",
  "description": "Cập nhật mô tả mới",
  "maxTeam": 3,
  "isDisable": false
}
```
| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| title | ❌ | Tối đa 200 ký tự |
| description | ❌ | |
| maxTeam | ❌ | >= 0 |
| isDisable | ❌ | true/false |

> Chỉ truyền field nào cần sửa

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
| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Resource Not Found | TrackId không tồn tại |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | Không phải Admin |
