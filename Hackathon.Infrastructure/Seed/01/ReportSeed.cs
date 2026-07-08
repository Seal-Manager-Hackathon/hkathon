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
                AssignEventId = SeedConstants.JudgeAssignEventId,
                SubmissionId = SeedConstants.GreenCodersFinalSubmissionId,
                Title = "Seed submission report",
                Description = "Seed report for final submission",
                ImgUrl = "https://seed.local/reports/image.png",
                FileUrl = "https://seed.local/reports/file.pdf",
                Status = ReportStatusEnum.Pending,
                Reason = "Seed review reason",
                TypeReport = "Submission",
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },

            // 10 New Reports connected to 10 new Submissions
            CreateReport(Guid.Parse("73000000-0000-0000-0000-000000000010"), SeedConstants.JudgeUserId, SeedConstants.JudgeAssignEventId, Guid.Parse("33000000-0000-0000-0000-000000000010")),
            CreateReport(Guid.Parse("73000000-0000-0000-0000-000000000011"), SeedConstants.JudgeUserId, SeedConstants.JudgeAssignEventId, Guid.Parse("33000000-0000-0000-0000-000000000011")),
            CreateReport(Guid.Parse("73000000-0000-0000-0000-000000000012"), SeedConstants.JudgeUserId, SeedConstants.JudgeAssignEventId, Guid.Parse("33000000-0000-0000-0000-000000000012")),
            CreateReport(Guid.Parse("73000000-0000-0000-0000-000000000013"), SeedConstants.JudgeUserId, SeedConstants.JudgeAssignEventId, Guid.Parse("33000000-0000-0000-0000-000000000013")),
            CreateReport(Guid.Parse("73000000-0000-0000-0000-000000000014"), SeedConstants.JudgeUserId, SeedConstants.JudgeAssignEventId, Guid.Parse("33000000-0000-0000-0000-000000000014")),
            CreateReport(Guid.Parse("73000000-0000-0000-0000-000000000015"), SeedConstants.JudgeUserId, SeedConstants.JudgeAssignEventId, Guid.Parse("33000000-0000-0000-0000-000000000015")),
            CreateReport(Guid.Parse("73000000-0000-0000-0000-000000000016"), Guid.Parse("10000000-0000-0000-0000-000000000021"), Guid.Parse("40000000-0000-0000-0000-000000000016"), Guid.Parse("33000000-0000-0000-0000-000000000016")), // customized JudgeAssign
            CreateReport(Guid.Parse("73000000-0000-0000-0000-000000000017"), Guid.Parse("10000000-0000-0000-0000-000000000022"), Guid.Parse("40000000-0000-0000-0000-000000000017"), Guid.Parse("33000000-0000-0000-0000-000000000017")),
            CreateReport(Guid.Parse("73000000-0000-0000-0000-000000000018"), SeedConstants.JudgeUserId, SeedConstants.JudgeAssignEventId, Guid.Parse("33000000-0000-0000-0000-000000000018")),
            CreateReport(Guid.Parse("73000000-0000-0000-0000-000000000019"), SeedConstants.JudgeUserId, SeedConstants.JudgeAssignEventId, Guid.Parse("33000000-0000-0000-0000-000000000019"))
        );
    }

    private static Reports CreateReport(Guid id, Guid userId, Guid assignEventId, Guid submissionId)
    {
        return new Reports
        {
            Id = id,
            UserId = userId,
            AssignEventId = assignEventId,
            SubmissionId = submissionId,
            Title = "Evaluation Report",
            Description = "Report detail comments",
            ImgUrl = "https://seed.local/reports/image.png",
            FileUrl = "https://seed.local/reports/file.pdf",
            Status = ReportStatusEnum.Pending,
            Reason = "Paging evaluation review comment",
            TypeReport = "Submission",
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }
}
