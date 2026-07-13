# POST /api/v1/admin/teams/{teamId}/change-leader

> Admin chuyển quyền đội trưởng từ người đang làm leader sang một member khác trong cùng team.

**Controller:** [AdminTeamController.cs](Controllers/Admin/AdminTeamController.cs)

## Nghiệp vụ

- **Admin** có thể chuyển leader cho bất kỳ team nào, không cần là thành viên của team đó.
- Người nhận quyền leader phải **đang là member active trong team** (`IsDisable = false`, `Status = Active`).
- **Không thể** chuyển leader cho member đã bị **disable** hoặc **inactive**.
- **Không thể** chuyển leader cho người đã là leader (đã là đội trưởng).
- Leader cũ tự động bị hạ xuống member thường.
- API này **không cần check `CanEdit`** của team — admin có thể đổi leader ngay cả khi team đã bị khóa.

## Phân quyền
- ✅ Admin — toàn quyền

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| teamId | Guid | ID của team |

### Body
```json
{
  "userId": "guid"
}
```

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| userId | Guid | Có | ID của user sẽ làm leader mới |

## Response (200)
```json
{
  "data": null,
  "message": "Team Leader Changed Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Cannot Transfer Leadership to an Inactive or Disabled Member | Member bị disable/inactive |
| 400 | This Member Is Already the Team Leader | Member đã là leader rồi |
| 400 | No Active Leader Found in This Team | Team ko có leader active |
| 401 | Invalid Or Expired Token | Token hết hạn |
| 404 | Team Not Found | teamId ko tồn tại hoặc bị disable |
| 404 | User Not Found in This Team | userId ko phải member của team này |
