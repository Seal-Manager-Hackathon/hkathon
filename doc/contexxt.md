# NGỮ CẢNH CHI TIẾT TOÀN BỘ HỆ THỐNG

> File này dành cho AI Agent hiểu rõ business rules, quy tắc phân quyền, và cách mỗi role tương tác với từng entity.
> Đây là bản chi tiết, KHÔNG phải tóm tắt.

---

## MỤC LỤC

- [Tổng quan kiến trúc](#tổng-quan-kiến-trúc)
- [Các Enum](#các-enum)
- [BaseEntity pattern](#baseentity-pattern)
- [1. Event](#1-event)
- [2. Round](#2-round)
- [3. Track & Topic](#3-track--topic)
- [4. Critical / Criteria](#4-critical--criteria)
- [5. AssignEvents (Phân công)](#5-assignevents-phân-công)
- [6. AssignTracks (Phân công Track)](#6-assigntracks-phân-công-track)
- [7. RegisterTeams (Đăng ký tham gia)](#7-registerteams-đăng-ký-tham-gia)
- [8. Submission (Bài nộp)](#8-submission-bài-nộp)
- [9. Score / Scoring (Chấm điểm)](#9-score--scoring-chấm-điểm)
- [10. LeaderBoard & Award](#10-leaderboard--award)
- [11. Team & Invitation](#11-team--invitation)
- [12. Mentor](#12-mentor)
- [13. Judge](#13-judge)
- [14. Staff](#14-staff)
- [15. Lecturer](#15-lecturer)
- [16. Student](#16-student)
- [17. Admin](#17-admin)
- [18. Auth & User](#18-auth--user)
- [19. Notification](#19-notification)
- [20. System & File](#20-system--file)
- [21. Report](#21-report)
- [Phụ lục: Policy mapping](#phụ-lục-policy-mapping)

---

## Tổng quan kiến trúc

### Các role chính (RoleEnum)

| Role         | Mô tả                                                       |
| ------------ | ----------------------------------------------------------- |
| **Admin**    | Toàn quyền, bypass mọi IsDisable, thấy tất cả               |
| **Staff**    | Nhân viên vận hành, phải được assign vào event mới thao tác |
| **Student**  | Sinh viên tham gia hackathon, đăng ký team, nộp bài         |
| **Lecturer** | Giảng viên, có thể là Mentor hoặc Judge trong 1 event       |

### EventRole (EventRoleEnum — dành cho Lecturer)

| Role       | Mô tả                                           |
| ---------- | ----------------------------------------------- |
| **Mentor** | Hỗ trợ đội, gửi notification, xem team          |
| **Judge**  | Chấm điểm submission                            |
| **Staff**  | (Chỉ phân biệt trong AssignEvents) Staff system |

### Base Pattern: IsDisable (soft-delete)

- **Mọi entity** đều có `IsDisable` (bool, kế thừa từ `BaseEntity`).
- `IsDisable = true` = đã bị xoá / vô hiệu hoá.
- **Nguyên tắc chung**: role không phải Admin chỉ thấy `IsDisable = false`.
- **Admin**: bypass mọi IsDisable (ngoại trừ staff helper `EnsureStaffAssignedToEvent` cũng bypass).

### Base Pattern: Check IsCurrentUserAdmin()

- Hầu hết Service đều có method `IsCurrentUserAdmin()` kiểm tra role từ JWT claim.
- Nếu là Admin → bỏ qua check IsDisable entity.
- Pattern phổ biến:

```csharp
if (!IsCurrentUserAdmin()) {
    var exists = await _dbContext.Events.AnyAsync(x => x.Id == id && !x.IsDisable);
    if (!exists) throw new NotFoundException("EVENT_NOT_FOUND");
}
```

### Base Pattern: EnsureStaffAssignedToEvent(eventId)

- Staff & Lecturer (khi thao tác vận hành) phải được assign vào event.
- Admin bypass check này.
- AssignEvents: `UserId == staffId && EventId == eventId && !IsDisable`
- Một số chỗ check thêm `!x.Event.IsDisable` (Rounds), một số không (Events, Tracks, v.v.)

---

## Các Enum

### EventStatusEnum

```csharp
Draft     // Mới tạo, chưa public
Published // Đang mở
Closed    // Đã đóng (kết thúc)
```

### SeasonEnum

```csharp
Spring, Summer, Autumn, Winter
```

### RegisterTeamStatusEnum

```csharp
Pending,  // Chờ duyệt
Approved, // Được duyệt vào event
Rejected  // Bị từ chối
```

### SubmissionStatusEnum

```csharp
Submitted,   // Đã nộp
Unsubmitted, // Chưa nộp
Failed       // Nộp thất bại
```

### ScoresStatusEnum

```csharp
IsRetake,   // Điểm đã được chấm lại (regrade)
IsMock,     // Điểm mock (thử)
IsDisable,  // Điểm bị vô hiệu
```

### LeaderBoardsStatusEnum

```csharp
IsDisabled
```

### TeamDetailStatusEnum

```csharp
Active, Inactive
```

### InvitationStatusEnum

```csharp
Pending, Accepted, Rejected, Expired
```

### UserStatusEnum

```csharp
Active, Inactive, Banned
```

### RoleEnum

```csharp
Admin, Staff, Student, Lecturer
```

### EventRoleEnum

```csharp
Mentor, Judge, Staff
```

### ReportStatusEnum

```csharp
Open, Closed, Approved
```

---

## 1. Event

### Entity: Events

```
Id, Name, Description, StartTime, EndTime, RegisterLimitTime,
LimitTeam, MinMember, MaxMember, Status (EventStatusEnum),
NumberRound, Season (SeasonEnum),
IsDisable, CreatedAt, UpdatedAt
```

### Quy tắc chung

- Khi **tạo**: `Status = Draft`, `IsDisable = true` (chưa hoàn thiện), `NumberRound = 0`.
- Khi **publish**: `Status = Draft → Published`, chỉ được nếu event đã setup đủ (rounds, criteria, tracks, topics, awards, assigned staff).
- Khi **close**: `Status = Published → Closed`, đồng thời lock leaderboard.
- Khi **unpublish**: `Status = Published → Draft`.
- Khi **disable** (xoá mềm): `IsDisable = true`.
- Khi **restore**: `IsDisable = false`.
- **Số không được âm**: LimitTeam, MinMember, MaxMember, NumberRound.
- **EndTime > StartTime > RegisterLimitTime** (validation: StartTime < EndTime, RegisterLimitTime < StartTime).
- **Không thể tạo event với now > StartTime** (ko tạo event đã qua thời gian bắt đầu).
- **Không thể disable true→false (enable) event khi chưa hoàn thành setup** (checks in PublishEvent).
- Khi tạo event → đồng thời tạo **LeaderBoard** với `Year = StartTime?.Year ?? CreatedAt.Year`.
- Khi update StartTime → cập nhật `LeaderBoard.Year`.

### Admin: Event

- **ĐƯỢC**: Xem tất cả events (kể cả IsDisable=true, Draft). Tạo, sửa, xoá, publish, unpublish, close, restore.
- **ĐƯỢC**: Xem event detail bất kỳ (không filter IsDisable/Status).
- **KHÔNG ĐƯỢC**: (Không có giới hạn — admin toàn quyền)
- **KHÔNG ĐƯỢC publish event khi chưa setup đủ** (rounds, criteria, tracks, topics, awards, assigned staff).
- API:
  - `GET /api/v1/admin/events` — danh sách (filter IsDisable, keyword, year, status)
  - `GET /api/v1/admin/events/{eventId}` — detail
  - `POST /api/v1/admin/events` — tạo
  - `PATCH /api/v1/admin/events/{eventId}` — sửa
  - `DELETE /api/v1/admin/events/{eventId}` — xoá mềm (IsDisable=true)
  - `PATCH /api/v1/admin/events/{eventId}/publish` — publish
  - `PATCH /api/v1/admin/events/{eventId}/unpublish` — unpublish
  - `PATCH /api/v1/admin/events/{eventId}/close` — close
  - `PATCH /api/v1/admin/events/{eventId}/restore` — restore
  - `GET /api/v1/admin/events/{eventId}/setup-status` — check setup (cùng Staff)

### Student: Event

- **ĐƯỢC**: Xem danh sách event Published & Closed, IsDisable=false.
- **ĐƯỢC**: Xem detail event Published & Closed, IsDisable=false.
- **ĐƯỢC**: Xem event đã tham gia (joined events = team đã approved vào event).
- **KHÔNG ĐƯỢC**: Xem event Draft.
- **KHÔNG ĐƯỢC**: Xem event IsDisable=true.
- API:
  - `GET /api/v1/events` — danh sách (Published + Closed, IsDisable=false)
  - `GET /api/v1/events/{eventId}` — detail (Published + Closed, IsDisable=false)
  - `GET /api/v1/events/events/joined` — joined events (đã [Authorize])
  - `GET /api/v1/events/most-participants` — top events

### Staff: Event

- **ĐƯỢC**: Xem danh sách event được assign (qua AssignEvents, IsDisable=false).
- **ĐƯỢC**: Xem event current đang diễn ra (Published, StartTime ≤ now ≤ EndTime).
- **KHÔNG XEM ĐƯỢC event Draft** — staff chỉ thấy Published & Closed (trừ admin).
- **Staff thấy AssignEvent `IsDisable=false`** nhưng không thấy event `IsDisable=true`.
- API:
  - `GET /api/v1/staff/events` — danh sách event đã assign (Published)
  - `GET /api/v1/staff/events/search` — tìm kiếm (Published + Closed, không Draft)
  - `GET /api/v1/staff/events/current` — event đang diễn ra

### Lecturer: Event (LecturersController)

- **ĐƯỢC**: Xem danh sách event được assign (qua AssignEvents với EventRole).
- **ĐƯỢC**: Xem tracks của event đã assign.
- **KHÔNG XEM ĐƯỢC**: Event chưa assign, event Draft, event IsDisable=true.
- API:
  - `GET /api/v1/lecturers/events` — danh sách
  - `GET /api/v1/lecturers/events/search` — tìm kiếm (keyword, year, eventRole)
  - `GET /api/v1/lecturers/events/current` — event đang diễn ra
  - `GET /api/v1/lecturers/events/{eventId}/tracks` — tracks

### Mentor: Event (MentorsController)

- **ĐƯỢC**: Xem event đã assign với EventRole=Mentor.
- API: `GET /api/v1/mentor/events`

### Judge: Event (JudgeController)

- **ĐƯỢC**: Xem event đã assign với EventRole=Judge (qua submissions endpoint).
- API: `GET /api/v1/judge/events/{eventId}/submissions`

### Public (không cần đăng nhập): Event

- **ĐƯỢC**: Xem danh sách event Published & Closed.
- **ĐƯỢC**: Xem detail event, awards, leaderboard, summary, tracks.
- **KHÔNG ĐƯỢC**: Xem event Draft, IsDisable=true.
- API:
  - `GET /api/v1/events` / `GET /api/v1/events/{eventId}`
  - `GET /api/v1/events/{eventId}/awards` / `leaderboard` / `summary` / `tracks`

---

## 2. Round

### Entity: Rounds

```
Id, EventId, Name, Description, RoundNo, StartTime, EndTime,
StartSubmission, EndSubmission, LimitTeam,
IsDisable, CreatedAt, UpdatedAt
```

### Entity: RoundDetails

```
Id, RoundId, RegisterTeamId,
IsDisable, CreatedAt, UpdatedAt
```

### Quy tắc chung

- Round thuộc về 1 Event (EventId).
- **Thời gian**: EndTime > StartTime, StartSubmission ≤ EndSubmission.
- Toàn bộ thời gian round phải nằm trong thời gian event.
- **RoundNo tự động**: tạo round mới → `RoundNo = max(RoundNo hiện tại) + 1`, đồng thời tăng `Event.NumberRound`.
- Khi xoá round → dồn RoundNo các round còn lại, giảm `Event.NumberRound`.
- **LimitTeam**: Round No 1 mặc định lấy từ `Event.LimitTeam`.
- **Không thể sửa round đã bắt đầu** (now > StartTime).
- **Không thể disable round đã bắt đầu**.
- **Background Job EndRoundJob**: chạy định kỳ, check `now > EndTime` → close round.

### EndRound (chuyển round)

- **EndRound (StaffOrAdmin)**: Set `EndTime` = now + tính ranking. Yêu cầu round.EndTime < now (round đã hết hạn) mới được gọi.
- **EndRoundFinal (Admin)**: Force set `EndTime = now` và `EndSubmission = now` kể cả round chưa hết hạn.
- Tính điểm: lấy trung bình score items của các judge cho mỗi team (từ submission mới nhất).
- Lấy top N team (theo LimitTeam của round kế) chuyển sang round sau.
- Tạo RoundDetails mới cho các team được chọn.
- **Không phân biệt track** — xét chung tất cả team trong event.
- **Background Job EndRoundJob**: monitor tất cả event đang được ScheduleEvent (gọi khi tạo event). Check mỗi 5 phút, nếu có round hết hạn (EndTime < now) thì tự động CloseAndAdvanceRoundAsync.

### Admin: Round

- **ĐƯỢC**: Xem tất cả rounds (kể cả IsDisable=true).
- **ĐƯỢC**: Tạo, sửa, xoá (disable), restore round.
- **ĐƯỢC**: EndRound (StaffOrAdmin), EndRoundFinal (Admin).
- **KHÔNG ĐƯỢC**: Sửa round đã bắt đầu.
- API:
  - `GET /api/v1/admin/events/{eventId}/rounds` — danh sách (filter IsDisable)
  - `POST /api/v1/admin/events/{eventId}/rounds` — tạo
  - `PATCH /api/v1/admin/rounds/{roundId}` — sửa
  - `DELETE /api/v1/admin/rounds/{roundId}` — xoá mềm
  - `PATCH /api/v1/admin/rounds/{roundId}/restore` — restore
  - `GET /api/v1/admin/rounds/{roundId}/submissions` — submissions của round

### Public / Student: Round

- **ĐƯỢC**: Xem danh sách round của event (chỉ IsDisable=false, event IsDisable=false).
- **ĐƯỢC**: Xem detail round.
- **ĐƯỢC**: Xem ranking, submission của team mình.
- **KHÔNG ĐƯỢC**: Xem round IsDisable=true, event IsDisable=true.
- API:
  - `GET /api/v1/rounds?eventId={eventId}` — danh sách
  - `GET /api/v1/rounds/{roundId}` — detail
  - `GET /api/v1/rounds/{roundId}/ranking` — ranking
  - `GET /api/v1/rounds/{roundId}/my-submissions` — submission của user
  - `POST /api/v1/rounds/{roundId}/submit-assignment` — nộp bài (cần auth)

### Staff: Round

- **ĐƯỢC**: Xem round và submissions (cần EnsureStaffAssignedToEvent).
- **ĐƯỢC**: Assign judges vào submission.
- API:
  - `GET /api/v1/staff/rounds/{roundId}/submissions`
  - `POST /api/v1/staff/submissions/{submissionId}/assign-judges`
  - `POST /api/v1/rounds/{roundId}/end` (StaffOrAdmin)

---

## 3. Track & Topic

### Entity: Tracks

```
Id, EventId, Title, Description, MaxTeam,
IsDisable, CreatedAt, UpdatedAt
```

- Track thuộc Event.
- **MaxTeam**: Số lượng team tối đa cho track này.
- Khi track bị disable → các topic của nó cũng bị disable theo.

### Entity: Topics

```
Id, TrackId, Title, Description,
IsDisable, CreatedAt, UpdatedAt
```

- Topic thuộc Track.
- Mỗi team có thể chọn (hoặc được gán) 1 topic.

### Admin: Track & Topic

- **ĐƯỢC**: CRUD tracks (kể cả IsDisable=true).
- **ĐƯỢC**: CRUD topics (kể cả IsDisable=true).
- **ĐƯỢC**: Update track visibility (disable/enable).
- API:
  - `POST /api/v1/admin/events/{eventId}/tracks` — tạo track
  - `PATCH /api/v1/admin/tracks/{trackId}` — sửa track
  - `PATCH /api/v1/admin/tracks/{trackId}/visibility` — toggle disable
  - `DELETE /api/v1/admin/tracks/{trackId}` — xoá
  - `GET /api/v1/admin/tracks/{trackId}/topics` — topics (StaffOrAdmin)
  - `POST /api/v1/admin/tracks/{trackId}/topics` — tạo topic (StaffOrAdmin)

### Staff: Track & Topic

- **ĐƯỢC**: Xem tracks của event đã assign.
- **ĐƯỢC**: Xem topics của track.
- **ĐƯỢC**: Gán track/topic cho team.
- API:
  - `GET /api/v1/staff/events/{eventId}/tracks` (StaffPolicy)
  - `GET /api/v1/staff/tracks/{trackId}/topics` (StaffPolicy)
  - `PATCH /api/v1/staff/events/{eventId}/teams/{teamId}/track` — gán track
  - `PATCH /api/v1/staff/events/{eventId}/teams/{teamId}/topic` — gán topic (StaffPolicy)

### Public / Student: Track & Topic

- **ĐƯỢC**: Xem tracks của event IsDisable=false.
- API:
  - `GET /api/v1/events/{eventId}/tracks`
  - `GET /api/v1/tracks`
  - `GET /api/v1/tracks/{trackId}`
  - `GET /api/v1/tracks/{trackId}/topics`
  - `GET /api/v1/tracks/{trackId}/teams/count`
  - `GET /api/v1/events/{eventId}/register-teams/{registerTeamId}/topic`

### Lecturer: Track

- **Judge**: `GET /api/v1/judge/tracks`
- **Mentor**: `GET /api/v1/mentor/tracks`
- **Lecturer (chung)**: `GET /api/v1/lecturers/events/{eventId}/tracks`

---

## 4. Critical / Criteria

### Entity: CriteriaTemplates

```
Id, RoundId, Title, Description,
IsDisable, CreatedAt, UpdatedAt
```

### Entity: CriteriaItems

```
Id, CriteriaTemplateId, Name, Description, Score (decimal),
IsDisable, CreatedAt, UpdatedAt
```

### Quy tắc chung

- CriteriaTemplate thuộc Round.
- CriteriaItems thuộc CriteriaTemplate.
- **1 round có thể có nhiều template, nhưng chỉ 1 template active** (`IsDisable = false`).
- Khi tạo template mới: mặc định `IsDisable = true` (inactive).
- **Kích hoạt template**: set `IsDisable = false` cho template đó, đồng thời set `IsDisable = true` cho tất cả template khác trong cùng round.
- Khi round bị disable → các criteria template của nó cũng bị disable theo.
- Khi template bị disable → các item trong đó cũng bị disable.
- **CriteriaItem không sửa, chỉ tạo mới** (không có update item).

### Admin: Criteria

- **ĐƯỢC**: Tạo template + items.
- **ĐƯỢC**: Activate template.
- **ĐƯỢC**: Xem tất cả template (cả active & inactive).
- API:
  - `POST /api/v1/admin/events/{eventId}/rounds/{roundId}/criteria` — tạo
  - `PATCH /api/v1/admin/events/{eventId}/rounds/{roundId}/criteria/{templateId}/activate` — kích hoạt
  - `GET /api/v1/admin/events/{eventId}/rounds/{roundId}/criteria` — danh sách

### Public: Criteria

- **ĐƯỢC**: Xem criteria active của round (`IsDisable=false`).
- API:
  - `GET /api/v1/rounds/{roundId}/criteria`
  - `GET /api/v1/events/{eventId}/criteria`

### Judge: Criteria

- **ĐƯỢC**: Xem criteria của submission để chấm.
- API: `GET /api/v1/judge/submissions/{submissionId}/criteria`

---

## 5. AssignEvents (Phân công)

### Entity: AssignEvents

```
Id, UserId, EventRoleId (nullable), EventId,
IsDisable, CreatedAt, UpdatedAt
```

### Quy tắc chung

- Ghi nhận việc phân công User (Staff hoặc Lecturer) vào Event.
- **EventRoleId**:
  - `null` = Staff system (nhân viên vận hành).
  - Có giá trị = Lecturer (Mentor hoặc Judge).
- **1 Lecturer không thể vừa là Mentor vừa là Judge trong cùng 1 event**.
- **1 Lecturer có thể làm Mentor/Judge cho nhiều event** cùng lúc.
- **Phân công vào event bị disable được** — Lecturer vẫn thấy event đó.
- Khi xoá phân công event → cascade xoá toàn bộ AssignTracks của phân công đó.
- Staff chỉ thấy Lecturer assignments (không thấy Staff assignments của nhau).
- Admin thấy tất cả.

### Admin: AssignEvents

- **ĐƯỢC**: Phân công Staff vào event (EventRoleId=null).
- **ĐƯỢC**: Xem danh sách Staff đã phân công (phân trang, chỉ Staff role).
- **ĐƯỢC**: Xem Staff available (chưa phân công, Active, không disable, không trùng).
- **ĐƯỢC**: Xoá phân công Staff.
- **ĐƯỢC**: Phân công Lecturer vào event (StaffOrAdmin).
- API:
  - `POST /api/v1/admin/events/{eventId}/staff` — assign staff
  - `GET /api/v1/admin/events/{eventId}/assignments` — danh sách staff đã assign
  - `GET /api/v1/admin/events/{eventId}/staff/available` — staff chưa assign
  - `DELETE /api/v1/admin/assign-events/{id}` — xoá staff assignment
  - `PATCH /api/v1/admin/assign-events/{id}/role` — đổi role lecturer

### Staff: AssignEvents

- **ĐƯỢC**: Xem danh sách Lecturer đã phân công trong event (filter EventRole, track, keyword, isDisable).
- **ĐƯỢC**: Xem Lecturer available chưa phân công.
- **ĐƯỢC**: Phân công Lecturer vào event (chỉ Lecturer).
- **ĐƯỢC**: Xoá phân công Lecturer khỏi event (cascade xoá AssignTracks).
- **KHÔNG ĐƯỢC**: Tự assign Staff khác — chỉ Admin.
- API:
  - `GET /api/v1/staff/events/{eventId}/assignments`
  - `GET /api/v1/staff/events/{eventId}/lecturers/available`
  - `POST /api/v1/staff/events/{eventId}/assign-lecturers`
  - `DELETE /api/v1/staff/assign-events/{id}`
  - `POST /api/v1/staff/events/{eventId}/tracks/{trackId}/assign-lecturers`

---

## 6. AssignTracks (Phân công Track)

### Entity: AssignTracks

```
Id, AssignEventId, TrackId,
IsDisable, CreatedAt, UpdatedAt
```

### Quy tắc chung

- Ghi nhận việc phân công Lecturer (đã assign vào event) vào 1 track cụ thể.
- Chỉ áp dụng cho Lecturer có EventRole là Mentor hoặc Judge.
- **1 Lecturer có thể phân công vào nhiều track trong cùng event**.
- Khi xoá khỏi event → cascade xoá toàn bộ AssignTracks.
- Khi xoá khỏi track → chỉ xoá track đó, không out khỏi event.

### Admin & Staff: AssignTracks

- API:
  - `POST /api/v1/staff/events/{eventId}/tracks/{trackId}/assign-lecturers` — assign
  - `DELETE /api/v1/staff/assign-tracks/{id}` — xoá
  - `GET /api/v1/staff/events/{eventId}/tracks/{trackId}/lecturers` — xem

---

## 7. RegisterTeams (Đăng ký tham gia)

### Entity: RegisterTeams

```
Id, TeamId, EventId, TrackId (nullable), TopicId (nullable),
Description, RejectionReason,
Status (RegisterTeamStatusEnum), IsBanned,
IsDisable, CreatedAt, UpdatedAt
```

### Quy tắc chung

- Team đăng ký tham gia 1 event.
- **Status flow**: Pending → Approved / Rejected.
- **IsBanned**: team bị ban khỏi event.
- Chỉ đăng ký được vào event Published (không Draft/Closed) và còn hạn đăng ký (RegisterLimitTime).
- **1 team chỉ được approved cho 1 event duy nhất**. Nếu team đã từng Approved ở event khác → không đăng ký thêm được (Forbidden).
- Nếu team bị Rejected → có thể đăng ký lại (re-register) cho cùng event đó.
- **Team must có leader** mới đăng ký được (chỉ leader mới gọi API register).
- **Số lượng thành viên** phải nằm trong MinMember..MaxMember của event.
- **LimitTeam event**: kiểm tra số team đã đăng ký (không tính Rejected) không vượt quá LimitTeam.
- **Khi Approve**:
  - Status → Approved, RejectionReason → null
  - **Team.CanEdit = false** (khoá team, không thay đổi thành viên được nữa)
  - **Tự động tạo RoundDetails** cho Round No 1 (nếu có round 1)
  - Gửi notification cho tất cả thành viên
- **Khi Reject**:
  - Status → Rejected, RejectionReason ghi lý do
  - **Team.CanEdit = true** (mở khoá team)
  - Gửi notification cho tất cả thành viên
- **Khi Ban**:
  - IsBanned = true, Status → Rejected, CanEdit = true (mở khoá)
- **Khi Unban**: IsBanned = false (status không đổi)
- **IsEliminated**: Nếu team đã approved nhưng không có RoundDetails nào (bị loại từ vòng nào đó).

### Register Event validation rules (từ service code)

1. User phải là Student
2. User phải là Team Leader
3. Event phải Published và IsDisable=false
4. now < RegisterLimitTime (nếu có)
5. Số member active trong team phải ≥ MinMember và ≤ MaxMember
6. Team chưa từng Approved event nào khác
7. Event chưa đạt LimitTeam
8. Nếu đã đăng ký (Pending/Approved) → ConflictException
9. Nếu đã từng Rejected → cho đăng ký lại (re-register)

### Student: RegisterTeams

- **ĐƯỢC**: Đăng ký team vào event.
- **ĐƯỢC**: Xem đăng ký của bản thân.
- **ĐƯỢC**: Xem lý do reject.
- API:
  - `POST /api/v1/register-teams` — đăng ký
  - `GET /api/v1/register-teams/me` — danh sách
  - `GET /api/v1/register-teams/{registerId}` — detail
  - `GET /api/v1/register-teams/{registerId}/rejection-reason` — lý do từ chối
  - `GET /api/v1/register-teams/{registerTeamId}/assignment-status`

### Staff: RegisterTeams

- **ĐƯỢC**: Xem tất cả đăng ký của event (filter keyword, status, isDisable).
- **ĐƯỢC**: Duyệt (approve) / từ chối (reject).
- **ĐƯỢC**: Ban / Unban team.
- **ĐƯỢC**: Xem submissions của team.
- API:
  - `GET /api/v1/register-teams/staff/events/{eventId}` (StaffOrAdmin)
  - `GET /api/v1/register-teams/staff/{registerTeamId}` (StaffLecturerOrAdmin)
  - `PUT /api/v1/register-teams/staff/{registerId}/approve` (StaffOrAdmin)
  - `PUT /api/v1/register-teams/staff/{registerId}/reject` (StaffOrAdmin)
  - `PATCH /api/v1/register-teams/staff/{registerId}/ban` (StaffOrAdmin)
  - `PATCH /api/v1/register-teams/staff/{registerId}/unban` (StaffOrAdmin)
  - `GET /api/v1/staff/events/{eventId}/register-teams`

---

## 8. Submission (Bài nộp)

### Entity: Submissions

```
Id, RoundDetailId, Url, Description,
Status (SubmissionStatusEnum), SubmittedAt, IsRegrade,
IsDisable, CreatedAt, UpdatedAt
```

### Quy tắc chung

- RoundDetails: 1 team trong 1 round có 1 RoundDetail → nhiều Submissions.
- **Mỗi team nộp nhiều lần** (lấy submission mới nhất qua `SubmittedAt`).
- **IsRegrade**: bài nộp đã yêu cầu chấm lại.
- Submission chỉ được tạo khi round đang trong thời gian submission (StartSubmission ≤ now ≤ EndSubmission).
- Chỉ **Team Leader** mới nộp được bài.
- Team phải **Approved**, không bị **Banned**, đã được gán **Track và Topic**.
- Khi tạo submission lần đầu: RoundDetails được tạo nếu chưa tồn tại.
- **Appeal**: Student có thể appeal (khiếu nại) round hoặc submission (tạo report, không phải API riêng).

### Student: Submission

- **ĐƯỢC**: Nộp bài (nếu leader, round đang mở).
- **ĐƯỢC**: Xem submission của team mình.
- **KHÔNG ĐƯỢC**: Nộp bài khi team chưa có track/topic.
- API:
  - `POST /api/v1/rounds/{roundId}/submit-assignment` — nộp bài (trong RoundsController)
  - `POST /api/v1/submissions/rounds/{roundId}/register-teams/{registerTeamId}` — nộp bài (trong SubmissionsController)
  - `GET /api/v1/submissions/rounds/{roundId}/register-teams/{registerTeamId}` — xem
  - `GET /api/v1/submissions/{submissionId}` — detail
  - `POST /api/v1/teams/{teamId}/rounds/{roundId}/appeal` — appeal round (StudentPolicy)
  - `POST /api/v1/teams/{teamId}/submissions/{submissionId}/appeal` — appeal submission (StudentPolicy)

### Staff: Submission

- **ĐƯỢC**: Xem submissions của round (filter track, topic, keyword, status).
- **ĐƯỢC**: Assign judges vào submission.
- API:
  - `GET /api/v1/staff/rounds/{roundId}/submissions`
  - `POST /api/v1/staff/submissions/{submissionId}/assign-judges`

### Judge: Submission

- **ĐƯỢC**: Xem submission cần chấm, chấm điểm.
- **KHÔNG ĐƯỢC**: Chấm submission không được assign.

---

## 9. Score / Scoring (Chấm điểm)

### Entity: Scores

```
Id, SubmissionId, AssignTrackId, IsRetake,
RetakeFromScoreId (nullable), TotalScore (decimal), IsMock,
IsDisable, CreatedAt, UpdatedAt
```

### Entity: ScoreItems

```
Id, ScoreId, CriteriaItemId, AssignTrackId,
Score (decimal), Comment,
CreatedAt, UpdatedAt
```

### Quy tắc tính điểm

- **Mỗi Judge chấm 1 bài → 1 Score** (gắn với AssignTrackId của judge đó).
- **ScoreItems**: điểm cho từng tiêu chí (CriteriaItem).
- **Điểm 1 round của 1 team** = tổng (trung bình các ScoreItems của các Judge) = tổng các tiêu chí(item) trung bình các judge.
- **Điểm event** = trung bình điểm các round.
- **Mock score**: chấm thử, không tính vào kết quả (IsMock=true).
- **Regrade**: chấm lại. Score mới ghi đè score cũ (IsRetake=true). Mỗi judge chỉ lấy score mới nhất (theo AssignTrackId, UpdatedAt).
- **Finalize**: đánh dấu IsFinalized.
- **Công thức tính điểm (CalculateTotalScore)**:
  1. Lọc scores không IsDisable, không IsMock
  2. Nhóm theo AssignTrackId, lấy score mới nhất (mỗi judge 1 score)
  3. Lấy tất cả ScoreItems từ các score đó (không disable, có Score.HasValue)
  4. Gom nhóm theo CriteriaItemId, lấy **trung bình** các judge cho mỗi criteria
  5. **Tổng** các trung bình đó = TotalScore
- **GradingStatus**:
  - `NoJudgesAssigned`: chưa có judge nào assign
  - `PendingGrading`: có judge nhưng chưa ai chấm
  - `GradingInProgress`: 1 số judge đã chấm, 1 số chưa
  - `Graded`: tất cả judge đã chấm
  - `Finalized`: tất cả đã finalize
- **Judge không được xem submissions khi round chưa đóng submission** (LectureRoundSubmissions kiểm tra EndSubmission).
- **Admin/Staff không bị giới hạn thời gian**.

### Judge: Score

- **ĐƯỢC**: Submit, mock, update, finalize, retake.
- **ĐƯỢC**: Xem điểm của mình.
- Chi tiết API xem phần [Judge](#13-judge).

### Staff: Score

- **ĐƯỢC**: Approve regrade.
- **ĐƯỢC**: Xem regrade submissions.
- API:
  - `POST /api/v1/staff/submissions/{submissionId}/regrade`
  - `GET /api/v1/staff/submissions/regrade`

---

## 10. LeaderBoard & Award

### Entity: LeaderBoards

```
Id, EventId, Year (int), IsLocked, IsPublished,
IsDisable, CreatedAt, UpdatedAt
```

- Mỗi Event có 1 LeaderBoard (chỉ 1 leaderboard được tạo cùng lúc với event).

### Entity: LeaderBoardDetails

```
Id, LeaderBoardId, TeamId, Score (decimal), LevelAward (int?),
IsDisable, CreatedAt, UpdatedAt
```

### Entity: Awards

```
Id, EventId, Name, Description, LevelAward, NumberOfAward, Prize,
IsDisable, CreatedAt, UpdatedAt
```

- **Không thể có 2 award cùng LevelAward trong 1 event**.
- **1 team không thể có 2 award**.

### Quy tắc gán award

- Tiêu chí: lấy LevelAward trước, NumberOfAward sau.
- Ví dụ: LevelAward=1, NumberOfAward=3 → 3 team top đầu nhận giải nhất.
- **LevelAward**: thứ tự giải (1 = nhất, 2 = nhì, ...).

### Admin/Staff: LeaderBoard

- **ĐƯỢC**: Recalculate, Lock, Publish.
- **ĐƯỢC**: Assign award cho team leaderboard.
- API:
  - `POST /api/v1/admin/events/{eventId}/leaderboard/recalculate` (StaffOrAdmin)
  - `PATCH /api/v1/admin/events/{eventId}/leaderboard/lock` (StaffOrAdmin)
  - `PATCH /api/v1/admin/events/{eventId}/leaderboard/publish` (StaffOrAdmin)
  - `PATCH /api/v1/admin/leaderboards/{leaderBoardId}/details/{teamId}` — assign award (StaffOrAdmin)

### Admin: Awards

- **ĐƯỢC**: CRUD awards.
- API:
  - `POST /api/v1/admin/events/{eventId}/awards` — tạo
  - `PATCH /api/v1/admin/awards/{id}` — sửa
  - `DELETE /api/v1/admin/awards/{id}` — xoá

### Public: LeaderBoard & Award

- **ĐƯỢC**: Xem leaderboard, awards, yearly leaderboard.
- API:
  - `GET /api/v1/events/{eventId}/leaderboard`
  - `GET /api/v1/events/{eventId}/awards`
  - `GET /api/v1/leaderboards/year/{year}` — yearly (không cần auth)

---

## 11. Team & Invitation

### Entity: Teams

```
Id, Name, CanEdit,
IsDisable, CreatedAt, UpdatedAt
```

### Entity: TeamDetails

```
Id, TeamId, UserId, IsLeader, Status (TeamDetailStatusEnum),
IsDisable, CreatedAt, UpdatedAt
```

- **IsLeader**: leader của team.
- **Status**: Active / Inactive.

### Entity: Invitations

```
Id, TeamId, UserId, LimitTime, Status (InvitationStatusEnum), Description,
IsDisable, CreatedAt, UpdatedAt
```

### Quy tắc chung

- Chỉ **Student** mới tạo team, invite, join.
- **Tạo team**: cần profile completed (Email, HashPassword, FirstName, LastName, PhoneNumber, StudentId, College).
- **Invitation**: leader gửi → user nhận → accept/reject.
- Team có thể bị **lock** (`CanEdit = false`) → không thay đổi thành viên.
- Team có thể bị **disable** (`IsDisable = true`) → không tham gia được.

### Student: Team

- **ĐƯỢC**: Tạo, update, xoá member, chuyển leader, rời team.
- **ĐƯỢC**: Mời thành viên, xem teams của mình.
- **ĐƯỢC**: Appeal round/submission.
- API: (chi tiết trong controller TeamController.cs)

### Admin: Team

- **ĐƯỢC**: Xem tất cả teams (kể cả IsDisable).
- **ĐƯỢC**: Disable/Enable team.
- API:
  - `GET /api/v1/admin/teams`
  - `PATCH /api/v1/admin/teams/{teamId}/disable`
  - `PATCH /api/v1/admin/teams/{teamId}/enable`

### Staff: Team

- **ĐƯỢC**: Lock/Unlock team.
- API:
  - `PATCH /api/v1/teams/{teamId}/lock` (StaffOrAdmin)
  - `PATCH /api/v1/teams/{teamId}/unlock` (StaffOrAdmin)

### Invitation (chung — cần auth)

- `GET /api/v1/invitations/me`
- `GET /api/v1/invitations/pending/count`
- `POST /api/v1/invitations/{invitationId}/accept`
- `POST /api/v1/invitations/{invitationId}/reject`

---

## 12. Mentor

Mentor là Lecturer được phân công với `EventRole = Mentor`.

### Quyền

- **ĐƯỢC**: Xem event đã assign làm Mentor.
- **ĐƯỢC**: Xem tracks được phân công.
- **ĐƯỢC**: Xem teams trong track (kèm leader, topic, member count).
- **ĐƯỢC**: Gửi notification (toàn track hoặc 1 team).
- **KHÔNG ĐƯỢC**: Chấm điểm (đó là việc của Judge).
- **KHÔNG ĐƯỢC**: Xem event/track chưa assign.
- API:
  - `GET /api/v1/mentor/events`
  - `GET /api/v1/mentor/tracks`
  - `GET /api/v1/mentor/tracks/{trackId}/teams`
  - `POST /api/v1/mentor/tracks/{trackId}/notifications`
  - `POST /api/v1/mentor/teams/{teamId}/notifications`

### Controller lưu ý

- `MentorsController` chỉ có `[Authorize]` (không policy cụ thể) — bảo mật yếu.

---

## 13. Judge

Judge là Lecturer được phân công với `EventRole = Judge`.

### Quyền

- **ĐƯỢC**: Xem tracks được phân công (kèm submission count, graded count).
- **ĐƯỢC**: Xem, search submissions của track/event/round.
- **ĐƯỢC**: Xem criteria của submission.
- **ĐƯỢC**: Chấm điểm (submit, update, finalize, retake).
- **ĐƯỢC**: Chấm mock.
- **KHÔNG ĐƯỢC**: Chấm submission không được assign.

### API (JudgeController — LecturerPolicy)

- Tracks: `GET /api/v1/judge/tracks`
- Submissions: `GET .../tracks/{trackId}/submissions`, `.../events/{eventId}/submissions`, `.../rounds/{roundId}/submissions`
- Chấm điểm: `POST/PATCH .../submissions/{submissionId}/scores`, `PATCH .../scores/{scoreId}`, `POST .../scores/{scoreId}/finalize`, `.../retake`
- Regrade: `GET .../submissions/regrade`
- Teams: `GET .../events/{eventId}/teams`, `.../events/{eventId}/rounds/{roundId}`

---

## 14. Staff

### Quyền

- **ĐƯỢC**: Quản lý RegisterTeams (duyệt, từ chối, ban, unban).
- **ĐƯỢC**: Quản lý AssignEvents (phân công Lecturer vào event/track).
- **ĐƯỢC**: Quản lý AssignTracks.
- **ĐƯỢC**: Xem rounds, submissions, assign judges.
- **ĐƯỢC**: Gán track/topic cho team.
- **ĐƯỢC**: Quản lý report (xem, update status, approve regrade).
- **ĐƯỢC**: Đổi role user (StaffPolicy).
- **KHÔNG ĐƯỢC**: Tự assign Staff khác (chỉ Admin).
- **KHÔNG ĐƯỢC**: Làm việc với event chưa được assign.
- **KHÔNG ĐƯỢC**: Thấy event Draft/IsDisable=true.
- **KHÔNG ĐƯỢC**: Thấy Staff assignments của nhau (chỉ thấy Lecturer).

### API (Staff.cs — StaffOrAdminPolicy)

- Events: `GET /api/v1/staff/events`, `.../events/search`, `.../events/current`
- Tracks: `GET .../events/{eventId}/tracks` (StaffPolicy), `.../tracks/{trackId}/topics` (StaffPolicy)
- Assignments: `GET .../events/{eventId}/assignments`, `.../lecturers/available`, `POST .../assign-lecturers`
- Teams: `GET .../events/{eventId}/teams`, `PATCH .../teams/{teamId}/track`, `.../topic`
- Submissions: `GET .../rounds/{roundId}/submissions`, `POST .../assign-judges`
- Reports: `GET .../reports`, `.../reports/{reportId}`, `PATCH .../reports/{reportId}/status`, `POST .../reports/{reportId}/regrade`
- Regrade: `GET .../submissions/regrade`
- User: `PATCH .../users/{userId}/role` (StaffPolicy)

---

## 15. Lecturer

Lecturer là role cha của Mentor và Judge.

### Quyền

- **ĐƯỢC**: Xem event + tracks đã assign (bất kể Mentor hay Judge).
- **ĐƯỢC**: Xem submissions của round (nếu assign).
- **KHÔNG ĐƯỢC**: Xem event chưa assign, Draft, IsDisable=true.

### API (LecturersController — LecturerPolicy)

- `GET /api/v1/lecturers/events`
- `GET /api/v1/lecturers/events/search`
- `GET /api/v1/lecturers/events/current`
- `GET /api/v1/lecturers/events/{eventId}/tracks`
- `GET /api/v1/lecturers/rounds/{roundId}/submissions`

---

## 16. Student

### Quyền

- **ĐƯỢC**: Xem event Published/Closed (IsDisable=false).
- **ĐƯỢC**: Đăng ký team vào event.
- **ĐƯỢC**: Tạo và quản lý team.
- **ĐƯỢC**: Nhận và xử lý invitation.
- **ĐƯỢC**: Nộp bài cho round.
- **ĐƯỢC**: Xem điểm, ranking.
- **ĐƯỢC**: Appeal round/submission.
- **KHÔNG ĐƯỢC**: Xem event Draft/IsDisable=true.
- **KHÔNG ĐƯỢC**: Làm tác vụ staff/admin.

### API (cần StudentPolicy)

- `GET /api/v1/teams/me`, `POST /api/v1/teams`, `POST .../invitations`, `PUT ...`, `DELETE .../members`, `PUT .../leader`, `POST .../leave`, `POST .../appeal`

---

## 17. Admin

### Quyền — TỔNG QUAN

- **Toàn quyền** trên hệ thống.
- **Bypass mọi IsDisable** (xem cả dữ liệu đã xoá mềm).
- **Bypass mọi EnsureStaffAssignedToEvent**.
- **Có thể làm mọi thứ Staff làm + quyền đặc biệt**.

### Quyền RIÊNG (chỉ Admin)

- CRUD events (tạo, sửa, xoá, publish, unpublish, close, restore).
- CRUD rounds (tạo, sửa, xoá, restore, endRoundFinal).
- CRUD tracks (tạo, sửa, xoá, update visibility).
- CRUD criteria templates.
- **Phân công Staff vào event** (Staff không tự làm được).
- Xem danh sách Staff available, xoá Staff assignment.
- CRUD awards.
- Xem danh sách users, search users.
- Disable/Enable teams.
- Gửi system notification.
- Đổi role user.

### Quyền DÙNG CHUNG với Staff (StaffOrAdminPolicy)

- Leaderboard: recalculate, lock, publish, assign award.
- Gán track cho event (AssignEventToTrack).
- CRUD topics.
- End round.
- Lock/unlock team.

---

## 18. Auth & User

### AuthController — KHÔNG cần token

| API                                            | Mô tả                       |
| ---------------------------------------------- | --------------------------- |
| `POST /api/v1/auth/register`                   | Đăng ký                     |
| `POST /api/v1/auth/login`                      | Đăng nhập (token + cookies) |
| `POST /api/v1/auth/tokens/refresh`             | Refresh token               |
| `POST /api/v1/auth/email-verifications`        | Xác thực email              |
| `POST /api/v1/auth/email-verifications/resend` | Gửi lại mã                  |
| `POST /api/v1/auth/forgot-password`            | Quên mật khẩu               |
| `POST /api/v1/auth/reset-password`             | Reset mật khẩu              |
| `POST /api/v1/auth/logout`                     | Đăng xuất                   |

### AuthController — CẦN token

| API                                  | Mô tả                   |
| ------------------------------------ | ----------------------- |
| `GET /api/v1/auth/me`                | Thông tin user hiện tại |
| `PATCH /api/v1/auth/change-password` | Đổi mật khẩu            |

### UserController — CẦN token

| API                                | Mô tả               |
| ---------------------------------- | ------------------- |
| `GET /api/v1/users/{userId}`       | User detail         |
| `GET /api/v1/users/students`       | Search students     |
| `GET /api/v1/users/profile`        | Profile của tôi     |
| `PATCH /api/v1/users/profile`      | Update profile      |
| `POST /api/v1/users/system-report` | Tạo report          |
| `GET /api/v1/me/assignments`       | Assignments của tôi |
| `GET /api/v1/users/reports/me`     | Reports của tôi     |
| `PATCH /api/v1/users/me/avatar`    | Update avatar       |

---

## 19. Notification

### Entity: Notifications

```
Id, UserId (nullable), TeamId (nullable),
Title, Status, Description,
TargetType (NotificationTargetTypeEnum),
CreatedAt, UpdatedAt
```

### API (cần token)

- `GET /api/v1/notifications/me` — thông báo của tôi
- `PATCH /api/v1/notifications/{notificationId}/read` — đã đọc
- `GET /api/v1/notifications/me/unread-count` — số chưa đọc
- `PATCH /api/v1/notifications/read-all` — đọc tất cả
- `PATCH /api/v1/notifications/all/disable` — tắt tất cả

### Admin: System Notification

- `POST /api/v1/admin/notifications` (AdminPolicy)

---

## 20. System & File

### SystemController — public

- `GET /api/v1/enums` — danh sách enum
- `GET /api/v1/health` — health check
- `GET /api/v1/version` — version

### SystemController — cần auth

- `POST /api/v1/files/upload` — upload file

### RolesController — public

- `GET /api/v1/roles` — danh sách roles
- `GET /api/v1/roles/event-roles` — danh sách event roles

---

## 21. Report

### Entity: Reports

```
Id, UserId, AssignEventId (nullable), SubmissionId (nullable),
Title, Description, ImgUrl, FileUrl,
Status (ReportStatusEnum), Reason, TypeReport,
CreatedAt, UpdatedAt
```

### Quy tắc

- User tạo report (khiếu nại, báo cáo).
- **Status**: Open → Closed (Staff update), Approved (khi duyệt regrade).
- **TypeReport = "Phúc khảo"** (appeal) → Staff có thể approve regrade.
- **Approve regrade** chỉ được khi:
  - Report có TypeReport = "Phúc khảo" (case-insensitive)
  - Report must be Open
  - Submission chưa IsRegrade
  - Submission đã có score (không phải NotGraded)
- Khi approve regrade: set `Submission.IsRegrade = true`, báo cho judge chấm lại.

### API

- `POST /api/v1/users/system-report` — tạo report (user)
- `GET /api/v1/users/reports/me`, `GET /api/v1/users/reports/{reportId}` — xem
- `GET /api/v1/staff/reports`, `GET /api/v1/staff/reports/{reportId}` — staff xem
- `PATCH /api/v1/staff/reports/{reportId}/status` — update status
- `POST /api/v1/staff/reports/{reportId}/regrade` — approve regrade

---

## Phụ lục: Policy mapping

| Policy                       | Roles được phép        | Mục đích                    |
| ---------------------------- | ---------------------- | --------------------------- |
| _(không)_                    | Tất cả (public)        | API công khai               |
| `[Authorize]`                | User đã login          | Cần token                   |
| `AdminPolicy`                | Admin                  | Quyền cao nhất              |
| `StaffPolicy`                | Staff                  | Nhân viên                   |
| `StudentPolicy`              | Student                | Sinh viên                   |
| `LecturerPolicy`             | Lecturer               | Giảng viên (Mentor + Judge) |
| `StaffOrAdminPolicy`         | Staff, Admin           | Staff + Admin               |
| `StaffLecturerOrAdminPolicy` | Staff, Lecturer, Admin | Nhân viên + GV + Admin      |

### Lưu ý bảo mật

- **MentorsController**: chỉ `[Authorize]` — bất kỳ user login cũng dùng được.
- **SubmissionsController**: chỉ `[Authorize]` — cần thêm policy.
- **RoundsController**: nhiều endpoint public (GET rounds, GET round detail, GET ranking).
- **RegisterTeamController**: `POST /api/v1/register-teams` không có policy — student only trong service check thủ công.

### Hằng số Policy (JwtExtensions)

```csharp
AdminPolicy = "AdminPolicy"
StaffPolicy = "StaffPolicy"
LecturerPolicy = "LecturerPolicy"
StudentPolicy = "StudentPolicy"
StaffOrAdminPolicy = "StaffOrAdminPolicy"
StaffLecturerOrAdminPolicy = "StaffLecturerOrAdminPolicy"
```
