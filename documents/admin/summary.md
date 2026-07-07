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
| POST | `/events/{eventId}/awards` | Tạo award (level tự động: max+1) |
| PATCH | `/events/{eventId}/awards/{awardId}` | Update award (ko đổi level) |
| PATCH | `/events/{eventId}/awards/{awardId}/delete` | Xóa mềm (LevelAward=0, IsDisable=true, higher -1) |

**Logic đặc biệt:**
- Level tự động: nếu chưa có level 1 → gán 1, nếu có → max + 1
- Update ko đổi được LevelAward
- **Xóa là vĩnh viễn — ko có restore**

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
| POST | `/tracks/{trackId}/topics` | Tạo topic |
| PATCH | `/topics/{topicId}` | Update topic |

---

## 📊 Criteria Template
**Controller:** `AdminCriteriaTemplateController` | **Service:** `Services/Admin/CriteriaTemplate/`

| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/rounds/{roundId}/criteria-templates` | Danh sách template của round |
| GET | `/criteria-templates/{templateId}/criteria-items` | Danh sách item của template |
| POST | `/rounds/{roundId}/criteria-templates` | Tạo template |
| PATCH | `/criteria-templates/{templateId}` | Update template |
| POST | `/criteria-templates/{templateId}/delete` | Xóa mềm |
| POST | `/criteria-templates/{templateId}/restore` | Khôi phục |

---

## 📈 Report
**Controller:** `AdminReportController` | **Service:** `Services/Admin/Report/`

| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/reports/recent` | Report gần đây |

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
| GET | `/assign/events/{eventId}/users/assigned` | User đã assign + event name + track (filter EventRole) |

**Logic đặc biệt:**
- **Staff** → chỉ được gán EventRole = `Staff`
- **Lecturer** → chỉ được gán EventRole = `Judge` hoặc `Mentor`
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
└── DependencyInjection.cs → AddAdminServices()
```
