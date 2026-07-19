# Admin — Nghiệp vụ (Business Rules)

> Chỉ ghi các check **nghiệp vụ** (BadRequest, Conflict, Forbidden).  
> Không ghi: check role, check tồn tại (NotFound), pagination, enum parse, check read-only.

---

## 1. AdminEventController

### POST /api/v1/admin/events — `CreateEvent`
- **EndTime > StartTime** — event phải có thời gian kết thúc sau thời gian bắt đầu

### PATCH /api/v1/admin/events/{eventId} — `UpdateEvent`
- **Không cho update event Closed** — chỉ cho phép sửa IsDisable (mở/ẩn)
- **Status transitions:**
  - ❌ Published → Draft (ko hạ)
  - ❌ Draft → Closed (ko nhảy cóc — phải Publish trước)
  - Draft → Published: **bắt buộc setup đủ** (gọi IsEventSetupComplete)
- **EndTime > StartTime** — nếu có cập nhật thời gian
- **IsDisable = false của Draft** → phải setup đủ mới show
- **Tự động tạo LeaderBoard** nếu event chưa có và đã có StartTime

### POST /api/v1/admin/events/{eventId}/delete — `DeleteEvent`
- Ko delete event đã delete (IsDisable)

### POST /api/v1/admin/events/{eventId}/restore — `RestoreEvent`
- Ko restore event chưa delete
- Tự động tạo/tìm LeaderBoard

---

## 2. AdminRoundController

### POST /api/v1/admin/rounds — `CreateRound`
- **Duplicate name** trong cùng event
- **EndTime > StartTime**
- **Round time phải nằm trong event time** (StartTime >= event.StartTime, EndTime <= event.EndTime)
- **Round no ≥ 2: StartTime phải >= EndTime của round trước** (previous round)
- Auto-calculate RoundNo (max + 1)
- Cập nhật NumberRound của event (+1)

### PATCH /api/v1/admin/rounds/{roundId} — `UpdateRound`
- **Bắt buộc có StartTime + EndTime** (khi update)
- **EndTime > StartTime**
- **Round time phải nằm trong event time**
- **Previous round: StartTime >= EndTime của round trước**
- Re-validate các field sau update

### POST /api/v1/admin/rounds/{roundId}/swap — `SwapRound`
- **TargetRoundNo > 0**
- **Ko swap round đã delete** (IsDisable == true hoặc RoundNo == 0)
- **Ko swap với chính nó**
- Swap RoundNo, StartTime, EndTime, StartSubmission, EndSubmission giữa 2 round

### POST /api/v1/admin/rounds/{roundId}/delete — `DeleteRound`
- Set RoundNo = 0, IsDisable = true
- **Giảm NumberRound của event** (không âm)
- **Các round có RoundNo > round bị xóa → giảm RoundNo đi 1**

### POST /api/v1/admin/rounds/{roundId}/restore — `RestoreRound`
- **Chỉ restore round đã delete** (IsDisable == true)
- RoundNo mới = max roundNo hiện tại + 1
- **Xóa toàn bộ thời gian** (StartTime, EndTime, StartSubmission, EndSubmission = null)
- Cộng lại NumberRound của event

### POST /api/v1/admin/rounds/{roundId}/end — `EndRound`
- **Set EndTime = hiện tại**
- **Set EndSubmission = hiện tại**

---

## 3. AdminRegisterTeamController

### POST /api/v1/admin/register-teams/{registerTeamId}/approve — `ApproveRegisterTeam`
- **Chỉ approve Pending**
- **Round 1 full check** — nếu LimitTeam != null && số team >= LimitTeam → ko approve
- **Conflict check** — thành viên team đã được duyệt ở team khác trong cùng event? → ko approve
- **Lock team** — CanEdit = false (ko thể sửa member)
- **Tự động add team vào Round đầu tiên** (tạo RoundDetail)

