# POST /api/v1/student/invitations/{invitationId}/accept

> Student chap nhan loi moi vao team.

## Nghiep vu

User chap nhan loi moi vao team:
- Loi moi phai dang o trang thai Pending.
- Loi moi phai con han (LimitTime > now).
- User duoc them vao team voi vai tro member (IsLeader = false, Status = Active).
- Team phai con ton tai (khong bi disable).
- Neu het han, tu dong set Expired va bao loi.
- **Phải điền đủ profile trước khi chấp nhận:** Email, FirstName, LastName, College, StudentId, PhoneNumber.

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
| 400 | Please Complete Your Profile Before Proceeding. Missing Fields: ... | Thieu thong tin profile | Vao trang edit profile |
| 400 | Invitation Is Not in Pending Status | Loi moi da duoc xu ly |
| 400 | Invitation Has Expired | Loi moi het han |
| 400 | You Are Already a Member of This Team | User da la member |
| 403 | This Invitation Is Not for You | Loi moi khong danh cho user nay |
| 404 | Invitation Not Found | invitationId khong ton tai |
| 404 | Team Not Found | Team da bi disable hoac khong ton tai |
