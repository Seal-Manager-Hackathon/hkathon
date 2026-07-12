# Nghiệp vụ API theo role — kiểm tra từ code đang chạy

> Phạm vi: Admin, Staff, Student, Judge, Lecturer.
>
> Chỉ ghi **business rule đang active**: điều kiện gây `BadRequest`, `Conflict`, `Forbidden`, ownership/assignment/team membership, trạng thái chuyển tiếp, giới hạn số lượng, tính điểm, visibility/filter ẩn và side effect nghiệp vụ.
>
> Không ghi: authorization role chung, pagination, parse enum/query thông thường hoặc check entity tồn tại đơn thuần (`NotFound`).
>
> GitNexus MCP đã được gọi lại bằng `query`, `list_repos` và resource API nhưng server bị health hook chặn với `spawn EINVAL`. Vì vậy bản kiểm tra này được đối chiếu trực tiếp từ toàn bộ controller/service/helper liên quan; không tuyên bố là kết quả GitNexus khi MCP chưa chạy được.

---

# I. Admin

## 1. Event

### `POST /api/v1/admin/events` — `CreateEvent`

- `EndTime` phải lớn hơn `StartTime`; sai trả `End Time Must Be After Start Time`.
- Event mới luôn được tạo với `Status = Draft`, `IsDisable = true`, `NumberRound = 0`.

### `GET /api/v1/admin/events/{eventId}/setup-check` — `IsEventSetupComplete`

Event được coi là setup đủ khi có:

- `Name` không rỗng.
- `EndTime` có giá trị và lớn hơn `StartTime`.
- `Season` có giá trị.
- `Description` không rỗng.
- `LimitTeam > 0`.
- `MinMember > 0`.
- `MaxMember > 0`.
- Có ít nhất một round.

### `PATCH /api/v1/admin/events/{eventId}` — `UpdateEvent`

- Event `Closed` chỉ được đổi `IsDisable`; gửi bất kỳ field khác trả `Cannot Update A Closed Event`.
- Không cho `Published -> Draft`: `Cannot Change Status From Published Back To Draft`.
- Không cho `Draft -> Closed`: phải publish trước.
- `Draft -> Published` chỉ thành công khi setup-check đầy đủ; lỗi liệt kê các field còn thiếu.
- Nếu request đổi `StartTime` hoặc `EndTime`, giá trị sau merge phải thỏa `EndTime > StartTime`.
- Bật hiển thị Draft (`IsDisable = false`) cũng yêu cầu setup-check đầy đủ.
- Đổi `StartTime` cập nhật `Year` của leaderboard đã tồn tại.
- Nếu event chưa có leaderboard và đã có `StartTime`, tự tạo leaderboard với `Year = StartTime.Year`, `IsPublished = true`.

### `POST /api/v1/admin/events/{eventId}/delete` — `DeleteEvent`

- Không xóa lại event đã disable: `Event Is Already Deleted`.
- Xóa mềm bằng `IsDisable = true`.

### `POST /api/v1/admin/events/{eventId}/restore` — `RestoreEvent`

- Không restore event chưa bị delete: `Event Is Not Deleted`.
- Restore event đồng thời bảo đảm leaderboard tồn tại; nếu đã có thì đồng bộ lại `Year` từ `StartTime`.

---

## 2. Round

### `POST /api/v1/admin/events/{eventId}/rounds` — `CreateRound`

- Tên round không được trùng trong cùng event: `Round Name Already Exists In This Event`.
- `EndTime > StartTime`.
- `StartTime >= Event.StartTime` và `EndTime <= Event.EndTime` nếu event có các mốc tương ứng.
- `RoundNo` tự động bằng `max RoundNo + 1`.
- Nếu là round số 2 trở lên, `StartTime` không được nhỏ hơn `EndTime` của round trước.
- Tạo round tăng `Event.NumberRound` thêm 1.

### `PATCH /api/v1/admin/rounds/{roundId}` — `UpdateRound`

- Giá trị sau merge bắt buộc có cả `StartTime` và `EndTime`: `Start Time And End Time Are Required`.
- `EndTime > StartTime`.
- Thời gian round phải nằm trong thời gian event.
- Với round số 2 trở lên, `StartTime` không được nhỏ hơn `EndTime` round trước.

### `POST /api/v1/admin/rounds/{roundId}/swap` — `SwapRound`

- `TargetRoundNo > 0`.
- Không swap round đã xóa (`IsDisable = true` hoặc `RoundNo = 0`).
- Không swap với chính round đó.
- Target round phải thuộc cùng event và có `RoundNo` được yêu cầu.
- Swap đồng thời `RoundNo`, `StartTime`, `EndTime`, `StartSubmission`, `EndSubmission`.

### `POST /api/v1/admin/rounds/{roundId}/delete` — `DeleteRound`

- Xóa mềm: `RoundNo = 0`, `IsDisable = true`.
- Giảm `Event.NumberRound`, không xuống dưới 0.
- Các round active có số lớn hơn round bị xóa được giảm `RoundNo` đi 1.

### `POST /api/v1/admin/rounds/{roundId}/restore` — `RestoreRound`

- Chỉ restore round đang bị xóa: `Round Not Deleted` nếu không.
- `RoundNo` mới bằng `max RoundNo + 1`.
- Xóa toàn bộ `StartTime`, `EndTime`, `StartSubmission`, `EndSubmission` khi restore.
- Tăng `Event.NumberRound` thêm 1.

### `POST /api/v1/admin/rounds/{roundId}/end-round` — `EndRound`

