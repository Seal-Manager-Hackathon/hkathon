# Staff API — Tổng quan

## Vai trò Staff

Staff là người được phân công vào một event để hỗ trợ quản lý và vận hành. Staff **không phải** là Admin — chỉ có thể xem và thao tác trên các event mà họ được phân công (thông qua bảng `AssignEvents`).

## Nguyên tắc xác thực và phân quyền

Tất cả API Staff đều yêu cầu:
1. **JWT token hợp lệ** — xác thực qua `Authorization: Bearer {token}`
2. **Role = Staff** — kiểm tra qua `IAuthorizationService.Authorize(RoleEnum.Staff)`
3. **Event assignment** — kiểm tra user hiện tại có trong `AssignEvents` với `EventId` tương ứng không
4. **Soft-delete filter** — dữ liệu có `IsDisable = true` được ẩn khỏi danh sách, nhưng response vẫn trả field `IsDisable` để FE biết trạng thái

## Danh sách API

### Event
| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/api/v1/staff/events` | [Lấy danh sách event được phân công (mọi EventRole)](event/get/staff.events.md) |
| GET | `/api/v1/staff/events/my-staff` | [Lấy danh sách event với EventRole=Staff](event/get/staff.events.my-staff.md) |
| GET | `/api/v1/staff/events/current` | [Lấy event đang diễn ra (được phân công)](event/get/staff.events.current.md) |
| GET | `/api/v1/staff/events/{eventId}` | [Xem chi tiết một event được phân công](event/get/staff.events.detail.md) |

### Round
| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/api/v1/staff/events/{eventId}/rounds` | [Lấy danh sách round của event](round/get/staff.rounds.md) |
| GET | `/api/v1/staff/events/{eventId}/rounds/{roundId}` | [Xem chi tiết round](round/get/staff.rounds.detail.md) |

### Track
| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/api/v1/staff/events/{eventId}/tracks` | [Lấy danh sách track của event](track/get/staff.tracks.md) |
| GET | `/api/v1/staff/tracks/{trackId}` | [Xem chi tiết track](track/get/staff.tracks.detail.md) |

### Topic
| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/api/v1/staff/tracks/{trackId}/topics` | [Lấy danh sách topic của track](topic/get/staff.topics.md) |
| GET | `/api/v1/staff/topics/{topicId}` | [Xem chi tiết topic](topic/get/staff.topics.detail.md) |

### Criteria Template
| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/api/v1/staff/events/{eventId}/rounds/{roundId}/criteria-templates` | [Lấy danh sách criteria template của round](criteria-template/get/staff.criteria-templates.md) |
| GET | `/api/v1/staff/events/{eventId}/criteria-templates/{criteriaTemplateId}/items` | [Lấy danh sách criteria items của template](criteria-template/get/criteria-items/staff.criteria-items.md) |

### Assign
| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/api/v1/staff/events/{eventId}/lecturers/available` | [Lấy danh sách Lecturer có thể phân công](assign/get/staff.assign.lecturers.available.md) |
| POST | `/api/v1/staff/events/{eventId}/assign/users` | [Phân công Lecturer vào event](assign/post/staff.assign.assign-user.md) |
| GET | `/api/v1/staff/events/{eventId}/assigned` | [Lấy danh sách người được phân công vào event](assign/get/staff.assign.assigned.md) |
| GET | `/api/v1/staff/events/{eventId}/assigned/{assignEventId}` | [Xem chi tiết một phân công](assign/get/staff.assign.assigned.detail.md) |
| POST | `/api/v1/staff/event-assigns/{assignEventId}/remove` | [Xóa mềm Lecturer khỏi event (cascade tracks)](assign/post/staff.assign.event-remove.md) |
| POST | `/api/v1/staff/event-assigns/{assignEventId}/restore` | [Khôi phục Lecturer vào event (cascade tracks)](assign/post/staff.assign.event-restore.md) |
| POST | `/api/v1/staff/event-assigns/{assignEventId}/tracks` | [Phân công track cho Lecturer](assign/post/staff.assign.tracks.assign.md) |
| POST | `/api/v1/staff/event-assigns/{assignEventId}/tracks/{trackId}/remove` | [Xóa track khỏi Lecturer](assign/post/staff.assign.tracks.remove.md) |
| POST | `/api/v1/staff/event-assigns/{assignEventId}/tracks/{trackId}/restore` | [Khôi phục track cho Lecturer](assign/post/staff.assign.tracks.restore.md) |

