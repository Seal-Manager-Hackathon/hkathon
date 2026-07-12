# GET /api/v1/student/invitations

> Student xem danh sach loi moi minh da nhan duoc tu cac team.

**Controller:** [StudentInvitationController.cs](Controllers/Student/StudentInvitationController.cs)

## Nghiep vu

User xem tat ca loi moi minh nhan duoc:
- Co filter keyword (tim theo ten team).
- Co filter theo trang thai: Pending, Accepted, Rejected, Expired.
- Co phan trang.
- Moi item co thong tin team, nguoi moi (leader team), trang thai, thoi han.
- Sap xep: Pending (chua phan hoi) len tren cung, xep theo thoi gian tao moi nhat. Cac trang thai khac (Accepted, Rejected, Expired) xep o duoi, cung theo thoi gian.

## Phan quyen
- Student (RoleEnum = Student)

## Request

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| keyword | string | No | - | Tim kiem theo ten team |
| status | string | No | - | Lọc theo trạng thái: Pending, Accepted, Rejected, Expired |
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
        "invitedUserEmail": "me@fpt.edu.vn",
        "invitedUserFirstName": "Me",
        "invitedUserLastName": "User",
        "invitedUserAvatarUrl": "https://example.com/avatar.png",
        "sentByUserId": "guid",
        "sentByEmail": "leader@fpt.edu.vn",
        "sentByFirstName": "Leader",
        "sentByLastName": "User",
        "sentByAvatarUrl": "https://example.com/avatar.png",
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
| 401 | Unauthorized | Token het han/thieu |
