# GET /api/v1/student/teams/{teamId}

> Student lấy thông tin chi tiết của team, gồm danh sách thành viên (chỉ member active).

**Controller:** [StudentTeamController.cs](Controllers/Student/StudentTeamController.cs)

> **Ref:** [Admin API tương ứng](/api/v1/admin/teams/{teamId})

## Nghiệp vụ
- Giống Admin API nhưng **chỉ lấy member có IsDisable = false**.
- Trả về thông tin team + danh sách thành viên kèm role (isLeader).
- 404 nếu teamId không tồn tại.

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| teamId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

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
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Ko phải Student | Ẩn chức năng |
| 404 | Team Not Found | teamId ko tồn tại | Báo "Team không tồn tại" |