### POST /api/v1/admin/register-teams/{registerTeamId}/reject — `RejectRegisterTeam`
- **Chỉ reject Pending**
- **Unlock team** — CanEdit = true

### POST /api/v1/admin/register-teams/{registerTeamId}/assign-next-round — `AssignToNextRound`
- **Ko up round nếu đang ở round cuối** (ko có roundNo + 1)
- **Ko gán trùng** — team đã có round detail cho round này
- **Round full check** — LimitTeam của round tiếp theo

### POST /api/v1/admin/register-teams/{registerTeamId}/revert-previous-round — `RevertToPreviousRound`
- **Cần ≥ 2 round** mới down được
- **Ko revert nếu round hiện tại đã có submission** — phải xóa submission trước
- **Hard delete** round detail

### POST /api/v1/admin/register-teams/{registerTeamId}/ban — `BanRegisterTeam`
- **Chỉ ban Approved** (ko ban Pending/Rejected)
- **Ko ban lại** (nếu IsBanned == true)
- Set IsBanned = true, Status = Banned

### POST /api/v1/admin/register-teams/{registerTeamId}/unban — `UnbanRegisterTeam`
- **Chỉ unban khi đang bị ban** (IsBanned == true)
- Revert Status = Approved, xóa RejectionReason

### POST /api/v1/admin/register-teams/{registerTeamId}/assign-track-topic — `AssignTrackTopic`
- **Track phải thuộc cùng event**
- **Nếu có Topic → Topic phải thuộc Track đó**
- Ghi đè TrackId/TopicId

### POST /api/v1/admin/register-teams/{registerTeamId}/remove-track-topic — `RemoveTrackTopic`
- Set TrackId = null, TopicId = null

### PATCH /api/v1/admin/register-teams/{registerTeamId} — `UpdateRegisterTeam`
- Parse status enum

---

## 4. AdminAwardController

### POST /api/v1/admin/awards — `CreateAward`
- **Duplicate name** trong cùng event
- **Auto-calculate LevelAward**: nếu chưa có level 1 → level = 1, nếu có → level = max + 1

### PATCH /api/v1/admin/awards/{awardId} — `UpdateAward`
- *(chỉ update field cơ bản, ko business rule đặc biệt)*

### POST /api/v1/admin/awards/{awardId}/swap — `SwapAwardLevel`
- **TargetLevel > 0**
- **Ko swap award đã delete** (IsDisable || LevelAward == 0)
- **Ko swap với chính nó** (cùng level)
- **Target level phải tồn tại** trong event
- Swap LevelAward giữa 2 award

### POST /api/v1/admin/awards/{awardId}/delete — `DeleteAward`
- Set LevelAward = 0, IsDisable = true
- **Các award có LevelAward > level bị xóa → giảm 1**

### POST /api/v1/admin/awards/{awardId}/restore — `RestoreAward`
- LevelAward mới = max level hiện tại + 1

---

## 5. AdminTrackController

### POST /api/v1/admin/tracks — `CreateTrack`
- **Duplicate title** trong cùng event

### PATCH /api/v1/admin/tracks/{trackId} — `UpdateTrack`
- *(no business rule)*

### POST /api/v1/admin/tracks/{trackId}/delete — `DeleteTrack`
- IsDisable = true (xóa mềm)

### POST /api/v1/admin/tracks/{trackId}/restore — `RestoreTrack`
- IsDisable = false

---

## 6. AdminTopicController

### POST /api/v1/admin/topics — `CreateTopic`
- **Duplicate title** trong cùng track

### PATCH /api/v1/admin/topics/{topicId} — `UpdateTopic`
- *(no business rule)*

### POST /api/v1/admin/topics/{topicId}/delete — `DeleteTopic`
- Ko delete topic đã delete

### POST /api/v1/admin/topics/{topicId}/restore — `RestoreTopic`
- Ko restore topic chưa delete

---

## 7. AdminUserController

