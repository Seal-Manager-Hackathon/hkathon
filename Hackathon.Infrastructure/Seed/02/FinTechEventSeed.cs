using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Event;
using Hackathon.Domain.Enums.RegisterTeam;
using Hackathon.Domain.Enums.TeamDetail;
using Hackathon.Domain.Enums.User;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Seed;

/// <summary>
/// Self-contained seed for a complete FinTech &amp; Web3 hackathon event.
/// Coverage: 1 event (Published, 2 rounds) + 3 tracks (3 topics each = 9 topics)
/// + 3 mentors (1 per track) + 2 judges (assigned to all 3 tracks) + 2 staff
/// + 3 teams (2 Approved, 1 Pending), each team with 2 members.
/// All GUIDs use the 070+ suffix range to avoid colliding with existing seeds.
/// </summary>
public static class FinTechEventSeed
{
    // ── Event ────────────────────────────────────────────────
    private static readonly Guid EventId = Guid.Parse("20000000-0000-0000-0000-000000000070");

    // ── Rounds (2) ───────────────────────────────────────────
    private static readonly Guid Round1Id = Guid.Parse("21000000-0000-0000-0000-000000000070");
    private static readonly Guid Round2Id = Guid.Parse("21000000-0000-0000-0000-000000000071");

    // ── Tracks (3) ───────────────────────────────────────────
    private static readonly Guid Track1Id = Guid.Parse("24000000-0000-0000-0000-000000000070");
    private static readonly Guid Track2Id = Guid.Parse("24000000-0000-0000-0000-000000000071");
    private static readonly Guid Track3Id = Guid.Parse("24000000-0000-0000-0000-000000000072");

    // ── Topics (9 = 3 per track) ─────────────────────────────
    private static readonly Guid Topic1_1Id = Guid.Parse("25000000-0000-0000-0000-000000000070");
    private static readonly Guid Topic1_2Id = Guid.Parse("25000000-0000-0000-0000-000000000071");
    private static readonly Guid Topic1_3Id = Guid.Parse("25000000-0000-0000-0000-000000000072");
    private static readonly Guid Topic2_1Id = Guid.Parse("25000000-0000-0000-0000-000000000073");
    private static readonly Guid Topic2_2Id = Guid.Parse("25000000-0000-0000-0000-000000000074");
    private static readonly Guid Topic2_3Id = Guid.Parse("25000000-0000-0000-0000-000000000075");
    private static readonly Guid Topic3_1Id = Guid.Parse("25000000-0000-0000-0000-000000000076");
    private static readonly Guid Topic3_2Id = Guid.Parse("25000000-0000-0000-0000-000000000077");
    private static readonly Guid Topic3_3Id = Guid.Parse("25000000-0000-0000-0000-000000000078");

    // ── Users: Mentors (3, Lecturer) ─────────────────────────
    private static readonly Guid Mentor1UserId = Guid.Parse("10000000-0000-0000-0000-000000000070");
    private static readonly Guid Mentor2UserId = Guid.Parse("10000000-0000-0000-0000-000000000071");
    private static readonly Guid Mentor3UserId = Guid.Parse("10000000-0000-0000-0000-000000000072");

    // ── Users: Judges (2, Lecturer) ──────────────────────────
    private static readonly Guid Judge1UserId = Guid.Parse("10000000-0000-0000-0000-000000000073");
    private static readonly Guid Judge2UserId = Guid.Parse("10000000-0000-0000-0000-000000000074");

    // ── Users: Staff (2, Staff) ──────────────────────────────
    private static readonly Guid Staff1UserId = Guid.Parse("10000000-0000-0000-0000-000000000075");
    private static readonly Guid Staff2UserId = Guid.Parse("10000000-0000-0000-0000-000000000076");

    // ── Users: Students (6 = 2 per team) ─────────────────────
    private static readonly Guid Student1UserId = Guid.Parse("10000000-0000-0000-0000-000000000077");
    private static readonly Guid Student2UserId = Guid.Parse("10000000-0000-0000-0000-000000000078");
    private static readonly Guid Student3UserId = Guid.Parse("10000000-0000-0000-0000-000000000079");
    private static readonly Guid Student4UserId = Guid.Parse("10000000-0000-0000-0000-000000000080");
    private static readonly Guid Student5UserId = Guid.Parse("10000000-0000-0000-0000-000000000081");
    private static readonly Guid Student6UserId = Guid.Parse("10000000-0000-0000-0000-000000000082");