### Register Team
| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/api/v1/staff/events/{eventId}/register-teams` | [DS teams đăng ký của event](register-team/get/staff.events.register-teams.md) |
| GET | `/api/v1/staff/register-teams/{registerTeamId}` | [Xem chi tiết team](register-team/get/staff.register-teams.detail.md) |
| PATCH | `/api/v1/staff/register-teams/{registerTeamId}` | [Cập nhật team](register-team/patch/staff.register-teams.update.md) |
| POST | `/api/v1/staff/register-teams/{registerTeamId}/approve` | [Duyệt team](register-team/post/staff.register-teams.approve.md) |
| POST | `/api/v1/staff/register-teams/{registerTeamId}/reject` | [Từ chối team](register-team/post/staff.register-teams.reject.md) |
| POST | `/api/v1/staff/register-teams/{registerTeamId}/ban` | [Cấm team](register-team/post/staff.register-teams.ban.md) |
| POST | `/api/v1/staff/register-teams/{registerTeamId}/unban` | [Bỏ cấm](register-team/post/staff.register-teams.unban.md) |
| POST | `/api/v1/staff/register-teams/{registerTeamId}/assign-next-round` | [Chuyển vòng](register-team/post/staff.register-teams.assign-next-round.md) |
| POST | `/api/v1/staff/register-teams/{registerTeamId}/revert-previous-round` | [Về vòng trước](register-team/post/staff.register-teams.revert-previous-round.md) |
| POST | `/api/v1/staff/register-teams/{registerTeamId}/assign-track-topic` | [Gán track/topic](register-team/post/staff.register-teams.assign-track-topic.md) |
| POST | `/api/v1/staff/register-teams/{registerTeamId}/remove-track-topic` | [Xóa track/topic](register-team/post/staff.register-teams.remove-track-topic.md) |
| GET | `/api/v1/staff/teams/{teamId}/register-teams` | [DS register teams của 1 team](register-team/get/staff.teams.register-teams.md) |
| GET | `/api/v1/staff/users/{userId}/events` | [DS event của 1 user](register-team/get/staff.users.events.md) |

### Submission
| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/api/v1/staff/submissions/{submissionId}` | [Xem chi tiết submission](submission/get/staff.submissions.detail.md) |
| GET | `/api/v1/staff/events/{eventId}/submissions` | [DS submission của event](submission/get/staff.submissions.md) |
| GET | `/api/v1/staff/rounds/{roundId}/submissions` | [DS submission của round](submission/get/staff.submissions.by-round.md) |
| GET | `/api/v1/staff/register-teams/{registerTeamId}/submissions` | [DS submission của team](submission/get/staff.submissions.by-register-team.md) |
| GET | `/api/v1/staff/tracks/{trackId}/submissions` | [DS submission của track](submission/get/staff.submissions.by-track.md) |