- Set `EndTime`, `EndSubmission`, `UpdatedAt` bằng thời điểm hiện tại.

---

## 3. Register team

### `PATCH /api/v1/admin/register-teams/{registerTeamId}` — `UpdateRegisterTeam`

- Có thể cập nhật trực tiếp `Description`, `RejectionReason`, `Status`, `IsBanned`, `IsDisable`.
- Không có state-transition guard ngoài parse status.

### `POST /api/v1/admin/register-teams/{registerTeamId}/approve` — `ApproveRegisterTeam`

- Chỉ approve registration `Pending`.
- Nếu round đầu có `LimitTeam`, số team trong round phải nhỏ hơn giới hạn.
- Không approve nếu bất kỳ member **không disabled** của team đã thuộc một registration Approved khác trong cùng event.
- Khi approve: `Status = Approved`, khóa team (`CanEdit = false`) và tự thêm team vào round đầu nếu có.

### `POST /api/v1/admin/register-teams/{registerTeamId}/reject` — `RejectRegisterTeam`

- Chỉ reject registration `Pending`.
- Set `Status = Rejected`, lưu rejection reason nếu được gửi.
- Mở khóa team (`CanEdit = true`).

### `POST /api/v1/admin/register-teams/{registerTeamId}/assign-next-round` — `AssignToNextRound`

- Phải tồn tại round tiếp theo; nếu không: `This Is The Last Round. Cannot Assign To Next Round`.
- Không tạo trùng `RoundDetail` cho cùng round.
- Nếu round kế có `LimitTeam`, số team hiện tại phải nhỏ hơn giới hạn.
- Thành công tạo `RoundDetail` mới, không xóa round cũ.

### `POST /api/v1/admin/register-teams/{registerTeamId}/revert-previous-round` — `RevertToPreviousRound`

- Team phải có ít nhất hai active round details.
- Round hiện tại không được có submission; nếu có phải xóa submission trước.
- Revert bằng hard-delete `RoundDetail` của round hiện tại.

### `POST /api/v1/admin/register-teams/{registerTeamId}/ban` — `BanRegisterTeam`

- Không ban lại registration đang banned.
- Chỉ ban registration `Approved`.
- Set `IsBanned = true`, `Status = Banned`, lưu lý do.

### `POST /api/v1/admin/register-teams/{registerTeamId}/unban` — `UnbanRegisterTeam`

- Chỉ unban khi `IsBanned = true`.
- Set `IsBanned = false`, `Status = Approved`, xóa `RejectionReason`.

### `POST /api/v1/admin/register-teams/{registerTeamId}/assign-track-topic` — `AssignTrackTopic`

- Track phải thuộc cùng event với register team.
- Nếu có Topic, Topic phải thuộc Track được chọn.
- Ghi đè `TrackId` và `TopicId` hiện tại.

### `POST /api/v1/admin/register-teams/{registerTeamId}/remove-track-topic` — `RemoveTrackTopic`

- Set cả `TrackId` và `TopicId` về null.

---

## 4. Award

### `POST /api/v1/admin/events/{eventId}/awards` — `CreateAward`

- Tên award không được trùng trong cùng event.
- Award đầu tiên có `LevelAward = 1`; các award sau nhận `max level + 1`.

### `POST /api/v1/admin/awards/{awardId}/swap` — `SwapAwardLevel`

- `TargetLevel > 0`.
- Không swap award bị delete (`IsDisable = true` hoặc `LevelAward = 0`).
- Không swap với chính level hiện tại.
- Target level phải tồn tại và active trong cùng event.
- Thành công đổi level giữa hai award.

### `POST /api/v1/admin/awards/{awardId}/delete` — `DeleteAward`

- Set `IsDisable = true`, `LevelAward = 0`.
- Các award có level cao hơn level bị xóa được giảm 1.

### `POST /api/v1/admin/awards/{awardId}/restore` — `RestoreAward`

- Restore với `LevelAward = max active level + 1`.

---

## 5. Track và Topic

### `POST /api/v1/admin/events/{eventId}/tracks` — `CreateTrack`

- Title không được trùng trong cùng event.

### `POST /api/v1/admin/tracks/{trackId}/topics` — `CreateTopic`

- Title không được trùng trong cùng track.

### `POST /api/v1/admin/topics/{topicId}/delete` — `DeleteTopic`

- Không delete lại topic đã delete.

### `POST /api/v1/admin/topics/{topicId}/restore` — `RestoreTopic`

- Chỉ restore topic đang delete; nếu chưa delete trả `Topic Is Not Deleted`.

Các update/delete/restore Track chỉ đổi field/`IsDisable`, không có state guard đặc biệt.

---

## 6. User và Team

### `POST /api/v1/admin/users` — `CreateUser`

- Email không được trùng; trùng trả Conflict `Email Already Exists`.
- User được tạo với password mặc định `string`, `IsVerified = true`, `Status = Active`.

### `POST /api/v1/admin/users/{userId}/ban` — `BanUser`

- Set `BanReason`, `BannedAt`, `Status = Banned`.
- Không set `IsDisable`; user bị ban vẫn visible theo rule hiện tại.

### `POST /api/v1/admin/users/{userId}/unban` — `UnbanUser`

- Xóa ban reason/time và set `Status = Active`.

### `POST /api/v1/admin/teams/{teamId}/delete` — `DeleteTeam`

- Không disable lại team đã disabled.

### `POST /api/v1/admin/teams/{teamId}/change-leader` — `ChangeLeader`

- New leader phải là member của team.
- New leader phải active và không disabled.
- Team phải có leader active hiện tại.
- Không chuyển cho member đã là leader.
- Thành công hạ leader cũ và nâng leader mới.