### POST /api/v1/admin/users — `CreateUser`
- **Duplicate email** (check trùng)

### PATCH /api/v1/admin/users/{userId} — `UpdateUser`
- *(update các field thông thường, ko đặc biệt)*

### POST /api/v1/admin/users/{userId}/ban — `BanUser`
- Set BanReason, BannedAt, Status = Banned
- (IsDisable ko đổi — ban ko ẩn user)

### POST /api/v1/admin/users/{userId}/unban — `UnbanUser`
- Xóa BanReason, BannedAt, Status = Active

### POST /api/v1/admin/users/{userId}/delete — `DeleteUser`
- IsDisable = true

### POST /api/v1/admin/users/{userId}/restore — `RestoreUser`
- IsDisable = false

---

## 8. AdminTeamController

### PATCH /api/v1/admin/teams/{teamId} — `UpdateTeam`
- *(no business rule)*

### POST /api/v1/admin/teams/{teamId}/delete — `DeleteTeam`
- Ko delete team đã disable

### POST /api/v1/admin/teams/{teamId}/restore — `RestoreTeam`
- IsDisable = false

### POST /api/v1/admin/teams/{teamId}/change-leader — `ChangeLeader`
- **Member mới phải thuộc team**
- **Ko chuyển leader cho inactive/disabled member**
- **Ko chuyển cho chính người đang là leader**
- **Phải có leader hiện tại trong team**
- Cập nhật IsLeader cho cả cũ và mới

### POST /api/v1/admin/teams/{teamId}/lock — `LockTeam`
- Ko lock team đã lock (CanEdit == false)

### POST /api/v1/admin/teams/{teamId}/unlock — `UnlockTeam`
- Ko unlock team đã unlock (CanEdit == true)

---

## 9. AdminInvitationController

### GET /api/v1/admin/teams/{teamId}/invitations — `GetInvitations`
- *(chỉ đọc, không business rule)*

---

## 10. AdminNotificationController

### POST /api/v1/admin/notifications — `CreateNotification`
- **System**: ko cần UserId/TeamId
- **Team**: yêu cầu TeamId, bỏ qua UserId
- **Personal**: yêu cầu UserId

### POST /api/v1/admin/notifications/{notificationId}/delete — `DeleteNotification`
- Ko delete notification đã disable

### POST /api/v1/admin/notifications/{notificationId}/restore — `RestoreNotification`
- IsDisable = false

### PATCH /api/v1/admin/notifications/{notificationId} — `UpdateNotification`
- *(no business rule)*

### POST /api/v1/admin/my-notifications/{notificationId}/read — `ReadNotification`
- **Chỉ đọc được của mình hoặc System**
- Nếu đã Read rồi thì skip (ko save lại)

### POST /api/v1/admin/my-notifications/read-all — `ReadAllNotifications`
- Đọc tất cả Unread của admin đó

---

## 11. AdminAssignController

### POST /api/v1/admin/assign/user-to-event — `AssignUserToEvent`
- **Ko assign user đã disable**
- **Ko assign user đã ban**
- **Ko assign Student** (chỉ Staff/Lecturer)
- **Ko assign Admin**
- **Staff chỉ được role Staff**
- **Lecturer ko được role Staff** (chỉ Judge/Mentor)
- **Nếu assign đã tồn tại + IsDisable → re-enable + update role**
- **Nếu assign đã tồn tại + !IsDisable → Conflict**

### POST /api/v1/admin/assign/change-lecturer-role — `AssignEventRoleToLecturer`
- **Chỉ dành cho Lecturer**
- **Ko assign Staff role cho Lecturer**
- Parse role

### POST /api/v1/admin/assign/{assignEventId}/assign-track — `AssignTrackToEvent`
- **Staff ko được assign track**
- **Track phải thuộc cùng event**
- **Ko assign track đã được assign (Conflict)**

