using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.TeamDetail;
using Hackathon.Domain.Enums.RegisterTeam;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Seed;

public static class TeamSeed
{
    public static void SeedTeams(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Teams>().HasData(
            new Teams
            {
                Id = SeedConstants.SeedInnovatorsTeamId,
                Name = "Seed Innovators",
                CanEdit = true,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },
            new Teams
            {
                Id = SeedConstants.GreenCodersTeamId,
                Name = "Green Coders",
                CanEdit = true,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },

            // 10 New Teams
            CreateTeam(Guid.Parse("30000000-0000-0000-0000-000000000010"), "Nexus Startups"),
            CreateTeam(Guid.Parse("30000000-0000-0000-0000-000000000011"), "Solar Energy Coders"),
            CreateTeam(Guid.Parse("30000000-0000-0000-0000-000000000012"), "Cyber Defenders"),
            CreateTeam(Guid.Parse("30000000-0000-0000-0000-000000000013"), "AI Model Makers"),
            CreateTeam(Guid.Parse("30000000-0000-0000-0000-000000000014"), "Web3 Validators"),
            CreateTeam(Guid.Parse("30000000-0000-0000-0000-000000000015"), "SEAL Coders Elite"),
            CreateTeam(Guid.Parse("30000000-0000-0000-0000-000000000016"), "Cloud Deployers"),
            CreateTeam(Guid.Parse("30000000-0000-0000-0000-000000000017"), "Data Analytics Hackers"),
            CreateTeam(Guid.Parse("30000000-0000-0000-0000-000000000018"), "Smart Grids builders"),
            CreateTeam(Guid.Parse("30000000-0000-0000-0000-000000000019"), "Robotics Pathfinder Team")
        );

        modelBuilder.Entity<TeamDetails>().HasData(
            CreateTeamDetail(Guid.Parse("30100000-0000-0000-0000-000000000001"), SeedConstants.SeedInnovatorsTeamId, SeedConstants.StudentLeaderUserId, true),
            CreateTeamDetail(Guid.Parse("30100000-0000-0000-0000-000000000002"), SeedConstants.SeedInnovatorsTeamId, SeedConstants.StudentMemberUserId, false),
            CreateTeamDetail(Guid.Parse("30100000-0000-0000-0000-000000000003"), SeedConstants.GreenCodersTeamId, SeedConstants.GreenLeaderUserId, true),

            // 10 TeamDetails connecting 10 new Student Users as leaders to 10 new Teams
            CreateTeamDetail(Guid.Parse("30100000-0000-0000-0000-000000000010"), Guid.Parse("30000000-0000-0000-0000-000000000010"), Guid.Parse("10000000-0000-0000-0000-000000000010"), true),
            CreateTeamDetail(Guid.Parse("30100000-0000-0000-0000-000000000011"), Guid.Parse("30000000-0000-0000-0000-000000000011"), Guid.Parse("10000000-0000-0000-0000-000000000011"), true),
            CreateTeamDetail(Guid.Parse("30100000-0000-0000-0000-000000000012"), Guid.Parse("30000000-0000-0000-0000-000000000012"), Guid.Parse("10000000-0000-0000-0000-000000000012"), true),
            CreateTeamDetail(Guid.Parse("30100000-0000-0000-0000-000000000013"), Guid.Parse("30000000-0000-0000-0000-000000000013"), Guid.Parse("10000000-0000-0000-0000-000000000013"), true),
            CreateTeamDetail(Guid.Parse("30100000-0000-0000-0000-000000000014"), Guid.Parse("30000000-0000-0000-0000-000000000014"), Guid.Parse("10000000-0000-0000-0000-000000000014"), true),
            CreateTeamDetail(Guid.Parse("30100000-0000-0000-0000-000000000015"), Guid.Parse("30000000-0000-0000-0000-000000000015"), Guid.Parse("10000000-0000-0000-0000-000000000015"), true),
            CreateTeamDetail(Guid.Parse("30100000-0000-0000-0000-000000000016"), Guid.Parse("30000000-0000-0000-0000-000000000016"), Guid.Parse("10000000-0000-0000-0000-000000000016"), true),
            CreateTeamDetail(Guid.Parse("30100000-0000-0000-0000-000000000017"), Guid.Parse("30000000-0000-0000-0000-000000000017"), Guid.Parse("10000000-0000-0000-0000-000000000017"), true),
            CreateTeamDetail(Guid.Parse("30100000-0000-0000-0000-000000000018"), Guid.Parse("30000000-0000-0000-0000-000000000018"), Guid.Parse("10000000-0000-0000-0000-000000000018"), true),
            CreateTeamDetail(Guid.Parse("30100000-0000-0000-0000-000000000019"), Guid.Parse("30000000-0000-0000-0000-000000000019"), Guid.Parse("10000000-0000-0000-0000-000000000019"), true)
        );

        modelBuilder.Entity<RegisterTeams>().HasData(
            new RegisterTeams
            {
                Id = SeedConstants.SeedInnovatorsRegisterTeamId,
                TeamId = SeedConstants.SeedInnovatorsTeamId,
                EventId = SeedConstants.SealHackathonEventId,
                Description = "Seed Innovators registration",
                Status = RegisterTeamStatusEnum.Approved,
                IsBanned = false,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },
            new RegisterTeams
            {
                Id = SeedConstants.GreenCodersRegisterTeamId,
                TeamId = SeedConstants.GreenCodersTeamId,
                EventId = SeedConstants.SealHackathonEventId,
                Description = "Green Coders registration",
                Status = RegisterTeamStatusEnum.Approved,
                IsBanned = false,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },

            // 10 RegisterTeams connecting new teams to new events
            CreateRegisterTeam(Guid.Parse("31000000-0000-0000-0000-000000000010"), Guid.Parse("30000000-0000-0000-0000-000000000010"), Guid.Parse("20000000-0000-0000-0000-000000000010")),
            CreateRegisterTeam(Guid.Parse("31000000-0000-0000-0000-000000000011"), Guid.Parse("30000000-0000-0000-0000-000000000011"), Guid.Parse("20000000-0000-0000-0000-000000000011")),
            CreateRegisterTeam(Guid.Parse("31000000-0000-0000-0000-000000000012"), Guid.Parse("30000000-0000-0000-0000-000000000012"), Guid.Parse("20000000-0000-0000-0000-000000000012")),
            CreateRegisterTeam(Guid.Parse("31000000-0000-0000-0000-000000000013"), Guid.Parse("30000000-0000-0000-0000-000000000013"), Guid.Parse("20000000-0000-0000-0000-000000000013")),
            CreateRegisterTeam(Guid.Parse("31000000-0000-0000-0000-000000000014"), Guid.Parse("30000000-0000-0000-0000-000000000014"), Guid.Parse("20000000-0000-0000-0000-000000000014")),
            CreateRegisterTeam(Guid.Parse("31000000-0000-0000-0000-000000000015"), Guid.Parse("30000000-0000-0000-0000-000000000015"), Guid.Parse("20000000-0000-0000-0000-000000000015")),
            CreateRegisterTeam(Guid.Parse("31000000-0000-0000-0000-000000000016"), Guid.Parse("30000000-0000-0000-0000-000000000016"), Guid.Parse("20000000-0000-0000-0000-000000000016")),
            CreateRegisterTeam(Guid.Parse("31000000-0000-0000-0000-000000000017"), Guid.Parse("30000000-0000-0000-0000-000000000017"), Guid.Parse("20000000-0000-0000-0000-000000000017")),
            CreateRegisterTeam(Guid.Parse("31000000-0000-0000-0000-000000000018"), Guid.Parse("30000000-0000-0000-0000-000000000018"), Guid.Parse("20000000-0000-0000-0000-000000000018")),
            CreateRegisterTeam(Guid.Parse("31000000-0000-0000-0000-000000000019"), Guid.Parse("30000000-0000-0000-0000-000000000019"), Guid.Parse("20000000-0000-0000-0000-000000000019"))
        );
    }

    private static Teams CreateTeam(Guid id, string name)
    {
        return new Teams
        {
            Id = id,
            Name = name,
            CanEdit = true,
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }

    private static RegisterTeams CreateRegisterTeam(Guid id, Guid teamId, Guid eventId)
    {
        return new RegisterTeams
        {
            Id = id,
            TeamId = teamId,
            EventId = eventId,
            Description = "Paging team registration",
            Status = RegisterTeamStatusEnum.Approved,
            IsBanned = false,
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
}