### `POST /api/v1/admin/teams/{teamId}/lock` — `LockTeam`

- Không lock team đã khóa (`CanEdit = false`).

### `POST /api/v1/admin/teams/{teamId}/unlock` — `UnlockTeam`

- Không unlock team đang editable (`CanEdit = true`).

---

## 7. Notification

### `POST /api/v1/admin/notifications` — `CreateNotification`

- `System`: bỏ qua `UserId`/`TeamId`, tạo một notification global.
- `Team`: bắt buộc `TeamId`, bỏ qua `UserId`.
- `Personal`: bắt buộc `UserId`, `TeamId = null`.

### `POST /api/v1/admin/notifications/{notificationId}/delete` — `DeleteNotification`

- Không disable lại notification đã disabled.

### `GET /api/v1/admin/notifications/{notificationId}` — `GetNotificationDetail`

- **Không** kiểm tra ownership/access. Chỉ check tồn tại. Bất kỳ admin nào cũng xem được mọi notification (Personal của người khác, Team, System).

### `GET /api/v1/admin/notifications/recent` — `GetRecentNotifications`

- Dùng `GetRecentAsync` chỉ filter `!IsDisable`, **không** giới hạn user scope. Trả về tất cả notification (Personal của bất kỳ ai, Team, System) không bị disable — khác biệt với Lecturer (xem mục V.5).

### `GET /api/v1/admin/notifications/my/{notificationId}` — `GetMyNotificationDetail`

- Chỉ truy cập notification Personal của chính admin hoặc System.
- Team notification không được chấp nhận bởi method này.

### `POST /api/v1/admin/notifications/my/{notificationId}/read` — `ReadNotification`

- Chỉ mark-read notification Personal của chính admin hoặc System.
- Method luôn set `Read` và save; không có nhánh skip khi đã Read.

### `POST /api/v1/admin/notifications/my/read-all` — `ReadAllNotifications`

- Chỉ lấy các notification Unread trong inbox của admin theo repository scope và set tất cả thành Read.

---

## 8. Assign event/track

> Base route của controller: `/api/v1/admin/assign`.

### `POST /api/v1/admin/assign/events/{eventId}/assign/users` — `AssignUserToEvent`

- Không assign user disabled hoặc banned.
- Không assign Student hoặc Admin.
- User role Staff chỉ được event role Staff.
- User role Lecturer không được event role Staff; chỉ Judge/Mentor.
- Assignment cũ disabled được re-enable và đổi role.
- Assignment đang active gây Conflict `User Is Already Assigned To This Event`.

### `PATCH /api/v1/admin/assign/event-assigns/{assignEventId}/event-role` — `AssignEventRoleToLecturer`

- Chỉ đổi role cho user có global role Lecturer.
- Không gán event role Staff cho Lecturer.
- Lưu ý code lấy `oldRoleName` sau khi đổi `EventRoleId`; navigation có thể vẫn phản ánh role cũ tùy tracking, nhưng rule nghiệp vụ là gửi notification old -> new.

### `POST /api/v1/admin/assign/event-assigns/{assignEventId}/tracks` — `AssignTrackToEvent`

- Assignment có event role Staff không được gán track.
- Track phải thuộc cùng event.
- Không gán trùng track cho cùng assignment; trùng trả Conflict.

### `POST /api/v1/admin/assign/event-assigns/{assignEventId}/tracks/{trackId}/restore` — `RestoreTrackToEvent`

- Chỉ restore assign-track đang disabled; active trả `Assign Track Is Already Active`.

### `POST /api/v1/admin/assign/event-assigns/{assignEventId}/remove` — `RemoveAssignEvent`

- Không remove assignment đã removed.
- Remove assignment đồng thời disable toàn bộ assign-tracks.

### `POST /api/v1/admin/assign/event-assigns/{assignEventId}/restore` — `RestoreAssignEvent`

- Không restore assignment đang active.
- Restore assignment đồng thời restore toàn bộ assign-tracks.

### `GET /api/v1/admin/assign/users/{userId}/assign-events` — `GetUserAssignEvents`

- Target user phải có global role Staff hoặc Lecturer.

---

## 9. Criteria template

### `POST /api/v1/admin/rounds/{roundId}/criteria-templates` — `CreateCriteriaTemplate`

- Tạo template cùng toàn bộ items trong một lần.
- Không tự activate template.

### `POST /api/v1/admin/criteria-templates/{templateId}/activate` — `ActivateCriteriaTemplate`

- Không activate template đã delete.
- Không activate lại template đã active.
- Chỉ một template active trong mỗi round: activate template được chọn và deactivate các template còn lại.

### `POST /api/v1/admin/criteria-templates/{templateId}/delete` — `DeleteCriteriaTemplate`

- Không delete template đang active; phải chuyển active sang template khác trước.

Item/template update, restore và item delete chỉ thay field/`IsDisable`, không có state guard đặc biệt khác.

---

## 10. Leaderboard

### Các GET leaderboard — `GetRoundLeaderboard`, `GetEventLeaderboard`, `GetChapterLeaderboard`

