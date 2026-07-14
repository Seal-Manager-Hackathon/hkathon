using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.RegisterTeam;
using Hackathon.Domain.Enums.TeamDetail;
using Hackathon.Domain.Enums.User;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Seed;

public static class DemoSeed
{
    private static readonly Guid DemoTeam1Id = Guid.Parse("30000000-0000-0000-0000-000000000020");
    private static readonly Guid DemoTeam2Id = Guid.Parse("30000000-0000-0000-0000-000000000021");
    private static readonly Guid DemoTeam3Id = Guid.Parse("30000000-0000-0000-0000-000000000022");
    private static readonly Guid DemoTeam4Id = Guid.Parse("30000000-0000-0000-0000-000000000023");
    private static readonly Guid DemoTeam5Id = Guid.Parse("30000000-0000-0000-0000-000000000024");

    // Users: 10000000-...-000000000030 through ...000000000041
    private static readonly Guid User1Id = Guid.Parse("10000000-0000-0000-0000-000000000030");
    private static readonly Guid User2Id = Guid.Parse("10000000-0000-0000-0000-000000000031");
    private static readonly Guid User3Id = Guid.Parse("10000000-0000-0000-0000-000000000032");
    private static readonly Guid User4Id = Guid.Parse("10000000-0000-0000-0000-000000000033");
    private static readonly Guid User5Id = Guid.Parse("10000000-0000-0000-0000-000000000034");
    private static readonly Guid User6Id = Guid.Parse("10000000-0000-0000-0000-000000000035");
    private static readonly Guid User7Id = Guid.Parse("10000000-0000-0000-0000-000000000036");
    private static readonly Guid User8Id = Guid.Parse("10000000-0000-0000-0000-000000000037");
    private static readonly Guid User9Id = Guid.Parse("10000000-0000-0000-0000-000000000038");
    private static readonly Guid User10Id = Guid.Parse("10000000-0000-0000-0000-000000000039");
    private static readonly Guid User11Id = Guid.Parse("10000000-0000-0000-0000-000000000040");
    private static readonly Guid User12Id = Guid.Parse("10000000-0000-0000-0000-000000000041");

    // BCrypt EnhancedHash (SHA256) of password "string" + Pepper from SecurityOptions
    private static string DemoPasswordHash => SeedHelper.HashDefaultPassword();

