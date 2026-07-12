# GET /api/v1/student/events/{eventId}/tracks

> Student lấy danh sách track của 1 event (chỉ track ko disable).

**Controller:** [StudentTrackController.cs](Controllers/Student/StudentTrackController.cs)

## Nghiệp vụ
- Lấy danh sách track của event.
- **Tự động filter** chỉ lấy track có `IsDisable = false`.
- Hỗ trợ tìm kiếm theo tên.

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| eventId | guid | ✅ (route) | `...` |
| Keyword | string | ❌ | `AI` |
| PageIndex | int | ❌ | `1` |
| PageSize | int | ❌ | `10` |

## Response (200)
```json
{ "data": { "tracks": [{ "id": "guid", "eventId": "guid", "title": "AI", "maxTeam": 20, "isDisable": false }], "totalCount": 5, "pageIndex": 1, "pageSize": 10 } }
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Student |
| 404 | Not Found | eventId ko tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId}/tracks)
