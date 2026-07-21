using System.Text;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Event;
using Hackathon.Domain.Enums.Notification;
using Hackathon.Domain.Enums.RegisterTeam;
using Hackathon.Domain.Enums.Submission;
using Hackathon.Domain.Enums.TeamDetail;
using Hackathon.Domain.Enums.User;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Seed;

public static class FPTSeed
{
    // BCrypt EnhancedHash (SHA256) of password "string" + Pepper from SecurityOptions
    private static string PasswordHash => SeedHelper.HashDefaultPassword();

    private static readonly DateTimeOffset Now = new(2026, 7, 3, 0, 0, 0, TimeSpan.Zero);

    // ── Constants: Event 1 (Spring 2026 — ongoing, Round 1) ──
    private static readonly Guid Event1Id = Guid.Parse("20000000-0000-0000-0000-000000000100");
    private static readonly Guid Event1Round1Id = Guid.Parse("21000000-0000-0000-0000-000000000100");
    private static readonly Guid Event1Round2Id = Guid.Parse("21000000-0000-0000-0000-000000000101");
    private static readonly Guid Event1LeaderBoardId = Guid.Parse("60000000-0000-0000-0000-000000000100");

    // ── Constants: Event 2 (Summer 2026 — open for registration) ──
    private static readonly Guid Event2Id = Guid.Parse("20000000-0000-0000-0000-000000000200");
    private static readonly Guid Event2Round1Id = Guid.Parse("21000000-0000-0000-0000-000000000200");
    private static readonly Guid Event2Round2Id = Guid.Parse("21000000-0000-0000-0000-000000000201");
    private static readonly Guid Event2LeaderBoardId = Guid.Parse("60000000-0000-0000-0000-000000000200");

    // ── Tracks Event 1 ──
    private static readonly Guid Ev1TrackAiId = Guid.Parse("24000000-0000-0000-0000-000000000100");
    private static readonly Guid Ev1TrackMobileId = Guid.Parse("24000000-0000-0000-0000-000000000101");
    private static readonly Guid Ev1TrackWebId = Guid.Parse("24000000-0000-0000-0000-000000000102");
    private static readonly Guid Ev1TrackDataId = Guid.Parse("24000000-0000-0000-0000-000000000103");
    private static readonly Guid Ev1TrackCloudId = Guid.Parse("24000000-0000-0000-0000-000000000104");
    private static readonly List<Guid> Ev1TrackIds = [Ev1TrackAiId, Ev1TrackMobileId, Ev1TrackWebId, Ev1TrackDataId, Ev1TrackCloudId];

    // ── Tracks Event 2 ──
    private static readonly Guid Ev2TrackAiId = Guid.Parse("24000000-0000-0000-0000-000000000200");
    private static readonly Guid Ev2TrackMobileId = Guid.Parse("24000000-0000-0000-0000-000000000201");
    private static readonly Guid Ev2TrackWebId = Guid.Parse("24000000-0000-0000-0000-000000000202");
    private static readonly Guid Ev2TrackDataId = Guid.Parse("24000000-0000-0000-0000-000000000203");
    private static readonly Guid Ev2TrackCloudId = Guid.Parse("24000000-0000-0000-0000-000000000204");
    private static readonly List<Guid> Ev2TrackIds = [Ev2TrackAiId, Ev2TrackMobileId, Ev2TrackWebId, Ev2TrackDataId, Ev2TrackCloudId];

    // ── Topics per Track (1 topic per track, 5 total per event) ──
    private static readonly Guid Ev1TopicAiId = Guid.Parse("25000000-0000-0000-0000-000000000100");
    private static readonly Guid Ev1TopicMobileId = Guid.Parse("25000000-0000-0000-0000-000000000101");
    private static readonly Guid Ev1TopicWebId = Guid.Parse("25000000-0000-0000-0000-000000000102");
    private static readonly Guid Ev1TopicDataId = Guid.Parse("25000000-0000-0000-0000-000000000103");
    private static readonly Guid Ev1TopicCloudId = Guid.Parse("25000000-0000-0000-0000-000000000104");
    private static readonly Guid Ev2TopicAiId = Guid.Parse("25000000-0000-0000-0000-000000000200");
    private static readonly Guid Ev2TopicMobileId = Guid.Parse("25000000-0000-0000-0000-000000000201");
    private static readonly Guid Ev2TopicWebId = Guid.Parse("25000000-0000-0000-0000-000000000202");
    private static readonly Guid Ev2TopicDataId = Guid.Parse("25000000-0000-0000-0000-000000000203");
    private static readonly Guid Ev2TopicCloudId = Guid.Parse("25000000-0000-0000-0000-000000000204");

    // ── Criteria Templates: Event 1 Round 1 (3 templates, 1st active) ──
    private static readonly Guid Ev1R1Tpl1Id = Guid.Parse("22000000-0000-0000-0000-000000000100");
    private static readonly Guid Ev1R1Tpl2Id = Guid.Parse("22000000-0000-0000-0000-000000000101");
    private static readonly Guid Ev1R1Tpl3Id = Guid.Parse("22000000-0000-0000-0000-000000000102");
    // Event 1 Round 2 (3 templates, 1st active)
    private static readonly Guid Ev1R2Tpl1Id = Guid.Parse("22000000-0000-0000-0000-000000000103");
    private static readonly Guid Ev1R2Tpl2Id = Guid.Parse("22000000-0000-0000-0000-000000000104");
    private static readonly Guid Ev1R2Tpl3Id = Guid.Parse("22000000-0000-0000-0000-000000000105");
    // Event 2 Round 1 (3 templates, 1st active)
    private static readonly Guid Ev2R1Tpl1Id = Guid.Parse("22000000-0000-0000-0000-000000000200");
    private static readonly Guid Ev2R1Tpl2Id = Guid.Parse("22000000-0000-0000-0000-000000000201");
    private static readonly Guid Ev2R1Tpl3Id = Guid.Parse("22000000-0000-0000-0000-000000000202");
    // Event 2 Round 2 (3 templates, 1st active)
    private static readonly Guid Ev2R2Tpl1Id = Guid.Parse("22000000-0000-0000-0000-000000000203");
    private static readonly Guid Ev2R2Tpl2Id = Guid.Parse("22000000-0000-0000-0000-000000000204");
    private static readonly Guid Ev2R2Tpl3Id = Guid.Parse("22000000-0000-0000-0000-000000000205");

    // ── Users: 5 Lecturers (shared) ──
    private static readonly Guid Judge1Id = Guid.Parse("10000000-0000-0000-0000-000000000280");
    private static readonly Guid Judge2Id = Guid.Parse("10000000-0000-0000-0000-000000000281");
    private static readonly Guid Judge3Id = Guid.Parse("10000000-0000-0000-0000-000000000282");
    private static readonly Guid Judge4Id = Guid.Parse("10000000-0000-0000-0000-000000000283");
    private static readonly Guid Judge5Id = Guid.Parse("10000000-0000-0000-0000-000000000284");
    private static readonly Guid Mentor1Id = Guid.Parse("10000000-0000-0000-0000-000000000285");
    private static readonly Guid Mentor2Id = Guid.Parse("10000000-0000-0000-0000-000000000286");
    private static readonly Guid Mentor3Id = Guid.Parse("10000000-0000-0000-0000-000000000287");
    private static readonly Guid Mentor4Id = Guid.Parse("10000000-0000-0000-0000-000000000288");
    private static readonly Guid Mentor5Id = Guid.Parse("10000000-0000-0000-0000-000000000289");
    private static readonly List<Guid> JudgeIds = [Judge1Id, Judge2Id, Judge3Id, Judge4Id, Judge5Id];
    private static readonly List<Guid> MentorIds = [Mentor1Id, Mentor2Id, Mentor3Id, Mentor4Id, Mentor5Id];
    // Staff
    private static readonly Guid Staff1Id = Guid.Parse("10000000-0000-0000-0000-000000000290");
    private static readonly Guid Staff2Id = Guid.Parse("10000000-0000-0000-0000-000000000291");
    private static readonly Guid Staff3Id = Guid.Parse("10000000-0000-0000-0000-000000000292");
    private static readonly Guid Staff4Id = Guid.Parse("10000000-0000-0000-0000-000000000293");
    private static readonly Guid Staff5Id = Guid.Parse("10000000-0000-0000-0000-000000000294");
    private static readonly List<Guid> StaffIds = [Staff1Id, Staff2Id, Staff3Id, Staff4Id, Staff5Id];

