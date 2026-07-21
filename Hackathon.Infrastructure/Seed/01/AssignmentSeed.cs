using Hackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Seed;

public static class AssignmentSeed
{
    public static void SeedAssignments(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AssignEvents>().HasData(
            new AssignEvents
            {
                Id = SeedConstants.MentorAssignEventId,
                UserId = SeedConstants.MentorUserId,
                EventRoleId = SeedConstants.MentorEventRoleId,
                EventId = SeedConstants.SealHackathonEventId,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },
            new AssignEvents
            {
                Id = SeedConstants.JudgeAssignEventId,
                UserId = SeedConstants.JudgeUserId,
                EventRoleId = SeedConstants.JudgeEventRoleId,
                EventId = SeedConstants.SealHackathonEventId,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },

            // 10 New AssignEvents for new Mentors
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000010"), Guid.Parse("10000000-0000-0000-0000-000000000020"), SeedConstants.MentorEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000010")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000011"), Guid.Parse("10000000-0000-0000-0000-000000000021"), SeedConstants.MentorEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000011")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000012"), Guid.Parse("10000000-0000-0000-0000-000000000022"), SeedConstants.MentorEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000012")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000013"), Guid.Parse("10000000-0000-0000-0000-000000000023"), SeedConstants.MentorEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000013")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000014"), Guid.Parse("10000000-0000-0000-0000-000000000024"), SeedConstants.MentorEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000014")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000015"), Guid.Parse("10000000-0000-0000-0000-000000000020"), SeedConstants.MentorEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000015")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000016"), Guid.Parse("10000000-0000-0000-0000-000000000021"), SeedConstants.MentorEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000016")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000017"), Guid.Parse("10000000-0000-0000-0000-000000000022"), SeedConstants.MentorEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000017")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000018"), Guid.Parse("10000000-0000-0000-0000-000000000023"), SeedConstants.MentorEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000018")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000019"), Guid.Parse("10000000-0000-0000-0000-000000000024"), SeedConstants.MentorEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000019")),

            // Staff Assignments
            CreateAssignEvent(SeedConstants.StaffAssignEventId, SeedConstants.StaffUserId, SeedConstants.StaffEventRoleId, SeedConstants.SealHackathonEventId),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000021"), Guid.Parse("10000000-0000-0000-0000-000000000025"), SeedConstants.StaffEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000010")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000022"), Guid.Parse("10000000-0000-0000-0000-000000000026"), SeedConstants.StaffEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000011")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000023"), Guid.Parse("10000000-0000-0000-0000-000000000027"), SeedConstants.StaffEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000012")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000024"), Guid.Parse("10000000-0000-0000-0000-000000000028"), SeedConstants.StaffEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000013")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000025"), Guid.Parse("10000000-0000-0000-0000-000000000029"), SeedConstants.StaffEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000014")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000026"), Guid.Parse("10000000-0000-0000-0000-000000000025"), SeedConstants.StaffEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000015")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000027"), Guid.Parse("10000000-0000-0000-0000-000000000026"), SeedConstants.StaffEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000016")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000028"), Guid.Parse("10000000-0000-0000-0000-000000000027"), SeedConstants.StaffEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000017")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000029"), Guid.Parse("10000000-0000-0000-0000-000000000028"), SeedConstants.StaffEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000018")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000030"), Guid.Parse("10000000-0000-0000-0000-000000000029"), SeedConstants.StaffEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000019")),

            // Judge assignments for paging events
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000040"), SeedConstants.JudgeUserId, SeedConstants.JudgeEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000010")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000041"), SeedConstants.JudgeUserId, SeedConstants.JudgeEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000011")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000042"), SeedConstants.JudgeUserId, SeedConstants.JudgeEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000012")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000043"), SeedConstants.JudgeUserId, SeedConstants.JudgeEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000013")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000044"), SeedConstants.JudgeUserId, SeedConstants.JudgeEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000014")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000045"), SeedConstants.JudgeUserId, SeedConstants.JudgeEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000015")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000046"), SeedConstants.JudgeUserId, SeedConstants.JudgeEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000016")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000047"), SeedConstants.JudgeUserId, SeedConstants.JudgeEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000017")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000048"), SeedConstants.JudgeUserId, SeedConstants.JudgeEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000018")),
            CreateAssignEvent(Guid.Parse("40000000-0000-0000-0000-000000000049"), SeedConstants.JudgeUserId, SeedConstants.JudgeEventRoleId, Guid.Parse("20000000-0000-0000-0000-000000000019"))
        );

        modelBuilder.Entity<AssignTracks>().HasData(
            CreateAssignTrack(SeedConstants.MentorAiAssignTrackId, SeedConstants.MentorAssignEventId, SeedConstants.AiTrackId),
            CreateAssignTrack(SeedConstants.JudgeAiAssignTrackId, SeedConstants.JudgeAssignEventId, SeedConstants.AiTrackId),
            CreateAssignTrack(SeedConstants.JudgeGreenAssignTrackId, SeedConstants.JudgeAssignEventId, SeedConstants.GreenTrackId),

            // 10 New AssignTracks connecting new AssignEvents to new Tracks
            CreateAssignTrack(Guid.Parse("41000000-0000-0000-0000-000000000010"), Guid.Parse("40000000-0000-0000-0000-000000000010"), Guid.Parse("24000000-0000-0000-0000-000000000010")),
            CreateAssignTrack(Guid.Parse("41000000-0000-0000-0000-000000000011"), Guid.Parse("40000000-0000-0000-0000-000000000011"), Guid.Parse("24000000-0000-0000-0000-000000000011")),
            CreateAssignTrack(Guid.Parse("41000000-0000-0000-0000-000000000012"), Guid.Parse("40000000-0000-0000-0000-000000000012"), Guid.Parse("24000000-0000-0000-0000-000000000012")),
            CreateAssignTrack(Guid.Parse("41000000-0000-0000-0000-000000000013"), Guid.Parse("40000000-0000-0000-0000-000000000013"), Guid.Parse("24000000-0000-0000-0000-000000000013")),
            CreateAssignTrack(Guid.Parse("41000000-0000-0000-0000-000000000014"), Guid.Parse("40000000-0000-0000-0000-000000000014"), Guid.Parse("24000000-0000-0000-0000-000000000014")),
            CreateAssignTrack(Guid.Parse("41000000-0000-0000-0000-000000000015"), Guid.Parse("40000000-0000-0000-0000-000000000015"), Guid.Parse("24000000-0000-0000-0000-000000000015")),
            CreateAssignTrack(Guid.Parse("41000000-0000-0000-0000-000000000016"), Guid.Parse("40000000-0000-0000-0000-000000000016"), Guid.Parse("24000000-0000-0000-0000-000000000016")),
            CreateAssignTrack(Guid.Parse("41000000-0000-0000-0000-000000000017"), Guid.Parse("40000000-0000-0000-0000-000000000017"), Guid.Parse("24000000-0000-0000-0000-000000000017")),
            CreateAssignTrack(Guid.Parse("41000000-0000-0000-0000-000000000018"), Guid.Parse("40000000-0000-0000-0000-000000000018"), Guid.Parse("24000000-0000-0000-0000-000000000018")),
            CreateAssignTrack(Guid.Parse("41000000-0000-0000-0000-000000000019"), Guid.Parse("40000000-0000-0000-0000-000000000019"), Guid.Parse("24000000-0000-0000-0000-000000000019")),

            CreateAssignTrack(Guid.Parse("41000000-0000-0000-0000-000000000040"), Guid.Parse("40000000-0000-0000-0000-000000000040"), Guid.Parse("24000000-0000-0000-0000-000000000010")),
            CreateAssignTrack(Guid.Parse("41000000-0000-0000-0000-000000000041"), Guid.Parse("40000000-0000-0000-0000-000000000041"), Guid.Parse("24000000-0000-0000-0000-000000000011")),
            CreateAssignTrack(Guid.Parse("41000000-0000-0000-0000-000000000042"), Guid.Parse("40000000-0000-0000-0000-000000000042"), Guid.Parse("24000000-0000-0000-0000-000000000012")),
            CreateAssignTrack(Guid.Parse("41000000-0000-0000-0000-000000000043"), Guid.Parse("40000000-0000-0000-0000-000000000043"), Guid.Parse("24000000-0000-0000-0000-000000000013")),
            CreateAssignTrack(Guid.Parse("41000000-0000-0000-0000-000000000044"), Guid.Parse("40000000-0000-0000-0000-000000000044"), Guid.Parse("24000000-0000-0000-0000-000000000014")),
            CreateAssignTrack(Guid.Parse("41000000-0000-0000-0000-000000000045"), Guid.Parse("40000000-0000-0000-0000-000000000045"), Guid.Parse("24000000-0000-0000-0000-000000000015")),
            CreateAssignTrack(Guid.Parse("41000000-0000-0000-0000-000000000046"), Guid.Parse("40000000-0000-0000-0000-000000000046"), Guid.Parse("24000000-0000-0000-0000-000000000016")),
            CreateAssignTrack(Guid.Parse("41000000-0000-0000-0000-000000000047"), Guid.Parse("40000000-0000-0000-0000-000000000047"), Guid.Parse("24000000-0000-0000-0000-000000000017")),
            CreateAssignTrack(Guid.Parse("41000000-0000-0000-0000-000000000048"), Guid.Parse("40000000-0000-0000-0000-000000000048"), Guid.Parse("24000000-0000-0000-0000-000000000018")),
            CreateAssignTrack(Guid.Parse("41000000-0000-0000-0000-000000000049"), Guid.Parse("40000000-0000-0000-0000-000000000049"), Guid.Parse("24000000-0000-0000-0000-000000000019"))
        );
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
}