### POST /api/v1/admin/assign/{assignEventId}/remove-track/{trackId} — `RemoveTrackFromEvent`
- Set IsDisable = true

### POST /api/v1/admin/assign/{assignEventId}/restore-track/{trackId} — `RestoreTrackToEvent`
- Set IsDisable = false
- Ko restore track đã active

### POST /api/v1/admin/assign/{assignEventId}/remove — `RemoveAssignEvent`
- Set IsDisable + disable tất cả tracks của assign đó
- Ko remove assign đã remove

### POST /api/v1/admin/assign/{assignEventId}/restore — `RestoreAssignEvent`
- Set IsDisable = false + restore tất cả tracks
- Ko restore assign đã active

---

## 12. AdminReportController

### PATCH /api/v1/admin/reports/{reportId} — `UpdateReportStatus`
- Parse status

---

## 13. AdminScoreController

*(Read-only — không có business mutation)*

---

## 14. AdminSubmissionController

*(Read-only — không có business mutation)*

---

## 15. AdminLeaderboardController

### POST /api/v1/admin/leaderboard/chapter/{year}/publish — `PublishChapter`
- *(delegate helper)*

### POST /api/v1/admin/leaderboard/chapter/{year}/hide — `HideChapter`
- *(delegate helper)*

### POST /api/v1/admin/leaderboard/events/{eventId}/publish — `PublishEvent`
- *(delegate helper)*

### POST /api/v1/admin/leaderboard/events/{eventId}/hide — `HideEvent`
- *(delegate helper)*

---

## 16. AdminCriteriaTemplateController

### POST /api/v1/admin/criteria-templates — `CreateCriteriaTemplate`
- *(tạo template + items — ko có check đặc biệt)*

### PATCH /api/v1/admin/criteria-templates/{templateId} — `UpdateCriteriaTemplate`
- *(no business rule)*

### POST /api/v1/admin/criteria-templates/{templateId}/activate — `ActivateCriteriaTemplate`
- **Ko activate template đã delete**
- **Chỉ 1 template active/round** — tất cả template khác trong round bị deactivate
- Ko activate template đã active

### POST /api/v1/admin/criteria-templates/{templateId}/delete — `DeleteCriteriaTemplate`
- **Ko delete template đang active** — phải deactivate (activate template khác) trước

### POST /api/v1/admin/criteria-templates/{templateId}/restore — `RestoreCriteriaTemplate`
- IsDisable = false

### POST /api/v1/admin/criteria-templates/{templateId}/items — `CreateCriteriaItem`
- *(no business rule)*

### PATCH /api/v1/admin/criteria-templates/items/{itemId} — `UpdateCriteriaItem`
- *(no business rule)*

### POST /api/v1/admin/criteria-templates/items/{itemId}/delete — `DeleteCriteriaItem`
- IsDisable = true

### POST /api/v1/admin/criteria-templates/items/{itemId}/restore — `RestoreCriteriaItem`
- IsDisable = false

---

# Staff — Nghiệp vụ (Business Rules)

> Staff có quyền **giới hạn hơn Admin**:
> - Phải được **assign vào event** mới thao tác được (mọi mutation đều check `EnsureStaffAssignedToEvent`)
> - Chỉ thao tác trên **Lecturer** (ko được assign/remove Staff)
> - Các API RegisterTeam giống hệt Admin nhưng có check staff assignment

---

## 1. StaffEventController

### GET /api/v1/staff/my-events — `GetMyEvents`
- *(chỉ đọc events được assign — ko business rule)*

### GET /api/v1/staff/my-staff-events — `GetMyStaffEvents`
- *(chỉ đọc events được assign — ko business rule)*

### GET /api/v1/staff/my-current-events — `GetMyCurrentEvents`
- *(chỉ đọc events được assign — ko business rule)*

### GET /api/v1/staff/events/{eventId} — `GetMyEventDetail`
- *(chỉ đọc — ko business rule)*

---

## 2. StaffRoundController

