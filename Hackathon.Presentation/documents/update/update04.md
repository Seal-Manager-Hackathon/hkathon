# Update 04 — Thêm field còn thiếu vào GET list responses

> Admin GET list các entity còn thiếu IsDisable và UpdatedAt trong response.

## 1. Events — GET /api/v1/admin/events + GET /admin/events/recent

**DTO:** `EventItem` — thiếu `IsDisable`, `UpdatedAt`

```csharp
// Cũ
public class EventItem {
    public Guid Id;
    public string Name;
    public string? Description;
    public string? Status;
    public DateTimeOffset? StartTime;
    public DateTimeOffset? EndTime;
    public DateTimeOffset CreatedAt;
}

// Mới — thêm IsDisable, UpdatedAt
public class EventItem {
    // ... giữ nguyên
    public bool IsDisable;          // ← thêm
    public DateTimeOffset CreatedAt;
    public DateTimeOffset UpdatedAt; // ← thêm
}
```

## 2. Rounds — GET /api/v1/admin/events/{eventId}/rounds

**DTO:** `RoundItem` — thiếu `IsDisable` (đã có UpdatedAt)

## 3. Teams — GET /api/v1/admin/teams

**DTO:** `TeamCard` — thiếu `UpdatedAt` (đã có IsDisable)

## 4. Notifications — GET /api/v1/admin/notifications + GET /admin/notifications/recent

**DTO:** `NotificationCard` — thiếu `IsDisable`, `UpdatedAt`

## 5. Doc files đã update response JSON

| File | Thêm field |
|------|-----------|
| `event/get/admin.events.md` | `isDisable`, `updatedAt` |
| `event/get/admin.events.recent.md` | `isDisable`, `updatedAt` |
| `round/get/admin.rounds.list.md` | `isDisable` |
| `team/get/admin.teams.md` | `updatedAt` |
| `notification/get/admin.notifications.md` | `isDisable`, `updatedAt` |
| `notification/get/admin.notifications.recent.md` | `isDisable`, `updatedAt` |

## 6. Các entity đã đủ

| API | Item | IsDisable | UpdatedAt |
|-----|------|-----------|-----------|
| .../tracks | `TrackItem` | ✅ | ✅ |
| .../topics | `TopicItem` | ✅ | ✅ |
| .../criteria-templates | `CriteriaTemplateItem` | ✅ | ✅ |
| .../criteria-items | `CriteriaItemInfo` | ✅ | ✅ |
| /users | `UserCard` | ✅ | ✅ |
| .../register-teams | `RegisterTeamCard` | ✅ | ✅ |
| /users/{id}/events | `UserEventItem` | ✅ | ✅ |
| /users/{id}/teams | `UserTeamItem` | ✅ | ✅ |