### Score
| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/api/v1/staff/scores/{scoreId}` | [Xem chi tiết score](score/get/staff.scores.detail.md) |
| GET | `/api/v1/staff/submissions/{submissionId}/grader-scores` | [DS điểm của grader](score/get/staff.scores.grader-scores.md) |
| GET | `/api/v1/staff/scores/{scoreId}/items` | [DS score items](score/get/staff.scores.items.md) |
| GET | `/api/v1/staff/rounds/{roundId}/register-teams/{registerTeamId}/scores` | [Điểm team trong round](score/get/staff.scores.team-round.md) |
| GET | `/api/v1/staff/score-items/{scoreItemId}` | [Xem chi tiết score item](score/get/staff.score-items.detail.md) |

### Award
| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/api/v1/staff/events/{eventId}/awards` | [Danh sách award của event (chỉ IsDisable=false)](award/get/staff.awards.list.md) |
| GET | `/api/v1/staff/events/{eventId}/awards/{awardId}` | [Chi tiết award](award/get/staff.awards.detail.md) |

### Leaderboard
| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/api/v1/staff/rounds/{roundId}/leaderboard` | [BXH round](leaderboard/get/staff.rounds.leaderboard.md) |
| GET | `/api/v1/staff/events/{eventId}/leaderboard` | [BXH event](leaderboard/get/staff.events.leaderboard.md) |
| GET | `/api/v1/staff/events/chapter/{year}/leaderboard` | [BXH chapter theo năm](leaderboard/get/staff.events.chapter-leaderboard.md) |
| POST | `/api/v1/staff/events/chapter/{year}/leaderboard/publish` | [Công bố leader board chapter](leaderboard/post/staff.chapter.publish.md) |
| POST | `/api/v1/staff/events/chapter/{year}/leaderboard/hide` | [Ẩn leader board chapter](leaderboard/post/staff.chapter.hide.md) |
| POST | `/api/v1/staff/events/{eventId}/leaderboard/publish` | [Công bố leader board event](leaderboard/post/staff.leaderboard.events.publish.md) |
| POST | `/api/v1/staff/events/{eventId}/leaderboard/hide` | [Ẩn leader board event](leaderboard/post/staff.leaderboard.events.hide.md) |

### Notification
| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/api/v1/staff/notifications` | [DS thông báo](notification/get/staff.notifications.md) |
| GET | `/api/v1/staff/notifications/recent` | [Thông báo gần đây](notification/get/staff.notifications.recent.md) |
| GET | `/api/v1/staff/notifications/{notificationId}` | [Chi tiết thông báo](notification/get/staff.notifications.detail.md) |
| POST | `/api/v1/staff/notifications` | [Tạo thông báo](notification/post/staff.notifications.create.md) |
| PATCH | `/api/v1/staff/notifications/{notificationId}` | [Cập nhật thông báo](notification/patch/staff.notifications.update.md) |
| POST | `/api/v1/staff/notifications/{notificationId}/delete` | [Xóa mềm thông báo](notification/post/staff.notifications.delete.md) |
| POST | `/api/v1/staff/notifications/{notificationId}/restore` | [Khôi phục thông báo](notification/post/staff.notifications.restore.md) |

### Report
| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/api/v1/staff/reports` | [DS báo cáo](report/get/staff.reports.md) |
| GET | `/api/v1/staff/reports/{reportId}` | [Chi tiết báo cáo](report/get/staff.reports.detail.md) |
| GET | `/api/v1/staff/reports/recent` | [Báo cáo gần đây](report/get/staff.reports.recent.md) |
| PATCH | `/api/v1/staff/reports/{reportId}/status` | [Cập nhật trạng thái](report/patch/staff.reports.status.md) |

### Team
| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/api/v1/staff/teams` | [Danh sách teams](team/get/staff.teams.md) |
| GET | `/api/v1/staff/teams/{teamId}` | [Chi tiết team](team/get/staff.teams.detail.md) |
| GET | `/api/v1/staff/teams/{teamId}/events` | [DS event team đã tham gia](team/get/staff.teams.events.md) |

### User
| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/api/v1/staff/users` | [Danh sách users](user/get/staff.users.md) |
| GET | `/api/v1/staff/users/{userId}` | [Chi tiết user](user/get/staff.users.detail.md) |
| GET | `/api/v1/staff/users/{userId}/teams` | [Team của user](user/get/staff.users.teams.md) |
