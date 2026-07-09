# GET /api/v1/staff/events/{eventId}/rounds/{roundId}

> Xem chi tiết round.

## Nghiệp vụ
- Staff phải được phân công vào event chứa round
- Nếu round bị disable (`isDisable = true`), API trả về 404
- Dùng trước khi xem submissions hoặc chấm điểm

## Phân quyền
- ✅ Staff (phải được assign vào event)

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| `eventId` | guid | ✅ | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | ID của event (route) |
| `roundId` | guid | ✅ | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | ID của round (route) |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "eventId": "guid",
    "name": "Vòng loại",
    "description": "Vòng loại chọn top 20 đội",
    "roundNo": 1,
    "startTime": "2026-06-10T00:00:00Z",
    "endTime": "2026-06-15T00:00:00Z",
    "startSubmission": "2026-06-11T00:00:00Z",
    "endSubmission": "2026-06-14T00:00:00Z",
    "limitTeam": 50,
    "isDisable": false,
    "createdAt": "2026-05-01T00:00:00Z",
    "updatedAt": "2026-06-01T00:00:00Z"
  },
  "message": "Round detail fetched successfully",
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
| 404 | Not Found | Không tìm thấy round hoặc round đã bị disable | Chuyển về danh sách |