*(Read-only — chỉ đọc round)*

---

## 3. StaffRegisterTeamController

> Giống Admin RegisterTeam, nhưng có **thêm check staff assignment**.  
> Xem chi tiết ở mục 3 Admin để biết business rules đầy đủ.

### POST /api/v1/staff/register-teams/{registerTeamId}/approve — `ApproveRegisterTeam`
- **Chỉ approve Pending**
- **Round 1 full check**
- **Conflict check** — thành viên team đã được duyệt ở team khác trong cùng event?
- **Lock team** — CanEdit = false

### POST /api/v1/staff/register-teams/{registerTeamId}/reject — `RejectRegisterTeam`
- **Chỉ reject Pending**
- **Unlock team** — CanEdit = true

### POST /api/v1/staff/register-teams/{registerTeamId}/assign-next-round — `AssignToNextRound`
- **Ko up round cuối**
- **Ko gán trùng**
- **Round full check**

### POST /api/v1/staff/register-teams/{registerTeamId}/revert-previous-round — `RevertToPreviousRound`
- **Cần ≥ 2 round**
- **Ko revert nếu round hiện tại có submission**

### POST /api/v1/staff/register-teams/{registerTeamId}/ban — `BanRegisterTeam`
- **Chỉ ban Approved**
- **Ko ban lại**

### POST /api/v1/staff/register-teams/{registerTeamId}/unban — `UnbanRegisterTeam`
- **Chỉ unban khi đang bị ban**

### POST /api/v1/staff/register-teams/{registerTeamId}/assign-track-topic — `AssignTrackTopic`
- **Track phải thuộc cùng event**
- **Topic phải thuộc Track đó**

### POST /api/v1/staff/register-teams/{registerTeamId}/remove-track-topic — `RemoveTrackTopic`
- Set TrackId = null, TopicId = null

### PATCH /api/v1/staff/register-teams/{registerTeamId} — `UpdateRegisterTeam`
- **Staff KO được sửa IsBanned, IsDisable** — chỉ Admin

---

## 4. StaffAwardController

*(Read-only — chỉ đọc award)*

---

## 5. StaffTrackController

*(Read-only — chỉ đọc track)*

---

## 6. StaffTopicController

*(Read-only — chỉ đọc topic)*

---

## 7. StaffUserController

*(Read-only — chỉ xem user)*

---

## 8. StaffTeamController

*(Read-only — chỉ xem team)*

---

## 9. StaffNotificationController

### POST /api/v1/staff/notifications — `CreateNotification`
- **System**: ko cần UserId/TeamId
- **Team**: yêu cầu TeamId
- **Personal**: yêu cầu UserId

### POST /api/v1/staff/notifications/{notificationId}/delete — `DeleteNotification`
- Ko delete notification đã disable

### POST /api/v1/staff/notifications/{notificationId}/restore — `RestoreNotification`
- IsDisable = false

### PATCH /api/v1/staff/notifications/{notificationId} — `UpdateNotification`
- *(no business rule)*

### POST /api/v1/staff/my-notifications/{notificationId}/read — `ReadMyNotification`
- **Chỉ đọc được của mình hoặc System**

### POST /api/v1/staff/my-notifications/read-all — `ReadAllMyNotifications`
- Đọc tất cả Unread của staff đó

---

## 10. StaffAssignController

### POST /api/v1/staff/assign/lecturer-to-event/{eventId} — `AssignLecturerToEvent`
- **Ko assign user đã disable**
- **Ko assign user đã ban**
- **Chỉ assign Lecturer** (ko assign Staff)
- **Staff chỉ được role Judge/Mentor** — ko được assign Staff role
- **Nếu assign đã tồn tại + IsDisable → re-enable + update role**
- **Nếu assign đã tồn tại + !IsDisable → Conflict**

### GET /api/v1/staff/assign/events/{eventId}/users — `GetAssignedUsers`
- **Filter IsDisable = false** (chỉ hiện active)

