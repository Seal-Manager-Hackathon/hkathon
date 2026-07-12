# POST /api/v1/student/teams/{teamId}/invitations

> Student (leader) gui loi moi vao team cho 1 user qua email.

**Controller:** [StudentInvitationController.cs](Controllers/Student/StudentInvitationController.cs)

## Nghiep vu

Leader muon moi 1 user vao team:
- Chi leader moi gui duoc loi moi.
- Team phai co CanEdit = true.
- Email phai chinh xac va ton tai trong he thong.
- User duoc moi khong duoc bi disable.
- User duoc moi chua la member active cua team.
- Chi duoc gui 1 loi moi Pending cho 1 user (khong spam).
- Loi moi co han 15 ngay (LimitTime = now + 15 days).

## Phan quyen
- Student (RoleEnum = Student), phai la leader cua team.

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| teamId | Guid | ID cua team |

### Body
```json
{
  "email": "user@email.com"
}
```

| Field | Bat buoc | Rang buoc |
|-------|----------|-----------|
| email | ✅ | Dinh dang email hop le |

## Response (201)
```json
{
  "data": null,
  "message": "Invitation Sent Successfully",
  "status": 201,
  "traceId": "00-..."
}
```

## Loi
| Status | message | Khi nao |
|--------|---------|---------|
| 400 | Only Team Leader Can Send Invitations | User khong phai leader |
| 400 | Team Cannot Be Edited | Team bi khoa CanEdit = false |
| 400 | Cannot Invite a Disabled User | User duoc moi bi disable |
| 400 | User Is Already a Member of This Team | User da la member |
| 400 | An Invitation Has Already Been Sent to This User | Da co loi moi Pending |
| 404 | Team Not Found | teamId khong ton tai hoac disable |
| 404 | User Not Found | Email khong ton tai |
