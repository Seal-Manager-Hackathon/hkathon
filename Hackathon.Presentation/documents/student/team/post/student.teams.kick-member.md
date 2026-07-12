# POST /api/v1/student/teams/{teamId}/members/{memberId}/kick

> Student la leader cua team kick 1 thanh vien ra khoi team.

**Controller:** [StudentTeamController.cs](Controllers/Student/StudentTeamController.cs)

## Nghiep vu

Leader co the kick thanh vien ra khoi team:
- Chi leader moi co quyen kick (IsLeader = true, IsDisable = false).
- Team phai co CanEdit = true (team chua bi khoa chinh sua).
- Khong the kick chinh minh (leader khong the tu kick).
- Khong the kick leader khac.
- Khong the kick member da bi disable hoac inactive.
- Khi kick: set IsDisable = true, Status = Inactive, UpdatedAt = now.
- **Ref:** [Admin API tuong ung] — Khong co, day la API rieng cua Student.

## Phan quyen
- Student (RoleEnum = Student), phai la leader cua team.

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| teamId | Guid | ID cua team |
| memberId | Guid | ID cua member can kick |

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
| 401 | Unauthorized | Token het han/thieu |
| 400 | Only Team Leader Can Kick Members | User khong phai leader |
| 400 | Team Cannot Be Edited | Team bi khoa CanEdit = false |
| 400 | Cannot Kick Yourself from the Team | Leader tu kick chinh minh |
| 400 | Cannot Kick the Team Leader | Kick 1 leader khac |
| 400 | Member Is Already Inactive or Disabled | Member da inactive/disable roi |
| 404 | Team Not Found | teamId khong ton tai hoac da disable |
| 404 | Member Not Found in This Team | memberId khong thuoc team |
