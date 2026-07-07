# POST /api/v1/admin/events/{eventId}/tracks

> Admin tạo track mới cho event.

## Phân quyền
- ✅ Admin

## Request
```json
{
  "title": "AI - Trí tuệ nhân tạo",
  "description": "Các giải pháp ứng dụng AI vào đời sống",
  "maxTeam": 2
}
```
| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| title | ✅ | Tối đa 200 ký tự |
| description | ❌ | |
| maxTeam | ❌ | >= 0 |

## Response (201)
```json
{
  "data": {
    "id": "guid",
    "eventId": "guid",
    "title": "AI - Trí tuệ nhân tạo",
    "description": "Các giải pháp ứng dụng AI vào đời sống",
    "maxTeam": 2,
    "isDisable": false
  },
  "message": "Created Successfully",
  "error": null,
  "isSuccess": true,
  "status": 201,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Invalid Request Data | Validation lỗi |
| 404 | Resource Not Found | EventId không tồn tại |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | Không phải Admin |
