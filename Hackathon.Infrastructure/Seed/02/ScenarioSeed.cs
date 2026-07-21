using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.EmailVerification;
using Hackathon.Domain.Enums.Event;
using Hackathon.Domain.Enums.Invitation;
using Hackathon.Domain.Enums.Notification;
using Hackathon.Domain.Enums.RegisterTeam;
using Hackathon.Domain.Enums.Report;
using Hackathon.Domain.Enums.Submission;
using Hackathon.Domain.Enums.TeamDetail;
using Hackathon.Domain.Enums.User;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Seed;

public static class ScenarioSeed
{
    private static readonly DateTimeOffset Now = new(2026, 7, 20, 0, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset Future = new(2027, 1, 1, 0, 0, 0, TimeSpan.Zero);

    public static void SeedScenarioData(this ModelBuilder modelBuilder)
    {
        SeedUsers(modelBuilder);
        SeedEventAndCompetitionStates(modelBuilder);
        SeedCommunicationStates(modelBuilder);
        SeedAuthStates(modelBuilder);
        SeedRegradeRetakeAndMock(modelBuilder);
        SeedSoftDeletedGraph(modelBuilder);
    }

    private static void SeedUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Users>().HasData(
            CreateUser("50", "inactive.scenario@seed.local", "Inactive", UserStatusEnum.Inactive),
            CreateUser("51", "banned.scenario@seed.local", "Banned", UserStatusEnum.Banned, banReason: "Seed banned account", bannedAt: Now),
            CreateUser("52", "deleted.scenario@seed.local", "Deleted", UserStatusEnum.Active, isDisable: true),
            CreateUser("53", "pending.verify@seed.local", "Pending", UserStatusEnum.Active, isVerified: false),
            CreateUser("54", "expired.verify@seed.local", "Expired", UserStatusEnum.Active, isVerified: false),
            CreateUser("55", "accepted.invite@seed.local", "Accepted", UserStatusEnum.Active),
            CreateUser("56", "rejected.invite@seed.local", "Rejected", UserStatusEnum.Active),
            CreateUser("57", "expired.invite@seed.local", "ExpiredInvite", UserStatusEnum.Active)
        );
    }