### POST /api/v1/staff/assign/{assignEventId}/remove — `RemoveAssignEvent`
- **Chỉ remove Lecturer** (ko remove Staff)
- Có check staff assignment
- Ko remove assign đã remove
- Cascade disable tất cả tracks

### POST /api/v1/staff/assign/{assignEventId}/restore — `RestoreAssignEvent`
- **Chỉ restore Lecturer** (ko restore Staff)
- Ko restore assign đã active
- Cascade restore tất cả tracks

### POST /api/v1/staff/assign/{assignEventId}/assign-track/{trackId} — `AssignTrackToEvent`
- **Chỉ assign track cho Lecturer** (ko cho Staff)
- Track phải thuộc cùng event
- **Ko assign track đã được assign (Conflict)**

### POST /api/v1/staff/assign/{assignEventId}/remove-track/{trackId} — `RemoveTrackFromEvent`
- **Chỉ modify Lecturer's tracks**
- Set IsDisable = true

### POST /api/v1/staff/assign/{assignEventId}/restore-track/{trackId} — `RestoreTrackToEvent`
- **Chỉ modify Lecturer's tracks**
- Ko restore track đã active

### GET /api/v1/staff/assign/users/{userId}/events — `GetUserAssignEvents`
- **Staff phải có ít nhất 1 event assignment** mới dùng được
- **User phải là Staff hoặc Lecturer**

---

## 11. StaffReportController

### PATCH /api/v1/staff/reports/{reportId} — `UpdateReportStatus`
- Parse status
- Gửi notification cho người gửi report

---

## 12. StaffScoreController

*(Read-only — chỉ đọc score)*

---

## 13. StaffSubmissionController

*(Read-only — chỉ đọc submission, có check staff assignment)*

---

## 14. StaffLeaderboardController

*(Delegate helper — giống Admin)*

---

## 15. StaffCriteriaTemplateController

*(Giống Admin CriteriaTemplate)*

---

# Student — Nghiệp vụ (Business Rules)

> Student tự quản lý team, invitation, đăng ký event, submission, report.
> Các API đọc (Event, Round, Track, Topic, Award, Leaderboard, User) — không có business rule.

---

## 1. StudentTeamController

### POST /api/v1/student/teams — `CreateTeam`
- **Duplicate team name** (ko trùng tên)
- **Phải có profile hoàn chỉnh** — gọi `StudentProfileHelper.ValidateProfile` (check StudentId, PhoneNumber, Address...)

### PATCH /api/v1/student/teams/{teamId} — `UpdateTeam`
- **Team ko bị disable**
- **Team đang editable** (CanEdit == true)
- **Chỉ leader mới được update team**
- Chỉ sửa được Name

### POST /api/v1/student/teams/{teamId}/disband — `DisbandTeam`
- **Chỉ leader mới disband được**
- **Ko disband nếu team đang tham gia event hoạt động** (Event.StartTime ≤ now ≤ Event.EndTime)
- Disable tất cả member + team

### POST /api/v1/student/teams/{teamId}/members/{memberId}/kick — `KickMember`
- **Team editable** (CanEdit == true)
- **Chỉ leader kick được**
- **Ko kick chính mình**
- **Ko kick leader** (chỉ member thường)
- **Ko kick inactive/disabled member**
- **Xóa cứng TeamDetails** (không IsDisable, xóa khỏi DB)

### POST /api/v1/student/teams/{teamId}/change-leader — `ChangeLeader`
- **Ko chuyển cho chính mình**
- **Chỉ leader mới chuyển được**
- **Member mới phải thuộc team**
- **Ko chuyển cho inactive/disabled member**
- Cập nhật IsLeader cho cả cũ và mới

### POST /api/v1/student/teams/{teamId}/leave — `LeaveTeam`
- **Team editable** (CanEdit == true)
- **Leader ko leave được** — phải disband hoặc change leader trước
- Set IsDisable = true, Status = Inactive

