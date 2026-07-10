# GET /api/v1/admin/tracks/{trackId}

> Admin xem chi tiết track + số lượng register team đã chọn track này.

## Phân quyền
- ✅ Admin

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "eventId": "guid",
    "title": "AI - Trí tuệ nhân tạo",
    "description": "Các giải pháp ứng dụng AI vào đời sống",
    "maxTeam": 2,
    "isDisable": false,
    "registerTeamCount": 5,
    "createdAt": "2026-07-07T12:00:00Z",
    "updatedAt": "2026-07-07T12:00:00Z"
  },
  "message": "Fetched Successfully",
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
