using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.User;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Seed;

public static class UserSeed
{
    public static void SeedUsers(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Users>().HasData(
            CreateUser(SeedConstants.AdminUserId, "admin@seed.local", "Admin", "Seed", "System Administrator", RoleEnum.Admin),
            CreateUser(SeedConstants.StaffUserId, "staff@seed.local", "Staff", "Seed", "Event Staff", RoleEnum.Staff),
            CreateUser(SeedConstants.MentorUserId, "mentor@seed.local", "Mentor", "Lecturer", "Seed Mentor", RoleEnum.Lecturer),
            CreateUser(SeedConstants.JudgeUserId, "judge@seed.local", "Judge", "Lecturer", "Seed Judge", RoleEnum.Lecturer),
            CreateUser(SeedConstants.StudentLeaderUserId, "leader@seed.local", "Student", "Leader", "SEAL001", RoleEnum.Student),
            CreateUser(SeedConstants.StudentMemberUserId, "member@seed.local", "Student", "Member", "SEAL002", RoleEnum.Student),
            CreateUser(SeedConstants.GreenLeaderUserId, "green.leader@seed.local", "Green", "Leader", "SEAL003", RoleEnum.Student),

            // 10 Student Data Seeds
            CreateUser(Guid.Parse("10000000-0000-0000-0000-000000000010"), "alex.jones@student.local", "Alex", "Jones", "SEAL010", RoleEnum.Student),
            CreateUser(Guid.Parse("10000000-0000-0000-0000-000000000011"), "emma.watson@student.local", "Emma", "Watson", "SEAL011", RoleEnum.Student),
            CreateUser(Guid.Parse("10000000-0000-0000-0000-000000000012"), "liam.smith@student.local", "Liam", "Smith", "SEAL012", RoleEnum.Student),
            CreateUser(Guid.Parse("10000000-0000-0000-0000-000000000013"), "sophia.brown@student.local", "Sophia", "Brown", "SEAL013", RoleEnum.Student),
            CreateUser(Guid.Parse("10000000-0000-0000-0000-000000000014"), "mason.davis@student.local", "Mason", "Davis", "SEAL014", RoleEnum.Student),
            CreateUser(Guid.Parse("10000000-0000-0000-0000-000000000015"), "olivia.miller@student.local", "Olivia", "Miller", "SEAL015", RoleEnum.Student),
            CreateUser(Guid.Parse("10000000-0000-0000-0000-000000000016"), "ethan.wilson@student.local", "Ethan", "Wilson", "SEAL016", RoleEnum.Student),
            CreateUser(Guid.Parse("10000000-0000-0000-0000-000000000017"), "ava.taylor@student.local", "Ava", "Taylor", "SEAL017", RoleEnum.Student),
            CreateUser(Guid.Parse("10000000-0000-0000-0000-000000000018"), "lucas.thomas@student.local", "Lucas", "Thomas", "SEAL018", RoleEnum.Student),
            CreateUser(Guid.Parse("10000000-0000-0000-0000-000000000019"), "mia.white@student.local", "Mia", "White", "SEAL019", RoleEnum.Student),

            // 5 Staff Data Seeds
            CreateUser(Guid.Parse("10000000-0000-0000-0000-000000000025"), "sarah.johnson@staff.local", "Sarah", "Johnson", "STF001", RoleEnum.Staff),
            CreateUser(Guid.Parse("10000000-0000-0000-0000-000000000026"), "michael.chen@staff.local", "Michael", "Chen", "STF002", RoleEnum.Staff),
            CreateUser(Guid.Parse("10000000-0000-0000-0000-000000000027"), "emily.davis@staff.local", "Emily", "Davis", "STF003", RoleEnum.Staff),
            CreateUser(Guid.Parse("10000000-0000-0000-0000-000000000028"), "kevin.nguyen@staff.local", "Kevin", "Nguyen", "STF004", RoleEnum.Staff),
            CreateUser(Guid.Parse("10000000-0000-0000-0000-000000000029"), "lisa.taylor@staff.local", "Lisa", "Taylor", "STF005", RoleEnum.Staff),

            // 5 Mentor / Lecturer Data Seeds
            CreateUser(Guid.Parse("10000000-0000-0000-0000-000000000020"), "robert.martin@lecturer.local", "Robert", "Martin", "LECT001", RoleEnum.Lecturer),
            CreateUser(Guid.Parse("10000000-0000-0000-0000-000000000021"), "linda.clark@lecturer.local", "Linda", "Clark", "LECT002", RoleEnum.Lecturer),
            CreateUser(Guid.Parse("10000000-0000-0000-0000-000000000022"), "david.lewis@lecturer.local", "David", "Lewis", "LECT003", RoleEnum.Lecturer),
            CreateUser(Guid.Parse("10000000-0000-0000-0000-000000000023"), "barbara.hall@lecturer.local", "Barbara", "Hall", "LECT004", RoleEnum.Lecturer),
            CreateUser(Guid.Parse("10000000-0000-0000-0000-000000000024"), "james.allen@lecturer.local", "James", "Allen", "LECT005", RoleEnum.Lecturer)
        );
    }

    private static Users CreateUser(Guid id, string email, string firstName, string lastName, string studentId, RoleEnum role)
    {
        return new Users
        {
            Id = id,
            Email = email,
            HashPassword = "seed-password-hash-not-for-login",
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = "0900000000",
            AvatarUrl = "https://seed.local/avatar.png",
            Bio = "Seed user",
            Address = "Seed address",
            DateOfBirth = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero),
            StudentId = studentId,
            Role = role,
            College = "Seed University",
            ImgUrl = "https://seed.local/profile.png",
            LinkUrl = "https://seed.local/users",
            VerifyEmailAt = SeedConstants.CreatedAt,
            Status = UserStatusEnum.Active,
            IsVerified = true,
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }
}
