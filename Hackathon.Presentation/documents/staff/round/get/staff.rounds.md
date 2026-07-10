# GET /api/v1/staff/events/{eventId}/rounds

> Lấy danh sách rounds của event.

## Nghiệp vụ
- Chỉ trả về round có `isDisable = false`
- Staff phải được phân công vào event chứa round
- Mỗi round có thời gian bắt đầu/kết thúc và thời gian nộp bài riêng
- Hỗ trợ lọc theo keyword và số thứ tự round

## Phân quyền
- ✅ Staff (phải được assign vào event)

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| `eventId` | guid | ✅ | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | ID của event (route) |
| `Keyword` | string | ❌ | `Vòng loại` | Tìm theo tên round |
| `RoundNo` | int | ❌ | `1` | Lọc theo số thứ tự round |
| `PageIndex` | int | ❌ | `1` | Mặc định 1 |
| `PageSize` | int | ❌ | `10` | Mặc định 10 |

## Response (200)
```json
{
  "data": {
    "rounds": [
      {
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
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Rounds fetched successfully",
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

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId}/rounds) — [`admin/round/get/admin.rounds.list.md`](../../../admin/round/get/admin.rounds.list.md)
