using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hackathon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFinTechEventSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "CreatedAt", "Description", "EndTime", "IsDisable", "LimitTeam", "MaxMember", "MinMember", "Name", "NumberRound", "RegisterLimitTime", "Season", "StartTime", "Status", "UpdatedAt" },
                values: new object[] { new Guid("20000000-0000-0000-0000-000000000070"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "FinTech & Web3 Hackathon 2026 là cuộc thi lập trình 48 giờ quy mô toàn quốc, thi đấu trên ba mảng: Thanh toán & Ngân hàng số, DeFi & Smart Contract, và Hạ tầng Web3 & Bảo mật. Các đội sẽ trải qua hai vòng: vòng Đề án & Chế tạo mẫu (Prototyping) và vòng Trình diễn & Bảo vệ (Final Demo). Mỗi track có ba chủ đề gợi mở để các đội lựa chọn, được cố vấn chuyên môn bởi các mentor giàu kinh nghiệm và đánh giá bởi hội đồng giám khảo trên cả ba track. Sự kiện mở đăng ký cho tối đa 30 đội, mỗi đội 2-4 thành viên.", new DateTimeOffset(new DateTime(2026, 10, 12, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, 30, 4, 2, "FinTech & Web3 Hackathon 2026", 2, new DateTimeOffset(new DateTime(2026, 10, 5, 16, 59, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, new DateTimeOffset(new DateTime(2026, 10, 10, 1, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Published", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "CanEdit", "CreatedAt", "IsDisable", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("30000000-0000-0000-0000-000000000070"), false, new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "PayForge", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("30000000-0000-0000-0000-000000000071"), false, new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "DeFiApe", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("30000000-0000-0000-0000-000000000072"), false, new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "ChainGuard", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "AvatarUrl", "BanReason", "BannedAt", "Bio", "College", "CreatedAt", "DateOfBirth", "Email", "FirstName", "HashPassword", "ImgUrl", "IsDisable", "IsVerified", "LastName", "LinkUrl", "PhoneNumber", "Role", "Status", "StudentId", "UpdatedAt", "VerifyEmailAt" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000070"), "Seed address", "https://robohash.org/bao.trinh@lecturer.seed.local", null, null, "Mentor for Payments & Digital Banking track", "FPT University", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "bao.trinh@lecturer.seed.local", "Quoc Bao", "$2a$11$FTrHahxSf5lojw6joRVC3.ArTfL/2tspZvqA/5i3FeZH1k.ATyvze", "https://robohash.org/bao.trinh@lecturer.seed.local", false, true, "Trinh", "", "0900000000", 3, "Active", "LECT070", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("10000000-0000-0000-0000-000000000071"), "Seed address", "https://robohash.org/ha.nguyen@lecturer.seed.local", null, null, "Mentor for DeFi & Smart Contracts track", "FPT University", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "ha.nguyen@lecturer.seed.local", "Thanh Ha", "$2a$11$FTrHahxSf5lojw6joRVC3.ArTfL/2tspZvqA/5i3FeZH1k.ATyvze", "https://robohash.org/ha.nguyen@lecturer.seed.local", false, true, "Nguyen", "", "0900000000", 3, "Active", "LECT071", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("10000000-0000-0000-0000-000000000072"), "Seed address", "https://robohash.org/duc.le@lecturer.seed.local", null, null, "Mentor for Web3 Infrastructure & Security track", "FPT University", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "duc.le@lecturer.seed.local", "Minh Duc", "$2a$11$FTrHahxSf5lojw6joRVC3.ArTfL/2tspZvqA/5i3FeZH1k.ATyvze", "https://robohash.org/duc.le@lecturer.seed.local", false, true, "Le", "", "0900000000", 3, "Active", "LECT072", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("10000000-0000-0000-0000-000000000073"), "Seed address", "https://robohash.org/tuananh.pham@lecturer.seed.local", null, null, "Judge evaluating all FinTech & Web3 tracks", "FPT University", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "tuananh.pham@lecturer.seed.local", "Tuan Anh", "$2a$11$FTrHahxSf5lojw6joRVC3.ArTfL/2tspZvqA/5i3FeZH1k.ATyvze", "https://robohash.org/tuananh.pham@lecturer.seed.local", false, true, "Pham", "", "0900000000", 3, "Active", "LECT073", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("10000000-0000-0000-0000-000000000074"), "Seed address", "https://robohash.org/hong.tran@lecturer.seed.local", null, null, "Judge evaluating all FinTech & Web3 tracks", "FPT University", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "hong.tran@lecturer.seed.local", "Thi Hong", "$2a$11$FTrHahxSf5lojw6joRVC3.ArTfL/2tspZvqA/5i3FeZH1k.ATyvze", "https://robohash.org/hong.tran@lecturer.seed.local", false, true, "Tran", "", "0900000000", 3, "Active", "LECT074", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("10000000-0000-0000-0000-000000000075"), "Seed address", "https://robohash.org/long.vu@staff.seed.local", null, null, "Event operations staff", "FPT University", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "long.vu@staff.seed.local", "Hoang Long", "$2a$11$FTrHahxSf5lojw6joRVC3.ArTfL/2tspZvqA/5i3FeZH1k.ATyvze", "https://robohash.org/long.vu@staff.seed.local", false, true, "Vu", "", "0900000000", 1, "Active", "STF070", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("10000000-0000-0000-0000-000000000076"), "Seed address", "https://robohash.org/mai.dang@staff.seed.local", null, null, "Event operations staff", "FPT University", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "mai.dang@staff.seed.local", "Thi Mai", "$2a$11$FTrHahxSf5lojw6joRVC3.ArTfL/2tspZvqA/5i3FeZH1k.ATyvze", "https://robohash.org/mai.dang@staff.seed.local", false, true, "Dang", "", "0900000000", 1, "Active", "STF071", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("10000000-0000-0000-0000-000000000077"), "Seed address", "https://robohash.org/minh.pham@student.seed.local", null, null, "PayForge team leader", "FPT University", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "minh.pham@student.seed.local", "Cong Minh", "$2a$11$FTrHahxSf5lojw6joRVC3.ArTfL/2tspZvqA/5i3FeZH1k.ATyvze", "https://robohash.org/minh.pham@student.seed.local", false, true, "Pham", "", "0900000000", 2, "Active", "STU070", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("10000000-0000-0000-0000-000000000078"), "Seed address", "https://robohash.org/linh.do@student.seed.local", null, null, "PayForge team member", "FPT University", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "linh.do@student.seed.local", "Thu Linh", "$2a$11$FTrHahxSf5lojw6joRVC3.ArTfL/2tspZvqA/5i3FeZH1k.ATyvze", "https://robohash.org/linh.do@student.seed.local", false, true, "Do", "", "0900000000", 2, "Active", "STU071", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("10000000-0000-0000-0000-000000000079"), "Seed address", "https://robohash.org/kien.tran@student.seed.local", null, null, "DeFiApe team leader", "FPT University", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "kien.tran@student.seed.local", "Van Kien", "$2a$11$FTrHahxSf5lojw6joRVC3.ArTfL/2tspZvqA/5i3FeZH1k.ATyvze", "https://robohash.org/kien.tran@student.seed.local", false, true, "Tran", "", "0900000000", 2, "Active", "STU072", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("10000000-0000-0000-0000-000000000080"), "Seed address", "https://robohash.org/quynh.vo@student.seed.local", null, null, "DeFiApe team member", "FPT University", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "quynh.vo@student.seed.local", "Ngoc Quynh", "$2a$11$FTrHahxSf5lojw6joRVC3.ArTfL/2tspZvqA/5i3FeZH1k.ATyvze", "https://robohash.org/quynh.vo@student.seed.local", false, true, "Vo", "", "0900000000", 2, "Active", "STU073", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("10000000-0000-0000-0000-000000000081"), "Seed address", "https://robohash.org/hung.bui@student.seed.local", null, null, "ChainGuard team leader", "FPT University", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "hung.bui@student.seed.local", "Quoc Hung", "$2a$11$FTrHahxSf5lojw6joRVC3.ArTfL/2tspZvqA/5i3FeZH1k.ATyvze", "https://robohash.org/hung.bui@student.seed.local", false, true, "Bui", "", "0900000000", 2, "Active", "STU074", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("10000000-0000-0000-0000-000000000082"), "Seed address", "https://robohash.org/trang.ngo@student.seed.local", null, null, "ChainGuard team member", "FPT University", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "trang.ngo@student.seed.local", "Hai Trang", "$2a$11$FTrHahxSf5lojw6joRVC3.ArTfL/2tspZvqA/5i3FeZH1k.ATyvze", "https://robohash.org/trang.ngo@student.seed.local", false, true, "Ngo", "", "0900000000", 2, "Active", "STU075", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "AssignEvents",
                columns: new[] { "Id", "CreatedAt", "EventId", "EventRoleId", "IsDisable", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { new Guid("40000000-0000-0000-0000-000000000070"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("20000000-0000-0000-0000-000000000070"), new Guid("55555555-5555-5555-5555-555555555555"), false, new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("10000000-0000-0000-0000-000000000070") },
                    { new Guid("40000000-0000-0000-0000-000000000071"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("20000000-0000-0000-0000-000000000070"), new Guid("55555555-5555-5555-5555-555555555555"), false, new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("10000000-0000-0000-0000-000000000071") },
                    { new Guid("40000000-0000-0000-0000-000000000072"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("20000000-0000-0000-0000-000000000070"), new Guid("55555555-5555-5555-5555-555555555555"), false, new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("10000000-0000-0000-0000-000000000072") },
                    { new Guid("40000000-0000-0000-0000-000000000073"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("20000000-0000-0000-0000-000000000070"), new Guid("66666666-6666-6666-6666-666666666666"), false, new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("10000000-0000-0000-0000-000000000073") },
                    { new Guid("40000000-0000-0000-0000-000000000074"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("20000000-0000-0000-0000-000000000070"), new Guid("66666666-6666-6666-6666-666666666666"), false, new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("10000000-0000-0000-0000-000000000074") },
                    { new Guid("40000000-0000-0000-0000-000000000075"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("20000000-0000-0000-0000-000000000070"), new Guid("77777777-7777-7777-7777-777777777777"), false, new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("10000000-0000-0000-0000-000000000075") },
                    { new Guid("40000000-0000-0000-0000-000000000076"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("20000000-0000-0000-0000-000000000070"), new Guid("77777777-7777-7777-7777-777777777777"), false, new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("10000000-0000-0000-0000-000000000076") }
                });

            migrationBuilder.InsertData(
                table: "Rounds",
                columns: new[] { "Id", "CreatedAt", "Description", "EndSubmission", "EndTime", "EventId", "IsDisable", "LimitTeam", "Name", "RoundNo", "StartSubmission", "StartTime", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("21000000-0000-0000-0000-000000000070"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Vòng 1 — Các đội trình bày ý tưởng, chọn chủ đề trong track đã đăng ký và chế tạo nguyên mẫu (prototype) trong vòng 48 giờ. Mentor phụ trách track sẽ hỗ trợ định hướng kỹ thuật. Nộp bài trước 12:00 trưa ngày hôm sau.", new DateTimeOffset(new DateTime(2026, 10, 11, 3, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 10, 11, 1, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("20000000-0000-0000-0000-000000000070"), false, 30, "Ideation & Prototyping", 1, new DateTimeOffset(new DateTime(2026, 10, 10, 3, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 10, 10, 1, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("21000000-0000-0000-0000-000000000071"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Vòng 2 — Các đội xuất sắc trình diễn sản phẩm hoàn thiện và bảo vệ trước hội đồng giám khảo. Cả hai giám khảo sẽ chấm điểm trên cả ba track theo cùng bộ tiêu chí. Nộp bản final trước 12:00 trưa ngày diễn ra.", new DateTimeOffset(new DateTime(2026, 10, 12, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 10, 12, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("20000000-0000-0000-0000-000000000070"), false, 15, "Final Demo & Pitch", 2, new DateTimeOffset(new DateTime(2026, 10, 11, 3, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 10, 11, 1, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "TeamDetails",
                columns: new[] { "Id", "CreatedAt", "IsDisable", "IsLeader", "Status", "TeamId", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { new Guid("30100000-0000-0000-0000-000000000070"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, true, "Active", new Guid("30000000-0000-0000-0000-000000000070"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("10000000-0000-0000-0000-000000000077") },
                    { new Guid("30100000-0000-0000-0000-000000000071"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, false, "Active", new Guid("30000000-0000-0000-0000-000000000070"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("10000000-0000-0000-0000-000000000078") },
                    { new Guid("30100000-0000-0000-0000-000000000072"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, true, "Active", new Guid("30000000-0000-0000-0000-000000000071"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("10000000-0000-0000-0000-000000000079") },
                    { new Guid("30100000-0000-0000-0000-000000000073"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, false, "Active", new Guid("30000000-0000-0000-0000-000000000071"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("10000000-0000-0000-0000-000000000080") },
                    { new Guid("30100000-0000-0000-0000-000000000074"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, true, "Active", new Guid("30000000-0000-0000-0000-000000000072"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("10000000-0000-0000-0000-000000000081") },
                    { new Guid("30100000-0000-0000-0000-000000000075"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, false, "Active", new Guid("30000000-0000-0000-0000-000000000072"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("10000000-0000-0000-0000-000000000082") }
                });

            migrationBuilder.InsertData(
                table: "Tracks",
                columns: new[] { "Id", "CreatedAt", "Description", "EventId", "IsDisable", "MaxTeam", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("24000000-0000-0000-0000-000000000070"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Giải pháp thanh toán xuyên biên giới, ngân hàng nhúng (embedded finance) và onboarding số cho người dùng mới.", new Guid("20000000-0000-0000-0000-000000000070"), false, 10, "Payments & Digital Banking", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("24000000-0000-0000-0000-000000000071"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Giao thức cho phi tập trung, thị trường tự động (AMM) và công cụ kiểm toán/bảo mật smart contract.", new Guid("20000000-0000-0000-0000-000000000070"), false, 10, "DeFi & Smart Contracts", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("24000000-0000-0000-0000-000000000072"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Hạ tầng Web3: bảo mật ví, mở rộng quy mô (Layer-2) và định danh phi tập trung (DID).", new Guid("20000000-0000-0000-0000-000000000070"), false, 10, "Web3 Infrastructure & Security", new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "AssignTracks",
                columns: new[] { "Id", "AssignEventId", "CreatedAt", "IsDisable", "TrackId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("41000000-0000-0000-0000-000000000070"), new Guid("40000000-0000-0000-0000-000000000070"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new Guid("24000000-0000-0000-0000-000000000070"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("41000000-0000-0000-0000-000000000071"), new Guid("40000000-0000-0000-0000-000000000071"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new Guid("24000000-0000-0000-0000-000000000071"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("41000000-0000-0000-0000-000000000072"), new Guid("40000000-0000-0000-0000-000000000072"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new Guid("24000000-0000-0000-0000-000000000072"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("41000000-0000-0000-0000-000000000073"), new Guid("40000000-0000-0000-0000-000000000073"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new Guid("24000000-0000-0000-0000-000000000070"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("41000000-0000-0000-0000-000000000074"), new Guid("40000000-0000-0000-0000-000000000073"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new Guid("24000000-0000-0000-0000-000000000071"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("41000000-0000-0000-0000-000000000075"), new Guid("40000000-0000-0000-0000-000000000073"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new Guid("24000000-0000-0000-0000-000000000072"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("41000000-0000-0000-0000-000000000076"), new Guid("40000000-0000-0000-0000-000000000074"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new Guid("24000000-0000-0000-0000-000000000070"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("41000000-0000-0000-0000-000000000077"), new Guid("40000000-0000-0000-0000-000000000074"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new Guid("24000000-0000-0000-0000-000000000071"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("41000000-0000-0000-0000-000000000078"), new Guid("40000000-0000-0000-0000-000000000074"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new Guid("24000000-0000-0000-0000-000000000072"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "Id", "CreatedAt", "Description", "IsDisable", "Title", "TrackId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("25000000-0000-0000-0000-000000000070"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Cổng thanh toán xuyên biên giới tối ưu chi phí giao dịch và tốc độ thanh toán liên ngân hàng quốc tế.", false, "Cross-Border Payment Gateway", new Guid("24000000-0000-0000-0000-000000000070"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("25000000-0000-0000-0000-000000000071"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Bộ API nhúng dịch vụ tài chính (thanh toán, ví, cấp tín dụng) trực tiếp vào nền tảng phi tài chính.", false, "Embedded Finance APIs", new Guid("24000000-0000-0000-0000-000000000070"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("25000000-0000-0000-0000-000000000072"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Luồng onboarding số với eKYC, phát hiện gian lận và trải nghiệm người dùng mới mượt mà.", false, "Digital KYC & Onboarding", new Guid("24000000-0000-0000-0000-000000000070"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("25000000-0000-0000-0000-000000000073"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Giao thức cho vay/mượn phi tập trung với hồ sơ quá mức và quản lý rủi ro tự động.", false, "Decentralized Lending Protocol", new Guid("24000000-0000-0000-0000-000000000071"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("25000000-0000-0000-0000-000000000074"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Thị trường tạo lập tự động với tối ưu thanh khoản và giảm trượt giá.", false, "Automated Market Maker (AMM)", new Guid("24000000-0000-0000-0000-000000000071"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("25000000-0000-0000-0000-000000000075"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Bot tự động rà soát lỗ hổng smart contract và đề xuất bản vá trước khi triển khai.", false, "Smart Contract Audit Bot", new Guid("24000000-0000-0000-0000-000000000071"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("25000000-0000-0000-0000-000000000076"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Cơ chế bảo mật và khôi phục ví đa chữ ký, chống mất cắp và khôi phục truy cập cho người dùng.", false, "Wallet Security & Recovery", new Guid("24000000-0000-0000-0000-000000000072"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("25000000-0000-0000-0000-000000000077"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Giải pháp mở rộng quy mô Layer-2 giảm phí và tăng thông lượng giao dịch.", false, "Layer-2 Scaling Solution", new Guid("24000000-0000-0000-0000-000000000072"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("25000000-0000-0000-0000-000000000078"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Hệ thống định danh phi tập trung cho phép người dùng tự quản trị danh tính và quyền riêng tư.", false, "Decentralized Identity (DID)", new Guid("24000000-0000-0000-0000-000000000072"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "RegisterTeams",
                columns: new[] { "Id", "CreatedAt", "Description", "EventId", "IsBanned", "IsDisable", "RejectionReason", "Status", "TeamId", "TopicId", "TrackId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("31000000-0000-0000-0000-000000000070"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "PayForge xây dựng cổng thanh toán xuyên biên giới tích hợp đa ví, giảm chi phí giao dịch 40% so với cổng truyền thống.", new Guid("20000000-0000-0000-0000-000000000070"), false, false, null, "Approved", new Guid("30000000-0000-0000-0000-000000000070"), new Guid("25000000-0000-0000-0000-000000000070"), new Guid("24000000-0000-0000-0000-000000000070"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("31000000-0000-0000-0000-000000000071"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "DeFiApe triển khai giao thức cho vay phi tập trung với hồ sơ quá mức tự động và mô hình quản lý rủi ro dựa trên dữ liệu on-chain.", new Guid("20000000-0000-0000-0000-000000000070"), false, false, null, "Approved", new Guid("30000000-0000-0000-0000-000000000071"), new Guid("25000000-0000-0000-0000-000000000073"), new Guid("24000000-0000-0000-0000-000000000071"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("31000000-0000-0000-0000-000000000072"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "ChainGuard phát triển giải pháp ví đa chữ ký với cơ chế khôi phục xã hội (social recovery) giúp người dùng chống mất cắp và khôi phục truy cập an toàn.", new Guid("20000000-0000-0000-0000-000000000070"), false, false, null, "Pending", new Guid("30000000-0000-0000-0000-000000000072"), new Guid("25000000-0000-0000-0000-000000000076"), new Guid("24000000-0000-0000-0000-000000000072"), new DateTimeOffset(new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AssignEvents",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000075"));

            migrationBuilder.DeleteData(
                table: "AssignEvents",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000076"));

            migrationBuilder.DeleteData(
                table: "AssignTracks",
                keyColumn: "Id",
                keyValue: new Guid("41000000-0000-0000-0000-000000000070"));

            migrationBuilder.DeleteData(
                table: "AssignTracks",
                keyColumn: "Id",
                keyValue: new Guid("41000000-0000-0000-0000-000000000071"));

            migrationBuilder.DeleteData(
                table: "AssignTracks",
                keyColumn: "Id",
                keyValue: new Guid("41000000-0000-0000-0000-000000000072"));

            migrationBuilder.DeleteData(
                table: "AssignTracks",
                keyColumn: "Id",
                keyValue: new Guid("41000000-0000-0000-0000-000000000073"));

            migrationBuilder.DeleteData(
                table: "AssignTracks",
                keyColumn: "Id",
                keyValue: new Guid("41000000-0000-0000-0000-000000000074"));

            migrationBuilder.DeleteData(
                table: "AssignTracks",
                keyColumn: "Id",
                keyValue: new Guid("41000000-0000-0000-0000-000000000075"));

            migrationBuilder.DeleteData(
                table: "AssignTracks",
                keyColumn: "Id",
                keyValue: new Guid("41000000-0000-0000-0000-000000000076"));

            migrationBuilder.DeleteData(
                table: "AssignTracks",
                keyColumn: "Id",
                keyValue: new Guid("41000000-0000-0000-0000-000000000077"));

            migrationBuilder.DeleteData(
                table: "AssignTracks",
                keyColumn: "Id",
                keyValue: new Guid("41000000-0000-0000-0000-000000000078"));

            migrationBuilder.DeleteData(
                table: "RegisterTeams",
                keyColumn: "Id",
                keyValue: new Guid("31000000-0000-0000-0000-000000000070"));

            migrationBuilder.DeleteData(
                table: "RegisterTeams",
                keyColumn: "Id",
                keyValue: new Guid("31000000-0000-0000-0000-000000000071"));

            migrationBuilder.DeleteData(
                table: "RegisterTeams",
                keyColumn: "Id",
                keyValue: new Guid("31000000-0000-0000-0000-000000000072"));

            migrationBuilder.DeleteData(
                table: "Rounds",
                keyColumn: "Id",
                keyValue: new Guid("21000000-0000-0000-0000-000000000070"));

            migrationBuilder.DeleteData(
                table: "Rounds",
                keyColumn: "Id",
                keyValue: new Guid("21000000-0000-0000-0000-000000000071"));

            migrationBuilder.DeleteData(
                table: "TeamDetails",
                keyColumn: "Id",
                keyValue: new Guid("30100000-0000-0000-0000-000000000070"));

            migrationBuilder.DeleteData(
                table: "TeamDetails",
                keyColumn: "Id",
                keyValue: new Guid("30100000-0000-0000-0000-000000000071"));

            migrationBuilder.DeleteData(
                table: "TeamDetails",
                keyColumn: "Id",
                keyValue: new Guid("30100000-0000-0000-0000-000000000072"));

            migrationBuilder.DeleteData(
                table: "TeamDetails",
                keyColumn: "Id",
                keyValue: new Guid("30100000-0000-0000-0000-000000000073"));

            migrationBuilder.DeleteData(
                table: "TeamDetails",
                keyColumn: "Id",
                keyValue: new Guid("30100000-0000-0000-0000-000000000074"));

            migrationBuilder.DeleteData(
                table: "TeamDetails",
                keyColumn: "Id",
                keyValue: new Guid("30100000-0000-0000-0000-000000000075"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("25000000-0000-0000-0000-000000000071"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("25000000-0000-0000-0000-000000000072"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("25000000-0000-0000-0000-000000000074"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("25000000-0000-0000-0000-000000000075"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("25000000-0000-0000-0000-000000000077"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("25000000-0000-0000-0000-000000000078"));

            migrationBuilder.DeleteData(
                table: "AssignEvents",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000070"));

            migrationBuilder.DeleteData(
                table: "AssignEvents",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000071"));

            migrationBuilder.DeleteData(
                table: "AssignEvents",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000072"));

            migrationBuilder.DeleteData(
                table: "AssignEvents",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000073"));

            migrationBuilder.DeleteData(
                table: "AssignEvents",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000074"));

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000070"));

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000071"));

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000072"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("25000000-0000-0000-0000-000000000070"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("25000000-0000-0000-0000-000000000073"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("25000000-0000-0000-0000-000000000076"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000075"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000076"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000077"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000078"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000079"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000080"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000081"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000082"));

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: new Guid("24000000-0000-0000-0000-000000000070"));

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: new Guid("24000000-0000-0000-0000-000000000071"));

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: new Guid("24000000-0000-0000-0000-000000000072"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000070"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000071"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000072"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000073"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000074"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000070"));
        }
    }
}
