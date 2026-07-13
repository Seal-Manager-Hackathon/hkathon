# Tài liệu Thiết kế Database (PostgreSQL & .NET Mapping)

Tài liệu này mô tả chi tiết cấu trúc cơ sở dữ liệu PostgreSQL tương ứng với các Entity trong dự án Hackathon, bao gồm định nghĩa các trường thừa kế từ `BaseEntity<Guid>` và `IAuditableEntity`, kiểu dữ liệu ánh xạ giữa PostgreSQL và C# (.NET), cùng bản đồ quan hệ (không trùng lặp) giữa các bảng.

---

## 1. Cấu trúc trường thừa kế chung (Common Inherited Fields)

Tất cả các thực thể trong Domain đều kế thừa từ lớp trừu tượng `BaseEntity<Guid>` và triển khai interface `IAuditableEntity`. Do đó, mỗi bảng trong cơ sở dữ liệu đều tự động bao gồm 4 cột sau:

| Tên trường (Database Column) | Kiểu dữ liệu PostgreSQL | Kiểu dữ liệu .NET / C# | Mô tả / Ràng buộc |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính (Primary Key), sinh ngẫu nhiên |
| `is_disable` | `boolean` | `bool` | Trạng thái vô hiệu hóa của bản ghi (mặc định: `false`) |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | Thời gian tạo bản ghi |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | Thời gian cập nhật bản ghi lần cuối |

---

## 2. Danh sách các bảng và cấu trúc chi tiết (Entities to DB Tables)

### 2.1. Bảng `users` (Thực thể `Users`)
Lưu trữ thông tin người dùng hệ thống.
*Lưu ý:* Trường `role` không cấu hình chuyển đổi chuỗi trong `AppDbContext` nên được lưu dạng `integer`. Trường `status` được cấu hình `.HasConversion<string>()` nên được lưu dạng `text`.

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `email` | `text` | `string` | `NOT NULL` |
| `hash_password` | `text` | `string` | `NOT NULL` |
| `first_name` | `text` | `string` | `NOT NULL` (Mặc định `""`) |
| `last_name` | `text` | `string` | `NOT NULL` (Mặc định `""`) |
| `phone_number` | `text` | `string` | `NOT NULL` (Mặc định `""`) |
| `avatar_url` | `text` | `string` | `NOT NULL` (Mặc định `""`) |
| `bio` | `text` | `string?` | `NULL` |
| `address` | `text` | `string?` | `NULL` (Mặc định `""`) |
| `date_of_birth` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `student_id` | `text` | `string` | `NOT NULL` (Mặc định `""`) |
| `college` | `text` | `string` | `NOT NULL` (Mặc định `""`) |
| `img_url` | `text` | `string?` | `NULL` |
| `link_url` | `text` | `string?` | `NULL` |
| `role` | `integer` | `RoleEnum` | `NOT NULL` |
| `verify_email_at` | `timestamp with time zone` | `DateTimeOffset?` | `NULL` |
| `status` | `text` | `UserStatusEnum?` | `NULL` (Lưu dưới dạng chuỗi tên Enum) |
| `ban_reason` | `text` | `string?` | `NULL` |
| `banned_at` | `timestamp with time zone` | `DateTimeOffset?` | `NULL` |
| `is_verified` | `boolean` | `bool?` | `NULL` |

### 2.2. Bảng `refresh_tokens` (Thực thể `RefreshTokens`)
Lưu trữ token làm mới phiên đăng nhập của người dùng.

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `user_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `users` |
| `refresh_token_hash` | `text` | `string` | `NOT NULL` |
| `ip_address` | `text` | `string?` | `NULL` |
| `user_agent` | `text` | `string?` | `NULL` |
| `device_label` | `text` | `string?` | `NULL` |
| `expired_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `revoked_at` | `timestamp with time zone` | `DateTimeOffset?` | `NULL` |

### 2.3. Bảng `reset_passwords` (Thực thể `ResetPasswords`)
Lưu thông tin token đặt lại mật khẩu của người dùng.

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `user_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `users` |
| `token_hash` | `text` | `string` | `NOT NULL` |
| `is_used` | `boolean` | `bool` | `NOT NULL` |
| `expires_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |

### 2.4. Bảng `email_verifications` (Thực thể `EmailVerifications`)
Quản lý các mã xác thực email gửi cho người dùng.

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `user_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `users` |
| `token_hash` | `text` | `string` | `NOT NULL` |
| `expired_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `status` | `text` | `EmailVerificationStatusEnum?` | `NULL` (Lưu dưới dạng chuỗi tên Enum) |

### 2.5. Bảng `events` (Thực thể `Events`)
Thông tin về các sự kiện Hackathon.
*Lưu ý:* Trường `season` được lưu dạng `integer` trong khi `status` lưu dạng `text` (được cấu hình chuyển đổi chuỗi).

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `name` | `text` | `string` | `NOT NULL` |
| `description` | `text` | `string?` | `NULL` |
| `start_time` | `timestamp with time zone` | `DateTimeOffset?` | `NULL` |
| `end_time` | `timestamp with time zone` | `DateTimeOffset?` | `NULL` |
| `register_limit_time` | `timestamp with time zone` | `DateTimeOffset?` | `NULL` |
| `limit_team` | `integer` | `int?` | `NULL` |
| `min_member` | `integer` | `int?` | `NULL` |
| `max_member` | `integer` | `int?` | `NULL` |
| `status` | `text` | `EventStatusEnum?` | `NULL` (Lưu dưới dạng chuỗi tên Enum) |
| `number_round` | `integer` | `int?` | `NULL` |
| `season` | `integer` | `SeasonEnum?` | `NULL` |

### 2.6. Bảng `rounds` (Thực thể `Rounds`)
Lưu các vòng thi của từng sự kiện.

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `event_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `events` |
| `name` | `text` | `string` | `NOT NULL` |
| `description` | `text` | `string?` | `NULL` |
| `round_no` | `integer` | `int?` | `NULL` |
| `start_time` | `timestamp with time zone` | `DateTimeOffset?` | `NULL` |
| `end_time` | `timestamp with time zone` | `DateTimeOffset?` | `NULL` |
| `start_submission` | `timestamp with time zone` | `DateTimeOffset?` | `NULL` |
| `end_submission` | `timestamp with time zone` | `DateTimeOffset?` | `NULL` |
| `limit_team` | `integer` | `int?` | `NULL` |

### 2.7. Bảng `criteria_templates` (Thực thể `CriteriaTemplates`)
Các mẫu tiêu chí đánh giá cho từng vòng thi.

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `round_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `rounds` |
| `title` | `text` | `string` | `NOT NULL` |
| `description` | `text` | `string?` | `NULL` |
| `is_active` | `boolean` | `bool` | `NOT NULL` |

### 2.8. Bảng `criteria_items` (Thực thể `CriteriaItems`)
Chi tiết các tiêu chí chấm điểm trong một mẫu tiêu chí.

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `criteria_template_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `criteria_templates` |
| `name` | `text` | `string` | `NOT NULL` |
| `description` | `text` | `string?` | `NULL` |
| `score` | `numeric` | `decimal` | `NOT NULL` |

### 2.9. Bảng `tracks` (Thực thể `Tracks`)
Các track/chủ đề lớn trong sự kiện.

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `event_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `events` |
| `title` | `text` | `string` | `NOT NULL` |
| `description` | `text` | `string?` | `NULL` |
| `max_team` | `integer` | `int?` | `NULL` |

### 2.10. Bảng `topics` (Thực thể `Topics`)
Các đề tài chi tiết thuộc về một Track.

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `track_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `tracks` |
| `title` | `text` | `string` | `NOT NULL` |
| `description` | `text` | `string?` | `NULL` |

### 2.11. Bảng `awards` (Thực thể `Awards`)
Cơ cấu giải thưởng của sự kiện.

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `event_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `events` |
| `name` | `text` | `string` | `NOT NULL` |
| `description` | `text` | `string?` | `NULL` |
| `level_award` | `integer` | `int` | `NOT NULL` |
| `number_of_award` | `integer` | `int?` | `NULL` (Mặc định `1`) |
| `prize` | `numeric` | `decimal?` | `NULL` |

