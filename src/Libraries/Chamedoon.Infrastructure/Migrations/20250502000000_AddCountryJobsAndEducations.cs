using Chamedoon.Infrastructure.Persistence.Seeds;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chamedoon.Infrastructure.Migrations
{
    public partial class AddCountryJobsAndEducations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CountryEducations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryId = table.Column<long>(type: "bigint", nullable: false),
                    FieldName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    LanguageRequirement = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryEducations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountryEducations_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountryJobs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    ExperienceImpact = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountryJobs_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CountryEducations_CountryId",
                table: "CountryEducations",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryJobs_CountryId",
                table: "CountryJobs",
                column: "CountryId");

            var seed = CountrySeedData.Build();

            foreach (var education in seed.Educations)
            {
                migrationBuilder.InsertData(
                    table: "CountryEducations",
                    columns: new[] { "Id", "CountryId", "Description", "FieldName", "IsDeleted", "LanguageRequirement", "Level", "Score" },
                    values: new object[] { education.Id, education.CountryId, education.Description, education.FieldName, education.IsDeleted, education.LanguageRequirement, education.Level, education.Score });
            }

            foreach (var job in seed.Jobs)
            {
                migrationBuilder.InsertData(
                    table: "CountryJobs",
                    columns: new[] { "Id", "CountryId", "Description", "ExperienceImpact", "IsDeleted", "Score", "Title" },
                    values: new object[] { job.Id, job.CountryId, job.Description, job.ExperienceImpact, job.IsDeleted, job.Score, job.Title });
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CountryEducations");

            migrationBuilder.DropTable(
                name: "CountryJobs");
        }
    }
}
