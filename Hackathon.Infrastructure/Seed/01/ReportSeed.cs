using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Report;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Seed;

public static class ReportSeed
{
    public static void SeedReports(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reports>().HasData(
            new Reports
            {
                Id = Guid.Parse("73000000-0000-0000-0000-000000000001"),
                UserId = SeedConstants.JudgeUserId,
                Title = "Seed submission report",
                Description = "Seed report for final submission",
                Status = ReportStatusEnum.Pending,
                Reason = "Seed review reason",
                TypeReport = "Submission",
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },

            // 10 New Reports
            CreateReport(Guid.Parse("73000000-0000-0000-0000-000000000010"), SeedConstants.JudgeUserId),
            CreateReport(Guid.Parse("73000000-0000-0000-0000-000000000011"), SeedConstants.JudgeUserId),
            CreateReport(Guid.Parse("73000000-0000-0000-0000-000000000012"), SeedConstants.JudgeUserId),
            CreateReport(Guid.Parse("73000000-0000-0000-0000-000000000013"), SeedConstants.JudgeUserId),
            CreateReport(Guid.Parse("73000000-0000-0000-0000-000000000014"), SeedConstants.JudgeUserId),
            CreateReport(Guid.Parse("73000000-0000-0000-0000-000000000015"), SeedConstants.JudgeUserId),
            CreateReport(Guid.Parse("73000000-0000-0000-0000-000000000016"), Guid.Parse("10000000-0000-0000-0000-000000000021")),
            CreateReport(Guid.Parse("73000000-0000-0000-0000-000000000017"), Guid.Parse("10000000-0000-0000-0000-000000000022")),
            CreateReport(Guid.Parse("73000000-0000-0000-0000-000000000018"), SeedConstants.JudgeUserId),
            CreateReport(Guid.Parse("73000000-0000-0000-0000-000000000019"), SeedConstants.JudgeUserId)
        );
    }

    private static Reports CreateReport(Guid id, Guid userId)
    {
        return new Reports
        {
            Id = id,
            UserId = userId,
            Title = "Evaluation Report",
            Description = "Report detail comments",
            Status = ReportStatusEnum.Pending,
            Reason = "Paging evaluation review comment",
            TypeReport = "Submission",
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }
}
