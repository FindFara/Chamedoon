using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Chamedoon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class migrationsname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Created", "Email", "EmailConfirmed", "LastModified", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { 1L, 0, "7C4049E0-28D8-4F73-8B3B-521C8AA86E01", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "fara@chamedoon.local", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, null, "FARA@CHAMEDOON.LOCAL", "FARA", "AQAAAAIAAYagAAAAENOL+hqKevXrCIxtNuT/pOk0vKIrqTX3JwxqjQOnZ1O4RUpUj70uXwG6mcdFB1tj3w==", "0000000000", false, "A7E549A7-3F8B-451E-91FA-1796BB7D35DD", false, "Fara" },
                    { 2L, 0, "8A1F656C-5E34-47A9-9F77-EDBE4F400C53", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "member@chamedoon.local", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, null, "MEMBER@CHAMEDOON.LOCAL", "MEMBERUSER", "AQAAAAIAAYagAAAAENOL+hqKevXrCIxtNuT/pOk0vKIrqTX3JwxqjQOnZ1O4RUpUj70uXwG6mcdFB1tj3w==", "0000000001", false, "B2C44D4B-6A05-4940-9F35-8840AD54ABCE", false, "MemberUser" }
                });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1L, 1L },
                    { 2L, 2L }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1L, 1L });

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2L, 2L });

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 2L,
                column: "NormalizedName",
                value: "Member");
        }
    }
}