- `scopeScore` của round leaderboard = trung bình `Scores.TotalScore` của các judge đã chấm; judge chưa có score bị bỏ qua, điểm 0 vẫn tính; không ai chấm thì helper trả 0.
- **Caveat API team-round-score:** Admin/Staff `GetTeamRoundScore` dùng `Average()` trực tiếp khi đã có submission; nếu submission tồn tại nhưng không có score nào có `TotalScore`, code có thể throw thay vì trả 0.
- `eventScore` = tổng scopeScore các round team đã tham gia.
- `chapterScore` = tổng eventScore các event trong năm.
- Xếp hạng dùng `DENSE_RANK`: cùng điểm cùng rank, rank kế tăng 1, không nhảy số.
- Admin Event leaderboard chỉ trả khi leaderboard `IsPublished = true` và không disabled.
- Admin Chapter leaderboard chỉ dùng leaderboard `IsPublished = true` và không disabled.
- Event leaderboard chỉ lấy Approved registrations qua repository; chapter còn loại register team disabled/banned.
- Tính rank trên toàn bộ dữ liệu rồi mới phân trang.

### `POST /api/v1/admin/events/chapter/{year}/leaderboard/publish` — `PublishChapter`

- Set tất cả leaderboard trong năm `IsDisable = false`, `IsPublished = true`.

### `POST /api/v1/admin/events/chapter/{year}/leaderboard/hide` — `HideChapter`

- Set tất cả leaderboard trong năm `IsDisable = true`.

### `POST /api/v1/admin/events/{eventId}/leaderboard/publish` — `PublishEvent`

- Set leaderboard event `IsDisable = false`, `IsPublished = true`.

### `POST /api/v1/admin/events/{eventId}/leaderboard/hide` — `HideEvent`

- Set leaderboard event `IsDisable = true`.

---

## 11. Report

### `PATCH /api/v1/admin/reports/{reportId}/status` — `UpdateReportStatus`

- Ghi đè `Status` và `Reason`.
- Gửi Personal notification cho người tạo report.
- Không có state-transition guard (ví dụ Pending -> Resolved) ngoài parse enum.

---

# II. Staff

## Rule assignment dùng chung

- Nhiều API Staff gọi `StaffAssignmentHelper.ValidateAndGetAssignmentAsync`: phải tìm thấy assignment theo user/event và event không disabled; nếu không trả Forbidden `You Are Not Assigned to This Event`.
- **Caveat:** repository mà helper dùng không lọc `AssignEvent.IsDisable`, nên assignment đã remove vẫn có thể vượt qua guard này.

## 1. Register team

Mọi mutation bên dưới có thêm assignment guard theo event của register team.

### `PATCH /api/v1/staff/register-teams/{registerTeamId}` — `UpdateRegisterTeam`

- Staff chỉ sửa Description, RejectionReason và Status.
- Service không cho Staff sửa `IsBanned`/`IsDisable`.

### `POST /api/v1/staff/register-teams/{registerTeamId}/approve` — `ApproveRegisterTeam`

- Chỉ Pending.
- Round 1 không được full.
- Không có member không-disabled đã Approved ở team khác cùng event.
- Thành công khóa team và thêm vào round đầu nếu có.

### `POST /api/v1/staff/register-teams/{registerTeamId}/reject` — `RejectRegisterTeam`

- Chỉ Pending.
- Thành công mở khóa team.

### `POST /api/v1/staff/register-teams/{registerTeamId}/ban` — `BanRegisterTeam`

- Không ban lại registration đang banned.
- Chỉ ban registration Approved.

### `POST /api/v1/staff/register-teams/{registerTeamId}/unban` — `UnbanRegisterTeam`

- Chỉ unban khi đang banned; trả về Approved và xóa rejection reason.

### `POST /api/v1/staff/register-teams/{registerTeamId}/assign-next-round` — `AssignToNextRound`

- Phải có round kế tiếp.
- Không tạo RoundDetail trùng.
- Round kế không được full.

### `POST /api/v1/staff/register-teams/{registerTeamId}/revert-previous-round` — `RevertToPreviousRound`

- Cần ít nhất hai active RoundDetails.
- Current round không được có submission.

### `POST /api/v1/staff/register-teams/{registerTeamId}/assign-track-topic` — `AssignTrackTopic`

- Track phải cùng event với RegisterTeam.
- Topic phải thuộc Track được chọn.

### `POST /api/v1/staff/register-teams/{registerTeamId}/remove-track-topic` — `RemoveTrackTopic`

- Set `TrackId` và `TopicId` về null.

## 2. Assign lecturer

> Base route: `/api/v1/staff/assign`.

### `POST /api/v1/staff/assign/events/{eventId}/assign/users` — `AssignLecturerToEvent`

- Staff thực hiện phải được assign vào event.
- Target user không disabled/banned và phải có global role Lecturer.
- Chỉ gán event role Judge/Mentor; không gán Staff.
- Assignment disabled được re-enable; assignment active gây Conflict.

### `POST /api/v1/staff/assign/event-assigns/{assignEventId}/remove` — `RemoveAssignEvent`

- Chỉ remove assignment của Lecturer, không remove Staff.
- Staff thực hiện phải được assign vào cùng event.
- Không remove assignment đã removed.
- Cascade disable toàn bộ assign-tracks.

### `POST /api/v1/staff/assign/event-assigns/{assignEventId}/restore` — `RestoreAssignEvent`

- Chỉ restore Lecturer.
- Không restore assignment active.
- Cascade restore toàn bộ assign-tracks.

### `POST /api/v1/staff/assign/event-assigns/{assignEventId}/tracks` — `AssignTrackToEvent`

- Chỉ gán track cho Lecturer.
- Track phải thuộc cùng event.
- Không gán trùng track; trùng trả Conflict.

### Remove/restore track

- Chỉ sửa track assignment của Lecturer.
- Restore chỉ áp dụng khi assign-track đang disabled.

### `GET /api/v1/staff/assign/users/{userId}/assign-events` — `GetUserAssignEvents`

