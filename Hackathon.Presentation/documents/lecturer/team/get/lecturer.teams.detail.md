# GET /api/v1/lecturer/teams/{teamId}

> Lấy thông tin chi tiết của team.

## Nghiệp vụ

- Bất kỳ user nào đã đăng nhập đều có thể xem thông tin team.
- Trả về thông tin team + danh sách thành viên kèm role (isLeader).
- Response giống hệt Admin `GET /api/v1/admin/teams/{teamId}`.

## Phân quyền
- ✅ Lecturer (RoleEnum = Lecturer)

## Request
| Parameter | Type | Bắt buộc | Ghi chú |
|-----------|------|----------|---------|
| teamId | Guid | ✅ | ID của team |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "name": "FTeam",
    "canEdit": true,
    "isDisable": false,
    "createdAt": "...",
    "updatedAt": "...",
    "members": [
      {
        "userId": "guid",
        "email": "leader@fpt.edu.vn",
        "firstName": "Leader",
        "lastName": "User",
        "avatarUrl": "...",
        "isLeader": true,
        "status": "Active"
      }
    ]
  },
  "message": "Team Detail Fetched Successfully",
  "status": 200,
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission | Không phải Lecturer |
| 404 | Team Not Found | teamId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/teams/{teamId})