    public static void SeedDemoData(this ModelBuilder modelBuilder)
    {
        // ── Users ──────────────────────────────────────────────
        modelBuilder.Entity<Users>().HasData(
            CreateDemoUser(User1Id, "thanh.nguyen@demo.local", "Thanh", "Nguyen", "SEAL030"),
            CreateDemoUser(User2Id, "anh.pham@demo.local", "Anh", "Pham", "SEAL031"),
            CreateDemoUser(User3Id, "minh.tran@demo.local", "Minh", "Tran", "SEAL032"),
            CreateDemoUser(User4Id, "hoa.le@demo.local", "Hoa", "Le", "SEAL033"),
            CreateDemoUser(User5Id, "binh.hoang@demo.local", "Binh", "Hoang", "SEAL034"),
            CreateDemoUser(User6Id, "lan.vu@demo.local", "Lan", "Vu", "SEAL035"),
            CreateDemoUser(User7Id, "tuan.do@demo.local", "Tuan", "Do", "SEAL036"),
            CreateDemoUser(User8Id, "hieu.nguyen@demo.local", "Hieu", "Nguyen", "SEAL037"),
            CreateDemoUser(User9Id, "quynh.pham@demo.local", "Quynh", "Pham", "SEAL038"),
            CreateDemoUser(User10Id, "nam.hoang@demo.local", "Nam", "Hoang", "SEAL039"),
            CreateDemoUser(User11Id, "khoa.le@demo.local", "Khoa", "Le", "SEAL040"),
            CreateDemoUser(User12Id, "thu.tran@demo.local", "Thu", "Tran", "SEAL041")
        );

        // ── Teams ──────────────────────────────────────────────
        modelBuilder.Entity<Teams>().HasData(
            new Teams { Id = DemoTeam1Id, Name = "AI Mavericks", CanEdit = true, IsDisable = false, CreatedAt = SeedConstants.CreatedAt, UpdatedAt = SeedConstants.CreatedAt },
            new Teams { Id = DemoTeam2Id, Name = "Eco Guardians", CanEdit = true, IsDisable = false, CreatedAt = SeedConstants.CreatedAt, UpdatedAt = SeedConstants.CreatedAt },
            new Teams { Id = DemoTeam3Id, Name = "Code Visionaries", CanEdit = true, IsDisable = false, CreatedAt = SeedConstants.CreatedAt, UpdatedAt = SeedConstants.CreatedAt },
            new Teams { Id = DemoTeam4Id, Name = "GreenTech Solutions", CanEdit = true, IsDisable = false, CreatedAt = SeedConstants.CreatedAt, UpdatedAt = SeedConstants.CreatedAt },
            new Teams { Id = DemoTeam5Id, Name = "AI Builders", CanEdit = true, IsDisable = false, CreatedAt = SeedConstants.CreatedAt, UpdatedAt = SeedConstants.CreatedAt }
        );

        // ── TeamDetails ────────────────────────────────────────
        modelBuilder.Entity<TeamDetails>().HasData(
            // Team 1: AI Mavericks (leader: Thanh, member: Anh)
            CreateDemoTeamDetail(Guid.Parse("30100000-0000-0000-0000-000000000020"), DemoTeam1Id, User1Id, isLeader: true),
            CreateDemoTeamDetail(Guid.Parse("30100000-0000-0000-0000-000000000021"), DemoTeam1Id, User2Id, isLeader: false),
            // Team 2: Eco Guardians (leader: Minh, members: Hoa, Binh)
            CreateDemoTeamDetail(Guid.Parse("30100000-0000-0000-0000-000000000022"), DemoTeam2Id, User3Id, isLeader: true),
            CreateDemoTeamDetail(Guid.Parse("30100000-0000-0000-0000-000000000023"), DemoTeam2Id, User4Id, isLeader: false),
            CreateDemoTeamDetail(Guid.Parse("30100000-0000-0000-0000-000000000024"), DemoTeam2Id, User5Id, isLeader: false),
            // Team 3: Code Visionaries (leader: Lan, member: Tuan)
            CreateDemoTeamDetail(Guid.Parse("30100000-0000-0000-0000-000000000025"), DemoTeam3Id, User6Id, isLeader: true),
            CreateDemoTeamDetail(Guid.Parse("30100000-0000-0000-0000-000000000026"), DemoTeam3Id, User7Id, isLeader: false),
            // Team 4: GreenTech Solutions (leader: Hieu, members: Quynh, Nam)
            CreateDemoTeamDetail(Guid.Parse("30100000-0000-0000-0000-000000000027"), DemoTeam4Id, User8Id, isLeader: true),
            CreateDemoTeamDetail(Guid.Parse("30100000-0000-0000-0000-000000000028"), DemoTeam4Id, User9Id, isLeader: false),
            CreateDemoTeamDetail(Guid.Parse("30100000-0000-0000-0000-000000000029"), DemoTeam4Id, User10Id, isLeader: false),
            // Team 5: AI Builders (leader: Khoa, member: Thu)
            CreateDemoTeamDetail(Guid.Parse("30100000-0000-0000-0000-000000000030"), DemoTeam5Id, User11Id, isLeader: true),
            CreateDemoTeamDetail(Guid.Parse("30100000-0000-0000-0000-000000000031"), DemoTeam5Id, User12Id, isLeader: false)
        );

        // ── RegisterTeams (all approved, assigned track + topic) ─
        modelBuilder.Entity<RegisterTeams>().HasData(
            CreateDemoRegisterTeam(Guid.Parse("31000000-0000-0000-0000-000000000020"), DemoTeam1Id, SeedConstants.SealHackathonEventId, SeedConstants.AiTrackId, SeedConstants.AiTopicId, "AI Mavericks registration for SEAL Hackathon 2026"),
            CreateDemoRegisterTeam(Guid.Parse("31000000-0000-0000-0000-000000000021"), DemoTeam2Id, SeedConstants.SealHackathonEventId, SeedConstants.GreenTrackId, SeedConstants.GreenTopicId, "Eco Guardians registration for SEAL Hackathon 2026"),
            CreateDemoRegisterTeam(Guid.Parse("31000000-0000-0000-0000-000000000022"), DemoTeam3Id, SeedConstants.SealHackathonEventId, SeedConstants.AiTrackId, SeedConstants.AiTopicId, "Code Visionaries registration for SEAL Hackathon 2026"),
            CreateDemoRegisterTeam(Guid.Parse("31000000-0000-0000-0000-000000000023"), DemoTeam4Id, SeedConstants.SealHackathonEventId, SeedConstants.GreenTrackId, SeedConstants.GreenTopicId, "GreenTech Solutions registration for SEAL Hackathon 2026"),
            CreateDemoRegisterTeam(Guid.Parse("31000000-0000-0000-0000-000000000024"), DemoTeam5Id, SeedConstants.SealHackathonEventId, SeedConstants.AiTrackId, SeedConstants.AiTopicId, "AI Builders registration for SEAL Hackathon 2026")
        );

        // ── RoundDetails (Idea Round only, for submission demo) ──
        modelBuilder.Entity<RoundDetails>().HasData(
            CreateDemoRoundDetail(Guid.Parse("32000000-0000-0000-0000-000000000020"), SeedConstants.IdeaRoundId, Guid.Parse("31000000-0000-0000-0000-000000000020")),
            CreateDemoRoundDetail(Guid.Parse("32000000-0000-0000-0000-000000000021"), SeedConstants.IdeaRoundId, Guid.Parse("31000000-0000-0000-0000-000000000021")),
            CreateDemoRoundDetail(Guid.Parse("32000000-0000-0000-0000-000000000022"), SeedConstants.IdeaRoundId, Guid.Parse("31000000-0000-0000-0000-000000000022")),
            CreateDemoRoundDetail(Guid.Parse("32000000-0000-0000-0000-000000000023"), SeedConstants.IdeaRoundId, Guid.Parse("31000000-0000-0000-0000-000000000023")),
            CreateDemoRoundDetail(Guid.Parse("32000000-0000-0000-0000-000000000024"), SeedConstants.IdeaRoundId, Guid.Parse("31000000-0000-0000-0000-000000000024"))
        );
    }

