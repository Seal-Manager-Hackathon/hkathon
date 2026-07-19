# GET /api/v1/student/invitations/{invitationId}

> Chi tiết 1 lời mời vào team — cho người được mời xem hoặc leader team gửi xem.

**Controller:** [StudentInvitationController.cs](Controllers/Student/StudentInvitationController.cs)

## Nghiệp vụ

Student muốn xem chi tiết 1 lời mời: team nào mời, ai gửi, trạng thái ra sao, còn hạn không.

- API này **không kiểm tra phân quyền** — bất kỳ ai biết invitationId đều xem được.
- Mục đích: FE có thể gọi mà ko cần token phức tạp, dùng trong flow hiển thị thông báo lời mời.

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| invitationId | Guid | ID của lời mời |

## Response (200)

```json
{
  "data": {
    "id": "guid",
    "teamId": "guid",
    "teamName": "Demo Team 01",
    "teamMemberCount": 3,
    "teamCanEdit": true,
    "teamMembers": [
      {
        "userId": "guid",
        "email": "leader@email.com",
        "firstName": "Leader",
        "lastName": "User",
        "avatarUrl": "https://robohash.org/leader@email.com",
        "isLeader": true
      },
      {
        "userId": "guid",
        "email": "member@email.com",
        "firstName": "Member",
        "lastName": "User",
        "avatarUrl": null,
        "isLeader": false
      }
    ],

    "sentByUserId": "guid",
    "sentByEmail": "leader@email.com",
    "sentByFirstName": "Leader",
    "sentByLastName": "User",
    "sentByAvatarUrl": "https://robohash.org/leader@email.com",

    "status": "Pending",
    "description": "leader@email.com invited user@email.com to join Demo Team 01",
    "limitTime": "2026-08-03T14:30:00Z",
    "createdAt": "2026-07-19T14:30:00Z",
    "updatedAt": "2026-07-19T14:30:00Z"
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-..."
}
```

| Field | Ý nghĩa |
|-------|---------|
| teamId, teamName | Team đã gửi lời mời |
| teamMemberCount | Số member đang active của team |
| teamCanEdit | Team còn có thể chỉnh sửa member không |
| teamMembers[] | Danh sách thành viên active của team (mỗi item có userId, email, firstName, lastName, avatarUrl, isLeader) |
| sentByUserId... | Thông tin leader đã gửi lời mời (null nếu leader ko còn active) |
| status | Trạng thái: `Pending` / `Accepted` / `Rejected` / `Expired` |
| description | Nội dung lời mời (auto-generated: "{email leader} invited {email invitee} to join {team name}") |
| limitTime | Hạn chấp nhận lời mời (mặc định now + 15 ngày) |

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 404 | Invitation Not Found | invitationId không tồn tại |
