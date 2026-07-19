# Admin Notification Routing Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add missing Admin-triggered notifications, correct Admin notification persistence/query behavior, and make `admin.md` exactly match implementation.

**Architecture:** Keep the existing `Notifications` schema. Register-team event lifecycle changes create one `Team` notification; assignment/report/leader changes create one `Personal` notification for the affected user; Admin's personal inbox remains `Personal + System`, while Team notifications remain available through the management query filtered by `targetType=Team`.

**Tech Stack:** .NET 8, C#, Entity Framework repositories, GitNexus MCP, Markdown.

## Global Constraints

- Run GitNexus upstream impact analysis before every method, class, or constructor edit.
- Stop and warn before editing if impact risk is HIGH or CRITICAL.
- Do not change routes, request/response contracts, entities, enums, or database schema.
- Use `NotificationHelper.Create` and centralized `NotificationMessage` constants.
- Do not create `System` notification for `EndRound`.
- Write final mapping directly to `Hackathon.Presentation/documents/thongbao/admin.md`.
- Do not commit or push.

---

### Task 1: Centralize missing Admin notification messages

**Files:**
- Modify: `Hackathon.Application/Common/NotificationMessage.cs`

- [ ] Run GitNexus impact for affected nested message classes.
- [ ] Add constants for track/topic removal and assignment restoration where existing constants cannot express the event.
- [ ] Build the solution.

### Task 2: Add RegisterTeam Team notifications

**Files:**
- Modify: `Hackathon.Application/Services/Admin/RegisterTeam/Service.cs`

- [ ] Run GitNexus impact for the service constructor and `ApproveRegisterTeam`, `RejectRegisterTeam`, `BanRegisterTeam`, `UnbanRegisterTeam`, `AssignToNextRound`, `RevertToPreviousRound`, `AssignTrackTopic`, and `RemoveTrackTopic`.
- [ ] Inject `INotificationRepository`.
- [ ] After each successful mutation, create one `Team` notification using `rt.TeamId`, existing team/event/round/track/topic data, and centralized messages.
- [ ] Save notification in the same unit of work as the business mutation.
- [ ] Build the solution.

### Task 3: Add assignment Personal notifications

**Files:**
- Modify: `Hackathon.Application/Services/Admin/Assign/Service.cs`

- [ ] Run GitNexus impact for the constructor and `AssignUserToEvent`, `AssignEventRoleToLecturer`, `AssignTrackToEvent`, `RemoveTrackFromEvent`, `RestoreTrackToEvent`, `RemoveAssignEvent`, and `RestoreAssignEvent`.
- [ ] Inject `INotificationRepository`.
- [ ] Create one `Personal` notification for the affected assigned user after every successful assignment mutation.
- [ ] Preserve early-return behavior when reactivating an existing assignment while still creating the notification.
- [ ] Build the solution.

### Task 4: Add report and leader Personal notifications

**Files:**
- Modify: `Hackathon.Application/Services/Admin/Report/Service.cs`
- Modify: `Hackathon.Application/Services/Admin/Team/Service.cs`

- [ ] Run GitNexus impact for both constructors, `UpdateReportStatus`, and `ChangeLeader`.
- [ ] Inject `INotificationRepository` into each service.
- [ ] Notify `report.UserId` of report status changes.
- [ ] Notify `newLeaderUserId` after Admin changes team leader.
- [ ] Build the solution.

### Task 5: Correct Admin notification APIs

**Files:**
- Modify: `Hackathon.Application/Services/Admin/Notification/Service.cs`
- Modify: `Hackathon.Infrastructure/Repositories/NotificationRepository.cs`

- [ ] Run GitNexus impact for `ReadNotification`, `ReadAllNotifications`, `GetRecentNotifications`, and `GetRecentAsync`.
- [ ] Persist single and bulk read status changes with `SaveChangesAsync`.
- [ ] Exclude disabled notifications from recent management results.
- [ ] Keep `/notifications/my` semantics unchanged: current Admin Personal + System only.
- [ ] Keep `/notifications` as the management query, including `targetType=Team` filtering.
- [ ] Build the solution.

### Task 6: Update Admin mapping documentation

**Files:**
- Modify: `Hackathon.Presentation/documents/thongbao/admin.md`

- [ ] Replace stale routes/method names with actual controller routes.
- [ ] Document all implemented RegisterTeam, Assign, Report, and Admin ChangeLeader notifications and exact targets.
- [ ] Remove `EndRound → System`.
- [ ] Verify no conflict markers and every row maps to implemented code.

### Task 7: Verify

- [ ] Run the C# reviewer on all changed C# files.
- [ ] Run `dotnet build` and available tests.
- [ ] Run `git diff --check` for changed files.
- [ ] Run GitNexus `detect_changes` before any commit; if externally blocked, report the exact blocker.
- [ ] Report actual outcomes without committing.
