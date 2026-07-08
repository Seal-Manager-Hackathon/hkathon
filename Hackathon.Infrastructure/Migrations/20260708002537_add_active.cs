using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_active : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CriteriaTemplates",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "CriteriaTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22000000-0000-0000-0000-000000000001"),
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CriteriaTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22000000-0000-0000-0000-000000000002"),
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CriteriaTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22000000-0000-0000-0000-000000000010"),
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CriteriaTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22000000-0000-0000-0000-000000000011"),
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CriteriaTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22000000-0000-0000-0000-000000000012"),
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CriteriaTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22000000-0000-0000-0000-000000000013"),
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CriteriaTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22000000-0000-0000-0000-000000000014"),
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CriteriaTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22000000-0000-0000-0000-000000000015"),
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CriteriaTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22000000-0000-0000-0000-000000000016"),
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CriteriaTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22000000-0000-0000-0000-000000000017"),
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CriteriaTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22000000-0000-0000-0000-000000000018"),
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CriteriaTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22000000-0000-0000-0000-000000000019"),
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CriteriaTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22000000-0000-0000-0000-000000000100"),
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "CriteriaTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22000000-0000-0000-0000-000000000101"),
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CriteriaTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22000000-0000-0000-0000-000000000102"),
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CriteriaTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22000000-0000-0000-0000-000000000103"),
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "CriteriaTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22000000-0000-0000-0000-000000000104"),
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CriteriaTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22000000-0000-0000-0000-000000000105"),
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CriteriaTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22000000-0000-0000-0000-000000000200"),
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "CriteriaTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22000000-0000-0000-0000-000000000201"),
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CriteriaTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22000000-0000-0000-0000-000000000202"),
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CriteriaTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22000000-0000-0000-0000-000000000203"),
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "CriteriaTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22000000-0000-0000-0000-000000000204"),
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CriteriaTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22000000-0000-0000-0000-000000000205"),
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000011"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000012"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000013"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000014"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000015"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000016"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000017"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000018"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000019"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000020"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000021"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000022"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000023"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000024"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000025"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000026"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000027"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000028"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000029"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000030"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000031"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000032"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000033"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000034"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000035"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000036"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000037"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000038"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000039"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000040"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000041"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000c8"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000c9"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000ca"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000cb"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000cc"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000cd"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000ce"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000cf"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000d0"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000d1"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000d2"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000d3"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000d4"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000d5"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000d6"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000d7"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000d8"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000d9"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000da"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000db"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000dc"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000dd"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000de"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000df"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000e0"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000e1"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000e2"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000e3"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000e4"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000e5"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000f0"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000f1"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000f2"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000f3"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000f4"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000f5"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000f6"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000f7"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000f8"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000f9"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000fa"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000fb"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000fc"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000fd"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000fe"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000ff"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000100"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000101"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000102"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000103"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000104"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000105"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000106"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000107"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000108"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000109"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000010a"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000010b"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000010c"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000010d"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000280"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000281"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000282"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000283"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000284"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000285"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000286"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000287"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000288"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000289"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000290"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000291"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000292"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000293"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000294"),
                column: "HashPassword",
                value: "$2a$11$uVCxCh/Uq9tjf.2sunOkq.9DPvzOxTM54B0Eei7GUgIBxeHTGVTqO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CriteriaTemplates");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000011"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000012"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000013"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000014"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000015"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000016"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000017"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000018"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000019"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000020"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000021"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000022"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000023"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000024"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000025"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000026"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000027"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000028"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000029"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000030"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000031"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000032"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000033"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000034"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000035"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000036"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000037"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000038"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000039"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000040"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000041"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000c8"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000c9"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000ca"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000cb"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000cc"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000cd"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000ce"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000cf"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000d0"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000d1"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000d2"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000d3"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000d4"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000d5"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000d6"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000d7"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000d8"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000d9"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000da"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000db"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000dc"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000dd"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000de"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000df"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000e0"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000e1"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000e2"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000e3"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000e4"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000e5"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000f0"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000f1"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000f2"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000f3"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000f4"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000f5"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000f6"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000f7"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000f8"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000f9"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000fa"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000fb"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000fc"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000fd"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000fe"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-0000000000ff"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000100"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000101"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000102"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000103"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000104"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000105"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000106"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000107"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000108"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000109"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000010a"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000010b"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000010c"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000010d"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000280"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000281"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000282"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000283"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000284"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000285"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000286"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000287"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000288"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000289"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000290"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000291"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000292"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000293"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000294"),
                column: "HashPassword",
                value: "$2a$11$/cwAknWUu7V5K/QemrF4wOg4cgqdd8yb6KK3G8GNafymPKy7i3yC2");
        }
    }
}