- Staff hiện tại phải có ít nhất một event assignment bất kỳ.
- Target user phải là Staff hoặc Lecturer.

## 3. Visibility và access cho GET

- `GET /api/v1/staff/events` hiện đọc event toàn hệ thống theo filter, không giới hạn assignment.
- `events/my-staff`, recent và count chỉ lấy assignment không disabled, event không disabled, event role Staff và event không Draft.
- `GET /api/v1/staff/events/current` chỉ lấy assignment không disabled có event role Staff, event không disabled/không Draft, có đủ Start/End và `StartTime <= now <= EndTime`.
- Event detail yêu cầu current Staff được assign vào event; chưa assign trả NotFound.
- Award: Staff phải được assign vào event; chỉ thấy award active.
- Round: Staff phải được assign vào event chứa round; chỉ thấy round active.
- Track/Topic: Staff phải được assign vào event; chỉ thấy item active.
- Criteria template/item: Staff phải được assign vào event suy ra từ round/template/item; list loại item/template disabled.
- Score detail, submission detail/list và score theo round/register-team: Staff phải được assign vào event liên quan theo từng service method.
- **Caveat:** một số Staff Score GET dựa trên navigation `RegisterTeam`; nếu navigation null thì assignment guard có thể không chạy.
- RegisterTeam list/detail: Staff phải được assign vào event liên quan.
- Team/User: service cho phép đọc toàn hệ thống theo filter, không yêu cầu event assignment.

## 4. Staff leaderboard

- Round/Event leaderboard yêu cầu Staff được assign vào event.
- Event leaderboard chỉ trả khi leaderboard Published và không disabled.
- Chapter GET dùng các leaderboard Published và không disabled.
- Publish/Hide event chỉ cho event được assign.
- Publish/Hide chapter chỉ tác động các event trong năm mà Staff được assign; nếu không có event nào trả Forbidden `You Are Not Assigned to Any Event In This Year`.
- Công thức điểm và DENSE_RANK giống phần Admin.

## 5. Staff notification

- Create System/Team/Personal dùng cùng target rules như Admin.
- `GET /api/v1/staff/notifications/{notificationId}` — `GetNotificationDetail`: **không** kiểm tra ownership/access. Chỉ check tồn tại. Bất kỳ staff nào cũng xem được mọi notification.
- `GET /api/v1/staff/notifications/recent` — `GetRecentNotifications`: dùng `GetRecentAsync` chỉ filter `!IsDisable`, **không** giới hạn user scope. Trả về tất cả notification (Personal của bất kỳ ai, Team, System) không bị disable — khác biệt với Lecturer (xem mục V.5).
- `notifications/my/{id}` và mark-read chỉ cho Personal của chính Staff hoặc System.
- Service `ReadMyNotification`/`ReadAllMyNotifications` gọi repository update nhưng hiện không gọi `SaveChangesAsync`; đây là caveat implementation, không phải business rule mong muốn.

## 6. Staff report

### `PATCH /api/v1/staff/reports/{reportId}/status` — `UpdateReportStatus`

- Ghi đè status/reason và gửi Personal notification cho người tạo report.
- Không có event-assignment guard hoặc state-transition guard trong service hiện tại.

---

# III. Student

## 1. Visibility chung cho GET

- Event list/detail/count/recent loại event Draft và disabled theo từng method; Student không được filter lấy Draft.
- User search/detail loại user disabled; user banned vẫn visible.
- Track, Topic, Round và Award GET loại entity disabled theo service tương ứng.
- Criteria list theo round chỉ lấy template vừa `IsActive = true` vừa không disabled; CriteriaItems list/detail loại item disabled.
- Public chapter/event leaderboard chỉ dùng leaderboard Published và không disabled.

## 2. Team

### `POST /api/v1/student/teams` — `CreateTeam`

- Profile phải đủ: Email, FirstName, LastName, College, StudentId, PhoneNumber.
- Tên team không được trùng.
- Creator tự trở thành active leader.

### `PATCH /api/v1/student/teams/{teamId}` — `UpdateTeam`

- Team phải editable (`CanEdit = true`).
- Chỉ leader có `IsDisable = false` được update; method này không kiểm tra `TeamDetail.Status`.
- Service chỉ đổi Name.

### `POST /api/v1/student/teams/{teamId}/disband` — `DisbandTeam`

- Chỉ leader có `IsDisable = false` được disband; method này không kiểm tra `TeamDetail.Status`.
- Không disband nếu team có Approved registration active trong event thỏa `StartTime <= now <= EndTime` (mốc null được coi là không chặn).
- Thành công disable toàn bộ member, set họ Inactive, disable và khóa team.

### `POST /api/v1/student/teams/{teamId}/members/{memberId}/kick` — `KickMember`

- Team phải editable.
- Chỉ leader có `IsDisable = false` được kick; method này không kiểm tra `TeamDetail.Status` của leader.
- Không kick chính mình, inactive/disabled member hoặc leader.
- Thành công hard-delete `TeamDetails` của member.

### `POST /api/v1/student/teams/{teamId}/change-leader` — `ChangeLeader`

- Không chuyển cho chính current leader.
- Current user phải là leader có `IsDisable = false`; method này không kiểm tra `TeamDetail.Status` của current leader.
- Target phải là member của team, `IsDisable = false` và `Status != Inactive`.
- Thành công hạ leader cũ và nâng leader mới.

### `POST /api/v1/student/teams/{teamId}/leave` — `LeaveTeam`

