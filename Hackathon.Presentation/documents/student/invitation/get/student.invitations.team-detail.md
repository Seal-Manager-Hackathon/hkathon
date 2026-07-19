# GET /api/v1/student/invitations/{invitationId}/team

> Chi tiết team từ lời mời — xem team có những thành viên active nào, không cần phải là member của team.

**Controller:** [StudentInvitationController.cs](Controllers/Student/StudentInvitationController.cs)

## Nghiệp vụ

Khi nhận được lời mời vào team, người dùng muốn xem team đó có những ai trước khi quyết định tham gia.

- API này **không kiểm tra phân quyền** — bất kỳ ai biết invitationId đều xem được team detail.
- Chỉ trả về thành viên **Active** (`IsDisable = false` và `Status = Active`).
- Member bị disable hoặc inactive sẽ không hiển thị trong danh sách.
- Dùng khi chưa vào team cũng coi được.

> **Ref:** API team detail có auth tương tự: [GET /api/v1/student/teams/{teamId}](/api/v1/student/teams/{teamId})

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| invitationId | Guid | ID của lời mời — lấy từ invitation detail |

## Response (200)

```json
{
  "data": {
    "teamId": "guid",
    "teamName": "Demo Team 01",
    "memberCount": 3,
    "canEdit": true,
    "members": [
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
        "email": "member1@email.com",
        "firstName": "Member",
        "lastName": "One",
        "avatarUrl": null,
        "isLeader": false
      }
    ]
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-..."
}
```

| Field | Ý nghĩa |
|-------|---------|
| teamId | ID của team |
| teamName | Tên team |
| memberCount | Số thành viên active trong team |
| canEdit | Team còn có thể chỉnh sửa (thêm/kick member) không |
| members[].userId | ID của member |
| members[].email | Email của member |
| members[].firstName | Tên member |
| members[].lastName | Họ member |
| members[].avatarUrl | Avatar URL (có thể null) |
| members[].isLeader | Có phải leader team không |

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Invitation Not Found | invitationId không tồn tại |
| 404 | Team Not Found | Team đã bị xóa/disable (hiếm gặp — invitation trỏ tới team không còn) |