---

## 2. StudentInvitationController

### POST /api/v1/student/teams/{teamId}/invitations — `SendInvitation`
- **Team editable** (CanEdit == true)
- **Chỉ leader mới gửi invitation**
- **Ko invite disabled user**
- **Người nhận ko được là member rồi**
- **Ko gửi trùng** — đã có Pending invitation cho user đó trong team này
- LimitTime mặc định = now + 15 ngày

### POST /api/v1/student/invitations/{invitationId}/accept — `AcceptInvitation`
- **Phải có profile hoàn chỉnh**
- **Chỉ người được mời mới accept được**
- **Chỉ accept Pending**
- **Check limit time** — nếu hết hạn → tự động set Expired
- **Team editable** (CanEdit == true)
- **User chưa là member của team đó**
- Tự động add team member (IsLeader = false, Status = Active)

### POST /api/v1/student/invitations/{invitationId}/reject — `RejectInvitation`
- **Chỉ người được mời mới reject được**
- **Chỉ reject Pending**

---

## 3. StudentTrackTopicRegisterTeamController

### POST /api/v1/student/register-teams — `CreateRegisterTeam`
- **Chỉ leader mới register được**
- **Ko register vào Draft/Closed event**
- **Số lượng active member phải trong khoảng [MinMember, MaxMember]** của event
- **Ko register nếu team đã bị ban** từ event đó
- **Nếu đã từng bị Rejected → reset thành Pending**
- **Nếu đã đăng ký rồi → báo lỗi**
- **Conflict check** — thành viên team đã được duyệt ở team khác trong cùng event?
- **Lock team** — CanEdit = false

### GET /api/v1/student/events/{eventId}/register-teams — `GetRegisterTeams`
- *(read-only, chỉ Approved + ko banned/disabled)*

### GET /api/v1/student/register-teams/{registerTeamId} — `GetRegisterTeamDetail`
- *(read-only, chỉ Approved + ko banned/disabled)*

### GET /api/v1/student/teams/{teamId}/register-teams — `GetRegisterTeamsByTeam`
- *(read-only, chỉ Approved)*

---

## 4. StudentSubmissionController

### POST /api/v1/student/submissions — `CreateSubmission`
- **RegisterTeam phải Approved** (ko bị banned/disabled)
- **Phải có Track + Topic assigned**
- **Event phải Published** (ko Draft/Closed)
- **Chỉ leader mới submit được**
- **Round phải thuộc cùng event**
- **Team phải registered trong round đó** (có RoundDetail)

---

## 5. StudentReportController

### POST /api/v1/student/reports — `CreateReport`
- *(tạo report, ko business rule đặc biệt)*

### POST /api/v1/student/reports/{reportId}/cancel — `CancelReport`
- **Chỉ cancel report của mình**
- **Chỉ cancel Pending** — ko cancel đã xử lý

---

## 6. StudentNotificationController

### POST /api/v1/student/notifications/{notificationId}/read — `ReadNotification`
- **Chỉ đọc được của mình, của team mình, hoặc System**
- Nếu đã Read rồi thì skip

### POST /api/v1/student/notifications/read-all — `ReadAllNotifications`
- Đọc tất cả Unread của student đó (personal + team + system)

---

## 7. StudentUserController

### GET /api/v1/student/users/{userId} — `GetUserDetail`
- *(chỉ đọc public profile — ko business rule)*

---

## 8. StudentAssignController

*(Read-only — xem assigned users của event)*

---

## 9. StudentLeaderboardController

### GET /api/v1/student/leaderboard/my-year-rank — `GetMyYearRank`
- **Phải login** mới xem được rank của mình

### GET /api/v1/student/leaderboard/my-year-detail — `GetMyYearDetail`
- **Phải login**

### GET /api/v1/student/events/{eventId}/leaderboard/my-rank — `GetMyEventRank`
- **Phải login**