    private static void SeedEventAndCompetitionStates(ModelBuilder modelBuilder)
    {
        var draftEventId = Id("20000000", "50");
        var stateTeamId = Id("30000000", "50");
        var bannedTeamId = Id("30000000", "51");

        modelBuilder.Entity<Events>().HasData(new Events
        {
            Id = draftEventId,
            Name = "Seed Draft Event",
            Description = "Incomplete hidden draft used for setup and publish validation",
            StartTime = new DateTimeOffset(2027, 2, 1, 8, 0, 0, TimeSpan.FromHours(7)),
            EndTime = new DateTimeOffset(2027, 2, 10, 17, 0, 0, TimeSpan.FromHours(7)),
            RegisterLimitTime = new DateTimeOffset(2027, 1, 25, 23, 59, 0, TimeSpan.FromHours(7)),
            LimitTeam = 10,
            MinMember = 2,
            MaxMember = 4,
            Status = EventStatusEnum.Draft,
            NumberRound = 0,
            Season = SeasonEnum.Spring,
            IsDisable = true,
            CreatedAt = Now,
            UpdatedAt = Now
        });

        modelBuilder.Entity<Teams>().HasData(
            new Teams { Id = stateTeamId, Name = "State Coverage Team", CanEdit = false, IsDisable = false, CreatedAt = Now, UpdatedAt = Now },
            new Teams { Id = bannedTeamId, Name = "Banned Registration Team", CanEdit = false, IsDisable = false, CreatedAt = Now, UpdatedAt = Now }
        );

        modelBuilder.Entity<TeamDetails>().HasData(
            new TeamDetails { Id = Id("30100000", "50"), TeamId = stateTeamId, UserId = Id("10000000", "55"), IsLeader = true, Status = TeamDetailStatusEnum.Active, IsDisable = false, CreatedAt = Now, UpdatedAt = Now },
            new TeamDetails { Id = Id("30100000", "51"), TeamId = stateTeamId, UserId = Id("10000000", "50"), IsLeader = false, Status = TeamDetailStatusEnum.Inactive, IsDisable = false, CreatedAt = Now, UpdatedAt = Now },
            new TeamDetails { Id = Id("30100000", "52"), TeamId = bannedTeamId, UserId = Id("10000000", "56"), IsLeader = true, Status = TeamDetailStatusEnum.Active, IsDisable = false, CreatedAt = Now, UpdatedAt = Now }
        );

        modelBuilder.Entity<RegisterTeams>().HasData(
            new RegisterTeams
            {
                Id = Id("31000000", "50"),
                TeamId = stateTeamId,
                EventId = Guid.Parse("20000000-0000-0000-0000-000000000014"),
                TrackId = Guid.Parse("24000000-0000-0000-0000-000000000014"),
                TopicId = Guid.Parse("25000000-0000-0000-0000-000000000014"),
                Description = "Approved isolated score lifecycle scenario",
                Status = RegisterTeamStatusEnum.Approved,
                IsBanned = false,
                IsDisable = false,
                CreatedAt = new DateTimeOffset(2025, 6, 10, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = new DateTimeOffset(2025, 6, 10, 0, 0, 0, TimeSpan.Zero)
            },
            new RegisterTeams
            {
                Id = Id("31000000", "51"),
                TeamId = bannedTeamId,
                EventId = Guid.Parse("20000000-0000-0000-0000-000000000018"),
                TrackId = Guid.Parse("24000000-0000-0000-0000-000000000018"),
                TopicId = Guid.Parse("25000000-0000-0000-0000-000000000018"),
                Description = "Banned registration scenario",
                Status = RegisterTeamStatusEnum.Banned,
                IsBanned = true,
                RejectionReason = "Seed policy violation",
                IsDisable = false,
                CreatedAt = Now,
                UpdatedAt = Now
            });
    }

    private static void SeedCommunicationStates(ModelBuilder modelBuilder)
    {
        var teamId = Id("30000000", "50");

        modelBuilder.Entity<Invitations>().HasData(
            CreateInvitation("50", teamId, Id("10000000", "55"), InvitationStatusEnum.Accepted, Future),
            CreateInvitation("51", teamId, Id("10000000", "56"), InvitationStatusEnum.Rejected, Future),
            CreateInvitation("52", teamId, Id("10000000", "57"), InvitationStatusEnum.Expired, Now.AddDays(-1))
        );

        modelBuilder.Entity<Notifications>().HasData(
            CreateNotification("50", "Personal scenario", NotificationTargetTypeEnum.Personal, NotificationStatusEnum.Unread, Id("10000000", "55"), null, false),
            CreateNotification("51", "Team scenario", NotificationTargetTypeEnum.Team, NotificationStatusEnum.Unread, null, teamId, false),
            CreateNotification("52", "System scenario", NotificationTargetTypeEnum.System, NotificationStatusEnum.Read, null, null, false),
            CreateNotification("53", "Deleted notification", NotificationTargetTypeEnum.Personal, NotificationStatusEnum.Unread, Id("10000000", "56"), null, true),
            CreateNotification("54", "Pending notification", NotificationTargetTypeEnum.System, NotificationStatusEnum.Pending, null, null, false)
        );

        modelBuilder.Entity<Reports>().HasData(
            CreateReport("50", ReportStatusEnum.Reject, "Report rejected after review", false),
            CreateReport("51", ReportStatusEnum.Resolved, "Issue resolved by administrator", false),
            CreateReport("52", ReportStatusEnum.Canceled, "Reporter canceled the request", false),
            CreateReport("53", ReportStatusEnum.Pending, "Soft-deleted report", true)
        );
    }

    private static void SeedAuthStates(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmailVerifications>().HasData(
            new EmailVerifications { Id = Id("14000000", "50"), UserId = Id("10000000", "53"), TokenHash = "pending-verification-scenario", ExpiredAt = Future, Status = EmailVerificationStatusEnum.Pending, IsDisable = false, CreatedAt = Now, UpdatedAt = Now },
            new EmailVerifications { Id = Id("14000000", "51"), UserId = Id("10000000", "54"), TokenHash = "expired-verification-scenario", ExpiredAt = Now.AddDays(-1), Status = EmailVerificationStatusEnum.Expired, IsDisable = false, CreatedAt = Now.AddDays(-2), UpdatedAt = Now }
        );

        modelBuilder.Entity<ResetPasswords>().HasData(
            new ResetPasswords { Id = Id("13000000", "50"), UserId = Id("10000000", "53"), TokenHash = "active-reset-scenario", IsUsed = false, ExpiresAt = Future, IsDisable = false, CreatedAt = Now, UpdatedAt = Now },
            new ResetPasswords { Id = Id("13000000", "51"), UserId = Id("10000000", "55"), TokenHash = "used-reset-scenario", IsUsed = true, ExpiresAt = Future, IsDisable = false, CreatedAt = Now, UpdatedAt = Now },
            new ResetPasswords { Id = Id("13000000", "52"), UserId = Id("10000000", "56"), TokenHash = "expired-reset-scenario", IsUsed = false, ExpiresAt = Now.AddDays(-1), IsDisable = false, CreatedAt = Now.AddDays(-2), UpdatedAt = Now }
        );

        modelBuilder.Entity<RefreshTokens>().HasData(
            CreateRefreshToken("50", Id("10000000", "55"), Future, null, Now),
            CreateRefreshToken("51", Id("10000000", "56"), Future, Now, Now.AddDays(-1)),
            CreateRefreshToken("52", Id("10000000", "57"), Now.AddDays(-1), null, Now.AddDays(-2))
        );
    }

    private static void SeedRegradeRetakeAndMock(ModelBuilder modelBuilder)
    {
        var registerTeamId = Id("31000000", "50");
        var roundDetailId = Id("32000000", "50");
        var originalSubmissionId = Id("33000000", "50");
        var regradeSubmissionId = Id("33000000", "51");
        var mockSubmissionId = Id("33000000", "52");
        var assignTrackId = Guid.Parse("41000000-0000-0000-0000-000000000044");
        var originalScoreId = Id("50000000", "50");
        var retakeScoreId = Id("50000000", "51");
        var mockScoreId = Id("50000000", "52");
        var criteriaItemId = Guid.Parse("23000000-0000-0000-0000-000000000014");
        var lifecycleStart = new DateTimeOffset(2025, 6, 16, 9, 0, 0, TimeSpan.Zero);

        modelBuilder.Entity<RoundDetails>().HasData(new RoundDetails { Id = roundDetailId, RoundId = Guid.Parse("21000000-0000-0000-0000-000000000014"), RegisterTeamId = registerTeamId, IsDisable = false, CreatedAt = lifecycleStart, UpdatedAt = lifecycleStart });
        modelBuilder.Entity<Submissions>().HasData(
            new Submissions { Id = originalSubmissionId, RoundDetailId = roundDetailId, Url = "https://seed.local/submissions/original-scenario", Description = "Original submission before regrade", Status = SubmissionStatusEnum.Submitted, SubmittedAt = lifecycleStart.AddHours(1), IsRegrade = false, IsDisable = false, CreatedAt = lifecycleStart.AddHours(1), UpdatedAt = lifecycleStart.AddHours(2) },
            new Submissions { Id = regradeSubmissionId, RoundDetailId = roundDetailId, Url = "https://seed.local/submissions/regrade-scenario", Description = "Regrade and retake scenario", Status = SubmissionStatusEnum.Graded, SubmittedAt = lifecycleStart.AddHours(3), IsRegrade = true, IsDisable = false, CreatedAt = lifecycleStart.AddHours(3), UpdatedAt = lifecycleStart.AddHours(5) },
            new Submissions { Id = mockSubmissionId, RoundDetailId = roundDetailId, Url = "https://seed.local/submissions/mock-scenario", Description = "Mock score scenario", Status = SubmissionStatusEnum.Submitted, SubmittedAt = lifecycleStart.AddHours(6), IsRegrade = false, IsDisable = false, CreatedAt = lifecycleStart.AddHours(6), UpdatedAt = lifecycleStart.AddHours(7) }
        );
        modelBuilder.Entity<Scores>().HasData(
            CreateScenarioScore(originalScoreId, originalSubmissionId, assignTrackId, null, false, null, false, false, lifecycleStart.AddHours(2)),
            CreateScenarioScore(retakeScoreId, regradeSubmissionId, assignTrackId, 90m, true, originalScoreId, false, false, lifecycleStart.AddHours(5)),
            CreateScenarioScore(mockScoreId, mockSubmissionId, assignTrackId, null, false, null, true, false, lifecycleStart.AddHours(7))
        );
        modelBuilder.Entity<ScoreItems>().HasData(
            CreateScenarioScoreItem("51", retakeScoreId, criteriaItemId, assignTrackId, 90m, false, lifecycleStart.AddHours(5))
        );
    }

    private static void SeedSoftDeletedGraph(ModelBuilder modelBuilder)
    {
        var eventId = Id("20000000", "60");
        var roundId = Id("21000000", "60");
        var templateId = Id("22000000", "60");
        var criteriaItemId = Id("23000000", "60");
        var trackId = Id("24000000", "60");
        var topicId = Id("25000000", "60");
        var teamId = Id("30000000", "60");
        var registerTeamId = Id("31000000", "60");
        var roundDetailId = Id("32000000", "60");
        var submissionId = Id("33000000", "60");
        var assignEventId = Id("40000000", "60");
        var assignTrackId = Id("41000000", "60");
        var scoreId = Id("50000000", "60");
        var deletedStart = new DateTimeOffset(2026, 1, 10, 8, 0, 0, TimeSpan.FromHours(7));

        modelBuilder.Entity<Events>().HasData(new Events { Id = eventId, Name = "Soft Deleted Event", Description = "Soft-delete coverage graph", StartTime = deletedStart, EndTime = deletedStart.AddDays(5), RegisterLimitTime = deletedStart.AddDays(-5), LimitTeam = 5, MinMember = 1, MaxMember = 4, Status = EventStatusEnum.Closed, NumberRound = 1, Season = SeasonEnum.Winter, IsDisable = true, CreatedAt = deletedStart.AddDays(-10), UpdatedAt = deletedStart.AddDays(5) });
        modelBuilder.Entity<Rounds>().HasData(new Rounds { Id = roundId, EventId = eventId, Name = "Deleted Round", Description = "Soft-deleted round", RoundNo = 1, StartTime = deletedStart, EndTime = deletedStart.AddDays(1), StartSubmission = deletedStart, EndSubmission = deletedStart.AddHours(12), LimitTeam = 5, IsDisable = true, CreatedAt = deletedStart.AddDays(-1), UpdatedAt = deletedStart.AddDays(1) });
        modelBuilder.Entity<CriteriaTemplates>().HasData(new CriteriaTemplates { Id = templateId, RoundId = roundId, Title = "Deleted Template", Description = "Soft-deleted criteria template", IsActive = false, IsDisable = true, CreatedAt = Now, UpdatedAt = Now });
        modelBuilder.Entity<CriteriaItems>().HasData(new CriteriaItems { Id = criteriaItemId, CriteriaTemplateId = templateId, Name = "Deleted Criterion", Description = "Soft-deleted criterion", Score = 100m, IsDisable = true, CreatedAt = Now, UpdatedAt = Now });
        modelBuilder.Entity<Tracks>().HasData(new Tracks { Id = trackId, EventId = eventId, Title = "Deleted Track", Description = "Soft-deleted track", MaxTeam = 5, IsDisable = true, CreatedAt = Now, UpdatedAt = Now });
        modelBuilder.Entity<Topics>().HasData(new Topics { Id = topicId, TrackId = trackId, Title = "Deleted Topic", Description = "Soft-deleted topic", IsDisable = true, CreatedAt = Now, UpdatedAt = Now });
        modelBuilder.Entity<Awards>().HasData(new Awards { Id = Id("26000000", "60"), EventId = eventId, Name = "Deleted Award", Description = "Soft-deleted award", LevelAward = 1, NumberOfAward = 1, Prize = 1000m, IsDisable = true, CreatedAt = Now, UpdatedAt = Now });
        modelBuilder.Entity<LeaderBoards>().HasData(new LeaderBoards { Id = Id("60000000", "60"), EventId = eventId, Year = 2026, IsPublished = false, IsDisable = true, CreatedAt = Now, UpdatedAt = Now });
        modelBuilder.Entity<Teams>().HasData(new Teams { Id = teamId, Name = "Soft Deleted Team", CanEdit = true, IsDisable = true, CreatedAt = Now, UpdatedAt = Now });
        modelBuilder.Entity<TeamDetails>().HasData(new TeamDetails { Id = Id("30100000", "60"), TeamId = teamId, UserId = Id("10000000", "52"), IsLeader = true, Status = TeamDetailStatusEnum.Inactive, IsDisable = true, CreatedAt = Now, UpdatedAt = Now });
        modelBuilder.Entity<RegisterTeams>().HasData(new RegisterTeams { Id = registerTeamId, TeamId = teamId, EventId = eventId, TrackId = trackId, TopicId = topicId, Description = "Soft-deleted registration", Status = RegisterTeamStatusEnum.Approved, IsBanned = false, IsDisable = true, CreatedAt = Now, UpdatedAt = Now });
        modelBuilder.Entity<RoundDetails>().HasData(new RoundDetails { Id = roundDetailId, RoundId = roundId, RegisterTeamId = registerTeamId, IsDisable = true, CreatedAt = Now, UpdatedAt = Now });
        modelBuilder.Entity<Submissions>().HasData(new Submissions { Id = submissionId, RoundDetailId = roundDetailId, Url = "https://seed.local/submissions/deleted", Description = "Soft-deleted submission", Status = SubmissionStatusEnum.Graded, SubmittedAt = deletedStart.AddHours(1), IsDisable = true, CreatedAt = deletedStart.AddHours(1), UpdatedAt = deletedStart.AddHours(2) });
        modelBuilder.Entity<AssignEvents>().HasData(new AssignEvents { Id = assignEventId, UserId = SeedConstants.JudgeUserId, EventRoleId = SeedConstants.JudgeEventRoleId, EventId = eventId, IsDisable = true, CreatedAt = Now, UpdatedAt = Now });
        modelBuilder.Entity<AssignTracks>().HasData(new AssignTracks { Id = assignTrackId, AssignEventId = assignEventId, TrackId = trackId, IsDisable = true, CreatedAt = Now, UpdatedAt = Now });
        modelBuilder.Entity<Scores>().HasData(CreateScenarioScore(scoreId, submissionId, assignTrackId, 60m, false, null, false, true, deletedStart.AddHours(2)));
        modelBuilder.Entity<ScoreItems>().HasData(CreateScenarioScoreItem("60", scoreId, criteriaItemId, assignTrackId, 60m, true, deletedStart.AddHours(2)));
        modelBuilder.Entity<MentorNotifications>().HasData(new MentorNotifications { Id = Id("72000000", "60"), AssignTrackId = assignTrackId, Title = "Deleted mentor notification", Description = "Soft-deleted mentor notification", IsDisable = true, CreatedAt = Now, UpdatedAt = Now });
    }

    private static Users CreateUser(string suffix, string email, string firstName, UserStatusEnum status, bool isVerified = true, bool isDisable = false, string? banReason = null, DateTimeOffset? bannedAt = null)
    {
        return new Users
        {
            Id = Id("10000000", suffix), Email = email, HashPassword = SeedHelper.HashDefaultPassword(), FirstName = firstName, LastName = "Scenario", PhoneNumber = "0900000000", AvatarUrl = $"https://robohash.org/{email}", Bio = "Seed state coverage user", Address = "Seed address", DateOfBirth = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero), StudentId = $"SCN{suffix}", Role = RoleEnum.Student, College = "FPT University", ImgUrl = $"https://robohash.org/{email}", LinkUrl = string.Empty, VerifyEmailAt = isVerified ? Now : null, Status = status, BanReason = banReason, BannedAt = bannedAt, IsVerified = isVerified, IsDisable = isDisable, CreatedAt = Now, UpdatedAt = Now
        };
    }

