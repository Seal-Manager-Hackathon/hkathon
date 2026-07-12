# GET /api/v1/lecturer/users

> Lecturer lấy danh sách users — copy từ Admin `GET /api/v1/admin/users`, chỉ khác filter **IsDisable = false**.

**Controller:** [LecturerUserController.cs](Controllers/Lecturer/LecturerUserController.cs)

## Nghiệp vụ

- Giống Admin `GET /api/v1/admin/users` nhưng auth là Judge.
- **Tự động lọc** chỉ lấy user có `IsDisable = false` (ko thấy user bị disable).
- User bị ban (`BanReason != null`) vẫn hiển thị.
- Hỗ trợ tìm kiếm, lọc role, lọc isVerified, isBanned, khoảng thời gian.
- Hỗ trợ phân trang.

## Phân quyền
- ✅ Judge

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| Keyword | string | ❌ | `Fteam` | Search tên/email |
| Role | string | ❌ | `Student` | ⚠️ Enum: Admin, Staff, Student, Lecturer |
| IsVerified | bool | ❌ | `true` | |
| IsBanned | bool | ❌ | `false` | |
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
        "email": "user@email.com",
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
        "createdAt": "2026-07-01T00:00:00Z"
      }
    ],
    "totalCount": 50,
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
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Invalid Role | Role sai |
| 400 | Page Index/Size | Pagination sai |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Judge |

> **Ref:** [Admin API tương ứng](/api/v1/admin/users)
