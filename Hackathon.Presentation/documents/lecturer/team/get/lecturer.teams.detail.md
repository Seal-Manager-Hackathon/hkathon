# GET /api/v1/lecturer/teams/{teamId}

> Lecturer xem chi tiết team — thông tin team + danh sách thành viên.

**Controller:** [LecturerTeamController.cs](Controllers/Lecturer/LecturerTeamController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/teams/{teamId}`

- Giống hệt Admin `GET /api/v1/admin/teams/{teamId}`, khác auth là Lecturer.
- Trả về thông tin team + danh sách thành viên kèm role (isLeader).
- 404 nếu teamId không tồn tại.

## Phân quyền
- ✅ Lecturer

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| teamId | Guid | ID của team |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "name": "FTeam",
    "canEdit": true,
    "isDisable": false,
    "createdAt": "2026-07-07T12:00:00Z",
    "updatedAt": "2026-07-07T12:00:00Z",
    "members": [
      {
        "userId": "guid",
        "email": "leader@fpt.edu.vn",
        "firstName": "Leader",
        "lastName": "User",
        "avatarUrl": "https://robohash.org/...",
        "isLeader": true,
        "status": "Active"
      }
    ]
  },
  "message": "Team Detail Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |
| 404 | Team Not Found | teamId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/teams/{teamId})
