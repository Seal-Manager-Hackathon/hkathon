# GET /api/v1/admin/teams/{teamId}/invitations

> Admin lấy danh sách lời mời thành viên vào team, phân trang, có thể lọc theo status và tìm kiếm theo email/họ tên.

**Controller:** [AdminInvitationController.cs](Controllers/Admin/AdminInvitationController.cs)

## Nghiệp vụ
- Trả về danh sách lời mời của team (gồm thông tin user được mời).
- Hỗ trợ lọc theo `status`: `Pending`, `Accepted`, `Rejected`, `Expired`. Ko truyền = lấy hết.
- Hỗ trợ tìm kiếm theo `keyword` (tìm trong email hoặc firstName + lastName, không phân biệt hoa thường).
- Sắp xếp theo CreatedAt giảm dần (mới nhất lên trên).
- Phân trang: mặc định pageIndex=1, pageSize=10.
- 404 nếu teamId không tồn tại.

## Phân quyền
- ✅ Admin

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| teamId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |
| status | string | ❌ (query) | `Pending` |
| keyword | string | ❌ (query) | `nguyen` |
| pageIndex | int | ❌ (query) | `1` |
| pageSize | int | ❌ (query) | `10` |

## Response (200)
```json
{
  "data": {
    "items": [
      {
        "id": "guid",
        "teamId": "guid",
        "teamName": "FTeam",
        "userId": "guid",
        "userEmail": "invitee@fpt.edu.vn",
        "userFirstName": "Nguyễn",
        "userLastName": "Văn B",
        "userAvatarUrl": "https://robohash.org/...",
        "status": "Pending",
        "description": "Mời bạn vào team",
        "limitTime": "2026-07-20T12:00:00Z",
        "isDisable": false,
        "createdAt": "2026-07-12T12:00:00Z",
        "updatedAt": "2026-07-12T12:00:00Z"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
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
| 400 | Invalid Status. Must Be: Pending, Accepted, Rejected, Expired | Status sai enum | Validation form |
| 400 | Page Index Must Be Greater Than Zero | pageIndex < 1 | Báo "Trang không hợp lệ" |
| 400 | Page Size Must Be Between 1 And 100 | pageSize < 1 hoặc > 100 | Báo "Kích thước trang không hợp lệ" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Team Not Found | teamId không tồn tại | Báo "Team không tồn tại" |
