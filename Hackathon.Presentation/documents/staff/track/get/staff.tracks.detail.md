# GET /api/v1/staff/tracks/{trackId}

> Xem chi tiết track.

## Nghiệp vụ
- Trả về thêm `registerTeamCount` — số đội đã đăng ký vào track
- Track bị disable (`isDisable = true`) trả về 404
- Staff phải được phân công vào event chứa track

## Phân quyền
- ✅ Staff (phải được assign vào event)

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| `trackId` | guid | ✅ | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | ID của track (route) |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "eventId": "guid",
    "title": "Trí tuệ nhân tạo",
    "description": "Các đề tài về AI",
    "maxTeam": 20,
    "registerTeamCount": 15,
    "isDisable": false,
    "createdAt": "2026-05-01T00:00:00Z",
    "updatedAt": "2026-06-01T00:00:00Z"
  },
  "message": "Track detail fetched successfully",
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
| 404 | Not Found | Không tìm thấy track hoặc track đã bị disable | Chuyển về danh sách |

> **Ref:** [Admin API tương ứng](/api/v1/admin/tracks/{trackId}) — [`admin/track/get/admin.tracks.detail.md`](../../../admin/track/get/admin.tracks.detail.md)