    // ── Teams (3) ────────────────────────────────────────────
    private static readonly Guid Team1Id = Guid.Parse("30000000-0000-0000-0000-000000000070");
    private static readonly Guid Team2Id = Guid.Parse("30000000-0000-0000-0000-000000000071");
    private static readonly Guid Team3Id = Guid.Parse("30000000-0000-0000-0000-000000000072");

    // ── TeamDetails (6 = 2 per team) ─────────────────────────
    private static readonly Guid Team1Detail1Id = Guid.Parse("30100000-0000-0000-0000-000000000070");
    private static readonly Guid Team1Detail2Id = Guid.Parse("30100000-0000-0000-0000-000000000071");
    private static readonly Guid Team2Detail1Id = Guid.Parse("30100000-0000-0000-0000-000000000072");
    private static readonly Guid Team2Detail2Id = Guid.Parse("30100000-0000-0000-0000-000000000073");
    private static readonly Guid Team3Detail1Id = Guid.Parse("30100000-0000-0000-0000-000000000074");
    private static readonly Guid Team3Detail2Id = Guid.Parse("30100000-0000-0000-0000-000000000075");

    // ── RegisterTeams (3) ────────────────────────────────────
    private static readonly Guid Team1RegisterId = Guid.Parse("31000000-0000-0000-0000-000000000070");
    private static readonly Guid Team2RegisterId = Guid.Parse("31000000-0000-0000-0000-000000000071");
    private static readonly Guid Team3RegisterId = Guid.Parse("31000000-0000-0000-0000-000000000072");

    // ── AssignEvents (event-level role assignments) ──────────
    private static readonly Guid Mentor1AssignEventId = Guid.Parse("40000000-0000-0000-0000-000000000070");
    private static readonly Guid Mentor2AssignEventId = Guid.Parse("40000000-0000-0000-0000-000000000071");
    private static readonly Guid Mentor3AssignEventId = Guid.Parse("40000000-0000-0000-0000-000000000072");
    private static readonly Guid Judge1AssignEventId = Guid.Parse("40000000-0000-0000-0000-000000000073");
    private static readonly Guid Judge2AssignEventId = Guid.Parse("40000000-0000-0000-0000-000000000074");
    private static readonly Guid Staff1AssignEventId = Guid.Parse("40000000-0000-0000-0000-000000000075");
    private static readonly Guid Staff2AssignEventId = Guid.Parse("40000000-0000-0000-0000-000000000076");

    // ── AssignTracks (track-level assignments) ─────────────
    private static readonly Guid Mentor1Track1Id = Guid.Parse("41000000-0000-0000-0000-000000000070");
    private static readonly Guid Mentor2Track2Id = Guid.Parse("41000000-0000-0000-0000-000000000071");
    private static readonly Guid Mentor3Track3Id = Guid.Parse("41000000-0000-0000-0000-000000000072");
    private static readonly Guid Judge1Track1Id = Guid.Parse("41000000-0000-0000-0000-000000000073");
    private static readonly Guid Judge1Track2Id = Guid.Parse("41000000-0000-0000-0000-000000000074");
    private static readonly Guid Judge1Track3Id = Guid.Parse("41000000-0000-0000-0000-000000000075");
    private static readonly Guid Judge2Track1Id = Guid.Parse("41000000-0000-0000-0000-000000000076");
    private static readonly Guid Judge2Track2Id = Guid.Parse("41000000-0000-0000-0000-000000000077");
    private static readonly Guid Judge2Track3Id = Guid.Parse("41000000-0000-0000-0000-000000000078");

