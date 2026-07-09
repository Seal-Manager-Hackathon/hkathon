# GET /api/v1/staff/users

> Lấy danh sách users phân trang, hỗ trợ tìm kiếm và lọc theo nhiều tiêu chí.

## Nghiệp vụ
- Staff xem danh sách tất cả users (giống Admin)
- Keyword search theo email, firstName, lastName
- Lọc Role, IsDisable, IsVerified, IsBanned
- Lọc FromDate / ToDate theo CreatedAt
- Sắp xếp gần nhất trên cùng

## Phân quyền
- ✅ Staff

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| Keyword | string | ❌ | `user@example.com` | Search email, tên |
| Role | string | ❌ | `Student` | ⚠️ Enum: Admin, Staff, Student, Lecturer |
| IsDisable | bool | ❌ | `false` | |
| IsVerified | bool | ❌ | `true` | |
| IsBanned | bool | ❌ | `true` | BanReason != null |
| FromDate | datetime | ❌ | `2026-07-01T00:00:00Z` | |
| ToDate | datetime | ❌ | `2026-07-07T23:59:59Z` | |
| PageIndex | int | ❌ | `1` | Mặc định 1 |
| PageSize | int | ❌ | `10` | Mặc định 10 |

## Response (200)
```json
{
  "data": {
    "users": [
      {
        "id": "guid",
        "email": "user@example.com",
        "firstName": "Nguyễn",
        "lastName": "Văn A",
        "role": "Student",
        "status": "Active",
        "isVerified": true,
        "isDisable": false,
        "banReason": null,
        "bannedAt": null,
        "avatarUrl": "https://robohash.org/...",
        "college": "FPT University",
        "createdAt": "2026-07-07T12:00:00Z"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Users Fetched Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | Invalid Role | Role sai | Báo lỗi |
| 400 | Page Index Must Be Greater Than Zero | PageIndex < 1 | Fix pagination |
| 400 | Page Size Must Be Between 1 And 100 | PageSize ngoài khoảng | Fix pagination |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Staff | Ẩn chức năng |