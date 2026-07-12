# GET /api/v1/student/rounds/{roundId}

> Student xem chi tiết 1 round.

**Controller:** [StudentRoundController.cs](Controllers/Student/StudentRoundController.cs)

## Nghiệp vụ
- Xem chi tiết round: tên, thời gian, roundNo, limitTeam.
- Nếu round bị disable → 404.
- Giống Admin response.

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| roundId | guid | ✅ (route) | `...` |

## Response (200)
```json
{ "data": { "id": "guid", "eventId": "guid", "eventName": "...", "name": "Vòng 1", "roundNo": 1, "startTime": "...", "endTime": "...", "isDisable": false } }
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Student |
| 404 | Round Not Found | roundId ko tồn tại / bị disable |

> **Ref:** [Admin API tương ứng](/api/v1/admin/rounds/{roundId})
