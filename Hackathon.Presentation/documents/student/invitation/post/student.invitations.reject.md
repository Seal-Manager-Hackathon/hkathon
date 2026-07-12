# POST /api/v1/student/invitations/{invitationId}/reject

> Student tu choi loi moi vao team.

## Nghiep vu

User tu choi loi moi vao team:
- Loi moi phai dang o trang thai Pending.
- Sau khi tu choi, Status = Rejected.

## Phan quyen
- Student (RoleEnum = Student)

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| invitationId | Guid | ID cua loi moi |

## Response (200)
```json
{
  "data": null,
  "message": "Updated Successfully",
  "status": 200,
  "traceId": "00-..."
}
```

## Loi
| Status | message | Khi nao |
|--------|---------|---------|
| 400 | Invitation Is Not in Pending Status | Loi moi da duoc xu ly |
| 403 | This Invitation Is Not for You | Loi moi khong danh cho user nay |
| 404 | Invitation Not Found | invitationId khong ton tai |
