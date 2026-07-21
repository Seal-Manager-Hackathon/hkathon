# Audit Seed Data — Entity, quan hệ và coverage nghiệp vụ

> Phạm vi: toàn bộ 27 entity trong `Hackathon.Domain/Entities/`, tất cả seed tại `Hackathon.Infrastructure/Seed/01/`, `Seed/02/`, và thứ tự đăng ký seed trong `AppDbContext.OnModelCreating`.
>
> GitNexus MCP đã được sử dụng để đối chiếu `SeedFPTData`, `SeedLeaderBoards`, `CreateRegisterTeam`, các flow leaderboard, đăng ký team, nộp bài, chấm điểm và invitation. Endpoint GitNexus chuẩn ban đầu bị health hook chặn `spawn EINVAL`; plugin GitNexus endpoint hoạt động nhưng cảnh báo FTS index thiếu, vì vậy kết quả bên dưới còn được xác nhận trực tiếp từ source code.
>
> **Trạng thái 21/07/2026:** Đây là kết quả audit trước khi sửa. Các lỗi P0–P3 bên dưới đã được xử lý trong seed source; xem [VI. Trạng thái sau cập nhật](#vi-trạng-thái-sau-cập-nhật). Migration và database update chưa được tạo/chạy trong thay đổi này.

---

## I. Kết luận entity coverage

### ✅ 27/27 Domain entities đều có seed

| Nhóm             | Entity đã có seed                                                | Seed chính                                                                                       |
| ---------------- | ---------------------------------------------------------------- | ------------------------------------------------------------------------------------------------ |
| User/Auth        | `Users`, `RefreshTokens`, `ResetPasswords`, `EmailVerifications` | `UserSeed.cs`, `AuthSeed.cs`, `DemoSeed.cs`, `FPTSeed.cs`                                        |
| Event            | `Events`, `EventRoles`, `Rounds`, `Awards`, `Tracks`, `Topics`   | `EventSeed.cs`, `EventRoleSeed.cs`, `RoundSeed.cs`, `AwardSeed.cs`, `TrackSeed.cs`, `FPTSeed.cs` |
| Criteria         | `CriteriaTemplates`, `CriteriaItems`                             | `CriteriaSeed.cs`, `FPTSeed.cs`                                                                  |
| Team/Competition | `Teams`, `TeamDetails`, `RegisterTeams`, `RoundDetails`          | `TeamSeed.cs`, `RoundDetailSeed.cs`, `DemoSeed.cs`, `FPTSeed.cs`                                 |
| Assignment       | `AssignEvents`, `AssignTracks`                                   | `AssignmentSeed.cs`, `FPTSeed.cs`                                                                |
| Submission/Score | `Submissions`, `Scores`, `ScoreItems`                            | `SubmissionSeed.cs`, `ScoreSeed.cs`, `FPTSeed.cs`                                                |
| Leaderboard      | `LeaderBoards`, `LeaderBoardDetails`                             | `LeaderBoardSeed.cs`, `FPTSeed.cs`                                                               |
| Communication    | `Invitations`, `Notifications`, `MentorNotifications`, `Reports` | `NotificationSeed.cs`, `ReportSeed.cs`, `FPTSeed.cs`                                             |

### ✅ Tất cả seed methods đều được gọi

`AppDbContext.OnModelCreating` gọi đầy đủ từ `SeedEventRoles()` đến `SeedFPTData()`. Không có seed file bị tạo nhưng quên đăng ký.

> Kết luận: **không thiếu entity hoàn toàn**. Các vấn đề còn lại là thiếu trạng thái test, thiếu quan hệ, hoặc dữ liệu mâu thuẫn với nghiệp vụ.

---

# II. Lỗi dữ liệu/quan hệ cần sửa

## 🔴 Lỗi 1: Tất cả LeaderBoard seed đều thiếu `IsPublished = true`

**File:**

- `Hackathon.Infrastructure/Seed/01/LeaderBoardSeed.cs`
- `Hackathon.Infrastructure/Seed/02/FPTSeed.cs`

Seed 01 tạo **11 LeaderBoards** (1 SEAL + 10 paging); FPTSeed tạo 1 LeaderBoard cho Event 1. Không record nào set `IsPublished`, nên giá trị mặc định là `false`.

GitNexus xác nhận cả Admin và Student `GetEventLeaderboard` đều trả `null` khi:

```csharp
leaderBoard == null || leaderBoard.IsDisable || !leaderBoard.IsPublished
```

Chapter leaderboard cũng chỉ giữ:

```csharp
lb.IsPublished && !lb.IsDisable
```

**Ảnh hưởng:**

- 12 leaderboard đã seed không hiển thị qua Event leaderboard API.
- Chapter leaderboard năm 2024, 2025, 2026, 2027 rỗng dù có LeaderBoard records.
- `LeaderBoardDetails` đã seed cũng không giải quyết được vì API chặn trước ở `IsPublished`.

**Cần bổ sung:** `IsPublished = true` cho các leaderboard muốn public; giữ ít nhất một leaderboard `false` để test hidden/unpublished.

---

## 🔴 Lỗi 2: FPT Event 2 đã Published nhưng thiếu LeaderBoard

**File:** `Hackathon.Infrastructure/Seed/02/FPTSeed.cs`

- `Event2LeaderBoardId` đã khai báo.
- Event 2 Summer có `Status = Published` và 5 registrations Approved.
- Nhưng `SeedFPTData()` chỉ tạo LeaderBoard cho Event 1.

**Ảnh hưởng:** `GET event leaderboard` cho Event 2 luôn trả null vì `leaderBoard == null`.

**Cần bổ sung:** LeaderBoard cho `Event2Id`, `Year = 2026`, và chọn rõ `IsPublished` theo scenario muốn test.

---

## 🔴 Lỗi 3: Seed 01 có 12 Approved RegisterTeams nhưng thiếu `TrackId` và `TopicId`

**File:** `Hackathon.Infrastructure/Seed/01/TeamSeed.cs`

Hai RegisterTeams SEAL viết inline và 10 RegisterTeams paging tạo bởi `CreateRegisterTeam()` đều không set `TrackId`/`TopicId`. GitNexus xác nhận helper `CreateRegisterTeam()` thực tế set:

```csharp
Status = RegisterTeamStatusEnum.Approved
```

nhưng không set Track/Topic.

Student `CreateSubmission` đang active check:

```csharp
if (!registerTeam.TrackId.HasValue) ...
if (!registerTeam.TopicId.HasValue) ...
```

**Ảnh hưởng:** leader của cả 12 team Seed 01 không thể tạo submission mới qua API, dù seed đã chèn sẵn RoundDetails/Submissions cho các team này.

**Cần bổ sung:** `TrackId` và `TopicId` đúng event/track cho cả 12 RegisterTeams.

---

## 🔴 Lỗi 4: `Event.NumberRound` không khớp số Rounds thực tế ở 10 paging events

**File:**

- `Hackathon.Infrastructure/Seed/01/EventSeed.cs`
- `Hackathon.Infrastructure/Seed/01/RoundSeed.cs`

`CreateEvent()` set `NumberRound = 3` cho 10 paging events, nhưng mỗi event chỉ được seed đúng **1 round** (`RoundNo = 1`).

**Ảnh hưởng:**

- Event detail báo 3 rounds trong khi repository chỉ tìm được 1.
- Không test đúng `AssignToNextRound`, `RevertToPreviousRound`, round ordering, event score nhiều round.
- UI có thể hiển thị số round sai.

**Cần sửa một trong hai:**

1. Thêm đủ Round 2 và 3; hoặc
2. Đổi `NumberRound = 1` nếu paging events chỉ cần một round.

---

## 🔴 Lỗi 5: Score tồn tại nhưng Submission vẫn mang status `Submitted`

**File:**

- `Hackathon.Infrastructure/Seed/01/SubmissionSeed.cs`
- `Hackathon.Infrastructure/Seed/01/ScoreSeed.cs`
- `Hackathon.Infrastructure/Seed/02/FPTSeed.cs`

Các trường hợp bị ảnh hưởng:

- 4 submission SEAL có Scores.
- 6 submission FPT Event 1 có Scores.

Tất cả Submissions vẫn được seed `Status = Submitted`. Trong flow thật, GitNexus xác nhận `Judge.SubmitScore` set:

```csharp
submission.Status = SubmissionStatusEnum.Graded;
```

**Ảnh hưởng:**

- `isGraded` ở một số Judge API filter theo global `Submission.Status`, nên 10 bài đã có điểm vẫn hiện là chưa chấm.
- Dữ liệu Score và trạng thái Submission mâu thuẫn.

**Cần sửa:** set `SubmissionStatusEnum.Graded` cho submissions đã có ít nhất một Score; giữ một số `Submitted` không có score để test pending grading.

---

## 🔴 Lỗi 6: Seed 01 CriteriaTemplates đều inactive nên không thể chấm mới

**File:** `Hackathon.Infrastructure/Seed/01/CriteriaSeed.cs`

Cả 12 CriteriaTemplates đều:

```csharp
IsActive = false
```

GitNexus xác nhận `Judge.SubmitScore` bắt buộc `GetActiveByRoundIdAsync(...)` trả về template; nếu không sẽ lỗi `No Active Criteria Template Found for This Round`.

**Ảnh hưởng:** không thể test chấm điểm mới cho SEAL core và 10 paging events qua Judge API.

**Cần sửa:** mỗi round nên có đúng một template `IsActive = true` và `IsDisable = false`.

---

## 🔴 Lỗi 7: Seed 01 paging events có Submissions nhưng không có Judge assignment và Scores

**File:**

- `Hackathon.Infrastructure/Seed/01/AssignmentSeed.cs`
- `Hackathon.Infrastructure/Seed/01/ScoreSeed.cs`
- `Hackathon.Infrastructure/Seed/01/SubmissionSeed.cs`

10 paging events có:

- Mentor assignments + AssignTracks.
- Staff assignments.
- RoundDetails + Submissions.

Nhưng không có:

- Judge `AssignEvents`.
- Judge `AssignTracks`.
- Scores/ScoreItems cho 10 paging submissions.

**Ảnh hưởng:** không thể test Judge workflow hoặc leaderboard có điểm trên các event 2024–2027.

**Cần bổ sung:** Judge assignment/track và score data cho ít nhất một số paging events; không nhất thiết mọi event đều phải có score nếu muốn giữ scenario chưa chấm.

---

## 🔴 Lỗi 8: Thời điểm submission của 10 paging events sai năm và nằm ngoài event

**File:** `Hackathon.Infrastructure/Seed/01/SubmissionSeed.cs`

`CreateSubmission()` luôn dùng:

```csharp
SubmittedAt = SeedConstants.CreatedAt.AddDays(10) // 21/06/2026 UTC
```

Trong khi 10 paging events thuộc năm 2024, 2025, 2026, 2027 và round bắt đầu ngày 16/06 của năm tương ứng.

**Ảnh hưởng:**

- Submission của event 2024/2025 bị ghi nhận năm 2026.
- Submission của event 2027 xuất hiện trước event một năm.
- Ngay cả paging events 2026 cũng submit ngày 21/06, sau round kết thúc ngày 17/06.
- Date filtering và audit timeline cho submission không thực tế.

**Cần sửa:** `SubmittedAt` dựa trên `Round.StartSubmission/EndSubmission` của từng event.

---

## 🔴 Lỗi 9: Approved teams vẫn `CanEdit = true`

**File:**

- `Hackathon.Infrastructure/Seed/01/TeamSeed.cs`
- `Hackathon.Infrastructure/Seed/02/DemoSeed.cs`

Trong flow approve thật, team được khóa bằng `CanEdit = false`. Tuy nhiên:

- 12 teams Seed 01 có RegisterTeam Approved nhưng `CanEdit = true`.
- 5 DemoSeed teams có RegisterTeam Approved nhưng `CanEdit = true`.

FPT Event 1 làm đúng: Approved teams có `CanEdit = false`.

**Ảnh hưởng:** các team đã được duyệt vẫn có thể sửa team/member ở các API dựa vào `CanEdit`.

**Cần sửa:** set `CanEdit = false` cho các teams có registration Approved; giữ `true` cho Pending/Rejected teams.

---

## 🟠 Lỗi 10: Notification seed vi phạm cấu trúc target và thiếu Team/System scenario

**File:**

- `Hackathon.Infrastructure/Seed/01/NotificationSeed.cs`
- `Hackathon.Infrastructure/Seed/02/FPTSeed.cs`

Tất cả Notifications đều `TargetType = Personal`, nhưng nhiều record đồng thời set cả:

```csharp
UserId = ...
TeamId = ...
```

Theo flow create notification:

- Personal chỉ có `UserId`, `TeamId = null`.
- Team chỉ có `TeamId`, `UserId = null`.
- System có cả hai null.

**Ảnh hưởng:** seed Personal không giống dữ liệu tạo qua service, đồng thời không test được Student inbox Team/System scope.

**Cần sửa/bổ sung:** chuẩn hóa Personal `TeamId = null`; thêm Team notifications và System notifications, có cả Read/Unread.

---

## 🟠 Lỗi 11: Event seed không có status Draft hoặc Closed

**File:**

- `Hackathon.Infrastructure/Seed/01/EventSeed.cs`
- `Hackathon.Infrastructure/Seed/02/FPTSeed.cs`

Tất cả 13 Events đều `Published`; chỉ có 2 event Published bị `IsDisable = true`.

**Thiếu scenario:**

- Draft event để test setup-check, publish transition và visibility theo role.
- Closed event để test không update, không register/submit và chapter leaderboard lấy Published/Closed.

**Lưu ý:** Event 2024/2025 đã kết thúc theo thời gian nhưng status vẫn Published, nên dữ liệu status/timeline không nhất quán.

---

## 🟠 Lỗi 12: Không có dữ liệu Banned/Inactive/soft-deleted cho nhiều flow

Toàn bộ seed không có:

- User `Status = Inactive/Banned`, `BanReason`, hoặc `IsDisable = true`.
- TeamDetail `Status = Inactive`.
- RegisterTeam `Status = Banned` / `IsBanned = true`.
- Round/Track/Topic/Award/AssignEvent/AssignTrack/Team/Submission/Score/Notification/Report bị soft-delete.

FPTSeed có Pending/Rejected RegisterTeams và disabled CriteriaTemplates/Items, nhưng chưa đủ cho các API ban/unban, delete/restore và visibility filters.

**Cần bổ sung:** một số record riêng cho từng trạng thái, không biến toàn bộ dữ liệu thành active/happy path.

---

## 🟠 Lỗi 13: Invitation/Report/Auth chỉ seed một trạng thái

### Invitation

11 invitations đều `Pending`; tất cả dùng `LimitTime = 14/06/2026`, hiện đã hết hạn nhưng status vẫn Pending.

Thiếu: `Accepted`, `Rejected`, `Expired` đã được lưu trạng thái.

### Report

11 reports đều `Pending`; thiếu `Reject`, `Resolved`, `Canceled` để test filter và state update.

### EmailVerification

11 records đều `Verified`; thiếu `Pending`, `Expired`.

### ResetPassword/RefreshToken

- Tất cả ResetPasswords đều `IsUsed = false`, nhưng đã hết hạn từ 11/06/2026.
- Tất cả RefreshTokens đã hết hạn từ 18/06/2026.
- Không có active token hoặc used/revoked token scenario.

> Các auth token hash seed không nhất thiết dùng để đăng nhập thật, nhưng trạng thái đa dạng vẫn cần nếu API quản lý/cleanup được test bằng seed.

---

## 🟠 Lỗi 14: Không có scenario regrade/retake/mock score

Toàn bộ seed:

- `Submissions.IsRegrade = false` (mặc định).
- `Scores.IsRetake = false`.
- `Scores.RetakeFromScoreId = null`.
- `Scores.IsMock = false`.

Thiếu dữ liệu kiểm thử các field đang được trả trong Admin/Staff/Lecturer/Judge score/submission responses.

---

## 🟠 Lỗi 15: Không có submission nào được nhiều judge chấm

Công thức round score là trung bình `Scores.TotalScore` của nhiều judges, nhưng mọi seeded submission có tối đa một Score.

FPTSeed comment ghi “2 judges per track”, nhưng thực tế chỉ tạo một Judge assignment trên mỗi track và judge đó chấm hai teams trong track.

**Ảnh hưởng:** seed không kiểm thử được phép tính AVG nhiều judge, judge chưa chấm bị bỏ qua, và điểm 0 vẫn được tính.

**Cần bổ sung:** ít nhất một submission có 2–3 Scores từ các Judge AssignTracks khác nhau.

---

## 🟠 Lỗi 16: FPT Event 1 Round 2 chưa có dữ liệu thi đấu

FPT Event 1 có 2 rounds nhưng chỉ Round 1 có:

- RoundDetails.
- Submissions.
- Scores.

Round 2 không có team nào được advance, nên không kiểm thử được:

- `AssignToNextRound`/`RevertToPreviousRound` trên bộ FPT data.
- EventScore = tổng nhiều round.
- Leaderboard final round.

**Cần bổ sung:** chọn một số team Round 1 để seed RoundDetails + submissions + scores cho Round 2.

---

## 🟡 Lỗi 17: `LeaderBoardDetails.Score` là dữ liệu tĩnh không khớp nguồn tính điểm động

Seed 01 và FPTSeed tạo `LeaderBoardDetails` với Score/LevelAward tĩnh. Tuy nhiên GitNexus xác nhận current leaderboard flow tính lại điểm động từ:

`RegisterTeams -> RoundDetails -> latest Submission -> Scores -> ScoreItems`

`LeaderboardHelper` không đọc `LeaderBoardDetails` khi tạo Round/Event/Chapter response.

**Ảnh hưởng:**

- Người đọc seed dễ tưởng LeaderBoardDetails là nguồn ranking.
- Score tĩnh có thể khác score tính từ submission (ví dụ SEAL detail 90/82 trong khi event score từ hai rounds là 175/160).

**Khuyến nghị:** hoặc bỏ LeaderBoardDetails seed nếu không có API dùng, hoặc đồng bộ giá trị và ghi rõ đây chỉ là award snapshot.

---

# III. Coverage hiện có tốt

| Khu vực                  | Coverage tốt                                                                           |
| ------------------------ | -------------------------------------------------------------------------------------- |
| Entity existence         | 27/27 entities có seed và tất cả methods được đăng ký                                  |
| FPT Event 1              | Đầy đủ user/team/register/track/topic/criteria/assignment/submission/score cho Round 1 |
| FPT Event 2 registration | Có 5 Approved, 3 Pending, 2 Rejected                                                   |
| Assignment roles         | Có Staff, Mentor, Judge trên FPT events; Seed 01 có Staff/Mentor                       |
| Criteria lifecycle       | FPTSeed có active template và disabled backup templates/items                          |
| Notification read state  | Có Personal Read và Unread                                                             |
| Event visibility         | Có active Published và disabled Published events                                       |

---

# IV. Thứ tự ưu tiên sửa

| Priority | Việc cần sửa                                                                |
| -------- | --------------------------------------------------------------------------- |
| 🔴 P0    | Set `IsPublished` đúng cho LeaderBoards và thêm LeaderBoard cho FPT Event 2 |
| 🔴 P0    | Gán Track/Topic cho 12 Approved RegisterTeams Seed 01                       |
| 🔴 P0    | Đồng bộ Submission `Graded` khi đã có Score                                 |
| 🔴 P0    | Active đúng 1 CriteriaTemplate mỗi round Seed 01                            |
| 🔴 P1    | Đồng bộ `NumberRound` với số round thật; thêm Judge/Score cho paging data   |
| 🔴 P1    | Sửa `SubmittedAt` theo thời gian round/event                                |
| 🔴 P1    | Khóa `CanEdit=false` cho Approved teams                                     |
| 🟠 P2    | Thêm Draft/Closed event và state coverage Banned/Inactive/Deleted           |
| 🟠 P2    | Thêm Team/System notifications; chuẩn hóa Personal target IDs               |
| 🟠 P2    | Thêm invitation/report/auth status variants                                 |
| 🟠 P2    | Thêm multi-judge, Round 2, regrade/retake/mock scenarios                    |
| 🟡 P3    | Quyết định lại vai trò của `LeaderBoardDetails` tĩnh                        |

---

# V. Các kết luận cũ đã được sửa

- ❌ “10 RegisterTeams paging mặc định Pending” — **sai**. GitNexus và source xác nhận chúng được set `Approved`.
- ❌ “Seed 01 có 12 LeaderBoards” — **sai**. Seed 01 có 11; cộng 1 FPT Event 1 mới là 12 hiện có.
- ❌ “Events có đủ Published, Draft, Closed” — **sai tại thời điểm audit**. Seed sau cập nhật đã bổ sung/chuẩn hóa đủ ba trạng thái.
- ❌ “Các entity khác đều đủ” — chỉ đúng về việc entity có record; không đúng về trạng thái và quan hệ nghiệp vụ tại thời điểm audit.

---

# VI. Trạng thái sau cập nhật

| Nhóm | Thay đổi đã thực hiện |
| --- | --- |
| Leaderboard | Public hóa leaderboard dùng cho happy path; thêm leaderboard unpublished cho FPT Event 2; đồng bộ snapshot score với event score nhiều round. |
| RegisterTeam | Approved registrations có Track/Topic hợp lệ; không dựng GUID bằng substring; approved teams bị khóa `CanEdit = false`. |
| Event/Round | Paging event dùng `NumberRound = 1`; event lịch sử chuyển `Closed`; thêm Draft và soft-deleted event scenario; sửa submission window SEAL core. |
| Criteria | Seed 01 có đúng một active template trên mỗi round; backup/deleted templates vẫn inactive có chủ đích. |
| Submission/Score | Submission có score mang `Graded`; paging timestamps nằm trong round; paging events có Judge assignment, Score và ScoreItems. |
| FPT Round 2 | Giữ team chỉ ở Round 1 để test advance; thêm team Round 2 chưa submit để test revert; thêm team Round 2 đã chấm để test event score nhiều round. |
| Multi-judge | Mobile final submission có ba assigned judges, hai scores (80 và 100), judge chưa chấm không tham gia AVG. |
| Notification | Personal chỉ có UserId; bổ sung Team/System, Read/Unread và disabled notification scenarios. |
| State coverage | Bổ sung Inactive/Banned/disabled user, inactive member, banned/deleted registration, Draft/Closed/deleted event graph, deleted assignment/submission/score/report. |
| Invitation/Report/Auth | Bổ sung Accepted/Rejected/Expired invitation; Reject/Resolved/Canceled report; Pending/Expired verification; active/used/expired reset; active/revoked/expired refresh. |
| Regrade/Retake/Mock | Bổ sung một submission tách biệt có regrade, retake link và mock score; registration bị banned nên không làm sai leaderboard chuẩn. |
| Mật khẩu seed | Giữ cơ chế cũ: mật khẩu mặc định `string`, hash bằng BCrypt Enhanced SHA256 với Pepper trong `SeedHelper`. BCrypt dùng random salt nên cần review kỹ các `UpdateData` của user khi tạo migration. |

## Việc người chạy migration cần làm

1. Tạo migration mới từ model hiện tại; không sửa migration cũ đã áp dụng.
2. Review migration: thay đổi seed chỉ nên tạo `InsertData`, `UpdateData`, `DeleteData`; không nên có schema operation ngoài ý muốn.
3. Chạy `dotnet ef migrations has-pending-model-changes` sau khi tạo migration; kết quả mong đợi là không còn pending model changes.
4. Backup database trước khi chạy `database update`, vì model-managed data có thể update các record seed hiện hữu.