### 2.12. Bảng `leader_boards` (Thực thể `LeaderBoards`)
Bảng xếp hạng tổng thể của sự kiện (quan hệ 1-1 với `events`).

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `event_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại độc nhất (Unique index) đến `events` |
| `year` | `integer` | `int?` | `NULL` |
| `is_locked` | `boolean` | `bool` | `NOT NULL` |
| `is_published` | `boolean` | `bool` | `NOT NULL` |

### 2.13. Bảng `leader_board_details` (Thực thể `LeaderBoardDetails`)
Chi tiết các thứ hạng và điểm của từng đội trên bảng xếp hạng.

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `leader_board_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `leader_boards` |
| `team_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `teams` |
| `score` | `numeric` | `decimal?` | `NULL` |
| `level_award` | `integer` | `int?` | `NULL` |

### 2.14. Bảng `teams` (Thực thể `Teams`)
Lưu thông tin các đội thi.

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `name` | `text` | `string` | `NOT NULL` |
| `can_edit` | `boolean` | `bool` | `NOT NULL` |

### 2.15. Bảng `team_details` (Thực thể `TeamDetails`)
Chi tiết thành viên trong đội và vai trò trưởng nhóm.

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `team_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `teams` |
| `user_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `users` |
| `is_leader` | `boolean` | `bool` | `NOT NULL` |
| `status` | `text` | `TeamDetailStatusEnum?` | `NULL` (Lưu dưới dạng chuỗi tên Enum) |

### 2.16. Bảng `register_teams` (Thực thể `RegisterTeams`)
Thông tin đăng ký sự kiện của các đội thi (liên kết với sự kiện, track và topic).

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `team_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `teams` |
| `event_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `events` |
| `track_id` | `uuid` | `Guid?` | `NULL`, Khóa ngoại đến `tracks` |
| `topic_id` | `uuid` | `Guid?` | `NULL`, Khóa ngoại đến `topics` |
| `description` | `text` | `string?` | `NULL` |
| `rejection_reason` | `text` | `string?` | `NULL` |
| `status` | `text` | `RegisterTeamStatusEnum?` | `NULL` (Lưu dưới dạng chuỗi tên Enum) |
| `is_banned` | `boolean` | `bool` | `NOT NULL` |

### 2.17. Bảng `round_details` (Thực thể `RoundDetails`)
Lưu tiến trình của đội đăng ký đi qua từng vòng thi.

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `round_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `rounds` |
| `register_team_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `register_teams` |

### 2.18. Bảng `submissions` (Thực thể `Submissions`)
Các bài nộp của đội thi trong một vòng.

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `round_detail_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `round_details` |
| `url` | `text` | `string?` | `NULL` |
| `description` | `text` | `string?` | `NULL` |
| `status` | `text` | `SubmissionStatusEnum?` | `NULL` (Lưu dưới dạng chuỗi tên Enum) |
| `submitted_at` | `timestamp with time zone` | `DateTimeOffset?` | `NULL` |
| `is_regrade` | `boolean` | `bool` | `NOT NULL` |

### 2.19. Bảng `invitations` (Thực thể `Invitations`)
Các lời mời người dùng gia nhập đội thi.

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `team_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `teams` |
| `user_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `users` |
| `limit_time` | `timestamp with time zone` | `DateTimeOffset?` | `NULL` |
| `status` | `text` | `InvitationStatusEnum?` | `NULL` (Lưu dưới dạng chuỗi tên Enum) |
| `description` | `text` | `string?` | `NULL` |

### 2.20. Bảng `notifications` (Thực thể `Notifications`)
Hệ thống thông báo cho cá nhân hoặc đội nhóm.

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `user_id` | `uuid` | `Guid?` | `NULL`, Khóa ngoại đến `users` |
| `team_id` | `uuid` | `Guid?` | `NULL`, Khóa ngoại đến `teams` |
| `title` | `text` | `string?` | `NULL` |
| `status` | `text` | `NotificationStatusEnum?` | `NULL` (Lưu dưới dạng chuỗi tên Enum) |
| `description` | `text` | `string?` | `NULL` |
| `target_type` | `text` | `NotificationTargetTypeEnum` | `NOT NULL` (Mặc định `Personal`, lưu chuỗi tên Enum) |

