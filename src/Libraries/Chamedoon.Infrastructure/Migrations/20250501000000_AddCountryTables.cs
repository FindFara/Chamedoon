using Microsoft.EntityFrameworkCore.Migrations;
using Chamedoon.Infrastructure.Persistence.Seeds;

#nullable disable

namespace Chamedoon.Infrastructure.Migrations
{
    public partial class AddCountryTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    InvestmentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvestmentCurrency = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    InvestmentNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaritalStatusImpact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CountryLivingCosts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryLivingCosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountryLivingCosts_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountryRestrictions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryRestrictions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountryRestrictions_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Key",
                table: "Countries",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CountryLivingCosts_CountryId",
                table: "CountryLivingCosts",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryRestrictions_CountryId",
                table: "CountryRestrictions",
                column: "CountryId");

            var seed = CountrySeedData.Build();

            foreach (var country in seed.Countries)
            {
                migrationBuilder.InsertData(
                    table: "Countries",
                    columns: new[] { "Id", "AdditionalInfo", "InvestmentAmount", "InvestmentCurrency", "InvestmentNotes", "IsDeleted", "Key", "MaritalStatusImpact", "Name" },
                    values: new object[] { country.Id, country.AdditionalInfo, country.InvestmentAmount, country.InvestmentCurrency, country.InvestmentNotes, country.IsDeleted, country.Key, country.MaritalStatusImpact, country.Name });
            }

            foreach (var living in seed.LivingCosts)
            {
                migrationBuilder.InsertData(
                    table: "CountryLivingCosts",
                    columns: new[] { "Id", "CountryId", "IsDeleted", "Type", "Value" },
                    values: new object[] { living.Id, living.CountryId, living.IsDeleted, (int)living.Type, living.Value });
            }

            foreach (var restriction in seed.Restrictions)
            {
                migrationBuilder.InsertData(
                    table: "CountryRestrictions",
                    columns: new[] { "Id", "CountryId", "Description", "IsDeleted" },
                    values: new object[] { restriction.Id, restriction.CountryId, restriction.Description, restriction.IsDeleted });
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CountryLivingCosts");

            migrationBuilder.DropTable(
                name: "CountryRestrictions");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
