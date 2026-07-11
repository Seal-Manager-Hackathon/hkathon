# GET /api/v1/lecturer/tracks/{trackId}

> Lecturer xem chi tiết 1 track — gồm thông tin track, số lượng team đã đăng ký, và event role của lecturer trong event.

**Controller:** [LecturerTrackController.cs](Controllers/Lecturer/LecturerTrackController.cs)

## Nghiệp vụ

- Trả về thông tin track + số lượng team đã đăng ký (`registerTeamCount`).
- **Thêm** `eventRoleId` + `eventRoleName` của lecturer đang request trong event chứa track (VD: "Mentor", "Judge", "Staff").
- Nếu lecturer không được assign vào event → `eventRoleId` = null value.
- 404 nếu trackId không tồn tại.

## Phân quyền
- ✅ Lecturer

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| trackId | Guid | ID của track |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "eventId": "guid",
    "title": "Web3",
    "description": "Phát triển ứng dụng Blockchain",
    "maxTeam": 20,
    "isDisable": false,
    "registerTeamCount": 5,
    "eventRoleId": "guid",
    "eventRoleName": "Mentor",
    "createdAt": "2026-07-07T12:00:00Z",
    "updatedAt": "2026-07-07T12:00:00Z"
  },
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-11T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |
| 404 | Resource Not Found | trackId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/tracks/{trackId})
