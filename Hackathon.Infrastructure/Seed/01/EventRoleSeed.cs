using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.EventRole;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Seed;

public static class EventRoleSeed
{
    public static void SeedEventRoles(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventRoles>().HasData(
            new EventRoles
            {
                Id = SeedConstants.MentorEventRoleId,
                Name = EventRoleEnum.Mentor,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },
            new EventRoles
            {
                Id = SeedConstants.JudgeEventRoleId,
                Name = EventRoleEnum.Judge,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },
            new EventRoles
            {
                Id = SeedConstants.StaffEventRoleId,
                Name = EventRoleEnum.Staff,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            }
        );
    }
}
