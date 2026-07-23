# POST /api/v1/script/bulk-create-team

> Admin tạo team với leader và danh sách thành viên theo email.

**Controller:** [ScriptController.cs](Controllers/ScriptController.cs)

## Nghiệp vụ

- Admin muốn tạo nhanh 1 team hoàn chỉnh (leader + members) từ danh sách email.
- Hệ thống tự động:
  - Tìm leader user theo email (phải tồn tại, không bị disable).
  - Tạo team mới với tên đã nhập (kiểm tra không trùng).
  - Thêm leader làm thành viên đầu tiên (IsLeader = true).
  - Với mỗi email trong danh sách member: tìm user → tạo invitation (mặc định Accepted) → thêm thành viên vào team.
- Không cần user accept invitation — dữ liệu được tạo hoàn chỉnh ngay lập tức.
- Không validate profile user (StudentProfileHelper) vì đây là admin tạo.

## Phân quyền

- ✅ Admin

## Request

| Param | Kiểu | Bắt buộc | Mô tả |
|-------|------|----------|-------|
| LeaderEmail | string | ✅ | Email của leader (phải tồn tại trong hệ thống) |
| TeamName | string | ✅ | Tên team (không được trùng với team đang hoạt động) |
| MemberEmails | string[] | ❌ | Danh sách email các thành viên (có thể empty) |

### Ví dụ
```json
{
  "leaderEmail": "stu1@gmail.com",
  "teamName": "Team Alpha",
  "memberEmails": ["stu2@gmail.com", "stu3@gmail.com", "stu5@gmail.com"]
}
```

## Response (201)

### Thành công — tạo team với 3 member
```json
{
  "data": {
    "teamId": "guid-...",
    "teamName": "Team Alpha",
    "members": [
      {
        "userId": "guid-leader",
        "email": "stu1@gmail.com",
        "firstName": "Student 1",
        "lastName": "Nguyen 1",
        "isLeader": true
      },
      {
        "userId": "guid-member2",
        "email": "stu2@gmail.com",
        "firstName": "Student 2",
        "lastName": "Nguyen 2",
        "isLeader": false
      },
      {
        "userId": "guid-member3",
        "email": "stu3@gmail.com",
        "firstName": "Student 3",
        "lastName": "Nguyen 3",
        "isLeader": false
      }
    ]
  },
  "message": "Created Successfully",
  "error": null,
  "isSuccess": true,
  "status": 201,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-24T12:00:00Z"
}
```

### Thành công — tạo team chỉ có leader (không member)
```json
{
  "data": {
    "teamId": "guid-...",
    "teamName": "Team Solo",
    "members": [
      {
        "userId": "guid-leader",
        "email": "stu1@gmail.com",
        "firstName": "Student 1",
        "lastName": "Nguyen 1",
        "isLeader": true
      }
    ]
  },
  "message": "Created Successfully",
  "status": 201
}
```

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Leader With Email '...' Not Found | Email leader không tồn tại |
| 404 | Member With Email '...' Not Found | Email member không tồn tại |
| 400 | Leader User Is Disabled | Leader bị disable |
| 400 | Member '...' Is Disabled | Member bị disable |
| 400 | Team Name Already Exists | Tên team đã tồn tại |
| 400 | User '...' Is Already a Member of This Team | Member đã trong team |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |

> **Ref:** [Student CreateTeam](/api/v1/student/teams), [Student SendInvitation](/api/v1/student/teams/{teamId}/invitations)