    private static Users CreateDemoUser(Guid id, string email, string firstName, string lastName, string studentId)
    {
        return new Users
        {
            Id = id,
            Email = email,
            HashPassword = DemoPasswordHash,
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = "0900000000",
            AvatarUrl = "https://robohash.org/" + email,
            Bio = "Demo user",
            Address = "Demo address",
            DateOfBirth = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero),
            StudentId = studentId,
            Role = RoleEnum.Student,
            College = "FPT University",
            ImgUrl = "https://robohash.org/" + email,
            LinkUrl = "",
            VerifyEmailAt = SeedConstants.CreatedAt,
            Status = UserStatusEnum.Active,
            IsVerified = true,
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }

    private static TeamDetails CreateDemoTeamDetail(Guid id, Guid teamId, Guid userId, bool isLeader)
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

    private static RegisterTeams CreateDemoRegisterTeam(Guid id, Guid teamId, Guid eventId, Guid trackId, Guid topicId, string description)
    {
        return new RegisterTeams
        {
            Id = id,
            TeamId = teamId,
            EventId = eventId,
            TrackId = trackId,
            TopicId = topicId,
            Description = description,
            Status = RegisterTeamStatusEnum.Approved,
            IsBanned = false,
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }

    private static RoundDetails CreateDemoRoundDetail(Guid id, Guid roundId, Guid registerTeamId)
    {
        return new RoundDetails
        {
            Id = id,
            RoundId = roundId,
            RegisterTeamId = registerTeamId,
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }
}