    // ── Teams Event 1 ──
    // Teams Event 2

    // ── RoundDetails & Submissions Event 1 Round 1 ──
    // RoundDetail base: 32000000-0000-0000-0000-000000000100 (10 entries)
    // Submission base: 33000000-0000-0000-0000-000000000100 (10 entries)
    // Score base: 50000000-0000-0000-0000-000000000100 (10 entries)

    // ── AssignEvents Event 1 ──
    // Judges: 40000000-0000-0000-0000-000000000100 to 104
    // Mentors: 40000000-0000-0000-0000-000000000105 to 109
    // Staff: 40000000-0000-0000-0000-000000000110 to 114

    // ── AssignEvents Event 2 ──
    // Judges: 40000000-0000-0000-0000-000000000200 to 204
    // Mentors: 40000000-0000-0000-0000-000000000205 to 209
    // Staff: 40000000-0000-0000-0000-000000000210 to 214

    // ── AssignTracks Event 1 (1 judge + 1 mentor per track) ──
    // AssignTracks Event 2

    // ══════════════════════════════════════════════════════════════
    //  MAIN SEED METHOD
    // ══════════════════════════════════════════════════════════════

    public static void SeedFPTData(this ModelBuilder modelBuilder)
    {
        // ──────────────────────────────────────────────────────────
        //  USERS: 60 FPT Students
        // ──────────────────────────────────────────────────────────
        var fptStudents = new List<Users>();
        // 30 students for Event 1 (SE101–SE130)
        var e1Names = new (string first, string last)[]
        {
            ("Nguyen Van", "An"), ("Tran Thi", "Bich"), ("Le Hoang", "Cuong"),
            ("Pham Minh", "Dung"), ("Vo Thi", "Em"), ("Dang Van", "Phuoc"),
            ("Bui Thi", "Giang"), ("Do Quoc", "Huy"), ("Ho Van", "Hung"),
            ("Ngo Thi", "Hong"), ("Duong Van", "Kien"), ("Ly Thi", "Lan"),
            ("Mai Thanh", "Long"), ("Luong Thi", "Mai"), ("Chu Van", "Manh"),
            ("Cao Thi", "Ngoc"), ("Phan Van", "Nhan"), ("Ta Thi", "Oanh"),
            ("Quach Van", "Phat"), ("La Thi", "Quynh"), ("Su Van", "Son"),
            ("Lam Thi", "Thanh"), ("Kieu Van", "Tien"), ("Dinh Thi", "Tuyet"),
            ("Vuong Van", "Trong"), ("Ha Thi", "Van"), ("Khuc Van", "Xuyen"),
            ("Dao Thi", "Yen"), ("Vu Van", "Binh"), ("Luc Thi", "Nhung")
        };
        for (int i = 0; i < 30; i++)
        {
            var id = Guid.Parse($"10000000-0000-0000-0000-00000000{200 + i:X4}");
            var email = $"{ToEmailName(e1Names[i].first)}{ToEmailName(e1Names[i].last)}@fpt.edu.vn";
            fptStudents.Add(CreateFptUser(id, email, e1Names[i].first, e1Names[i].last, $"SE{2018001 + i}"));
        }

        // 30 students for Event 2 (SE131–SE160)
        var e2Names = new (string first, string last)[]
        {
            ("Nguyen Thi", "Anh"), ("Tran Van", "Bao"), ("Le Thi", "Chi"),
            ("Pham Van", "Dat"), ("Vo Thi", "Dung"), ("Dang Van", "Duy"),
            ("Bui Thi", "Ha"), ("Do Van", "Hieu"), ("Ho Thi", "Hue"),
            ("Ngo Van", "Khoa"), ("Duong Thi", "Lai"), ("Ly Van", "Loc"),
            ("Mai Thi", "My"), ("Luong Van", "Nam"), ("Chu Thi", "Nga"),
            ("Cao Van", "Phong"), ("Phan Thi", "Phuong"), ("Ta Van", "Quan"),
            ("Quach Thi", "Thao"), ("La Van", "Thang"), ("Su Thi", "Thuy"),
            ("Lam Van", "Tung"), ("Kieu Thi", "Tuoi"), ("Dinh Van", "Vinh"),
            ("Vuong Thi", "Xuan"), ("Ha Van", "Y"), ("Khuc Thi", "Hanh"),
            ("Dao Van", "Loi"), ("Vu Thi", "Lien"), ("Luc Van", "Hai")
        };
        for (int i = 0; i < 30; i++)
        {
            var id = Guid.Parse($"10000000-0000-0000-0000-00000000{240 + i:X4}");
            var email = $"{ToEmailName(e2Names[i].first)}{ToEmailName(e2Names[i].last)}@fpt.edu.vn";
            fptStudents.Add(CreateFptUser(id, email, e2Names[i].first, e2Names[i].last, $"SE{2018031 + i}"));
        }

        // 10 Lecturers (5 Judge + 5 Mentor)
        var lecturers = new List<Users>
        {
            CreateFptUser(Judge1Id, "nguyen.thanh.tung@fpt.edu.vn", "Nguyen Thanh", "Tung", "GV001", RoleEnum.Lecturer),
            CreateFptUser(Judge2Id, "tran.le.hong@fpt.edu.vn", "Tran Le", "Hong", "GV002", RoleEnum.Lecturer),
            CreateFptUser(Judge3Id, "le.quoc.bao@fpt.edu.vn", "Le Quoc", "Bao", "GV003", RoleEnum.Lecturer),
            CreateFptUser(Judge4Id, "pham.duc.minh@fpt.edu.vn", "Pham Duc", "Minh", "GV004", RoleEnum.Lecturer),
            CreateFptUser(Judge5Id, "hoang.ngoc.son@fpt.edu.vn", "Hoang Ngoc", "Son", "GV005", RoleEnum.Lecturer),
            CreateFptUser(Mentor1Id, "vo.tuan.kiet@fpt.edu.vn", "Vo Tuan", "Kiet", "GV006", RoleEnum.Lecturer),
            CreateFptUser(Mentor2Id, "dang.thuy.linh@fpt.edu.vn", "Dang Thuy", "Linh", "GV007", RoleEnum.Lecturer),
            CreateFptUser(Mentor3Id, "bui.cong.thanh@fpt.edu.vn", "Bui Cong", "Thanh", "GV008", RoleEnum.Lecturer),
            CreateFptUser(Mentor4Id, "do.hoang.yen@fpt.edu.vn", "Do Hoang", "Yen", "GV009", RoleEnum.Lecturer),
            CreateFptUser(Mentor5Id, "ngo.quang.trung@fpt.edu.vn", "Ngo Quang", "Trung", "GV010", RoleEnum.Lecturer),
        };

        // 5 Staff
        var staff = new List<Users>
        {
            CreateFptUser(Staff1Id, "hoang.mai.anh@fpt.edu.vn", "Hoang Mai", "Anh", "STF006", RoleEnum.Staff),
            CreateFptUser(Staff2Id, "tran.minh.duc@fpt.edu.vn", "Tran Minh", "Duc", "STF007", RoleEnum.Staff),
            CreateFptUser(Staff3Id, "le.phuong.tha@fpt.edu.vn", "Le Phuong", "Thao", "STF008", RoleEnum.Staff),
            CreateFptUser(Staff4Id, "pham.quoc.huy@fpt.edu.vn", "Pham Quoc", "Huy", "STF009", RoleEnum.Staff),
            CreateFptUser(Staff5Id, "dang.thi.thu@fpt.edu.vn", "Dang Thi", "Thu", "STF010", RoleEnum.Staff),
        };

        modelBuilder.Entity<Users>().HasData(fptStudents.Concat(lecturers).Concat(staff));

        // ──────────────────────────────────────────────────────────
        //  EVENTS
        // ──────────────────────────────────────────────────────────
        modelBuilder.Entity<Events>().HasData(
            new Events
            {
                Id = Event1Id,
                Name = "SEAL Hackathon 2026 - Spring",
                Description = "Spring Hackathon for FPT students with creative technology solutions.",
                StartTime = new DateTimeOffset(2026, 6, 15, 8, 0, 0, TimeSpan.FromHours(7)),
                EndTime = new DateTimeOffset(2026, 7, 30, 17, 0, 0, TimeSpan.FromHours(7)),
                RegisterLimitTime = new DateTimeOffset(2026, 6, 25, 23, 59, 0, TimeSpan.FromHours(7)),
                LimitTeam = 10,
                MinMember = 2,
                MaxMember = 4,
                Status = EventStatusEnum.Published,
                NumberRound = 2,
                Season = SeasonEnum.Spring,
                IsDisable = false,
                CreatedAt = Now,
                UpdatedAt = Now
            },
            new Events
            {
                Id = Event2Id,
                Name = "SEAL Hackathon 2026 - Summer",
                Description = "Summer Hackathon for FPT students, now open for registration.",
                StartTime = new DateTimeOffset(2026, 8, 1, 8, 0, 0, TimeSpan.FromHours(7)),
                EndTime = new DateTimeOffset(2026, 9, 15, 17, 0, 0, TimeSpan.FromHours(7)),
                RegisterLimitTime = new DateTimeOffset(2026, 7, 25, 23, 59, 0, TimeSpan.FromHours(7)),
                LimitTeam = 10,
                MinMember = 2,
                MaxMember = 4,
                Status = EventStatusEnum.Published,
                NumberRound = 2,
                Season = SeasonEnum.Summer,
                IsDisable = false,
                CreatedAt = Now,
                UpdatedAt = Now
            }
        );

        // ──────────────────────────────────────────────────────────
        //  ROUNDS
        // ──────────────────────────────────────────────────────────
        modelBuilder.Entity<Rounds>().HasData(
            // Event 1 — Round 1 (ongoing)
            new Rounds
            {
                Id = Event1Round1Id,
                EventId = Event1Id,
                Name = "Round 1 - Idea",
                Description = "Present ideas and execution plans",
                RoundNo = 1,
                StartTime = new DateTimeOffset(2026, 6, 29, 8, 0, 0, TimeSpan.FromHours(7)),
                EndTime = new DateTimeOffset(2026, 7, 13, 17, 0, 0, TimeSpan.FromHours(7)),
                StartSubmission = new DateTimeOffset(2026, 6, 29, 8, 0, 0, TimeSpan.FromHours(7)),
                EndSubmission = new DateTimeOffset(2026, 7, 10, 23, 59, 0, TimeSpan.FromHours(7)),
                LimitTeam = 10,
                IsDisable = false,
                CreatedAt = Now,
                UpdatedAt = Now
            },
            // Event 1 — Round 2 (end of month)
            new Rounds
            {
                Id = Event1Round2Id,
                EventId = Event1Id,
                Name = "Round 2 - Final",
                Description = "Present the complete product",
                RoundNo = 2,
                StartTime = new DateTimeOffset(2026, 7, 21, 8, 0, 0, TimeSpan.FromHours(7)),
                EndTime = new DateTimeOffset(2026, 7, 30, 17, 0, 0, TimeSpan.FromHours(7)),
                StartSubmission = new DateTimeOffset(2026, 7, 21, 8, 0, 0, TimeSpan.FromHours(7)),
                EndSubmission = new DateTimeOffset(2026, 7, 28, 23, 59, 0, TimeSpan.FromHours(7)),
                LimitTeam = 6,
                IsDisable = false,
                CreatedAt = Now,
                UpdatedAt = Now
            },
            // Event 2 — Round 1 (not yet started)
            new Rounds
            {
                Id = Event2Round1Id,
                EventId = Event2Id,
                Name = "Round 1 - Idea",
                Description = "Present ideas and execution plans",
                RoundNo = 1,
                StartTime = new DateTimeOffset(2026, 8, 4, 8, 0, 0, TimeSpan.FromHours(7)),
                EndTime = new DateTimeOffset(2026, 8, 18, 17, 0, 0, TimeSpan.FromHours(7)),
                StartSubmission = new DateTimeOffset(2026, 8, 4, 8, 0, 0, TimeSpan.FromHours(7)),
                EndSubmission = new DateTimeOffset(2026, 8, 15, 23, 59, 0, TimeSpan.FromHours(7)),
                LimitTeam = 10,
                IsDisable = false,
                CreatedAt = Now,
                UpdatedAt = Now
            },
            // Event 2 — Round 2 (not yet started)
            new Rounds
            {
                Id = Event2Round2Id,
                EventId = Event2Id,
                Name = "Round 2 - Final",
                Description = "Present the complete product",
                RoundNo = 2,
                StartTime = new DateTimeOffset(2026, 8, 25, 8, 0, 0, TimeSpan.FromHours(7)),
                EndTime = new DateTimeOffset(2026, 9, 15, 17, 0, 0, TimeSpan.FromHours(7)),
                StartSubmission = new DateTimeOffset(2026, 8, 25, 8, 0, 0, TimeSpan.FromHours(7)),
                EndSubmission = new DateTimeOffset(2026, 9, 12, 23, 59, 0, TimeSpan.FromHours(7)),
                LimitTeam = 6,
                IsDisable = false,
                CreatedAt = Now,
                UpdatedAt = Now
            }
        );

        // ──────────────────────────────────────────────────────────
        //  AWARDS (2 per event)
        // ──────────────────────────────────────────────────────────
        modelBuilder.Entity<Awards>().HasData(
            new Awards { Id = Guid.Parse("26000000-0000-0000-0000-000000000100"), EventId = Event1Id, Name = "Champion", Description = "First place overall", LevelAward = 1, NumberOfAward = 1, Prize = 5000000m, IsDisable = false, CreatedAt = Now, UpdatedAt = Now },
            new Awards { Id = Guid.Parse("26000000-0000-0000-0000-000000000101"), EventId = Event1Id, Name = "Runner-up", Description = "Second place overall", LevelAward = 2, NumberOfAward = 1, Prize = 3000000m, IsDisable = false, CreatedAt = Now, UpdatedAt = Now },
            new Awards { Id = Guid.Parse("26000000-0000-0000-0000-000000000200"), EventId = Event2Id, Name = "Champion", Description = "First place overall", LevelAward = 1, NumberOfAward = 1, Prize = 5000000m, IsDisable = false, CreatedAt = Now, UpdatedAt = Now },
            new Awards { Id = Guid.Parse("26000000-0000-0000-0000-000000000201"), EventId = Event2Id, Name = "Runner-up", Description = "Second place overall", LevelAward = 2, NumberOfAward = 1, Prize = 3000000m, IsDisable = false, CreatedAt = Now, UpdatedAt = Now }
        );

        // ──────────────────────────────────────────────────────────
        //  TRACKS + TOPICS
        // ──────────────────────────────────────────────────────────
        var tracks = new List<Tracks>();
        var topics = new List<Topics>();

        // Event 1 tracks
        var ev1TrackData = new (Guid id, string title, string desc)[]
        {
            (Ev1TrackAiId, "AI - Artificial Intelligence", "AI-powered solutions for everyday life"),
            (Ev1TrackMobileId, "Mobile - Mobile Applications", "Developing applications on mobile platforms"),
            (Ev1TrackWebId, "Web - Web Technology", "Building modern web applications"),
            (Ev1TrackDataId, "Data - Data Science", "Data analysis and exploitation"),
            (Ev1TrackCloudId, "Cloud - Cloud Computing", "Deployment solutions on Cloud platforms"),
        };
        foreach (var (id, title, desc) in ev1TrackData)
            tracks.Add(new Tracks { Id = id, EventId = Event1Id, Title = title, Description = desc, MaxTeam = 2, IsDisable = false, CreatedAt = Now, UpdatedAt = Now });

        var ev1TopicData = new (Guid id, Guid trackId, string title, string desc)[]
        {
            (Ev1TopicAiId, Ev1TrackAiId, "Learning Support Chatbot", "Build an AI chatbot to assist student learning"),
            (Ev1TopicMobileId, Ev1TrackMobileId, "Personal Finance Manager App", "Mobile app for expense management"),
            (Ev1TopicWebId, Ev1TrackWebId, "Volunteer Connection Platform", "Social network connecting volunteer activities"),
            (Ev1TopicDataId, Ev1TrackDataId, "Weather Forecasting System", "Data analysis application for weather prediction"),
            (Ev1TopicCloudId, Ev1TrackCloudId, "Automated CI/CD System", "Deploy CI/CD pipeline on Cloud"),
        };
        foreach (var (id, trackId, title, desc) in ev1TopicData)
            topics.Add(new Topics { Id = id, TrackId = trackId, Title = title, Description = desc, IsDisable = false, CreatedAt = Now, UpdatedAt = Now });

        // Event 2 tracks
        var ev2TrackData = new (Guid id, string title, string desc)[]
        {
            (Ev2TrackAiId, "AI - Image Processing", "Intelligent image processing solutions"),
            (Ev2TrackMobileId, "Mobile - Gaming", "Developing games on mobile platforms"),
            (Ev2TrackWebId, "Web - E-Commerce", "Building e-commerce platforms"),
            (Ev2TrackDataId, "Data - Machine Learning", "Applied Machine Learning models"),
            (Ev2TrackCloudId, "Cloud - DevOps", "Infrastructure automation and deployment"),
        };
        foreach (var (id, title, desc) in ev2TrackData)
            tracks.Add(new Tracks { Id = id, EventId = Event2Id, Title = title, Description = desc, MaxTeam = 2, IsDisable = false, CreatedAt = Now, UpdatedAt = Now });

        var ev2TopicData = new (Guid id, Guid trackId, string title, string desc)[]
        {
            (Ev2TopicAiId, Ev2TrackAiId, "Facial Emotion Recognition", "AI recognizes emotions via camera"),
            (Ev2TopicMobileId, Ev2TrackMobileId, "Interactive Educational Game", "Learning games for children"),
            (Ev2TopicWebId, Ev2TrackWebId, "Online Agricultural Marketplace", "Platform connecting farmers and buyers"),
            (Ev2TopicDataId, Ev2TrackDataId, "Product Recommendation System", "Recommendation engine for e-commerce"),
            (Ev2TopicCloudId, Ev2TrackCloudId, "System Monitoring Platform", "Automated Cloud infrastructure monitoring"),
        };
        foreach (var (id, trackId, title, desc) in ev2TopicData)
            topics.Add(new Topics { Id = id, TrackId = trackId, Title = title, Description = desc, IsDisable = false, CreatedAt = Now, UpdatedAt = Now });

        modelBuilder.Entity<Tracks>().HasData(tracks);
        modelBuilder.Entity<Topics>().HasData(topics);

        // ──────────────────────────────────────────────────────────
        //  CRITERIA TEMPLATES + ITEMS
        // ──────────────────────────────────────────────────────────
        var criteriaTemplates = new List<CriteriaTemplates>();
        var criteriaItems = new List<CriteriaItems>();

        // Event 1 Round 1 — 3 templates, template 1 active (!IsDisable)
        criteriaTemplates.AddRange(new[]
        {
            new CriteriaTemplates { Id = Ev1R1Tpl1Id, RoundId = Event1Round1Id, Title = "Idea Evaluation", Description = "Criteria for evaluating ideas in round 1", IsDisable = false, IsActive = true, CreatedAt = Now, UpdatedAt = Now },
            new CriteriaTemplates { Id = Ev1R1Tpl2Id, RoundId = Event1Round1Id, Title = "Technical Evaluation", Description = "Criteria for evaluating technical aspects (backup)", IsDisable = true, IsActive = false, CreatedAt = Now, UpdatedAt = Now },
            new CriteriaTemplates { Id = Ev1R1Tpl3Id, RoundId = Event1Round1Id, Title = "Presentation Evaluation", Description = "Criteria for evaluating presentation skills (backup)", IsDisable = true, IsActive = false, CreatedAt = Now, UpdatedAt = Now },
        });
        // Items for template 1 (active)
        var e1r1Items = new (Guid id, string name, string desc, decimal score)[]
        {
            (Guid.Parse("23000000-0000-0000-0000-000000000100"), "Creativity", "Level of novelty and creativity of the idea", 25m),
            (Guid.Parse("23000000-0000-0000-0000-000000000101"), "Feasibility", "Ability to implement in practice", 25m),
            (Guid.Parse("23000000-0000-0000-0000-000000000102"), "Social Impact", "Value brought to the community", 20m),
            (Guid.Parse("23000000-0000-0000-0000-000000000103"), "Technology Usage", "Appropriate technology application", 20m),
            (Guid.Parse("23000000-0000-0000-0000-000000000104"), "Execution Plan", "Clarity and detail of the roadmap", 10m),
        };
        foreach (var (id, name, desc, score) in e1r1Items)
            criteriaItems.Add(new CriteriaItems { Id = id, CriteriaTemplateId = Ev1R1Tpl1Id, Name = name, Description = desc, Score = score, IsDisable = false, CreatedAt = Now, UpdatedAt = Now });
        // Items for template 2 (inactive) & template 3 (inactive)
        criteriaItems.AddRange(e1r1Items.Select((x, idx) => new CriteriaItems { Id = Guid.Parse($"23000000-0000-0000-0000-0000000001A{idx:X1}"), CriteriaTemplateId = Ev1R1Tpl2Id, Name = x.name, Description = x.desc, Score = x.score, IsDisable = true, CreatedAt = Now, UpdatedAt = Now }));
        criteriaItems.AddRange(e1r1Items.Select((x, idx) => new CriteriaItems { Id = Guid.Parse($"23000000-0000-0000-0000-0000000001B{idx:X1}"), CriteriaTemplateId = Ev1R1Tpl3Id, Name = x.name, Description = x.desc, Score = x.score, IsDisable = true, CreatedAt = Now, UpdatedAt = Now }));

        modelBuilder.Entity<CriteriaTemplates>().HasData(criteriaTemplates);
        modelBuilder.Entity<CriteriaItems>().HasData(criteriaItems);

        // Event 1 Round 2 — 3 templates, template 1 active
        var e1r2items = new (Guid id, string name, string desc, decimal score)[]
        {
            (Guid.Parse("23000000-0000-0000-0000-000000000105"), "Product Quality", "Complete and stable product", 30m),
            (Guid.Parse("23000000-0000-0000-0000-000000000106"), "Creativity", "Level of innovation in the solution", 25m),
            (Guid.Parse("23000000-0000-0000-0000-000000000107"), "Technical Quality", "Code quality and architecture", 20m),
            (Guid.Parse("23000000-0000-0000-0000-000000000108"), "Presentation", "Presentation and demo skills", 15m),
            (Guid.Parse("23000000-0000-0000-0000-000000000109"), "Impact", "Practical value and scalability", 10m),
        };
        modelBuilder.Entity<CriteriaTemplates>().HasData(
            new CriteriaTemplates { Id = Ev1R2Tpl1Id, RoundId = Event1Round2Id, Title = "Final Evaluation", Description = "Criteria for evaluating the final round", IsDisable = false, IsActive = true, CreatedAt = Now, UpdatedAt = Now },
            new CriteriaTemplates { Id = Ev1R2Tpl2Id, RoundId = Event1Round2Id, Title = "Backup Evaluation 1", Description = "Backup template", IsDisable = true, IsActive = false, CreatedAt = Now, UpdatedAt = Now },
            new CriteriaTemplates { Id = Ev1R2Tpl3Id, RoundId = Event1Round2Id, Title = "Backup Evaluation 2", Description = "Backup template", IsDisable = true, IsActive = false, CreatedAt = Now, UpdatedAt = Now }
        );
        modelBuilder.Entity<CriteriaItems>().HasData(
            e1r2items.Select((x, idx) => new CriteriaItems { Id = x.id, CriteriaTemplateId = Ev1R2Tpl1Id, Name = x.name, Description = x.desc, Score = x.score, IsDisable = false, CreatedAt = Now, UpdatedAt = Now }).Concat(
            e1r2items.Select((x, idx) => new CriteriaItems { Id = Guid.Parse($"23000000-0000-0000-0000-00000000011{idx:X1}"), CriteriaTemplateId = Ev1R2Tpl2Id, Name = x.name, Description = x.desc, Score = x.score, IsDisable = true, CreatedAt = Now, UpdatedAt = Now }).Concat(
            e1r2items.Select((x, idx) => new CriteriaItems { Id = Guid.Parse($"23000000-0000-0000-0000-00000000012{idx:X1}"), CriteriaTemplateId = Ev1R2Tpl3Id, Name = x.name, Description = x.desc, Score = x.score, IsDisable = true, CreatedAt = Now, UpdatedAt = Now }))));

        // Event 2 Round 1 — 3 templates, template 1 active
        modelBuilder.Entity<CriteriaTemplates>().HasData(
            new CriteriaTemplates { Id = Ev2R1Tpl1Id, RoundId = Event2Round1Id, Title = "Idea Evaluation", Description = "Criteria for evaluating ideas", IsDisable = false, IsActive = true, CreatedAt = Now, UpdatedAt = Now },
            new CriteriaTemplates { Id = Ev2R1Tpl2Id, RoundId = Event2Round1Id, Title = "Backup Evaluation 1", Description = "Backup template", IsDisable = true, IsActive = false, CreatedAt = Now, UpdatedAt = Now },
            new CriteriaTemplates { Id = Ev2R1Tpl3Id, RoundId = Event2Round1Id, Title = "Backup Evaluation 2", Description = "Backup template", IsDisable = true, IsActive = false, CreatedAt = Now, UpdatedAt = Now }
        );
        modelBuilder.Entity<CriteriaItems>().HasData(
            e1r1Items.Select((x, idx) => new CriteriaItems { Id = Guid.Parse($"23000000-0000-0000-0000-00000000013{idx:X1}"), CriteriaTemplateId = Ev2R1Tpl1Id, Name = x.name, Description = x.desc, Score = x.score, IsDisable = false, CreatedAt = Now, UpdatedAt = Now }).Concat(
            e1r1Items.Select((x, idx) => new CriteriaItems { Id = Guid.Parse($"23000000-0000-0000-0000-00000000014{idx:X1}"), CriteriaTemplateId = Ev2R1Tpl2Id, Name = x.name, Description = x.desc, Score = x.score, IsDisable = true, CreatedAt = Now, UpdatedAt = Now }).Concat(
            e1r1Items.Select((x, idx) => new CriteriaItems { Id = Guid.Parse($"23000000-0000-0000-0000-00000000015{idx:X1}"), CriteriaTemplateId = Ev2R1Tpl3Id, Name = x.name, Description = x.desc, Score = x.score, IsDisable = true, CreatedAt = Now, UpdatedAt = Now }))));

        // Event 2 Round 2 — 3 templates, template 1 active
        modelBuilder.Entity<CriteriaTemplates>().HasData(
            new CriteriaTemplates { Id = Ev2R2Tpl1Id, RoundId = Event2Round2Id, Title = "Final Evaluation", Description = "Criteria for evaluating the final round", IsDisable = false, IsActive = true, CreatedAt = Now, UpdatedAt = Now },
            new CriteriaTemplates { Id = Ev2R2Tpl2Id, RoundId = Event2Round2Id, Title = "Backup Evaluation 1", Description = "Backup template", IsDisable = true, IsActive = false, CreatedAt = Now, UpdatedAt = Now },
            new CriteriaTemplates { Id = Ev2R2Tpl3Id, RoundId = Event2Round2Id, Title = "Backup Evaluation 2", Description = "Backup template", IsDisable = true, IsActive = false, CreatedAt = Now, UpdatedAt = Now }
        );
        modelBuilder.Entity<CriteriaItems>().HasData(
            e1r2items.Select((x, idx) => new CriteriaItems { Id = Guid.Parse($"23000000-0000-0000-0000-00000000016{idx:X1}"), CriteriaTemplateId = Ev2R2Tpl1Id, Name = x.name, Description = x.desc, Score = x.score, IsDisable = false, CreatedAt = Now, UpdatedAt = Now }).Concat(
            e1r2items.Select((x, idx) => new CriteriaItems { Id = Guid.Parse($"23000000-0000-0000-0000-00000000017{idx:X1}"), CriteriaTemplateId = Ev2R2Tpl2Id, Name = x.name, Description = x.desc, Score = x.score, IsDisable = true, CreatedAt = Now, UpdatedAt = Now }).Concat(
            e1r2items.Select((x, idx) => new CriteriaItems { Id = Guid.Parse($"23000000-0000-0000-0000-00000000018{idx:X1}"), CriteriaTemplateId = Ev2R2Tpl3Id, Name = x.name, Description = x.desc, Score = x.score, IsDisable = true, CreatedAt = Now, UpdatedAt = Now }))));

        // ──────────────────────────────────────────────────────────
        //  TEAMS + TEAMDETAILS + REGISTERTEAMS
        // ──────────────────────────────────────────────────────────

        // Event 1 — 10 teams, 3 members each, all approved
        var e1TeamNames = new[]
        {
            "FPT AI Pioneers", "FPT Code Breakers", "FPT Mobile Knights", "FPT Web Wizards",
            "FPT Data Miners", "FPT Cloud Ninjas", "FPT AI Avengers", "FPT Mobile Stars",
            "FPT Web Builders", "FPT Data Hawks"
        };
        // Assign tracks to teams (2 teams per track)
        var e1TeamToTrack = new Guid[] { Ev1TrackAiId, Ev1TrackAiId, Ev1TrackMobileId, Ev1TrackMobileId, Ev1TrackWebId, Ev1TrackWebId, Ev1TrackDataId, Ev1TrackDataId, Ev1TrackCloudId, Ev1TrackCloudId };
        // Assign topics to teams (same topic per track)
        var e1TeamToTopic = new Guid[] { Ev1TopicAiId, Ev1TopicAiId, Ev1TopicMobileId, Ev1TopicMobileId, Ev1TopicWebId, Ev1TopicWebId, Ev1TopicDataId, Ev1TopicDataId, Ev1TopicCloudId, Ev1TopicCloudId };

        var e1Teams = new List<Teams>();
        var e1TeamDetails = new List<TeamDetails>();
        var e1RegisterTeams = new List<RegisterTeams>();

        for (int i = 0; i < 10; i++)
        {
            var teamId = Guid.Parse($"30000000-0000-0000-0000-0000000001{i:X1}0");
            e1Teams.Add(new Teams { Id = teamId, Name = e1TeamNames[i], CanEdit = false, IsDisable = false, CreatedAt = Now, UpdatedAt = Now });

            // 3 members per team
            var memberIds = new Guid[]
            {
                Guid.Parse($"10000000-0000-0000-0000-00000000{200 + i * 3:X4}"),
                Guid.Parse($"10000000-0000-0000-0000-00000000{201 + i * 3:X4}"),
                Guid.Parse($"10000000-0000-0000-0000-00000000{202 + i * 3:X4}"),
            };
            e1TeamDetails.Add(CreateTeamDetail(Guid.Parse($"30100000-0000-0000-0000-0000000001{i:X1}0"), teamId, memberIds[0], true));
            e1TeamDetails.Add(CreateTeamDetail(Guid.Parse($"30100000-0000-0000-0000-0000000001{i:X1}1"), teamId, memberIds[1], false));
            e1TeamDetails.Add(CreateTeamDetail(Guid.Parse($"30100000-0000-0000-0000-0000000001{i:X1}2"), teamId, memberIds[2], false));

            e1RegisterTeams.Add(new RegisterTeams
            {
                Id = Guid.Parse($"31000000-0000-0000-0000-0000000001{i:X1}0"),
                TeamId = teamId,
                EventId = Event1Id,
                TrackId = e1TeamToTrack[i],
                TopicId = e1TeamToTopic[i],
                Description = $"Registration - Team {e1TeamNames[i]}",
                Status = RegisterTeamStatusEnum.Approved,
                IsBanned = false,
                IsDisable = false,
                CreatedAt = Now,
                UpdatedAt = Now
            });
        }

        // Event 2 — 10 teams, 3 members each, mixed status
        var e2TeamNames = new[]
        {
            "FPT Green AI", "FPT Cyber Shield", "FPT Game Dev", "FPT E-Commerce",
            "FPT ML Masters", "FPT DevOps Pro", "FPT Vision AI", "FPT Edu Game",
            "FPT AgriTech", "FPT Monitor Team"
        };
        var e2TeamToTrack = new Guid[] { Ev2TrackAiId, Ev2TrackAiId, Ev2TrackMobileId, Ev2TrackWebId, Ev2TrackDataId, Ev2TrackCloudId, Ev2TrackAiId, Ev2TrackMobileId, Ev2TrackWebId, Ev2TrackCloudId };
        var e2TeamToTopic = new Guid[] { Ev2TopicAiId, Ev2TopicAiId, Ev2TopicMobileId, Ev2TopicWebId, Ev2TopicDataId, Ev2TopicCloudId, Ev2TopicAiId, Ev2TopicMobileId, Ev2TopicWebId, Ev2TopicCloudId };
        var e2Statuses = new RegisterTeamStatusEnum[]
        {
            RegisterTeamStatusEnum.Approved, RegisterTeamStatusEnum.Approved, RegisterTeamStatusEnum.Approved,
            RegisterTeamStatusEnum.Approved, RegisterTeamStatusEnum.Approved,
            RegisterTeamStatusEnum.Pending, RegisterTeamStatusEnum.Pending, RegisterTeamStatusEnum.Pending,
            RegisterTeamStatusEnum.Rejected, RegisterTeamStatusEnum.Rejected
        };
        var e2RejectionReasons = new string?[]
        {
            null, null, null, null, null, null, null, null, "Missing member information", "Application does not meet requirements"
        };

        var e2Teams = new List<Teams>();
        var e2TeamDetails = new List<TeamDetails>();
        var e2RegisterTeams = new List<RegisterTeams>();

        for (int i = 0; i < 10; i++)
        {
            var teamId = Guid.Parse($"30000000-0000-0000-0000-0000000002{i:X1}0");
            e2Teams.Add(new Teams { Id = teamId, Name = e2TeamNames[i], CanEdit = e2Statuses[i] != RegisterTeamStatusEnum.Approved, IsDisable = false, CreatedAt = Now, UpdatedAt = Now });

            var memberIds = new Guid[]
            {
                Guid.Parse($"10000000-0000-0000-0000-00000000{240 + i * 3:X4}"),
                Guid.Parse($"10000000-0000-0000-0000-00000000{241 + i * 3:X4}"),
                Guid.Parse($"10000000-0000-0000-0000-00000000{242 + i * 3:X4}"),
            };
            e2TeamDetails.Add(CreateTeamDetail(Guid.Parse($"30100000-0000-0000-0000-0000000002{i:X1}0"), teamId, memberIds[0], true));
            e2TeamDetails.Add(CreateTeamDetail(Guid.Parse($"30100000-0000-0000-0000-0000000002{i:X1}1"), teamId, memberIds[1], false));
            e2TeamDetails.Add(CreateTeamDetail(Guid.Parse($"30100000-0000-0000-0000-0000000002{i:X1}2"), teamId, memberIds[2], false));

            e2RegisterTeams.Add(new RegisterTeams
            {
                Id = Guid.Parse($"31000000-0000-0000-0000-0000000002{i:X1}0"),
                TeamId = teamId,
                EventId = Event2Id,
                TrackId = e2TeamToTrack[i],
                TopicId = e2TeamToTopic[i],
                Description = $"Registration - Team {e2TeamNames[i]}",
                Status = e2Statuses[i],
                RejectionReason = e2RejectionReasons[i],
                IsBanned = false,
                IsDisable = false,
                CreatedAt = Now,
                UpdatedAt = Now
            });
        }

        modelBuilder.Entity<Teams>().HasData(e1Teams.Concat(e2Teams));
        modelBuilder.Entity<TeamDetails>().HasData(e1TeamDetails.Concat(e2TeamDetails));
        modelBuilder.Entity<RegisterTeams>().HasData(e1RegisterTeams.Concat(e2RegisterTeams));

        // ──────────────────────────────────────────────────────────
        //  ROUNDDETAILS + SUBMISSIONS (Event 1 Round 1 only)
        // ──────────────────────────────────────────────────────────
        // Only teams approved for Event 1 get RoundDetails + Submissions
        var e1RoundDetails = new List<RoundDetails>();
        var e1Submissions = new List<Submissions>();

        for (int i = 0; i < 10; i++)
        {
            var rdId = Guid.Parse($"32000000-0000-0000-0000-0000000001{i:X1}0");
            var registerTeamId = Guid.Parse($"31000000-0000-0000-0000-0000000001{i:X1}0");

            e1RoundDetails.Add(new RoundDetails
            {
                Id = rdId,
                RoundId = Event1Round1Id,
                RegisterTeamId = registerTeamId,
                IsDisable = false,
                CreatedAt = Now,
                UpdatedAt = Now
            });

            e1Submissions.Add(new Submissions
            {
                Id = Guid.Parse($"33000000-0000-0000-0000-0000000001{i:X1}0"),
                RoundDetailId = rdId,
                Url = $"https://github.com/fpt-hackathon-2026/team-{i + 1}-submission",
                Description = $"Round 1 submission - {e1TeamNames[i]}",
                Status = i < 6 ? SubmissionStatusEnum.Graded : SubmissionStatusEnum.Submitted,
                SubmittedAt = new DateTimeOffset(2026, 7, 1, 9 + i, 0, 0, TimeSpan.FromHours(7)),
                IsDisable = false,
                CreatedAt = Now,
                UpdatedAt = Now
            });
        }

        // Round 2 scenarios: team 1 can still advance, team 2 can revert, team 3 has completed the final round.
        e1RoundDetails.Add(new RoundDetails
        {
            Id = Guid.Parse("32000000-0000-0000-0000-000000000300"),
            RoundId = Event1Round2Id,
            RegisterTeamId = Guid.Parse("31000000-0000-0000-0000-000000000110"),
            IsDisable = false,
            CreatedAt = new DateTimeOffset(2026, 7, 21, 8, 0, 0, TimeSpan.FromHours(7)),
            UpdatedAt = new DateTimeOffset(2026, 7, 21, 8, 0, 0, TimeSpan.FromHours(7))
        });
        e1RoundDetails.Add(new RoundDetails
        {
            Id = Guid.Parse("32000000-0000-0000-0000-000000000301"),
            RoundId = Event1Round2Id,
            RegisterTeamId = Guid.Parse("31000000-0000-0000-0000-000000000120"),
            IsDisable = false,
            CreatedAt = new DateTimeOffset(2026, 7, 21, 8, 0, 0, TimeSpan.FromHours(7)),
            UpdatedAt = new DateTimeOffset(2026, 7, 21, 8, 0, 0, TimeSpan.FromHours(7))
        });
        e1Submissions.Add(new Submissions
        {
            Id = Guid.Parse("33000000-0000-0000-0000-000000000300"),
            RoundDetailId = Guid.Parse("32000000-0000-0000-0000-000000000301"),
            Url = "https://github.com/fpt-hackathon-2026/team-3-final",
            Description = "Round 2 final submission - FPT Mobile Knights",
            Status = SubmissionStatusEnum.Graded,
            SubmittedAt = new DateTimeOffset(2026, 7, 25, 10, 0, 0, TimeSpan.FromHours(7)),
            IsDisable = false,
            CreatedAt = new DateTimeOffset(2026, 7, 25, 10, 0, 0, TimeSpan.FromHours(7)),
            UpdatedAt = new DateTimeOffset(2026, 7, 25, 14, 0, 0, TimeSpan.FromHours(7))
        });

        modelBuilder.Entity<RoundDetails>().HasData(e1RoundDetails);
        modelBuilder.Entity<Submissions>().HasData(e1Submissions);

        // ──────────────────────────────────────────────────────────
        //  ASSIGNMENTS (Event 1 + Event 2)
        // ──────────────────────────────────────────────────────────
        var assignEvents = new List<AssignEvents>();
        var assignTracks = new List<AssignTracks>();

        // Event 1: 5 judges
        for (int i = 0; i < 5; i++)
        {
            var aeId = Guid.Parse($"40000000-0000-0000-0000-0000000001{i:X1}0");
            assignEvents.Add(new AssignEvents
            {
                Id = aeId,
                UserId = JudgeIds[i],
                EventRoleId = SeedConstants.JudgeEventRoleId,
                EventId = Event1Id,
                IsDisable = false,
                CreatedAt = Now,
                UpdatedAt = Now
            });
            assignTracks.Add(new AssignTracks
            {
                Id = Guid.Parse($"41000000-0000-0000-0000-0000000001{i:X1}0"),
                AssignEventId = aeId,
                TrackId = Ev1TrackIds[i],
                IsDisable = false,
                CreatedAt = Now,
                UpdatedAt = Now
            });
        }

        // Event 1: 5 mentors
        for (int i = 0; i < 5; i++)
        {
            var aeId = Guid.Parse($"40000000-0000-0000-0000-0000000001{5 + i:X1}0");
            assignEvents.Add(new AssignEvents
            {
                Id = aeId,
                UserId = MentorIds[i],
                EventRoleId = SeedConstants.MentorEventRoleId,
                EventId = Event1Id,
                IsDisable = false,
                CreatedAt = Now,
                UpdatedAt = Now
            });
            assignTracks.Add(new AssignTracks
            {
                Id = Guid.Parse($"41000000-0000-0000-0000-0000000001{5 + i:X1}0"),
                AssignEventId = aeId,
                TrackId = Ev1TrackIds[i],
                IsDisable = false,
                CreatedAt = Now,
                UpdatedAt = Now
            });
        }

        // Event 1: 5 staff
        for (int i = 0; i < 5; i++)
        {
            assignEvents.Add(new AssignEvents
            {
                Id = Guid.Parse($"40000000-0000-0000-0000-0000000001{10 + i:X1}0"),
                UserId = StaffIds[i],
                EventRoleId = SeedConstants.StaffEventRoleId,
                EventId = Event1Id,
                IsDisable = false,
                CreatedAt = Now,
                UpdatedAt = Now
            });
        }

        // Event 2: 5 judges
        for (int i = 0; i < 5; i++)
        {
            var aeId = Guid.Parse($"40000000-0000-0000-0000-0000000002{i:X1}0");
            assignEvents.Add(new AssignEvents
            {
                Id = aeId,
                UserId = JudgeIds[i],
                EventRoleId = SeedConstants.JudgeEventRoleId,
                EventId = Event2Id,
                IsDisable = false,
                CreatedAt = Now,
                UpdatedAt = Now
            });
            assignTracks.Add(new AssignTracks
            {
                Id = Guid.Parse($"41000000-0000-0000-0000-0000000002{i:X1}0"),
                AssignEventId = aeId,
                TrackId = Ev2TrackIds[i],
                IsDisable = false,
                CreatedAt = Now,
                UpdatedAt = Now
            });
        }

        // Event 2: 5 mentors
        for (int i = 0; i < 5; i++)
        {
            var aeId = Guid.Parse($"40000000-0000-0000-0000-0000000002{5 + i:X1}0");
            assignEvents.Add(new AssignEvents
            {
                Id = aeId,
                UserId = MentorIds[i],
                EventRoleId = SeedConstants.MentorEventRoleId,
                EventId = Event2Id,
                IsDisable = false,
                CreatedAt = Now,
                UpdatedAt = Now
            });
            assignTracks.Add(new AssignTracks
            {
                Id = Guid.Parse($"41000000-0000-0000-0000-0000000002{5 + i:X1}0"),
                AssignEventId = aeId,
                TrackId = Ev2TrackIds[i],
                IsDisable = false,
                CreatedAt = Now,
                UpdatedAt = Now
            });
        }

        // Event 2: 5 staff
        for (int i = 0; i < 5; i++)
        {
            assignEvents.Add(new AssignEvents
            {
                Id = Guid.Parse($"40000000-0000-0000-0000-0000000002{10 + i:X1}0"),
                UserId = StaffIds[i],
                EventRoleId = SeedConstants.StaffEventRoleId,
                EventId = Event2Id,
                IsDisable = false,
                CreatedAt = Now,
                UpdatedAt = Now
            });
        }

        // Additional judges on the Mobile track for multi-judge averaging.
        assignTracks.Add(new AssignTracks
        {
            Id = Guid.Parse("41000000-0000-0000-0000-000000000300"),
            AssignEventId = Guid.Parse("40000000-0000-0000-0000-000000000100"),
            TrackId = Ev1TrackMobileId,
            IsDisable = false,
            CreatedAt = Now,
            UpdatedAt = Now
        });
        assignTracks.Add(new AssignTracks
        {
            Id = Guid.Parse("41000000-0000-0000-0000-000000000301"),
            AssignEventId = Guid.Parse("40000000-0000-0000-0000-000000000120"),
            TrackId = Ev1TrackMobileId,
            IsDisable = false,
            CreatedAt = Now,
            UpdatedAt = Now
        });

        modelBuilder.Entity<AssignEvents>().HasData(assignEvents);
        modelBuilder.Entity<AssignTracks>().HasData(assignTracks);

        // ──────────────────────────────────────────────────────────
        //  SCORES + SCOREITEMS (Event 1 Round 1 — 2 teams per track)
        //  One judge per track scores Round 1; Round 2 adds a multi-judge Mobile scenario.
        // ──────────────────────────────────────────────────────────

        // For simplicity: the judge assigned to each track scores both teams in their track
        var judgeScores = new List<Scores>();
        var judgeScoreItems = new List<ScoreItems>();

        // Get the active criteria items for Event 1 Round 1
        var e1r1ActiveItems = e1r1Items; // from template Ev1R1Tpl1Id

        for (int t = 0; t < 5; t++)
        {
            var judgeTrackId = Guid.Parse($"41000000-0000-0000-0000-0000000001{t:X1}0");
            // 2 teams per track
            for (int teamIdx = 0; teamIdx < 2; teamIdx++)
            {
                var teamGlobalIdx = t * 2 + teamIdx;
                var submissionId = Guid.Parse($"33000000-0000-0000-0000-0000000001{teamGlobalIdx:X1}0");
                var scoreId = Guid.Parse($"50000000-0000-0000-0000-0000000001{teamGlobalIdx:X1}0");

                // Deterministic score generation
                var seed = teamGlobalIdx * 7 + 50;
                var itemScores = new decimal[5];
                var remaining = seed % 41 + 60m; // 60-100
                for (int si = 0; si < 4; si++)
                {
                    var maxItem = e1r1ActiveItems[si].score;
                    var portion = remaining * (maxItem / 100m);
                    itemScores[si] = Math.Max(0, Math.Min(maxItem, Math.Round(portion, 1)));
                    remaining -= itemScores[si];
                }
                itemScores[4] = Math.Max(0, Math.Min(e1r1ActiveItems[4].score, Math.Round(remaining, 1)));

                // Only grade some teams (6 out of 10 graded, 4 pending)
                bool isGraded = teamGlobalIdx < 6;
                if (isGraded)
                {
                    judgeScores.Add(new Scores
                    {
                        Id = scoreId,
                        SubmissionId = submissionId,
                        AssignTrackId = judgeTrackId,
                        IsRetake = false,
                        TotalScore = itemScores.Sum(),
                        IsMock = false,
                        IsDisable = false,
                        CreatedAt = Now,
                        UpdatedAt = Now
                    });

                    for (int si = 0; si < 5; si++)
                    {
                        judgeScoreItems.Add(new ScoreItems
                        {
                            Id = Guid.Parse($"51000000-0000-0000-0000-0000000001{teamGlobalIdx:X1}{si:X1}"),
                            ScoreId = scoreId,
                            CriteriaItemId = e1r1ActiveItems[si].id,
                            AssignTrackId = judgeTrackId,
                            Score = itemScores[si],
                            Comment = itemScores[si] >= e1r1ActiveItems[si].score * 0.8m ? "Excellent"
                                : itemScores[si] >= e1r1ActiveItems[si].score * 0.6m ? "Good"
                                : itemScores[si] >= e1r1ActiveItems[si].score * 0.4m ? "Average" : "Needs Improvement",
                            IsDisable = false,
                            CreatedAt = Now,
                            UpdatedAt = Now
                        });
                    }
                }
            }
        }

        var round2ScoreData = new[]
        {
            new
            {
                ScoreId = Guid.Parse("50000000-0000-0000-0000-000000000300"),
                AssignTrackId = Guid.Parse("41000000-0000-0000-0000-000000000110"),
                ItemScores = new decimal[] { 24m, 20m, 16m, 12m, 8m }
            },
            new
            {
                ScoreId = Guid.Parse("50000000-0000-0000-0000-000000000301"),
                AssignTrackId = Guid.Parse("41000000-0000-0000-0000-000000000300"),
                ItemScores = new decimal[] { 30m, 25m, 20m, 15m, 10m }
            }
        };

        for (int judgeIndex = 0; judgeIndex < round2ScoreData.Length; judgeIndex++)
        {
            var scoreData = round2ScoreData[judgeIndex];
            judgeScores.Add(new Scores
            {
                Id = scoreData.ScoreId,
                SubmissionId = Guid.Parse("33000000-0000-0000-0000-000000000300"),
                AssignTrackId = scoreData.AssignTrackId,
                IsRetake = false,
                TotalScore = scoreData.ItemScores.Sum(),
                IsMock = false,
                IsDisable = false,
                CreatedAt = new DateTimeOffset(2026, 7, 25, 14 + judgeIndex, 0, 0, TimeSpan.FromHours(7)),
                UpdatedAt = new DateTimeOffset(2026, 7, 25, 14 + judgeIndex, 0, 0, TimeSpan.FromHours(7))
            });

            for (int itemIndex = 0; itemIndex < e1r2items.Length; itemIndex++)
            {
                judgeScoreItems.Add(new ScoreItems
                {
                    Id = Guid.Parse($"51000000-0000-0000-0000-000000003{judgeIndex}{itemIndex}0"),
                    ScoreId = scoreData.ScoreId,
                    CriteriaItemId = e1r2items[itemIndex].id,
                    AssignTrackId = scoreData.AssignTrackId,
                    Score = scoreData.ItemScores[itemIndex],
                    Comment = judgeIndex == 0 ? "Good final submission" : "Excellent final submission",
                    IsDisable = false,
                    CreatedAt = new DateTimeOffset(2026, 7, 25, 14 + judgeIndex, 0, 0, TimeSpan.FromHours(7)),
                    UpdatedAt = new DateTimeOffset(2026, 7, 25, 14 + judgeIndex, 0, 0, TimeSpan.FromHours(7))
                });
            }
        }

        modelBuilder.Entity<Scores>().HasData(judgeScores);
        modelBuilder.Entity<ScoreItems>().HasData(judgeScoreItems);

        // ──────────────────────────────────────────────────────────
        //  LEADERBOARDS + DETAILS (Event 1 only)
        // ──────────────────────────────────────────────────────────
        modelBuilder.Entity<LeaderBoards>().HasData(
            new LeaderBoards { Id = Event1LeaderBoardId, EventId = Event1Id, Year = 2026, IsPublished = true, IsDisable = false, CreatedAt = Now, UpdatedAt = Now },
            new LeaderBoards { Id = Event2LeaderBoardId, EventId = Event2Id, Year = 2026, IsPublished = false, IsDisable = false, CreatedAt = Now, UpdatedAt = Now }
        );

        // Only teams that submitted AND got scored appear
        var leaderBoardDetails = new List<LeaderBoardDetails>();
        for (int i = 0; i < 6; i++)
        {
            var teamId = Guid.Parse($"30000000-0000-0000-0000-0000000001{i:X1}0");
            var scoreId = Guid.Parse($"50000000-0000-0000-0000-0000000001{i:X1}0");
            var score = judgeScores.FirstOrDefault(s => s.Id == scoreId)?.TotalScore ?? 0;
            if (i == 2)
                score += round2ScoreData.Average(x => x.ItemScores.Sum());

            leaderBoardDetails.Add(new LeaderBoardDetails
            {
                Id = Guid.Parse($"61000000-0000-0000-0000-0000000001{i:X1}0"),
                LeaderBoardId = Event1LeaderBoardId,
                TeamId = teamId,
                Score = score,
                LevelAward = i == 2 ? 1 : i == 4 ? 2 : null,
                IsDisable = false,
                CreatedAt = Now,
                UpdatedAt = Now
            });
        }
        modelBuilder.Entity<LeaderBoardDetails>().HasData(leaderBoardDetails);

        // ──────────────────────────────────────────────────────────
        //  NOTIFICATIONS
        // ──────────────────────────────────────────────────────────
        var notifications = new List<Notifications>();
        // Notify all Event 1 team leaders about approval
        for (int i = 0; i < 10; i++)
        {
            var leaderUserId = Guid.Parse($"10000000-0000-0000-0000-00000000{200 + i * 3:X4}");
            var teamId = Guid.Parse($"30000000-0000-0000-0000-0000000001{i:X1}0");
            notifications.Add(new Notifications
            {
                Id = Guid.Parse($"71000000-0000-0000-0000-0000000001{i:X1}0"),
                UserId = leaderUserId,
                Title = "Registration Approved",
                Status = NotificationStatusEnum.Read,
                Description = $"Team {e1TeamNames[i]} has been approved to participate in SEAL Hackathon 2026 - Spring.",
                TargetType = NotificationTargetTypeEnum.Personal,
                IsDisable = false,
                CreatedAt = Now,
                UpdatedAt = Now
            });
        }
        // Notify Event 2 approved team leaders
        for (int i = 0; i < 5; i++)
        {
            var leaderUserId = Guid.Parse($"10000000-0000-0000-0000-00000000{240 + i * 3:X4}");
            var teamId = Guid.Parse($"30000000-0000-0000-0000-0000000002{i:X1}0");
            notifications.Add(new Notifications
            {
                Id = Guid.Parse($"71000000-0000-0000-0000-0000000002{i:X1}0"),
                UserId = leaderUserId,
                Title = "Registration Approved",
                Status = NotificationStatusEnum.Unread,
                Description = $"Team {e2TeamNames[i]} has been approved to participate in SEAL Hackathon 2026 - Summer.",
                TargetType = NotificationTargetTypeEnum.Personal,
                IsDisable = false,
                CreatedAt = Now,
                UpdatedAt = Now
            });
        }
        modelBuilder.Entity<Notifications>().HasData(notifications);
    }