- Team phải editable.
- Member active mới leave được.
- Leader không được leave; phải change leader hoặc disband.
- Thành công set member disabled + Inactive.

## 3. Invitation

### `POST /api/v1/student/teams/{teamId}/invitations` — `SendInvitation`

- Team phải editable.
- Chỉ leader có `IsDisable = false` gửi được; method này không kiểm tra `TeamDetail.Status` của leader.
- Không invite user disabled.
- Không invite người đã là active member.
- Không tạo invitation Pending trùng cho cùng user/team.
- Invitation có `LimitTime = now + 15 days`.

### `GET /api/v1/student/teams/{teamId}/invitations` — `GetSentInvitations`

- Chỉ active leader của team được xem sent invitations.

### `GET /api/v1/student/invitations/{invitationId}` và `/team`

- Service hiện không kiểm tra owner/leader; ai có invitationId có thể gọi theo logic hiện tại.

### `POST /api/v1/student/invitations/{invitationId}/accept` — `AcceptInvitation`

- Profile phải đủ các field của `StudentProfileHelper`.
- Chỉ đúng invited user được accept.
- Invitation phải Pending.
- Hết `LimitTime` thì service đổi status thành Expired, save rồi trả lỗi.
- Team phải editable.
- User chưa được là active member của team.
- Thành công tạo active non-leader `TeamDetails`.

### `POST /api/v1/student/invitations/{invitationId}/reject` — `RejectInvitation`

- Chỉ invited user được reject.
- Invitation phải Pending.

## 4. Register team

### `POST /api/v1/student/register-teams` — `CreateRegisterTeam`

- Chỉ leader có `IsDisable = false` của team được đăng ký; method này không kiểm tra `TeamDetail.Status` của leader.
- Event không được Draft hoặc Closed.
- Số active members phải nằm trong `[Event.MinMember, Event.MaxMember]` khi các limit có giá trị.
- Nếu registration cũ Banned: chặn đăng ký lại.
- Nếu registration cũ Rejected: tái sử dụng record, set Pending và xóa rejection reason.
- Các trạng thái cũ khác: chặn vì team đã đăng ký event.
- Không đăng ký nếu bất kỳ member có `IsDisable = false` và `Status = Active` đã nằm trong một Approved team khác cùng event.
- Tạo mới thành công khóa team (`CanEdit = false`).

### GET register team

- Public student list/detail chính chỉ lấy registration Approved, không banned, không disabled.
- Route `/teams/{teamId}/register-teams/all` cho phép lọc toàn bộ trạng thái theo service hiện tại.

## 5. Submission

### `POST /api/v1/student/submissions` — `CreateSubmission`

- RegisterTeam phải Approved, không banned/disabled.
- Phải có cả Track và Topic.
- Event phải Published.
- Chỉ leader có `IsDisable = false` và `Status = Active` được submit.
- Round phải thuộc cùng event.
- Team phải có active `RoundDetail` trong round.
- Mỗi call tạo một submission mới; không có duplicate/upsert guard.

## 6. Notification

### `GET /api/v1/student/register-teams/{registerTeamId}/mentor-notifications`

- RegisterTeam phải Approved.
- Current user phải là member active của team.
- Chỉ lấy mentor notifications qua assign-track cùng Track của RegisterTeam.

### `GET /api/v1/student/mentor-notifications/{mentorNotificationId}`

- User phải thuộc ít nhất một active team có Approved registration cùng Track với notification.

### Notification detail/read

- Personal: chỉ owner.
- Team: user phải thuộc active team đó.
- System: được truy cập.
- `ReadNotification` skip save nếu status đã Read.
- `ReadAllNotifications` lấy Unread trong scope Personal + active teams + System rồi mark Read.

## 7. Report

### `GET /api/v1/student/reports/{reportId}` — `GetReportDetail`

- Chỉ xem report của chính user; service dùng NotFound để che report người khác.

### `POST /api/v1/student/reports/{reportId}/cancel` — `CancelReport`

- Chỉ cancel report của chính user.
- Chỉ report Pending được cancel; đã xử lý trả `Cannot Cancel a Report That Has Been Processed`.

## 8. Student leaderboard

- Chapter/Event leaderboard công khai chỉ dùng leaderboard Published và không disabled.
- My-year rank/detail chỉ trả các active team của current user.
- My-event rank chỉ trả active team của current user trong event.
- Công thức scope/event/chapter score và DENSE_RANK giống phần Admin.

---

# IV. Judge

> Judge endpoint dùng global role Lecturer, sau đó kiểm tra assignment có event role Judge.

## 1. Quyền xem dữ liệu

### `GET /api/v1/judge/events/{eventId}/tracks` — `GetMyTracks`

- User phải được assign Judge trong event.
- Chỉ trả assign-tracks active và Track active.

### `GET /api/v1/judge/tracks/{trackId}/submissions` — `GetTrackSubmissions`

- Judge phải được assign đúng Track.
- `isGraded` hiện filter theo global `Submission.Status`, không phải riêng score của judge.

### `GET /api/v1/judge/events/{eventId}/myscope` — `GetMyScope`

- Chỉ kiểm tra Judge assignment ở event.
- **Caveat code hiện tại:** method fetch submissions theo filter request nhưng không loại submission ngoài các track được assign nếu `trackId` không được truyền; `myScore` chỉ dùng để map điểm.

### `GET /api/v1/judge/submissions/{submissionId}/criteria` — `GetSubmissionCriteria`

- RegisterTeam phải xác định được Track.
- Judge phải được assign Track đó.
- Không có active template thì trả response rỗng, không lỗi.

