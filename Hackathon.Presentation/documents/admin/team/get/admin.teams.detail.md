# GET /api/v1/admin/teams/{teamId}

> Lấy thông tin chi tiết của team, gồm danh sách thành viên.

## Nghiệp vụ
- Trả về thông tin team + danh sách thành viên kèm role (isLeader)
- 404 nếu teamId không tồn tại

## Phân quyền
- ✅ Admin

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
  "message": "Team Detail Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Team Not Found | teamId không tồn tại | Báo "Team không tồn tại" |