    private static Invitations CreateInvitation(string suffix, Guid teamId, Guid userId, InvitationStatusEnum status, DateTimeOffset limitTime)
    {
        var createdAt = limitTime <= Now ? limitTime.AddDays(-1) : Now;
        return new Invitations { Id = Id("70000000", suffix), TeamId = teamId, UserId = userId, LimitTime = limitTime, Status = status, Description = $"{status} invitation scenario", IsDisable = false, CreatedAt = createdAt, UpdatedAt = Now };
    }

    private static Notifications CreateNotification(string suffix, string title, NotificationTargetTypeEnum targetType, NotificationStatusEnum status, Guid? userId, Guid? teamId, bool isDisable)
        => new() { Id = Id("71000000", suffix), UserId = userId, TeamId = teamId, Title = title, Description = $"{targetType} notification scenario", TargetType = targetType, Status = status, IsDisable = isDisable, CreatedAt = Now, UpdatedAt = Now };

    private static Reports CreateReport(string suffix, ReportStatusEnum status, string reason, bool isDisable)
        => new() { Id = Id("73000000", suffix), UserId = SeedConstants.JudgeUserId, Title = $"{status} report scenario", Description = "Report state coverage", Status = status, Reason = reason, TypeReport = "Submission", IsDisable = isDisable, CreatedAt = Now, UpdatedAt = Now };