### `GET /api/v1/judge/submissions/{submissionId}` và `/my-score`

- Chỉ kiểm tra Judge assignment ở event của submission; không kiểm tra Track assignment trong hai method này.

### `GET /api/v1/judge/events/{eventId}/scores/me` — `GetMyScores`

- Judge phải được assign Judge trong event.
- Chỉ xét các active AssignTracks của current judge.
- Chỉ giữ submission thuộc Track mà judge được assign.
- `isGraded` hiện filter theo global `Submission.Status`, không theo việc current judge đã có score.

### `GET /api/v1/judge/register-teams/{registerTeamId}/submissions` — `GetRegisterTeamSubmissions`

- Judge phải được assign Judge vào event của RegisterTeam.
- Method lấy **submission mới nhất tuyệt đối qua tất cả round**, không phải last submission cho từng round như mô tả trong controller.
- Method không kiểm tra Track assignment của Judge.

### `GET /api/v1/judge/scores/{scoreId}/items` và `score-items/{scoreItemId}`

- Score/ScoreItem phải thuộc current judge.

### `GET /api/v1/judge/rounds/{roundId}/submissions` — `GetSubmissionsByRound`

- Judge phải được assign event của round.
- Nếu truyền `trackId`, Track phải nằm trong danh sách Track của judge.
- Nếu không truyền, chỉ lấy các Track được assign.
- `isGraded` filter theo việc current judge đã có score (`myScore`), không theo global status.

## 2. Chấm điểm

### `POST /api/v1/judge/submissions/{submissionId}/scores` — `SubmitScore`

- RegisterTeam phải xác định được Track.
- Judge phải được assign Track đó.
- Round phải có CriteriaTemplate active.
- Request phải chứa tất cả active CriteriaItems; thiếu trả tên các criteria còn thiếu.
- Nếu judge đã có Score cho submission: thay toàn bộ old ScoreItems bằng items mới và tính lại TotalScore.
- Nếu chưa có: tạo Score mới.
- `TotalScore` là tổng item scores của một judge.
- Thành công set `Submission.Status = Graded`.

### `PATCH /api/v1/judge/submissions/{submissionId}/scores` — `UpdateScoreBySubmission`

- Tìm Score của current judge trên submission; chưa chấm thì NotFound.
- Gọi method nội bộ `UpdateScore(scoreId, ...)`.
- Chỉ update CriteriaItems được gửi, giữ item không gửi.
- Tính lại TotalScore bằng tổng toàn bộ ScoreItems và giữ Submission = Graded.

### `PATCH /api/v1/judge/score-items/{scoreItemId}` — `UpdateScoreItem`

- ScoreItem phải thuộc current judge.
- Chỉ đổi Score/Comment nếu request có giá trị.
- Tính lại TotalScore của parent Score.

> `UpdateScore(Guid scoreId, ...)` là method service nội bộ, **không có route public** `/judge/scores/{scoreId}`.

---

# V. Lecturer

> Lecturer chủ yếu read-only. Các assignment guards dưới đây vẫn là business rule của GET.

## 1. Event và assignment

- `GET /api/v1/lecturer/events`: service hiện trả event toàn hệ thống theo filter, không yêu cầu assignment.
- `GET /api/v1/lecturer/events/my-lecturer`, recent và count: chỉ dựa trên lecturer assignments.
- `GET /api/v1/lecturer/events/{eventId}`: cho xem event dù chưa assign; nếu có assignment thì bổ sung EventRole.
- `GET /api/v1/lecturer/events/{eventId}/assign`: Lecturer phải được assign event; list loại assignment disabled.
- `GET /api/v1/lecturer/events/{eventId}/my-assignment`: trả assignment + active tracks của chính lecturer.
- Hai API available staff/lecturer không kiểm tra current lecturer đã assign event trong service hiện tại.

## 2. Submission

Các API sau yêu cầu Lecturer được assign vào event liên quan:

- `GET /api/v1/lecturer/events/{eventId}/submissions` — `GetSubmissions`.
- `GET /api/v1/lecturer/register-teams/{registerTeamId}/submissions` — `GetSubmissionsByRegisterTeam`.
- `GET /api/v1/lecturer/tracks/{trackId}/submissions` — `GetSubmissionsByTrack`.
- `GET /api/v1/lecturer/submissions/{submissionId}` — `GetSubmissionDetail`.

## 3. RegisterTeam, Track, Score

- RegisterTeam list/detail/by-team/by-track/user-events/competition-status hiện không có lecturer assignment guard trong service.
- `GetCompetitionStatus` xác định current round bằng `StartTime <= now <= EndTime`, rồi `IsStillCompeting = MaxRoundNo >= CurrentRoundNo`.
- Track list theo event không yêu cầu assignment; `my-tracks` chỉ trả active assign-tracks của current lecturer.
- Score GET service hiện không có assignment/ownership guard đặc biệt ngoài role chung.

## 4. Leaderboard

- Event leaderboard chỉ trả khi leaderboard không disabled.
- Chapter leaderboard chỉ dùng leaderboard Published và không disabled.
- Không có lecturer assignment guard cho leaderboard.
- Công thức điểm và DENSE_RANK giống phần Admin.

## 5. Notification

- List/count/recent/unread dùng repository scope Personal của current Lecturer + System, loại disabled.
- `POST /api/v1/lecturer/notifications/{notificationId}/read`: chỉ Personal của chính Lecturer hoặc System.
- `GET /api/v1/lecturer/notifications/{notificationId}` hiện **không kiểm tra ownership/access**; chỉ check tồn tại.
- `ReadNotification`/`ReadAllNotifications` gọi repository update nhưng service không có `SaveChangesAsync`; đây là caveat persistence của implementation hiện tại.

