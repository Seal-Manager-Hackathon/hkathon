# GET /api/v1/student/teams/{teamId}/invitations

> Student (leader) xem danh sach loi moi da gui cua team.

**Controller:** [StudentInvitationController.cs](Controllers/Student/StudentInvitationController.cs)

## Nghiep vu

Leader xem tat ca loi moi da gui tu team (Pending, Accepted, Rejected, Expired):
- Chi leader moi xem duoc.
- Co phan trang.
- Sap xep theo CreatedAt giam dan (moi nhat truoc).
- Moi item co thong tin user duoc moi, trang thai, thoi han.

## Phan quyen
- Student (RoleEnum = Student), phai la leader cua team.

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| teamId | Guid | ID cua team |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| pageIndex | int | No | 1 | Trang hien tai |
| pageSize | int | No | 10 | So item moi trang |

## Response (200)
```json
{
  "data": {
    "items": [
      {
        "id": "guid",
        "teamId": "guid",
        "teamName": "FTeam",
        "teamCanEdit": true,
        "invitedUserId": "guid",
        "invitedUserEmail": "invited@fpt.edu.vn",
        "invitedUserFirstName": "Invited",
        "invitedUserLastName": "User",
        "invitedUserAvatarUrl": "https://example.com/avatar.png",
        "status": "Pending",
        "description": null,
        "limitTime": "2026-07-27T12:00:00Z",
        "createdAt": "2026-07-12T12:00:00Z",
        "updatedAt": "2026-07-12T12:00:00Z"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-..."
}
```

## Loi
| Status | message | Khi nao |
|--------|---------|---------|
| 400 | Only Team Leader Can View Sent Invitations | User khong phai leader |
| 404 | Team Not Found | teamId khong ton tai hoac disable |
