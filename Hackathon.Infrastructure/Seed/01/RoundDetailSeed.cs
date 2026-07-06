using Hackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Seed;

public static class RoundDetailSeed
{
    public static void SeedRoundDetails(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoundDetails>().HasData(
            CreateRoundDetail(SeedConstants.SeedInnovatorsIdeaRoundDetailId, SeedConstants.IdeaRoundId, SeedConstants.SeedInnovatorsRegisterTeamId),
            CreateRoundDetail(SeedConstants.SeedInnovatorsFinalRoundDetailId, SeedConstants.FinalRoundId, SeedConstants.SeedInnovatorsRegisterTeamId),
            CreateRoundDetail(SeedConstants.GreenCodersIdeaRoundDetailId, SeedConstants.IdeaRoundId, SeedConstants.GreenCodersRegisterTeamId),
            CreateRoundDetail(SeedConstants.GreenCodersFinalRoundDetailId, SeedConstants.FinalRoundId, SeedConstants.GreenCodersRegisterTeamId),

            // 10 New RoundDetails connecting 10 new rounds to 10 new registered teams
            CreateRoundDetail(Guid.Parse("32000000-0000-0000-0000-000000000010"), Guid.Parse("21000000-0000-0000-0000-000000000010"), Guid.Parse("31000000-0000-0000-0000-000000000010")),
            CreateRoundDetail(Guid.Parse("32000000-0000-0000-0000-000000000011"), Guid.Parse("21000000-0000-0000-0000-000000000011"), Guid.Parse("31000000-0000-0000-0000-000000000011")),
            CreateRoundDetail(Guid.Parse("32000000-0000-0000-0000-000000000012"), Guid.Parse("21000000-0000-0000-0000-000000000012"), Guid.Parse("31000000-0000-0000-0000-000000000012")),
            CreateRoundDetail(Guid.Parse("32000000-0000-0000-0000-000000000013"), Guid.Parse("21000000-0000-0000-0000-000000000013"), Guid.Parse("31000000-0000-0000-0000-000000000013")),
            CreateRoundDetail(Guid.Parse("32000000-0000-0000-0000-000000000014"), Guid.Parse("21000000-0000-0000-0000-000000000014"), Guid.Parse("31000000-0000-0000-0000-000000000014")),
            CreateRoundDetail(Guid.Parse("32000000-0000-0000-0000-000000000015"), Guid.Parse("21000000-0000-0000-0000-000000000015"), Guid.Parse("31000000-0000-0000-0000-000000000015")),
            CreateRoundDetail(Guid.Parse("32000000-0000-0000-0000-000000000016"), Guid.Parse("21000000-0000-0000-0000-000000000016"), Guid.Parse("31000000-0000-0000-0000-000000000016")),
            CreateRoundDetail(Guid.Parse("32000000-0000-0000-0000-000000000017"), Guid.Parse("21000000-0000-0000-0000-000000000017"), Guid.Parse("31000000-0000-0000-0000-000000000017")),
            CreateRoundDetail(Guid.Parse("32000000-0000-0000-0000-000000000018"), Guid.Parse("21000000-0000-0000-0000-000000000018"), Guid.Parse("31000000-0000-0000-0000-000000000018")),
            CreateRoundDetail(Guid.Parse("32000000-0000-0000-0000-000000000019"), Guid.Parse("21000000-0000-0000-0000-000000000019"), Guid.Parse("31000000-0000-0000-0000-000000000019"))
        );
    }

    private static RoundDetails CreateRoundDetail(Guid id, Guid roundId, Guid registerTeamId)
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