    // Schedule (UTC) — event runs in October 2026, registration closes beforehand.
    private static readonly DateTimeOffset EventStartTime = new(2026, 10, 10, 1, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset EventEndTime = new(2026, 10, 12, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset RegisterLimitTime = new(2026, 10, 5, 16, 59, 0, TimeSpan.Zero);

    private static string PasswordHash => SeedHelper.HashDefaultPassword();

    // ── Users: 3 mentors + 2 judges + 2 staff + 6 students ──
    private static void SeedUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Users>().HasData(
            // Mentors (Lecturer) — one per track
            CreateUser(Mentor1UserId, "bao.trinh@lecturer.seed.local", "Quoc Bao", "Trinh", "LECT070", RoleEnum.Lecturer, "Mentor for Payments & Digital Banking track"),
            CreateUser(Mentor2UserId, "ha.nguyen@lecturer.seed.local", "Thanh Ha", "Nguyen", "LECT071", RoleEnum.Lecturer, "Mentor for DeFi & Smart Contracts track"),
            CreateUser(Mentor3UserId, "duc.le@lecturer.seed.local", "Minh Duc", "Le", "LECT072", RoleEnum.Lecturer, "Mentor for Web3 Infrastructure & Security track"),
            // Judges (Lecturer) — assigned to all 3 tracks
            CreateUser(Judge1UserId, "tuananh.pham@lecturer.seed.local", "Tuan Anh", "Pham", "LECT073", RoleEnum.Lecturer, "Judge evaluating all FinTech & Web3 tracks"),
            CreateUser(Judge2UserId, "hong.tran@lecturer.seed.local", "Thi Hong", "Tran", "LECT074", RoleEnum.Lecturer, "Judge evaluating all FinTech & Web3 tracks"),
            // Staff — manage the event
            CreateUser(Staff1UserId, "long.vu@staff.seed.local", "Hoang Long", "Vu", "STF070", RoleEnum.Staff, "Event operations staff"),
            CreateUser(Staff2UserId, "mai.dang@staff.seed.local", "Thi Mai", "Dang", "STF071", RoleEnum.Staff, "Event operations staff"),
            // Students — team members (2 per team)
            CreateUser(Student1UserId, "minh.pham@student.seed.local", "Cong Minh", "Pham", "STU070", RoleEnum.Student, "PayForge team leader"),
            CreateUser(Student2UserId, "linh.do@student.seed.local", "Thu Linh", "Do", "STU071", RoleEnum.Student, "PayForge team member"),
            CreateUser(Student3UserId, "kien.tran@student.seed.local", "Van Kien", "Tran", "STU072", RoleEnum.Student, "DeFiApe team leader"),
            CreateUser(Student4UserId, "quynh.vo@student.seed.local", "Ngoc Quynh", "Vo", "STU073", RoleEnum.Student, "DeFiApe team member"),
            CreateUser(Student5UserId, "hung.bui@student.seed.local", "Quoc Hung", "Bui", "STU074", RoleEnum.Student, "ChainGuard team leader"),
            CreateUser(Student6UserId, "trang.ngo@student.seed.local", "Hai Trang", "Ngo", "STU075", RoleEnum.Student, "ChainGuard team member")
        );
    }

    // ── Event ────────────────────────────────────────────────
    private static void SeedEvent(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Events>().HasData(
            new Events
            {
                Id = EventId,
                Name = "FinTech & Web3 Hackathon 2026",
                Description =
                    "FinTech & Web3 Hackathon 2026 là cuộc thi lập trình 48 giờ quy mô toàn quốc, " +
                    "thi đấu trên ba mảng: Thanh toán & Ngân hàng số, DeFi & Smart Contract, và Hạ tầng Web3 & Bảo mật. " +
                    "Các đội sẽ trải qua hai vòng: vòng Đề án & Chế tạo mẫu (Prototyping) và vòng Trình diễn & Bảo vệ (Final Demo). " +
                    "Mỗi track có ba chủ đề gợi mở để các đội lựa chọn, được cố vấn chuyên môn bởi các mentor giàu kinh nghiệm " +
                    "và đánh giá bởi hội đồng giám khảo trên cả ba track. Sự kiện mở đăng ký cho tối đa 30 đội, mỗi đội 2-4 thành viên.",
                StartTime = EventStartTime,
                EndTime = EventEndTime,
                RegisterLimitTime = RegisterLimitTime,
                LimitTeam = 30,
                MinMember = 2,
                MaxMember = 4,
                Status = EventStatusEnum.Published,
                NumberRound = 2,
                Season = SeasonEnum.Autumn,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            });
    }

    // ── Rounds (2) ──────────────────────────────────────────
    private static void SeedRounds(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rounds>().HasData(
            new Rounds
            {
                Id = Round1Id,
                EventId = EventId,
                Name = "Ideation & Prototyping",
                Description =
                    "Vòng 1 — Các đội trình bày ý tưởng, chọn chủ đề trong track đã đăng ký và chế tạo nguyên mẫu (prototype) " +
                    "trong vòng 48 giờ. Mentor phụ trách track sẽ hỗ trợ định hướng kỹ thuật. Nộp bài trước 12:00 trưa ngày hôm sau.",
                RoundNo = 1,
                StartTime = EventStartTime,
                EndTime = EventStartTime.AddDays(1),
                StartSubmission = EventStartTime.AddHours(2),
                EndSubmission = EventStartTime.AddDays(1).AddHours(2),
                LimitTeam = 30,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },
            new Rounds
            {
                Id = Round2Id,
                EventId = EventId,
                Name = "Final Demo & Pitch",
                Description =
                    "Vòng 2 — Các đội xuất sắc trình diễn sản phẩm hoàn thiện và bảo vệ trước hội đồng giám khảo. " +
                    "Cả hai giám khảo sẽ chấm điểm trên cả ba track theo cùng bộ tiêu chí. Nộp bản final trước 12:00 trưa ngày diễn ra.",
                RoundNo = 2,
                StartTime = EventStartTime.AddDays(1),
                EndTime = EventEndTime,
                StartSubmission = EventStartTime.AddDays(1).AddHours(2),
                EndSubmission = EventEndTime.AddHours(-3),
                LimitTeam = 15,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            });
    }

    // ── Tracks (3) + Topics (9) ──────────────────────────────
    private static void SeedTracksAndTopics(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tracks>().HasData(
            new Tracks
            {
                Id = Track1Id,
                EventId = EventId,
                Title = "Payments & Digital Banking",
                Description = "Giải pháp thanh toán xuyên biên giới, ngân hàng nhúng (embedded finance) và onboarding số cho người dùng mới.",
                MaxTeam = 10,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },
            new Tracks
            {
                Id = Track2Id,
                EventId = EventId,
                Title = "DeFi & Smart Contracts",
                Description = "Giao thức cho phi tập trung, thị trường tự động (AMM) và công cụ kiểm toán/bảo mật smart contract.",
                MaxTeam = 10,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },
            new Tracks
            {
                Id = Track3Id,
                EventId = EventId,
                Title = "Web3 Infrastructure & Security",
                Description = "Hạ tầng Web3: bảo mật ví, mở rộng quy mô (Layer-2) và định danh phi tập trung (DID).",
                MaxTeam = 10,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            });

        modelBuilder.Entity<Topics>().HasData(
            // Track 1 — Payments & Digital Banking
            CreateTopic(Topic1_1Id, Track1Id, "Cross-Border Payment Gateway", "Cổng thanh toán xuyên biên giới tối ưu chi phí giao dịch và tốc độ thanh toán liên ngân hàng quốc tế."),
            CreateTopic(Topic1_2Id, Track1Id, "Embedded Finance APIs", "Bộ API nhúng dịch vụ tài chính (thanh toán, ví, cấp tín dụng) trực tiếp vào nền tảng phi tài chính."),
            CreateTopic(Topic1_3Id, Track1Id, "Digital KYC & Onboarding", "Luồng onboarding số với eKYC, phát hiện gian lận và trải nghiệm người dùng mới mượt mà."),
            // Track 2 — DeFi & Smart Contracts
            CreateTopic(Topic2_1Id, Track2Id, "Decentralized Lending Protocol", "Giao thức cho vay/mượn phi tập trung với hồ sơ quá mức và quản lý rủi ro tự động."),
            CreateTopic(Topic2_2Id, Track2Id, "Automated Market Maker (AMM)", "Thị trường tạo lập tự động với tối ưu thanh khoản và giảm trượt giá."),
            CreateTopic(Topic2_3Id, Track2Id, "Smart Contract Audit Bot", "Bot tự động rà soát lỗ hổng smart contract và đề xuất bản vá trước khi triển khai."),
            // Track 3 — Web3 Infrastructure & Security
            CreateTopic(Topic3_1Id, Track3Id, "Wallet Security & Recovery", "Cơ chế bảo mật và khôi phục ví đa chữ ký, chống mất cắp và khôi phục truy cập cho người dùng."),
            CreateTopic(Topic3_2Id, Track3Id, "Layer-2 Scaling Solution", "Giải pháp mở rộng quy mô Layer-2 giảm phí và tăng thông lượng giao dịch."),
            CreateTopic(Topic3_3Id, Track3Id, "Decentralized Identity (DID)", "Hệ thống định danh phi tập trung cho phép người dùng tự quản trị danh tính và quyền riêng tư.")
        );
    }

    // ── Teams (3) + TeamDetails (6 = 2 members per team) ────
    private static void SeedTeamsAndMembers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Teams>().HasData(
            new Teams { Id = Team1Id, Name = "PayForge", CanEdit = false, IsDisable = false, CreatedAt = SeedConstants.CreatedAt, UpdatedAt = SeedConstants.CreatedAt },
            new Teams { Id = Team2Id, Name = "DeFiApe", CanEdit = false, IsDisable = false, CreatedAt = SeedConstants.CreatedAt, UpdatedAt = SeedConstants.CreatedAt },
            new Teams { Id = Team3Id, Name = "ChainGuard", CanEdit = false, IsDisable = false, CreatedAt = SeedConstants.CreatedAt, UpdatedAt = SeedConstants.CreatedAt });

        modelBuilder.Entity<TeamDetails>().HasData(
            // Team 1 — PayForge (Approved, Track 1)
            CreateTeamDetail(Team1Detail1Id, Team1Id, Student1UserId, true),
            CreateTeamDetail(Team1Detail2Id, Team1Id, Student2UserId, false),
            // Team 2 — DeFiApe (Approved, Track 2)
            CreateTeamDetail(Team2Detail1Id, Team2Id, Student3UserId, true),
            CreateTeamDetail(Team2Detail2Id, Team2Id, Student4UserId, false),
            // Team 3 — ChainGuard (Pending, Track 3)
            CreateTeamDetail(Team3Detail1Id, Team3Id, Student5UserId, true),
            CreateTeamDetail(Team3Detail2Id, Team3Id, Student6UserId, false)
        );
    }

