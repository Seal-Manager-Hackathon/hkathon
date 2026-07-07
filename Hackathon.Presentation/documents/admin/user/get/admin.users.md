# GET /api/v1/admin/users

> Lấy danh sách users phân trang, có search keyword và filter.

## Nghiệp vụ
- Keyword search trên: email, firstName, lastName, fullName (firstName + " " + lastName)
- Các filter: Role, IsDisable, IsVerified, IsBanned, FromDate, ToDate
- Mặc định page 1, page size 10
- Không truyền filter gì → lấy tất cả
- Sắp xếp gần nhất trên cùng

## Phân quyền
- ✅ Admin

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| Keyword | string | ❌ | `john` | Search email, firstName, lastName, fullName |
| Role | string | ❌ | `Staff` | ⚠️ Enum: Admin, Staff, Student, Lecturer |
| IsDisable | bool | ❌ | `false` | Lọc disable |
| IsVerified | bool | ❌ | `true` | Lọc verified |
| IsBanned | bool | ❌ | `true` | Lọc banned (BanReason ≠ null) |
| FromDate | datetime | ❌ | `2026-07-01T00:00:00Z` | Lọc CreatedAt từ ngày |
| ToDate | datetime | ❌ | `2026-07-07T23:59:59Z` | Lọc CreatedAt đến ngày |
| PageIndex | int | ❌ | `1` | Mặc định 1 |
| PageSize | int | ❌ | `10` | Mặc định 10 |

### Ví dụ
```
GET /api/v1/admin/users                                         → Tất cả, page 1
GET /api/v1/admin/users?Keyword=john&Role=Staff                 → Search + filter role
GET /api/v1/admin/users?IsVerified=true&PageSize=20             → 20 items/page, verified only
GET /api/v1/admin/users?FromDate=...&ToDate=...                 → Lọc thời gian tạo
GET /api/v1/admin/users?IsDisable=true&Role=Student             → Disabled students
```

## Response (200)
```json
{
  "data": {
    "users": [
      {
        "id": "guid",
        "email": "staff@fpt.edu.vn",
        "firstName": "Staff",
        "lastName": "User",
        "role": "Staff",
        "status": "Active",
        "isVerified": true,
        "isDisable": false,
        "banReason": null,
        "bannedAt": null,
        "avatarUrl": "https://robohash.org/staff@fpt.edu.vn",
        "college": "FPT University",
        "createdAt": "2026-07-07T12:00:00Z"
      }
    ],
    "totalCount": 42,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Users Fetched Successfully",
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
| 400 | Invalid Role. Must be: Admin, Staff, Student, Lecturer | Role sai | Báo "Vai trò không hợp lệ" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
