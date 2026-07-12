# GET /api/v1/student/tracks/{trackId}

> Student xem chi tiết 1 track.

**Controller:** [StudentTrackController.cs](Controllers/Student/StudentTrackController.cs)

## Nghiệp vụ
- Xem chi tiết track: title, maxTeam, số đội đã đăng ký.
- Nếu track bị disable → 404.

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| trackId | guid | ✅ (route) | `...` |

## Response (200)
```json
{ "data": { "id": "guid", "eventId": "guid", "title": "AI", "maxTeam": 20, "isDisable": false, "registerTeamCount": 15 } }
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Student |
| 404 | Not Found | trackId ko tồn tại / bị disable |

> **Ref:** [Admin API tương ứng](/api/v1/admin/tracks/{trackId})
