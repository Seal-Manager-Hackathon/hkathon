# POST /api/v1/student/teams/{teamId}/change-leader

> Student là leader chuyển quyền leader cho 1 thành viên khác trong team.

## Nghiệp vụ

Leader có thể chuyển quyền leader cho thành viên khác:
- Chỉ leader mới có quyền chuyển (IsLeader = true, IsDisable = false).
- Team phải có CanEdit = true.
- Người nhận quyền leader phải là thành viên active trong team (IsDisable = false, Status = Active).
- Không thể chuyển cho chính mình.
- Sau khi chuyển: người cũ set IsLeader = false, người mới set IsLeader = true.

## Phân quyền
- ✅ Student (RoleEnum = Student), phải là leader của team.

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| teamId | Guid | ID của team |

### Body
```json
{
  "newLeaderUserId": "guid"
}
```

## Response (200)
```json
{
  "data": null,
  "message": "Updated Successfully",
  "status": 200,
  "traceId": "00-..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 400 | Only Team Leader Can Change Leader | User không phải leader |
| 400 | Team Cannot Be Edited | Team bị khóa CanEdit = false |
| 400 | Cannot Transfer Leadership to Yourself | Tự chuyển cho chính mình |
| 400 | Cannot Transfer Leadership to an Inactive or Disabled Member | Người nhận đã inactive/disable |
| 404 | Team Not Found | teamId không tồn tại hoặc đã disable |
| 404 | User Not Found in This Team | newLeaderUserId không thuộc team |
