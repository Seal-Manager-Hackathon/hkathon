# Hệ thống API — Admin

**Base URL:** `api/v1/admin`

---

## 📋 Event
**Controller:** `AdminEventController` | **Service:** `Services/Admin/Event/`

| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/events` | Danh sách event (pagination, search, filter status/disable) |
| GET | `/events/recent` | 10 event gần nhất |
| GET | `/events/count` | Đếm số lượng event (filter theo status) |
| GET | `/events/{eventId}` | Chi tiết 1 event |
| GET | `/events/{eventId}/setup-check` | Kiểm tra event đã setup đủ chưa (fields + round) |
| POST | `/events` | Tạo event mới (status = Draft, IsDisable = false) |
| PATCH | `/events/{eventId}` | Update event (IsDisable độc lập, Draft→Published check setup) |
| POST | `/events/{eventId}/delete` | Xóa mềm (IsDisable = true) |
| POST | `/events/{eventId}/restore` | Khôi phục (IsDisable = false) |

**Logic đặc biệt:**
- Tạo event: `IsDisable = false`, `Status = Draft`
- Publish (Draft→Published): gọi setup-check, nếu thiếu field → throw lỗi
- IsDisable hoàn toàn độc lập với status

---

## 🏆 Award
**Controller:** `AdminAwardController` | **Service:** `Services/Admin/Award/`

| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/events/{eventId}/awards` | Danh sách award của event (search, filter disable, sort level) |
| GET | `/events/{eventId}/awards/{awardId}` | Chi tiết 1 award |
| POST | `/events/{eventId}/awards` | Tạo award (level tự động: max+1) |
| PATCH | `/events/{eventId}/awards/{awardId}` | Update award (ko đổi level) |
| POST | `/events/{eventId}/awards/{awardId}/swap` | Swap thứ tự level giữa 2 award |
| POST | `/events/{eventId}/awards/{awardId}/delete` | Xóa mềm (LevelAward=0, IsDisable=true, higher -1) |
| POST | `/events/{eventId}/awards/{awardId}/restore` | Khôi phục (IsDisable=false, LevelAward = max+1) |

**Logic đặc biệt:**
- Level tự động: nếu chưa có level 1 → gán 1, nếu có → max + 1
- Update ko đổi được LevelAward
- **Xóa mềm — có restore** (POST .../delete → IsDisable=true, LevelAward=0; POST .../restore → IsDisable=false, LevelAward = max+1)

---

## 🌀 Round
**Controller:** `AdminRoundController` | **Service:** `Services/Admin/Round/`

| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/events/{eventId}/rounds` | Danh sách round của event (filter roundNo, disable) |
| GET | `/events/{eventId}/rounds/max-round-no` | RoundNo lớn nhất hiện tại |
| GET | `/rounds/{roundId}` | Chi tiết 1 round (kèm event name) |
| POST | `/events/{eventId}/rounds` | Tạo round mới |
| PATCH | `/rounds/{roundId}` | Update round |
| POST | `/events/{eventId}/rounds/{roundId}/swap` | Swap thứ tự 2 round (ko swap với deleted) |
| POST | `/rounds/{roundId}/delete` | Xóa mềm (RoundNo=0, IsDisable=true, higher -1, NumberRound giảm) |
| POST | `/rounds/{roundId}/restore` | Khôi phục (IsDisable=false, RoundNo = max+1, NumberRound tăng) |

**Logic đặc biệt:**
- Delete: `RoundNo=0`, `IsDisable=true`, các round cao hơn -1, NumberRound giảm
- Restore: chỉ restore được khi `IsDisable=true`, set `IsDisable=false`, RoundNo mới

---

## 👥 User
**Controller:** `AdminUserController` | **Service:** `Services/Admin/User/`

| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/users` | Danh sách user (pagination, search, filter role/disable/verified/banned) |
| GET | `/users/recent` | 10 user gần nhất |
| GET | `/users/count` | Đếm số lượng user (filter role) |
| GET | `/users/{userId}` | Chi tiết 1 user |
| GET | `/users/{userId}/teams` | Danh sách team của user |
| POST | `/users` | Tạo user mới |
| PATCH | `/users/{userId}` | Update thông tin user |
| POST | `/users/{userId}/delete` | Xóa mềm (IsDisable=true) |
| POST | `/users/{userId}/restore` | Khôi phục (IsDisable=false) |
| POST | `/users/{userId}/ban` | Ban user (BanReason, BannedAt, IsDisable vẫn false) |
| POST | `/users/{userId}/unban` | Unban (xóa BanReason, BannedAt) |

**Logic đặc biệt:**
- Ban: `IsDisable` vẫn `false` — banned user vẫn visible
- Delete: `IsDisable = true` — ẩn hoàn toàn với non-admin

---

## 📢 Notification
**Controller:** `AdminNotificationController` | **Service:** `Services/Admin/Notification/`

| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/notifications` | Danh sách notification (pagination) |
| GET | `/notifications/recent` | Notification gần đây |
| GET | `/notifications/{notificationId}` | Chi tiết 1 notification |
| POST | `/notifications` | Tạo notification (System/Team/Personal) |
| PATCH | `/notifications/{notificationId}` | Update notification |
| POST | `/notifications/{notificationId}/delete` | Xóa mềm |
| POST | `/notifications/{notificationId}/restore` | Khôi phục |

**Logic đặc biệt — tạo notification theo type:**
- **System:** 1 notification, ko UserId, ko TeamId
- **Team:** gán TeamId, bỏ qua UserId
- **Personal:** gán UserId, bỏ qua TeamId

---

## 👥 Team
**Controller:** `AdminTeamController` | **Service:** `Services/Admin/Team/`

| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/teams` | Danh sách team (pagination) |
| GET | `/teams/count` | Đếm số team |
| GET | `/teams/{teamId}` | Chi tiết 1 team |
| PATCH | `/teams/{teamId}` | Update team |
| POST | `/teams/{teamId}/delete` | Xóa mềm |
| POST | `/teams/{teamId}/restore` | Khôi phục |
| POST | `/teams/{teamId}/lock` | Khóa team (CanEdit = false) |
| POST | `/teams/{teamId}/unlock` | Mở khóa team (CanEdit = true) |

---

## 📄 Submission (Bài nộp)
**Controller:** `AdminSubmissionController` | **Service:** `Services/Admin/Submission/`

| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/events/{eventId}/submissions` | Danh sách bài nộp (filter round/track/topic/registerTeam, search keyword, last + records) |
| GET | `/rounds/{roundId}/submissions` | Danh sách bài nộp trong 1 round (search keyword) |
| GET | `/register-teams/{registerTeamId}/submissions` | Bài nộp của team trong event (lọc theo round, ko truyền = all) |
| GET | `/tracks/{trackId}/submissions` | Bài nộp theo track |
| GET | `/submissions/{submissionId}` | Chi tiết 1 bài nộp (thông tin chung, tính tổng scores + judgeCount) |

---

## 🏅 Leaderboard (Bảng xếp hạng)
**Controller:** `AdminLeaderboardController` | **Service:** `Services/Admin/Leaderboard/`

| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/rounds/{roundId}/leaderboard` | Bảng xếp hạng round (scopeScore từng team, sắp xếp theo điểm) |
| GET | `/events/{eventId}/leaderboard` | Bảng xếp hạng event (eventScore = weighted avg round, weight=1) |
| GET | `/events/chapter/{year}/leaderboard` | Bảng xếp hạng chapter (chapterScore = AVG eventScores trong năm) |

---

## 📊 Score (Điểm)
**Controller:** `AdminScoreController` | **Service:** `Services/Admin/Score/`

**Cấu trúc phân cấp điểm:**
```
Submissions (bài nộp)
  └── Scores (lượt chấm — 1 judge / record)
        └── ScoreItems (điểm từng tiêu chí)
```
- **Score** = 1 lượt chấm của 1 judge, `TotalScore` = SUM(ScoreItems)
- **scopeScore** (điểm team trong round) = SUM(Scores.TotalScore) của submission cuối

| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/scores/{scoreId}` | Chi tiết 1 lượt chấm (score) — **ko kèm items** |
| GET | `/scores/{scoreId}/items` | Score items của 1 lượt chấm (phân trang) |
| GET | `/score-items/{scoreItemId}` | Chi tiết 1 score item (điểm + comment + người chấm) |
| GET | `/submissions/{submissionId}/grader-scores` | Danh sách từng lượt chấm của bài nộp (phân trang) |
| GET | `/rounds/{roundId}/register-teams/{registerTeamId}/scores` | **scopeScore** team trong round (submission cuối) |

---

## 📝 Register Team
**Controller:** `AdminRegisterTeamController` | **Service:** `Services/Admin/RegisterTeam/`

| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/events/{eventId}/register-teams` | Danh sách đăng ký của event |
| GET | `/teams/{teamId}/register-teams` | Danh sách đăng ký của team |
| GET | `/register-teams/{registerTeamId}` | Chi tiết 1 đăng ký |
| GET | `/users/{userId}/events` | Event của user (qua register team) |
| PATCH | `/register-teams/{registerTeamId}` | Update đăng ký |
| POST | `/register-teams/{registerTeamId}/approve` | Duyệt đăng ký |
| POST | `/register-teams/{registerTeamId}/reject` | Từ chối (kèm lý do) |
| POST | `/register-teams/{registerTeamId}/ban` | Ban đăng ký |
| POST | `/register-teams/{registerTeamId}/unban` | Unban đăng ký |
| POST | `/register-teams/{registerTeamId}/assign-next-round` | Chuyển team lên vòng tiếp theo |
| POST | `/register-teams/{registerTeamId}/revert-previous-round` | Đưa team về vòng trước |
| POST | `/register-teams/{registerTeamId}/assign-track-topic` | Gán track + topic cho register team |
| POST | `/register-teams/{registerTeamId}/remove-track-topic` | Gỡ track + topic khỏi register team |

---

## 🎯 Track
**Controller:** `AdminTrackController` | **Service:** `Services/Admin/Track/`

| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/events/{eventId}/tracks` | Danh sách track của event |
| GET | `/events/{eventId}/tracks/{trackId}` | Chi tiết 1 track |
| POST | `/events/{eventId}/tracks` | Tạo track |
| PATCH | `/events/{eventId}/tracks/{trackId}` | Update track |
| POST | `/tracks/{trackId}/delete` | Xóa mềm |
| POST | `/tracks/{trackId}/restore` | Khôi phục |

---

## 📌 Topic
**Controller:** `AdminTopicController` | **Service:** `Services/Admin/Topic/`

| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/tracks/{trackId}/topics` | Danh sách topic của track |
| GET | `/topics/{topicId}` | Chi tiết 1 topic |
| POST | `/tracks/{trackId}/topics` | Tạo topic |
| PATCH | `/topics/{topicId}` | Update topic |
| POST | `/topics/{topicId}/delete` | Xóa mềm topic |
| POST | `/topics/{topicId}/restore` | Khôi phục topic |

---

## 📊 Criteria Template
**Controller:** `AdminCriteriaTemplateController` | **Service:** `Services/Admin/CriteriaTemplate/`

| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/rounds/{roundId}/criteria-templates` | Danh sách template của round (ko kèm items) |
| GET | `/criteria-templates/{templateId}` | Chi tiết 1 template (kèm items) |
| GET | `/criteria-templates/{templateId}/criteria-items` | Danh sách item của template |
| GET | `/criteria-items/{itemId}` | Chi tiết 1 criteria item |
| POST | `/rounds/{roundId}/criteria-templates` | Tạo template |
| PATCH | `/criteria-templates/{templateId}` | Update template |
| POST | `/criteria-templates/{templateId}/delete` | Xóa mềm |
| POST | `/criteria-templates/{templateId}/restore` | Khôi phục |
| POST | `/criteria-templates/{templateId}/activate` | Active template (chỉ 1 template/round được active) |
| POST | `/criteria-templates/{templateId}/criteria-items` | Tạo criteria item mới |
| PATCH | `/criteria-items/{itemId}` | Update criteria item |
| POST | `/criteria-items/{itemId}/delete` | Xóa mềm criteria item |
| POST | `/criteria-items/{itemId}/restore` | Khôi phục criteria item |

---

## 📈 Report
**Controller:** `AdminReportController` | **Service:** `Services/Admin/Report/`

| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/reports` | Danh sách report (search keyword, filter status, phân trang) |
| GET | `/reports/{reportId}` | Chi tiết 1 report (đầy đủ fields) |
| GET | `/reports/recent` | Report gần đây |
| PATCH | `/reports/{reportId}/status` | Cập nhật status report |

---

## 🔄 Assign (Phân công)
**Controller:** `AdminAssignController` | **Service:** `Services/Admin/Assign/`

| Method | Route | Chức năng |
|--------|-------|-----------|
| POST | `/assign/events/{eventId}/assign/users` | Phân công user (Staff/Lecturer) vào event kèm EventRole |
| PATCH | `/assign/event-assigns/{assignEventId}/event-role` | Gán event role (Judge/Mentor) cho lecturer đã assign |
| GET | `/assign/events/{eventId}/staff/available` | Staff chưa assign, chưa ban, chưa disable |
| GET | `/assign/events/{eventId}/lecturers/available` | Lecturer chưa assign, chưa ban, chưa disable |
| GET | `/assign/events/{eventId}/assigned` | User đã assign trong event (filter EventRole) |
| POST | `/assign/event-assigns/{assignEventId}/tracks` | Gán track cho user (chỉ Lecturer, Staff ko được) |
| POST | `/assign/event-assigns/{assignEventId}/tracks/{trackId}/remove` | Xóa mềm track khỏi user |
| POST | `/assign/event-assigns/{assignEventId}/tracks/{trackId}/restore` | Khôi phục track đã xóa |
| POST | `/assign/event-assigns/{assignEventId}/remove` | Xóa mềm assign event |
| POST | `/assign/event-assigns/{assignEventId}/restore` | Khôi phục assign event |

**Logic đặc biệt:**
- **Staff** → chỉ được gán EventRole = `Staff`, **không được assign track**
- **Lecturer** → chỉ được gán EventRole = `Judge` hoặc `Mentor`, được assign track
- **Student/Admin** → ko được assign

---

## Cấu trúc code

```
Services/Admin/
├── Award/              → AdminAwardController
├── Event/              → AdminEventController
├── Round/              → AdminRoundController
├── User/               → AdminUserController
├── Notification/       → AdminNotificationController
├── Team/               → AdminTeamController
├── RegisterTeam/       → AdminRegisterTeamController
├── Track/              → AdminTrackController
├── Topic/              → AdminTopicController
├── CriteriaTemplate/   → AdminCriteriaTemplateController
├── Report/             → AdminReportController
├── Assign/             → AdminAssignController
├── Leaderboard/        → AdminLeaderboardController
└── DependencyInjection.cs → AddAdminServices()
```