### 2.21. Bảng `event_roles` (Thực thể `EventRoles`)
Định nghĩa vai trò của thành viên trong sự kiện (ví dụ: Ban giám khảo, Mentor, Người tổ chức).

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `name` | `text` | `EventRoleEnum` | `NOT NULL` (Lưu dưới dạng chuỗi tên Enum) |

### 2.22. Bảng `assign_events` (Thực thể `AssignEvents`)
Phân công người dùng vào vai trò cụ thể trong một sự kiện.

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `user_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `users` |
| `event_role_id` | `uuid` | `Guid?` | `NULL`, Khóa ngoại đến `event_roles` |
| `event_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `events` |

### 2.23. Bảng `assign_tracks` (Thực thể `AssignTracks`)
Phân công người dùng phụ trách chấm điểm hoặc hướng dẫn một Track trong sự kiện.

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `assign_event_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `assign_events` |
| `track_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `tracks` |

### 2.24. Bảng `scores` (Thực thể `Scores`)
Lưu tổng điểm chấm của bài nộp bởi người phụ trách chấm điểm.
*Lưu ý:* Bảng này hỗ trợ tính năng chấm lại (Retake) bằng cách tự liên kết (`retake_from_score_id`).

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `submission_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `submissions` |
| `assign_track_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `assign_tracks` |
| `is_retake` | `boolean` | `bool` | `NOT NULL` |
| `retake_from_score_id` | `uuid` | `Guid?` | `NULL`, Khóa ngoại tự liên kết đến `scores` (Độc nhất - Unique) |
| `total_score` | `numeric` | `decimal?` | `NULL` |
| `is_mock` | `boolean` | `bool` | `NOT NULL` |

### 2.25. Bảng `score_items` (Thực thể `ScoreItems`)
Chi tiết chấm điểm cho từng tiêu chí của bài nộp.

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `score_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `scores` |
| `criteria_item_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `criteria_items` |
| `assign_track_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `assign_tracks` |
| `score` | `numeric` | `decimal?` | `NULL` |
| `comment` | `text` | `string?` | `NULL` |

### 2.26. Bảng `mentor_notifications` (Thực thể `MentorNotifications`)
Thông báo dành riêng cho người dùng phụ trách Track (Mentor).

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `assign_track_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `assign_tracks` |
| `title` | `text` | `string?` | `NULL` |
| `description` | `text` | `string?` | `NULL` |

### 2.27. Bảng `reports` (Thực thể `Reports`)
Lưu trữ các báo cáo/phản hồi lỗi của người dùng trong hệ thống.

