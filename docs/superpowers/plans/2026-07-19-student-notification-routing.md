# Student Notification Routing Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Restrict Student `Team` notifications to event registration while routing internal team activity as `Personal`, and align the notification documentation with the code.

**Architecture:** Preserve the existing `Personal`, `Team`, and `System` notification schema. `Team` represents event-registration activity visible to team members and the Admin/Staff management query; all internal team activity uses a concrete personal recipient. No database or API response changes are required.

**Tech Stack:** .NET 8, C#, Entity Framework repositories, GitNexus MCP, Markdown.

## Global Constraints

- Run GitNexus upstream impact analysis before editing every affected method.
- Warn before editing if GitNexus reports HIGH or CRITICAL risk.
- Do not change API requests, responses, routes, entities, or database schema.
- Use existing `NotificationMessage` constants.
- Do not commit unless explicitly requested by the user.

---

### Task 1: Route invitation acceptance to the team leader

**Files:**
- Modify: `Hackathon.Application/Services/Student/Invitation/Service.cs:214-283`

**Interfaces:**
- Consumes: `NotificationHelper.Create(NotificationTargetTypeEnum, string?, string?, Guid?, Guid?)`
- Produces: one `Personal` notification addressed to the active team leader.

- [ ] **Step 1: Run GitNexus impact analysis for `AcceptInvitation`**

Run upstream callgraph impact scoped to `Hackathon.Application/Services/Student/Invitation/Service.cs` and stop for confirmation if risk is HIGH or CRITICAL.

- [ ] **Step 2: Change the notification recipient**

After reading team members, retain the active leader. Create `Member Joined` with `NotificationTargetTypeEnum.Personal` and `userId: teamLeader.UserId`; do not assign `teamId`.

- [ ] **Step 3: Build the solution**

Run: `dotnet build`
Expected: build succeeds with zero errors.

---

### Task 2: Route team lifecycle notifications personally

**Files:**
- Modify: `Hackathon.Application/Services/Student/Team/Service.cs:153-198`
- Modify: `Hackathon.Application/Services/Student/Team/Service.cs:394-437`

**Interfaces:**
- Consumes: active leader records already loaded by each method.
- Produces: `DisbandTeam` notification for the acting leader and `LeaveTeam` notification for the active leader.

- [ ] **Step 1: Run GitNexus impact analysis for `DisbandTeam` and `LeaveTeam`**

Run upstream callgraph impact for both methods and stop for confirmation if either risk is HIGH or CRITICAL.

- [ ] **Step 2: Change `DisbandTeam` to `Personal`**

Create `Team Disbanded` using `NotificationTargetTypeEnum.Personal` with `userId: userId`; remove `teamId` from the factory call.

- [ ] **Step 3: Change `LeaveTeam` to `Personal`**

Resolve the active leader from the already loaded `members`, then create `Member Left` using `NotificationTargetTypeEnum.Personal` with `userId: teamLeader.UserId`.

- [ ] **Step 4: Build the solution**

Run: `dotnet build`
Expected: build succeeds with zero errors.

---

### Task 3: Keep one team registration notification

**Files:**
- Modify: `Hackathon.Application/Services/Student/RegisterTeam/Service.cs:317-449`

**Interfaces:**
- Produces: one `Team` notification with `teamId` for each successful new registration.

- [ ] **Step 1: Run GitNexus impact analysis for `CreateRegisterTeam`**

Run upstream callgraph impact and stop for confirmation if risk is HIGH or CRITICAL.

- [ ] **Step 2: Remove duplicate Admin/Staff personal notifications**

Keep the existing `Event Registered` team notification. Delete the user lookup, role filtering, personal-notification projection, and add loop at the end of the method.

- [ ] **Step 3: Remove unused `IUserRepository` injection if no longer referenced**

Remove the field, constructor parameter, and assignment only after confirming the service contains no remaining `_userRepository` references.

- [ ] **Step 4: Build the solution**

Run: `dotnet build`
Expected: build succeeds with zero errors.

---

### Task 4: Route submission confirmation to the leader

**Files:**
- Modify: `Hackathon.Application/Services/Student/Submission/Service.cs:105-199`

**Interfaces:**
- Consumes: the active `leader` found during authorization.
- Produces: one `Personal` submission notification addressed to `leader.UserId`.

- [ ] **Step 1: Run GitNexus impact analysis for `CreateSubmission`**

Run upstream callgraph impact and stop for confirmation if risk is HIGH or CRITICAL.

- [ ] **Step 2: Change the submission notification to `Personal`**

Create `Submission Submitted` with `NotificationTargetTypeEnum.Personal` and `userId: leader.UserId`; remove the `teamId` argument.

- [ ] **Step 3: Build the solution**

Run: `dotnet build`
Expected: build succeeds with zero errors.

---

### Task 5: Resolve and update Student notification documentation

**Files:**
- Modify: `Hackathon.Presentation/documents/thongbao/student.md:1-46`

**Interfaces:**
- Documents the exact target and recipient implemented by Tasks 1-4.

- [ ] **Step 1: Remove all Git conflict markers**

Remove `<<<<<<<`, `=======`, and `>>>>>>>` blocks and retain one valid Markdown row per API.

- [ ] **Step 2: Document the final routing**

Record these targets: invitation sent → invited user; invitation accepted/rejected → leader; disband → acting leader; kick → kicked member; transfer leader → new leader; leave → active leader; register event → team; submission → leader.

- [ ] **Step 3: Verify conflict markers are absent**

Search `Hackathon.Presentation/documents/thongbao/student.md` for `<<<<<<<|=======|>>>>>>>`.
Expected: no matches.

---

### Task 6: Review and final verification

**Files:**
- Review all files changed in Tasks 1-5.

- [ ] **Step 1: Run C# code review**

Verify recipient correctness, null safety, dependency injection consistency, and preservation of business/API behavior.

- [ ] **Step 2: Build once from repository root**

Run: `dotnet build`
Expected: build succeeds with zero errors.

- [ ] **Step 3: Run GitNexus change detection**

Run `detect_changes` with scope `all` and verify only the five intended notification methods, constructor dependency cleanup, and Markdown documentation are affected.

- [ ] **Step 4: Report actual verification results**

List changed routing, build result, GitNexus affected flows, and any pre-existing unrelated working-tree changes. Do not commit.
