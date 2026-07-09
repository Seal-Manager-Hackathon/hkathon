# GET /api/v1/staff/events/{eventId}/tracks

> Lấy danh sách tracks của event.

## Nghiệp vụ
- Chỉ trả về track có `isDisable = false`, nhưng response vẫn trả field `isDisable`
- Sắp xếp theo `createdAt` giảm dần
- Staff phải được phân công vào event
- Track là các nhánh chuyên môn trong event (VD: AI, Web, Mobile)

## Phân quyền
- ✅ Staff (phải được assign vào event)

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| `eventId` | guid | ✅ | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | ID của event (route) |
| `Keyword` | string | ❌ | `AI` | Tìm theo tên track |
| `PageIndex` | int | ❌ | `1` | Mặc định 1 |
| `PageSize` | int | ❌ | `10` | Mặc định 10 |

## Response (200)
```json
{
  "data": {
    "items": [
      {
        "id": "guid",
        "eventId": "guid",
        "title": "Trí tuệ nhân tạo",
        "description": "Các đề tài về AI",
        "maxTeam": 20,
        "isDisable": false,
        "createdAt": "2026-05-01T00:00:00Z",
        "updatedAt": "2026-06-01T00:00:00Z"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Tracks fetched successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-...",
  "timestampUtc": "2026-07-09T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | User không có role Staff hoặc không được assign vào event | Ẩn chức năng |