---

# VI. Coverage toàn bộ controller API

> Bảng này chứng minh các controller/API đã được rà. Những controller read-only không có rule riêng vẫn xuất hiện ở đây.

| Role | Controller | Số endpoint | Nghiệp vụ/điều kiện chính |
|---|---|---:|---|
| Admin | `AdminAssignController` | 11 | Assign role/event/track, remove/restore |
| Admin | `AdminAwardController` | 7 | Level tự tăng, swap, delete reindex |
| Admin | `AdminCriteriaTemplateController` | 13 | Một active template/round |
| Admin | `AdminEventController` | 9 | Time, setup, status transition |
| Admin | `AdminInvitationController` | 1 | Read-only |
| Admin | `AdminLeaderboardController` | 7 | Score/rank, publish/hide |
| Admin | `AdminNotificationController` | 12 | Target routing, inbox ownership |
| Admin | `AdminRegisterTeamController` | 14 | Approve/reject/ban/round/track |
| Admin | `AdminReportController` | 4 | Update status/reason |
| Admin | `AdminRoundController` | 9 | Time/order/swap/delete/restore/end |
| Admin | `AdminScoreController` | 5 | Read-only score views |
| Admin | `AdminSubmissionController` | 5 | Read-only submission views |
| Admin | `AdminTeamController` | 11 | Leader, lock/unlock/delete |
| Admin | `AdminTopicController` | 6 | Duplicate title/delete/restore |
| Admin | `AdminTrackController` | 6 | Duplicate title, soft delete |
| Admin | `AdminUserController` | 11 | Create/ban/unban/disable |
| Staff | `StaffAssignController` | 11 | Chỉ Lecturer, assignment scope |
| Staff | `StaffAwardController` | 2 | Assigned event, active only |
| Staff | `StaffCriteriaTemplateController` | 4 | Read-only, assigned event |
| Staff | `StaffEventController` | 6 | Global list vs assigned lists |
| Staff | `StaffLeaderboardController` | 7 | Assigned event/year scope |
| Staff | `StaffNotificationController` | 12 | Target routing/inbox ownership |
| Staff | `StaffRegisterTeamController` | 14 | Admin-like mutations + assignment |
| Staff | `StaffReportController` | 4 | Update report status |
| Staff | `StaffRoundController` | 2 | Read-only, assigned event |
| Staff | `StaffScoreController` | 5 | Read-only, assigned event |
| Staff | `StaffSubmissionController` | 5 | Read-only, assigned event |
| Staff | `StaffTeamController` | 5 | Read-only |
| Staff | `StaffTopicController` | 2 | Assigned event, active only |
| Staff | `StaffTrackController` | 2 | Assigned event, active only |
| Staff | `StaffUserController` | 5 | Read-only |
| Student | `StudentAssignController` | 1 | Read-only assigned-user list |
| Student | `StudentAwardController` | 2 | Active only |
| Student | `StudentCriteriaController` | 4 | Active template/items only |
| Student | `StudentEventController` | 5 | Hide Draft/disabled |
| Student | `StudentInvitationController` | 7 | Leader/invitee/status/expiry |
| Student | `StudentLeaderboardController` | 5 | Published boards, my active teams |
| Student | `StudentNotificationController` | 8 | Personal/team/system/mentor scope |
| Student | `StudentReportController` | 4 | Own report, Pending cancel |
| Student | `StudentRoundController` | 2 | Active only |
| Student | `StudentSubmissionController` | 3 | Approved team, leader, round link |
| Student | `StudentTeamController` | 11 | Team lifecycle/membership |
| Student | `StudentTrackTopicRegisterTeamController` | 11 | Track/topic reads + registration |
| Student | `StudentUserController` | 2 | Hide disabled user |
| Judge | `JudgeController` | 14 | Assignment/ownership/scoring |
| Lecturer | `LecturerAssignController` | 4 | Assigned-event access cho một số GET |
| Lecturer | `LecturerAwardController` | 2 | Read-only |
| Lecturer | `LecturerCriteriaTemplateController` | 4 | Read-only |
| Lecturer | `LecturerEventController` | 7 | Global vs assigned event views |
| Lecturer | `LecturerLeaderboardController` | 3 | Published/visible boards |
| Lecturer | `LecturerNotificationController` | 7 | Inbox scope; detail caveat |
| Lecturer | `LecturerRegisterTeamController` | 7 | Read-only; competition-status time |
| Lecturer | `LecturerRoundController` | 3 | Read-only |
| Lecturer | `LecturerScoreController` | 5 | Read-only |
| Lecturer | `LecturerTeamController` | 5 | Read-only |
| Lecturer | `LecturerTopicController` | 2 | Read-only |
| Lecturer | `LecturerTrackController` | 4 | Global tracks vs my tracks |
| Lecturer | `LecturerUserController` | 4 | Read-only |

## Tổng coverage

| Role | Controller files | Endpoint attributes |
|---|---:|---:|
| Admin | 16 | 131 |
| Staff | 15 | 86 |
| Student | 13 | 65 |
| Judge | 1 | 14 |
| Lecturer | 13 | 57 |
| **Tổng** | **58** | **353** |

> `StudentTrackTopicRegisterTeamController.cs` chứa ba controller class (Track, Topic, RegisterTeam), nhưng coverage file được tính một lần. Tổng endpoint được tính trực tiếp từ `[HttpGet]`, `[HttpPost]`, `[HttpPatch]`.