    // ── RegisterTeams (3): 2 Approved + 1 Pending ───────────
    private static void SeedRegisterTeams(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RegisterTeams>().HasData(
            CreateRegisterTeam(
                Team1RegisterId, Team1Id, EventId, Track1Id, Topic1_1Id,
                "PayForge xây dựng cổng thanh toán xuyên biên giới tích hợp đa ví, giảm chi phí giao dịch 40% so với cổng truyền thống.",
                RegisterTeamStatusEnum.Approved),
            CreateRegisterTeam(
                Team2RegisterId, Team2Id, EventId, Track2Id, Topic2_1Id,
                "DeFiApe triển khai giao thức cho vay phi tập trung với hồ sơ quá mức tự động và mô hình quản lý rủi ro dựa trên dữ liệu on-chain.",
                RegisterTeamStatusEnum.Approved),
            CreateRegisterTeam(
                Team3RegisterId, Team3Id, EventId, Track3Id, Topic3_1Id,
                "ChainGuard phát triển giải pháp ví đa chữ ký với cơ chế khôi phục xã hội (social recovery) giúp người dùng chống mất cắp và khôi phục truy cập an toàn.",
                RegisterTeamStatusEnum.Pending)
        );
    }

    // ── AssignEvents + AssignTracks ─────────────────────────
    private static void SeedAssignments(ModelBuilder modelBuilder)
    {
        // Event-level role assignments (AssignEvents)
        modelBuilder.Entity<AssignEvents>().HasData(
            // 3 mentors (Mentor role) at event level
            CreateAssignEvent(Mentor1AssignEventId, Mentor1UserId, SeedConstants.MentorEventRoleId, EventId),
            CreateAssignEvent(Mentor2AssignEventId, Mentor2UserId, SeedConstants.MentorEventRoleId, EventId),
            CreateAssignEvent(Mentor3AssignEventId, Mentor3UserId, SeedConstants.MentorEventRoleId, EventId),
            // 2 judges (Judge role) at event level
            CreateAssignEvent(Judge1AssignEventId, Judge1UserId, SeedConstants.JudgeEventRoleId, EventId),
            CreateAssignEvent(Judge2AssignEventId, Judge2UserId, SeedConstants.JudgeEventRoleId, EventId),
            // 2 staff (Staff role) at event level
            CreateAssignEvent(Staff1AssignEventId, Staff1UserId, SeedConstants.StaffEventRoleId, EventId),
            CreateAssignEvent(Staff2AssignEventId, Staff2UserId, SeedConstants.StaffEventRoleId, EventId)
        );

        // Track-level assignments (AssignTracks)
        modelBuilder.Entity<AssignTracks>().HasData(
            // 3 mentors, one per track
            CreateAssignTrack(Mentor1Track1Id, Mentor1AssignEventId, Track1Id),
            CreateAssignTrack(Mentor2Track2Id, Mentor2AssignEventId, Track2Id),
            CreateAssignTrack(Mentor3Track3Id, Mentor3AssignEventId, Track3Id),

            // Judge 1 assigned to all 3 tracks
            CreateAssignTrack(Judge1Track1Id, Judge1AssignEventId, Track1Id),
            CreateAssignTrack(Judge1Track2Id, Judge1AssignEventId, Track2Id),
            CreateAssignTrack(Judge1Track3Id, Judge1AssignEventId, Track3Id),

            // Judge 2 assigned to all 3 tracks
            CreateAssignTrack(Judge2Track1Id, Judge2AssignEventId, Track1Id),
            CreateAssignTrack(Judge2Track2Id, Judge2AssignEventId, Track2Id),
            CreateAssignTrack(Judge2Track3Id, Judge2AssignEventId, Track3Id)
        );
    }