| Cột (PostgreSQL) | Kiểu PostgreSQL | Kiểu .NET / C# | Ràng buộc / Chi tiết |
| :--- | :--- | :--- | :--- |
| `id` | `uuid` | `Guid` | Khóa chính, `NOT NULL` |
| `is_disable` | `boolean` | `bool` | `NOT NULL` |
| `created_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `updated_at` | `timestamp with time zone` | `DateTimeOffset` | `NOT NULL` |
| `user_id` | `uuid` | `Guid` | `NOT NULL`, Khóa ngoại đến `users` |
| `title` | `text` | `string?` | `NULL` |
| `description` | `text` | `string?` | `NULL` |
| `status` | `text` | `ReportStatusEnum?` | `NULL` (Lưu dưới dạng chuỗi tên Enum) |
| `reason` | `text` | `string?` | `NULL` |
| `type_report` | `text` | `string?` | `NULL` |

---

## 3. Bản đồ quan hệ giữa các thực thể (Entity Relationships Map)

Dưới đây là sơ đồ chi tiết các mối quan hệ được khai báo trong hệ thống. Mỗi mối quan hệ chỉ được định nghĩa một chiều duy nhất nhằm tránh trùng lặp.

### 3.1. Thực thể `Users` (Người dùng)
- **`Users` (1) ─── (N) `RefreshTokens`**
  - Mối quan hệ: Một người dùng có thể có nhiều phiên đăng nhập đồng thời.
  - Ánh xạ: Khóa ngoại `user_id` trên bảng `refresh_tokens`.
- **`Users` (1) ─── (N) `ResetPasswords`**
  - Mối quan hệ: Một người dùng có thể có nhiều yêu cầu đặt lại mật khẩu theo thời gian.
  - Ánh xạ: Khóa ngoại `user_id` trên bảng `reset_passwords`.
- **`Users` (1) ─── (N) `EmailVerifications`**
  - Mối quan hệ: Một người dùng có thể gửi yêu cầu xác thực email nhiều lần.
  - Ánh xạ: Khóa ngoại `user_id` trên bảng `email_verifications`.
- **`Users` (1) ─── (N) `TeamDetails`**
  - Mối quan hệ: Một người dùng có thể tham gia vào nhiều đội (làm thành viên hoặc trưởng nhóm).
  - Ánh xạ: Khóa ngoại `user_id` trên bảng `team_details`.
- **`Users` (1) ─── (N) `Invitations`**
  - Mối quan hệ: Một người dùng có thể nhận được nhiều lời mời gia nhập đội.
  - Ánh xạ: Khóa ngoại `user_id` trên bảng `invitations`.
- **`Users` (1) ─── (0..1) `Notifications`**
  - Mối quan hệ: Hệ thống lưu và phân phối thông báo cá nhân đến người dùng.
  - Ánh xạ: Khóa ngoại `user_id` (cho phép NULL) trên bảng `notifications`.
- **`Users` (1) ─── (N) `AssignEvents`**
  - Mối quan hệ: Một người dùng có thể được phân công quản lý/chấm thi ở nhiều sự kiện.
  - Ánh xạ: Khóa ngoại `user_id` trên bảng `assign_events`.
- **`Users` (1) ─── (N) `Reports`**
  - Mối quan hệ: Một người dùng có thể gửi nhiều báo cáo lỗi/khiếu nại.
  - Ánh xạ: Khóa ngoại `user_id` trên bảng `reports`.

### 3.2. Thực thể `Teams` (Đội thi)
- **`Teams` (1) ─── (N) `TeamDetails`**
  - Mối quan hệ: Một đội có chứa danh sách nhiều thành viên.
  - Ánh xạ: Khóa ngoại `team_id` trên bảng `team_details`.
- **`Teams` (1) ─── (N) `RegisterTeams`**
  - Mối quan hệ: Một đội có thể đăng ký tham gia vào nhiều cuộc thi/sự kiện khác nhau.
  - Ánh xạ: Khóa ngoại `team_id` trên bảng `register_teams`.
- **`Teams` (1) ─── (N) `Invitations`**
  - Mối quan hệ: Trưởng nhóm có thể gửi lời mời gia nhập đội đến nhiều người dùng.
  - Ánh xạ: Khóa ngoại `team_id` trên bảng `invitations`.
- **`Teams` (1) ─── (0..1) `Notifications`**
  - Mối quan hệ: Hệ thống có thể gửi thông báo hướng tới một đội cụ thể.
  - Ánh xạ: Khóa ngoại `team_id` (cho phép NULL) trên bảng `notifications`.
- **`Teams` (1) ─── (N) `LeaderBoardDetails`**
  - Mối quan hệ: Một đội có thể có kết quả xếp hạng trong nhiều bảng xếp hạng sự kiện khác nhau.
  - Ánh xạ: Khóa ngoại `team_id` trên bảng `leader_board_details`.

### 3.3. Thực thể `Events` (Sự kiện Hackathon)
- **`Events` (1) ─── (N) `Rounds`**
  - Mối quan hệ: Một sự kiện gồm nhiều vòng thi diễn ra nối tiếp nhau.
  - Ánh xạ: Khóa ngoại `event_id` trên bảng `rounds`.
- **`Events` (1) ─── (N) `Tracks`**
  - Mối quan hệ: Một cuộc thi có nhiều Track chuyên đề khác nhau cho các đội lựa chọn.
  - Ánh xạ: Khóa ngoại `event_id` trên bảng `tracks`.
- **`Events` (1) ─── (N) `Awards`**
  - Mối quan hệ: Một cuộc thi định nghĩa cơ cấu giải thưởng riêng (Giải Nhất, Nhì, Ba...).
  - Ánh xạ: Khóa ngoại `event_id` trên bảng `awards`.
- **`Events` (1) ─── (1) `LeaderBoards`**
  - Mối quan hệ: Một sự kiện chỉ tương ứng với một bảng xếp hạng duy nhất.
  - Ánh xạ: Khóa ngoại `event_id` độc nhất (Unique) trên bảng `leader_boards`.
- **`Events` (1) ─── (N) `AssignEvents`**
  - Mối quan hệ: Một sự kiện có thể có nhiều người trong ban tổ chức/ban giám khảo được phân công.
  - Ánh xạ: Khóa ngoại `event_id` trên bảng `assign_events`.
- **`Events` (1) ─── (N) `RegisterTeams`**
  - Mối quan hệ: Một sự kiện đón nhận nhiều hồ sơ đăng ký thi từ các đội khác nhau.
  - Ánh xạ: Khóa ngoại `event_id` trên bảng `register_teams`.

### 3.4. Thực thể `EventRoles` (Vai trò Sự kiện)
- **`EventRoles` (1) ─── (0..1) `AssignEvents`**
  - Mối quan hệ: Vai trò sự kiện (Judge, Mentor, Organizer...) được gắn vào các phân công công việc.
  - Ánh xạ: Khóa ngoại `event_role_id` (cho phép NULL) trên bảng `assign_events`.

### 3.5. Thực thể `Tracks` (Chủ đề thi)
- **`Tracks` (1) ─── (N) `Topics`**
  - Mối quan hệ: Mỗi Track gồm nhiều đề tài gợi ý chi tiết.
  - Ánh xạ: Khóa ngoại `track_id` trên bảng `topics`.
- **`Tracks` (1) ─── (N) `AssignTracks`**
  - Mối quan hệ: Mỗi chuyên đề có thể phân công nhiều Mentor/Giám khảo phụ trách hướng dẫn và đánh giá.
  - Ánh xạ: Khóa ngoại `track_id` trên bảng `assign_tracks`.
- **`Tracks` (1) ─── (0..1) `RegisterTeams`**
  - Mối quan hệ: Các đội khi đăng ký tham gia sự kiện có thể chọn một Track cụ thể làm hướng đi chính.
  - Ánh xạ: Khóa ngoại `track_id` (cho phép NULL) trên bảng `register_teams`.

### 3.6. Thực thể `Topics` (Đề tài thi)
- **`Topics` (1) ─── (0..1) `RegisterTeams`**
  - Mối quan hệ: Đội dự thi có thể đăng ký giải quyết một đề tài cụ thể.
  - Ánh xạ: Khóa ngoại `topic_id` (cho phép NULL) trên bảng `register_teams`.

### 3.7. Thực thể `Rounds` (Vòng thi)
- **`Rounds` (1) ─── (N) `CriteriaTemplates`**
  - Mối quan hệ: Mỗi vòng thi áp dụng các bộ tiêu chí đánh giá khác nhau.
  - Ánh xạ: Khóa ngoại `round_id` trên bảng `criteria_templates`.
- **`Rounds` (1) ─── (N) `RoundDetails`**
  - Mối quan hệ: Theo dõi tiến trình của từng đội đăng ký đi qua vòng thi đó.
  - Ánh xạ: Khóa ngoại `round_id` trên bảng `round_details`.

### 3.8. Thực thể `CriteriaTemplates` (Bộ tiêu chí mẫu)
- **`CriteriaTemplates` (1) ─── (N) `CriteriaItems`**
  - Mối quan hệ: Một bộ tiêu chí mẫu gồm nhiều tiêu chí đánh giá thành phần.
  - Ánh xạ: Khóa ngoại `criteria_template_id` trên bảng `criteria_items`.

### 3.9. Thực thể `CriteriaItems` (Tiêu chí thành phần)
- **`CriteriaItems` (1) ─── (N) `ScoreItems`**
  - Mối quan hệ: Điểm của từng tiêu chí thành phần được lưu lại trong bảng điểm chi tiết.
  - Ánh xạ: Khóa ngoại `criteria_item_id` trên bảng `score_items`.

### 3.10. Thực thể `AssignEvents` (Phân công người dùng vào sự kiện)
- **`AssignEvents` (1) ─── (N) `AssignTracks`**
  - Mối quan hệ: Người dùng được phân công vào sự kiện sau đó sẽ được phân công phụ trách chi tiết từng Track.
  - Ánh xạ: Khóa ngoại `assign_event_id` trên bảng `assign_tracks`.

### 3.11. Thực thể `AssignTracks` (Phân công vào Track)
- **`AssignTracks` (1) ─── (N) `ScoreItems`**
  - Mối quan hệ: Giám khảo phụ trách Track sẽ chấm điểm và để lại nhận xét trên từng tiêu chí thành phần.
  - Ánh xạ: Khóa ngoại `assign_track_id` trên bảng `score_items`.
- **`AssignTracks` (1) ─── (N) `MentorNotifications`**
  - Mối quan hệ: Mentor nhận các thông báo đẩy tự động liên quan đến Track được giao phụ trách.
  - Ánh xạ: Khóa ngoại `assign_track_id` trên bảng `mentor_notifications`.
- **`AssignTracks` (1) ─── (N) `Scores`**
  - Mối quan hệ: Mỗi Mentor/Giám khảo chấm điểm bài nộp của đội thi và sinh ra một phiếu điểm tổng.
  - Ánh xạ: Khóa ngoại `assign_track_id` trên bảng `scores`.

### 3.12. Thực thể `RegisterTeams` (Đội thi đã đăng ký sự kiện)
- **`RegisterTeams` (1) ─── (N) `RoundDetails`**
  - Mối quan hệ: Một đội đăng ký tham gia sẽ lần lượt trải qua nhiều vòng thi khác nhau của sự kiện.
  - Ánh xạ: Khóa ngoại `register_team_id` trên bảng `round_details`.

### 3.13. Thực thể `RoundDetails` (Chi tiết vòng thi của đội)
- **`RoundDetails` (1) ─── (N) `Submissions`**
  - Mối quan hệ: Trong mỗi vòng thi, đội thi có thể nộp một hoặc nhiều phiên bản bài làm/bản cập nhật.
  - Ánh xạ: Khóa ngoại `round_detail_id` trên bảng `submissions`.

### 3.14. Thực thể `Submissions` (Bài nộp)
- **`Submissions` (1) ─── (N) `Scores`**
  - Mối quan hệ: Mỗi bài nộp của đội thi được đánh giá bởi một hoặc nhiều giám khảo với các phiếu điểm khác nhau.
  - Ánh xạ: Khóa ngoại `submission_id` trên bảng `scores`.

### 3.15. Thực thể `LeaderBoards` (Bảng xếp hạng)
- **`LeaderBoards` (1) ─── (N) `LeaderBoardDetails`**
  - Mối quan hệ: Một bảng xếp hạng tổng thể chứa thông tin xếp hạng chi tiết của các đội tham gia sự kiện.
  - Ánh xạ: Khóa ngoại `leader_board_id` trên bảng `leader_board_details`.

### 3.16. Thực thể `Scores` (Phiếu điểm tổng)
- **`Scores` (1) ─── (N) `ScoreItems`**
  - Mối quan hệ: Một phiếu điểm tổng được cấu thành từ tổng điểm chấm trên các tiêu chí thành phần cụ thể.
  - Ánh xạ: Khóa ngoại `score_id` trên bảng `score_items`.
- **`Scores` (0..1) ─── (0..1) `Scores` (Self-referencing)**
  - Mối quan hệ: Liên kết tự phản thân 1-1 hỗ trợ chức năng chấm lại bài (Retake). Phiếu điểm hiện tại được tham chiếu từ phiếu điểm chấm lại gốc.
  - Ánh xạ: Khóa ngoại `retake_from_score_id` độc nhất (Unique index) tự liên kết trên bảng `scores`.
