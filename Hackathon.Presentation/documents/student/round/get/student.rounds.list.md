# GET /api/v1/student/events/{eventId}/rounds

> Student lấy danh sách round của 1 event (chỉ round ko bị disable).

**Controller:** [StudentRoundController.cs](Controllers/Student/StudentRoundController.cs)

## Nghiệp vụ
- Lấy danh sách round của 1 event.
- **Tự động filter** chỉ lấy round có `IsDisable = false`.
- Hỗ trợ tìm kiếm theo tên, lọc theo roundNo.

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| eventId | guid | ✅ (route) | `...` |
| Keyword | string | ❌ | `Vòng 1` |
| RoundNo | int | ❌ | `1` |
| PageIndex | int | ❌ | `1` |
| PageSize | int | ❌ | `10` |

## Response (200)
```json
{ "data": { "rounds": [{ "id": "guid", "eventId": "guid", "name": "Vòng 1", "roundNo": 1, "startTime": "...", "endTime": "...", "isDisable": false }], "totalCount": 3, "pageIndex": 1, "pageSize": 10 } }
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Student |
| 404 | Event Not Found | eventId ko tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId}/rounds)