    private static RefreshTokens CreateRefreshToken(string suffix, Guid userId, DateTimeOffset expiredAt, DateTimeOffset? revokedAt, DateTimeOffset createdAt)
        => new() { Id = Id("12000000", suffix), UserId = userId, RefreshTokenHash = $"refresh-{suffix}-scenario", IpAddress = "127.0.0.1", UserAgent = "Seed Agent", DeviceLabel = "Scenario Device", ExpiredAt = expiredAt, RevokedAt = revokedAt, IsDisable = false, CreatedAt = createdAt, UpdatedAt = revokedAt ?? createdAt };

    private static Scores CreateScenarioScore(Guid id, Guid submissionId, Guid assignTrackId, decimal? totalScore, bool isRetake, Guid? retakeFromScoreId, bool isMock, bool isDisable, DateTimeOffset timestamp)
        => new() { Id = id, SubmissionId = submissionId, AssignTrackId = assignTrackId, IsRetake = isRetake, RetakeFromScoreId = retakeFromScoreId, TotalScore = totalScore, IsMock = isMock, IsDisable = isDisable, CreatedAt = timestamp, UpdatedAt = timestamp };

    private static ScoreItems CreateScenarioScoreItem(string suffix, Guid scoreId, Guid criteriaItemId, Guid assignTrackId, decimal score, bool isDisable, DateTimeOffset timestamp)
        => new() { Id = Id("51000000", suffix), ScoreId = scoreId, CriteriaItemId = criteriaItemId, AssignTrackId = assignTrackId, Score = score, Comment = "Scenario score item", IsDisable = isDisable, CreatedAt = timestamp, UpdatedAt = timestamp };

    private static Guid Id(string prefix, string suffix)
        => Guid.Parse($"{prefix}-0000-0000-0000-0000000000{suffix}");
}