    // ══════════════════════════════════════════════════════════════
    //  HELPERS
    // ══════════════════════════════════════════════════════════════

    private static Users CreateFptUser(Guid id, string email, string firstName, string lastName, string studentId, RoleEnum role = RoleEnum.Student)
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
            Bio = $"Student / Lecturer at FPT University",
            Address = "Saigon Hi-Tech Park, District 9, Ho Chi Minh City",
            DateOfBirth = role == RoleEnum.Student ? new DateTimeOffset(2002, 1, 1, 0, 0, 0, TimeSpan.Zero) : new DateTimeOffset(1990, 1, 1, 0, 0, 0, TimeSpan.Zero),
            StudentId = studentId,
            Role = role,
            College = "FPT University",
            ImgUrl = $"https://robohash.org/{email}",
            LinkUrl = $"https://fpt.edu.vn/users/{studentId}",
            VerifyEmailAt = Now,
            Status = UserStatusEnum.Active,
            IsVerified = true,
            IsDisable = false,
            CreatedAt = Now,
            UpdatedAt = Now
        };
    }

    private static string ToEmailName(string name)
    {
        // Remove Vietnamese accents using Unicode normalization
        var normalized = name.Normalize(NormalizationForm.FormD);
        var sb = new System.Text.StringBuilder();
        foreach (char c in normalized)
        {
            var uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
            if (uc != System.Globalization.UnicodeCategory.NonSpacingMark && c != ' ')
                sb.Append(c);
        }
        return sb.ToString().Normalize(NormalizationForm.FormC).ToLower();
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
            CreatedAt = Now,
            UpdatedAt = Now
        };
    }
}