---

## 10. Còn lại (Read-only)

| Controller | API |
|-----------|-----|
| StudentEventController | GET events, events/{id}, events/count, events/recent, events/popular |
| StudentRoundController | GET rounds, rounds/{id} |
| StudentAwardController | GET awards |
| StudentTrackTopicRegisterTeamController | GET tracks, topics (còn lại) |
| StudentCriteriaController | GET criteria-templates, criteria-items |
| StudentLeaderboardController | GET chapter/event leaderboard |

---

# Judge — Nghiệp vụ (Business Rules)

> Judge là Lecturer được assign Judge role trong event.
> - Phải được assign vào event + track mới chấm được
> - Chỉ 1 judge chấm 1 submission 1 lần (upsert — nếu đã có score thì update)

---

## JudgeController

### POST /api/v1/judge/submissions/{submissionId}/scores — `SubmitScore`
- **RegisterTeam phải có Track** (để lấy trackId)
- **Judge phải được assign vào track đó**
- **Phải có Criteria Template active** cho round đó
- **Phải chấm hết tất cả criteria items** trong template (ko thiếu)
- Upsert: nếu judge đã chấm → update score items + tính lại TotalScore
- Nếu chưa → tạo mới Score
- Set submission Status = Graded

### PATCH /api/v1/judge/submissions/{submissionId}/scores — `UpdateScoreBySubmission`
- Tìm score của judge hiện tại cho submission đó
- Nếu chưa chấm → NotFound
- Delegate sang `UpdateScore`

### PATCH /api/v1/judge/scores/{scoreId} — `UpdateScore`
- **Score phải thuộc judge hiện tại** (ko sửa score của judge khác)
- Chỉ update các criteria items gửi lên (giữ nguyên items ko gửi)
- Auto-recalculate TotalScore = SUM các ScoreItems
- Set submission Status = Graded

### PATCH /api/v1/judge/score-items/{scoreItemId} — `UpdateScoreItem`
- **Score item phải thuộc judge hiện tại**
- Auto-recalculate TotalScore của parent Score
- Chỉ update score/comment nếu có gửi

---

# Lecturer — Nghiệp vụ (Business Rules)

> Lecturer CHỈ ĐỌC (Read-only).
> - Phải được assign vào event để xem submissions
> - Các API mutation: chỉ có **Notification** (Read, ReadAll)

---

### GET /api/v1/lecturer/events/{eventId}/submissions — `GetSubmissions`
- *(chỉ đọc, check lecturer assigned)*

### GET /api/v1/lecturer/submissions/{submissionId} — `GetSubmissionDetail`
- *(chỉ đọc, check lecturer assigned)*

### GET /api/v1/lecturer/register-teams/{registerTeamId}/competition-status — `GetCompetitionStatus`
- *(chỉ đọc — dùng UtcNow để xác định round hiện tại, ko phải validation)*

### POST /api/v1/lecturer/notifications/{notificationId}/read — `ReadNotification`
- **Chỉ đọc được của mình hoặc System**

### POST /api/v1/lecturer/notifications/read-all — `ReadAllNotifications`
- Đọc tất cả Unread

### Còn lại: 100% Read-only

| Controller | APIs |
|-----------|------|
| LecturerEventController | GET events, my-lecturer, detail |
| LecturerRoundController | GET rounds, max-round-no |
| LecturerRegisterTeamController | GET register-teams, detail, by-team, by-track |
| LecturerAwardController | GET awards |
| LecturerTrackController | GET tracks, my-tracks, submissions |
| LecturerTopicController | GET topics |
| LecturerTeamController | GET teams, detail, events |
| LecturerUserController | GET users |
| LecturerScoreController | GET scores, items, round-score |
| LecturerLeaderboardController | GET leaderboard |
| LecturerCriteriaTemplateController | GET templates, items |
| LecturerAssignController | GET assign users, available |