    // ── Helper factory methods (match existing seed conventions) ──
    private static Users CreateUser(Guid id, string email, string firstName, string lastName, string studentId, RoleEnum role, string bio)
    {
        return new Users
        {
            Id = id,
            Email = email,
            HashPassword = PasswordHash,
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = "0900000000",
            AvatarUrl = $"https://robohash.org/{email}",
            Bio = bio,
            Address = "Seed address",
            DateOfBirth = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero),
            StudentId = studentId,
            Role = role,
            College = "FPT University",
            ImgUrl = $"https://robohash.org/{email}",
            LinkUrl = string.Empty,
            VerifyEmailAt = SeedConstants.CreatedAt,
            Status = UserStatusEnum.Active,
            IsVerified = true,
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }

    private static Topics CreateTopic(Guid id, Guid trackId, string title, string description)
    {
        return new Topics
        {
            Id = id,
            TrackId = trackId,
            Title = title,
            Description = description,
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }

    private static TeamDetails CreateTeamDetail(Guid id, Guid teamId, Guid userId, bool isLeader)
    {
        return new TeamDetails
        {
            Id = id,
            TeamId = teamId,
            UserId = userId,
            IsLeader = isLeader,
            Status = TeamDetailStatusEnum.Active,
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }

    private static RegisterTeams CreateRegisterTeam(Guid id, Guid teamId, Guid eventId, Guid trackId, Guid topicId, string description, RegisterTeamStatusEnum status)
    {
        return new RegisterTeams
        {
            Id = id,
            TeamId = teamId,
            EventId = eventId,
            TrackId = trackId,
            TopicId = topicId,
            Description = description,
            Status = status,
            IsBanned = false,
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }

    private static AssignEvents CreateAssignEvent(Guid id, Guid userId, Guid eventRoleId, Guid eventId)
    {
        return new AssignEvents
        {
            Id = id,
            UserId = userId,
            EventRoleId = eventRoleId,
            EventId = eventId,
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }

    private static AssignTracks CreateAssignTrack(Guid id, Guid assignEventId, Guid trackId)
    {
        return new AssignTracks
        {
            Id = id,
            AssignEventId = assignEventId,
            TrackId = trackId,
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }

    public static void SeedFinTechEventData(this ModelBuilder modelBuilder)
    {
        SeedUsers(modelBuilder);
        SeedEvent(modelBuilder);
        SeedRounds(modelBuilder);
        SeedTracksAndTopics(modelBuilder);
        SeedTeamsAndMembers(modelBuilder);
        SeedRegisterTeams(modelBuilder);
        SeedAssignments(modelBuilder);
    }
}
