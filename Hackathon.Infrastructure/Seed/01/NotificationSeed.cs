using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Notification;
using Hackathon.Domain.Enums.Invitation;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Seed;

public static class NotificationSeed
{
    public static void SeedNotifications(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Invitations>().HasData(
            new Invitations
            {
                Id = Guid.Parse("70000000-0000-0000-0000-000000000001"),
                TeamId = SeedConstants.GreenCodersTeamId,
                UserId = SeedConstants.StudentMemberUserId,
                LimitTime = new DateTimeOffset(2027, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Status = InvitationStatusEnum.Pending,
                Description = "Seed invitation",
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },

            // 10 New Invitations for new Students
            CreateInvitation(Guid.Parse("70000000-0000-0000-0000-000000000010"), Guid.Parse("30000000-0000-0000-0000-000000000010"), Guid.Parse("10000000-0000-0000-0000-000000000010")),
            CreateInvitation(Guid.Parse("70000000-0000-0000-0000-000000000011"), Guid.Parse("30000000-0000-0000-0000-000000000011"), Guid.Parse("10000000-0000-0000-0000-000000000011")),
            CreateInvitation(Guid.Parse("70000000-0000-0000-0000-000000000012"), Guid.Parse("30000000-0000-0000-0000-000000000012"), Guid.Parse("10000000-0000-0000-0000-000000000012")),
            CreateInvitation(Guid.Parse("70000000-0000-0000-0000-000000000013"), Guid.Parse("30000000-0000-0000-0000-000000000013"), Guid.Parse("10000000-0000-0000-0000-000000000013")),
            CreateInvitation(Guid.Parse("70000000-0000-0000-0000-000000000014"), Guid.Parse("30000000-0000-0000-0000-000000000014"), Guid.Parse("10000000-0000-0000-0000-000000000014")),
            CreateInvitation(Guid.Parse("70000000-0000-0000-0000-000000000015"), Guid.Parse("30000000-0000-0000-0000-000000000015"), Guid.Parse("10000000-0000-0000-0000-000000000015")),
            CreateInvitation(Guid.Parse("70000000-0000-0000-0000-000000000016"), Guid.Parse("30000000-0000-0000-0000-000000000016"), Guid.Parse("10000000-0000-0000-0000-000000000016")),
            CreateInvitation(Guid.Parse("70000000-0000-0000-0000-000000000017"), Guid.Parse("30000000-0000-0000-0000-000000000017"), Guid.Parse("10000000-0000-0000-0000-000000000017")),
            CreateInvitation(Guid.Parse("70000000-0000-0000-0000-000000000018"), Guid.Parse("30000000-0000-0000-0000-000000000018"), Guid.Parse("10000000-0000-0000-0000-000000000018")),
            CreateInvitation(Guid.Parse("70000000-0000-0000-0000-000000000019"), Guid.Parse("30000000-0000-0000-0000-000000000019"), Guid.Parse("10000000-0000-0000-0000-000000000019"))
        );

        modelBuilder.Entity<Notifications>().HasData(
            new Notifications
            {
                Id = Guid.Parse("71000000-0000-0000-0000-000000000001"),
                UserId = SeedConstants.StudentLeaderUserId,
                Title = "Registration approved",
                Status = NotificationStatusEnum.Unread,
                Description = "Your team registration has been approved",
                TargetType = NotificationTargetTypeEnum.Personal,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },

            // 10 New Notifications
            CreateNotification(Guid.Parse("71000000-0000-0000-0000-000000000010"), Guid.Parse("10000000-0000-0000-0000-000000000010"), "New Team Created"),
            CreateNotification(Guid.Parse("71000000-0000-0000-0000-000000000011"), Guid.Parse("10000000-0000-0000-0000-000000000011"), "New Team Created"),
            CreateNotification(Guid.Parse("71000000-0000-0000-0000-000000000012"), Guid.Parse("10000000-0000-0000-0000-000000000012"), "New Team Created"),
            CreateNotification(Guid.Parse("71000000-0000-0000-0000-000000000013"), Guid.Parse("10000000-0000-0000-0000-000000000013"), "New Team Created"),
            CreateNotification(Guid.Parse("71000000-0000-0000-0000-000000000014"), Guid.Parse("10000000-0000-0000-0000-000000000014"), "New Team Created"),
            CreateNotification(Guid.Parse("71000000-0000-0000-0000-000000000015"), Guid.Parse("10000000-0000-0000-0000-000000000015"), "New Team Created"),
            CreateNotification(Guid.Parse("71000000-0000-0000-0000-000000000016"), Guid.Parse("10000000-0000-0000-0000-000000000016"), "New Team Created"),
            CreateNotification(Guid.Parse("71000000-0000-0000-0000-000000000017"), Guid.Parse("10000000-0000-0000-0000-000000000017"), "New Team Created"),
            CreateNotification(Guid.Parse("71000000-0000-0000-0000-000000000018"), Guid.Parse("10000000-0000-0000-0000-000000000018"), "New Team Created"),
            CreateNotification(Guid.Parse("71000000-0000-0000-0000-000000000019"), Guid.Parse("10000000-0000-0000-0000-000000000019"), "New Team Created")
        );

        modelBuilder.Entity<MentorNotifications>().HasData(
            new MentorNotifications
            {
                Id = Guid.Parse("72000000-0000-0000-0000-000000000001"),
                AssignTrackId = SeedConstants.MentorAiAssignTrackId,
                Title = "New team registered",
                Description = "A new team joined your track",
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },

            // 10 New MentorNotifications
            CreateMentorNotification(Guid.Parse("72000000-0000-0000-0000-000000000010"), Guid.Parse("41000000-0000-0000-0000-000000000010")),
            CreateMentorNotification(Guid.Parse("72000000-0000-0000-0000-000000000011"), Guid.Parse("41000000-0000-0000-0000-000000000011")),
            CreateMentorNotification(Guid.Parse("72000000-0000-0000-0000-000000000012"), Guid.Parse("41000000-0000-0000-0000-000000000012")),
            CreateMentorNotification(Guid.Parse("72000000-0000-0000-0000-000000000013"), Guid.Parse("41000000-0000-0000-0000-000000000013")),
            CreateMentorNotification(Guid.Parse("72000000-0000-0000-0000-000000000014"), Guid.Parse("41000000-0000-0000-0000-000000000014")),
            CreateMentorNotification(Guid.Parse("72000000-0000-0000-0000-000000000015"), Guid.Parse("41000000-0000-0000-0000-000000000015")),
            CreateMentorNotification(Guid.Parse("72000000-0000-0000-0000-000000000016"), Guid.Parse("41000000-0000-0000-0000-000000000016")),
            CreateMentorNotification(Guid.Parse("72000000-0000-0000-0000-000000000017"), Guid.Parse("41000000-0000-0000-0000-000000000017")),
            CreateMentorNotification(Guid.Parse("72000000-0000-0000-0000-000000000018"), Guid.Parse("41000000-0000-0000-0000-000000000018")),
            CreateMentorNotification(Guid.Parse("72000000-0000-0000-0000-000000000019"), Guid.Parse("41000000-0000-0000-0000-000000000019"))
        );
    }

    private static Invitations CreateInvitation(Guid id, Guid teamId, Guid userId)
    {
        return new Invitations
        {
            Id = id,
            TeamId = teamId,
            UserId = userId,
            LimitTime = new DateTimeOffset(2027, 1, 1, 0, 0, 0, TimeSpan.Zero),
            Status = InvitationStatusEnum.Pending,
            Description = "Seed invitation detail",
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }

    private static Notifications CreateNotification(Guid id, Guid userId, string title)
    {
        return new Notifications
        {
            Id = id,
            UserId = userId,
            Title = title,
            Status = NotificationStatusEnum.Unread,
            Description = $"Notification for team activity in {title}",
            TargetType = NotificationTargetTypeEnum.Personal,
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }

    private static MentorNotifications CreateMentorNotification(Guid id, Guid assignTrackId)
    {
        return new MentorNotifications
        {
            Id = id,
            AssignTrackId = assignTrackId,
            Title = "Event Update Notification",
            Description = "A new track status update has been broadcasted",
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }
}
