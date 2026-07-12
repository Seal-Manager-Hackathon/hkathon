# GET /api/v1/admin/teams/{teamId}/members

> Admin lấy danh sách thành viên của team (gồm cả disable và active) kèm total, totalDisable, phân trang.

**Controller:** [AdminTeamController.cs](Controllers/Admin/AdminTeamController.cs)

## Nghiệp vụ
- Trả về danh sách thành viên của team (lấy hết, ko filter).
- `totalCount`: tổng số member.
- `totalDisable`: số member bị disable (tính trên tổng số, ko phải trên page).
- Phân trang: mặc định pageIndex=1, pageSize=10.
- 404 nếu teamId không tồn tại.

## Phân quyền
- ✅ Admin

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| teamId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |
| pageIndex | int | ❌ (query) | `1` |
| pageSize | int | ❌ (query) | `10` |

## Response (200)
```json
{
  "data": {
    "totalCount": 2,
    "totalDisable": 1,
    "pageIndex": 1,
    "pageSize": 10,
    "members": [
      {
        "userId": "guid",
        "email": "leader@fpt.edu.vn",
        "firstName": "Leader",
        "lastName": "User",
        "avatarUrl": "https://robohash.org/...",
        "isLeader": true,
        "isDisable": false,
        "status": "Active"
      },
      {
        "userId": "guid",
        "email": "member@fpt.edu.vn",
        "firstName": "Member",
        "lastName": "User",
        "avatarUrl": "https://robohash.org/...",
        "isLeader": false,
        "isDisable": true,
        "status": "Inactive"
      }
    ]
  },
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-12T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | Page Index Must Be Greater Than Zero | pageIndex < 1 | Báo "Trang không hợp lệ" |
| 400 | Page Size Must Be Between 1 And 100 | pageSize < 1 hoặc > 100 | Báo "Kích thước trang không hợp lệ" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Team Not Found | teamId không tồn tại | Báo "Team không tồn tại" |
