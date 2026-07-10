# GET /api/v1/staff/events/{eventId}

> Xem chi tiết event được phân công cho staff.

## Nghiệp vụ
- Chỉ trả về thông tin nếu staff được phân công vào event đó
- Nếu staff không có quyền truy cập, trả về 404 — không tiết lộ thông tin event
- Trả về thêm các field mở rộng so với danh sách: `registerLimitTime`, `limitTeam`, `minMember`, `maxMember`

## Phân quyền
- ✅ Staff (phải được assign vào event)

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| `eventId` | guid | ✅ | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | ID của event |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "name": "Hackathon AI 2026",
    "description": "Cuộc thi về trí tuệ nhân tạo",
    "status": "Ongoing",
    "numberRound": 3,
    "season": "Summer",
    "startTime": "2026-06-01T00:00:00Z",
    "endTime": "2026-07-01T00:00:00Z",
    "registerLimitTime": "2026-05-15T00:00:00Z",
    "limitTeam": 50,
    "minMember": 2,
    "maxMember": 5,
    "eventRoleId": "guid",
    "eventRoleName": "Mentor",
    "isDisable": false,
    "createdAt": "2026-05-01T00:00:00Z",
    "updatedAt": "2026-06-01T00:00:00Z"
  },
  "message": "Event detail fetched successfully",
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
| 403 | You do not have permission to perform this action | User không có role Staff | Ẩn chức năng |
| 404 | Not Found | Không tìm thấy event hoặc staff không được phân công | Chuyển về danh sách |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId}) — [`admin/event/get/admin.events.detail.md`](../../../admin/event/get/admin.events.detail.md)
