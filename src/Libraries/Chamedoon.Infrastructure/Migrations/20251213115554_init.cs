using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Chamedoon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Writer = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ArticleDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false),
                    ArticleImageName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VisitCount = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    InvestmentAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
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
                name: "DiscountCodes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(type: "bigint", nullable: true),
                    PermissionTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermissionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermission_RolePermission_ParentId",
                        column: x => x.ParentId,
                        principalTable: "RolePermission",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ArticleComment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ReaedAdmin = table.Column<bool>(type: "bit", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleComment_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Article",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleComment_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Job = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    SubscriptionPlanId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    SubscriptionStartDateUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubscriptionEndDateUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsedEvaluations = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_User_Id",
                        column: x => x.Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImmigrationEvaluations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    MaritalStatus = table.Column<int>(type: "int", nullable: false),
                    MBTIPersonality = table.Column<int>(type: "int", nullable: false),
                    InvestmentAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    JobCategory = table.Column<int>(type: "int", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    WorkExperienceYears = table.Column<int>(type: "int", nullable: false),
                    FieldCategory = table.Column<int>(type: "int", nullable: false),
                    DegreeLevel = table.Column<int>(type: "int", nullable: false),
                    LanguageCertificate = table.Column<int>(type: "int", nullable: false),
                    WillingToStudy = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImmigrationEvaluations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImmigrationEvaluations_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentRequests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    PlanId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    CallbackUrl = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    GatewayTrackId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ReferenceCode = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    DiscountCode = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    DiscountType = table.Column<int>(type: "int", nullable: true),
                    DiscountValue = table.Column<int>(type: "int", nullable: true),
                    DiscountAmount = table.Column<int>(type: "int", nullable: true),
                    FinalAmount = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PaymentUrl = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastError = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentRequests_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentResponses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentRequestId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ResultCode = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    RawPayload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GatewayTrackId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ReferenceId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CardNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentResponses_PaymentRequests_PaymentRequestId",
                        column: x => x.PaymentRequestId,
                        principalTable: "PaymentRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "AdditionalInfo", "InvestmentAmount", "InvestmentCurrency", "InvestmentNotes", "IsDeleted", "Key", "MaritalStatusImpact", "Name" },
                values: new object[,]
                {
                    { 1L, "طبق داده‌های رسمی بازار کار کانادا، نرخ بیکاری ۶٫۹ درصد است، جمعیت شاغل ۲۲٬۶۳۹٬۱۰۰ نفر و میانگین درآمد هفتگی ۱٬۳۱۲ دلار است؛ همچنین ۵۰۵٬۸۷۵ موقعیت شغلی خالی گزارش شده است【514275740625177†L129-L137】. این ارقام نشان می‌دهد بازار کار کانادا پویا و دارای فرصت‌های فراوان است و منابع رسمی به افراد برای برنامه‌ریزی شغلی کمک می‌کنند【514275740625177†L140-L155】. سیستم آموزش عالی کانادا از بهترین‌های جهان بوده و سرمایه‌گذاری زیادی در پژوهش و فناوری انجام می‌شود؛ اقتصاد کشور عمدتاً مبتنی بر خدمات و منابع طبیعی بوده و سرانهٔ تولید ناخالص داخلی بالاست.", 75000m, "دلار کانادا", "برای دریافت ویزای استارت‌آپ کانادا باید حداقل ۲۰۰٬۰۰۰ دلار کانادا از یک صندوق سرمایه‌گذاری خطرپذیر یا ۷۵٬۰۰۰ دلار کانادا از یک گروه سرمایه‌گذار فرشته جذب شود؛ در صورت پذیرش توسط انکوباتور تجاری، نیازی به سرمایه اولیه نیست.", false, "Canada", "در سیستم امتیازدهی اکسپرس انتری کانادا، مجردها امتیاز کامل دریافت می‌کنند و افراد متأهل به دلیل تقسیم امتیازات انسانی (سن، تحصیلات، زبان، سابقه کار) با همسر امتیاز کمتری می‌گیرند؛ با این حال امتیازات تحصیلات و زبان همسر می‌تواند اختلاف را جبران کند【791007699362788†L87-L106】.", "کانادا" },
                    { 2L, "در ژوئن ۲۰۲۵، نرخ بیکاری استرالیا ۴٫۲ درصد، نرخ اشتغال ۶۴٫۲ درصد و نرخ مشارکت نیروی کار ۶۷ درصد بود【786682023731967†L230-L237】. این نشان می‌دهد بازار کار استرالیا نسبتاً قوی است هرچند کمبود نیروی کار در حوزه‌های تکنسین و متخصص وجود دارد【101680514043225†L790-L801】. سیستم آموزشی استرالیا کیفیت بالایی دارد و دانشگاه‌های این کشور در رده‌بندی جهانی جایگاه خوبی دارند. اقتصاد کشور بر پایهٔ خدمات، معدن و کشاورزی است و سطح زندگی بالا است.", 2500000m, "دلار استرالیا", "برنامهٔ Investor stream (ساب کلاس ۱۸۸) در ۳۱ ژوئیهٔ ۲۰۲۴ بسته شد؛ پیش از آن متقاضیان باید حداقل ۲٫۵ میلیون دلار استرالیا سرمایه‌گذاری کرده و توسط یک ایالت یا قلمرو نامزد می‌شدند.", false, "Australia", "در ویزای مهارت استرالیا، مجرد بودن یا داشتن همسر شهروند یا دارندهٔ اقامت دائم ۱۰ امتیاز اضافی می‌دهد و اگر همسر دارای مهارت حرفه‌ای و زبان انگلیسی مناسب باشد نیز امتیاز کمتری (۵) داده می‌شود؛ در غیر این صورت امتیازی در این بخش دریافت نمی‌شود【809286149497200†L110-L147】.", "استرالیا" },
                    { 3L, "اقتصاد آلمان از پیشروترین اقتصادهای جهان است و صنایع خودرو، مهندسی و فناوری اطلاعات در آن نقش اساسی دارند. سایت «Make it in Germany» اعلام کرده است که شرکت‌های آلمانی به دلیل انقلاب صنعتی ۴٫۰ به دنبال متخصصان فناوری اطلاعات و مهندسان هستند و در این رشته‌ها کمبود نیروی متخصص وجود دارد【538120821009572†L329-L343】. همچنین در علوم و پژوهش نیز کمبود نیروی متخصص دیده می‌شود【538120821009572†L351-L353】. دانشگاه‌های آلمان استانداردهای آموزشی بالا دارند و بسیاری از دوره‌ها به زبان انگلیسی ارائه می‌شوند. نرخ بیکاری نسبتاً پایین و سطح دستمزدها بالا است.", 0m, "یورو", "برای دریافت اجازهٔ اقامت خوداشتغالی در آلمان، حداقل سرمایه مشخصی تعیین نشده است. متقاضی باید نشان دهد که طرح تجاری او برای اقتصاد منطقه مفید است و هزینه‌های طرح و زندگی خود را تامین کرده و دارای بیمه و بازنشستگی کافی است.", false, "Germany", "در برنامهٔ کارت فرصت آلمان، داشتن همسر که شرایط تحصیلی و زبانی را داشته باشد یک امتیاز اضافی می‌دهد و مجرد بودن موجب تقسیم امتیاز نمی‌شود؛ در سایر برنامه‌ها وضعیت تأهل نقش مستقیمی در امتیاز ندارد【270736592507908†L399-L411】.", "آلمان" },
                    { 4L, "طبق گزارش وضعیت اشتغال ماه سپتامبر ۲۰۲۵، نرخ بیکاری آمریکا ۴٫۴ درصد و تعداد افراد بیکار ۷٫۶ میلیون نفر بوده و میانگین دستمزد ساعتی ۳۶٫۶۷ دلار است【539452480876108†L327-L336】【539452480876108†L421-L427】. نرخ مشارکت نیروی کار در حدود ۶۲٫۴ درصد است و اقتصاد آمریکا با تولید ناخالص داخلی بالا به‌عنوان بزرگ‌ترین اقتصاد جهان شناخته می‌شود. دانشگاه‌های ایالات متحده در رتبه‌بندی جهانی جایگاه بالایی دارند و منابع مالی پژوهشی فراوانی در اختیار دارند.", 800000m, "دلار آمریکا", "برای دریافت ویزای EB‑5 باید حداقل ۱٬۰۵۰٬۰۰۰ دلار آمریکا در یک کسب‌وکار جدید سرمایه‌گذاری کرده و ده شغل تمام‌وقت ایجاد کنید؛ اگر سرمایه‌گذاری در منطقهٔ بیکاری بالا یا روستایی (TEA) انجام شود، حداقل سرمایه ۸۰۰٬۰۰۰ دلار است.", false, "USA", "مجرد یا متأهل بودن در سیستم‌های مهاجرتی آمریکا تأثیر مستقیمی بر امتیاز ندارد؛ تنها برای ویزاهای خانوادگی مانند ازدواج با شهروند یا مقیم دائم مزیت محسوب می‌شود.", "ایالات متحده آمریکا" },
                    { 5L, "طبق گزارش EURES، در هلند کمبود نیروی کار در حوزه‌های مدیریت و بازرگانی، ساختمان و رشته‌های علوم و مهندسی وجود دارد و بخش‌های بهداشت، آموزش، هنر و فناوری اطلاعات بیشترین نرخ خالی بودن شغل را دارند【394404756887990†L194-L222】. اقتصاد هلند به شدت باز و مبتنی بر تجارت و خدمات است و دانشگاه‌های هلند دوره‌های بسیاری را به زبان انگلیسی برگزار می‌کنند. نرخ بیکاری پایین و سطح رفاه اجتماعی بالا است.", 1250000m, "یورو", "برنامهٔ پذیرش سرمایه‌گذار خارجی هلند در ۱۷ آوریل ۲۰۲۴ لغو شد؛ پیش از آن متقاضیان باید حداقل ۱٫۲۵ میلیون یورو در یک شرکت نوآور هلندی سرمایه‌گذاری می‌کردند. در حال حاضر دریافت اقامت از طریق این برنامه ممکن نیست.", false, "Netherlands", "در سیستم مهاجرتی هلند، مجرد یا متأهل بودن امتیازی ایجاد نمی‌کند و تنها در ویزاهای پیوست همسر و خانواده اهمیت دارد.", "هلند" },
                    { 6L, "گزارش Cedefop نشان می‌دهد که اسپانیا با کمبود قابل توجهی در حوزهٔ متخصصان ICT و توسعه‌دهندگان نرم‌افزار مواجه است؛ دیجیتال‌سازی گسترده و تقاضای بالا علت این کمبود است【189557896059467†L342-L375】. همچنین تقاضا برای مهندسان و متخصصان تولید زیاد است و عرضهٔ نیروی کار در این رشته‌ها کافی نیست【189557896059467†L408-L433】. اقتصاد اسپانیا عمدتاً بر خدمات و گردشگری استوار است و نرخ بیکاری به‌ویژه در بین جوانان بالاست. دانشگاه‌های اسپانیا در رشته‌های علوم انسانی و هنر شناخته شده‌اند ولی بودجهٔ پژوهشی نسبت به برخی کشورهای اروپایی کمتر است.", 500000m, "یورو", "بر اساس قانون ۱۴/۲۰۱۳، سرمایه‌گذاران می‌توانستند با ۲ میلیون یورو سرمایه‌گذاری در اوراق قرضه دولتی، ۱ میلیون یورو در سهام یا صندوق‌ها، یا خرید ملک به ارزش حداقل ۵۰۰ هزار یورو، ویزای طلایی اسپانیا دریافت کنند. این برنامه قرار است در سال ۲۰۲۵ خاتمه یابد.", false, "Spain", "اسپانیا سیستم امتیازدهی برای مهاجرت ندارد و مجرد یا متأهل بودن در امتیاز نقشی ندارد؛ تنها در ویزاهای پیوست خانواده مؤثر است.", "اسپانیا" },
                    { 7L, "آمارهای رسمی نشان می‌دهند که در سپتامبر ۲۰۲۵ نرخ بیکاری سوئد ۸٫۳ درصد (۸٫۷ درصد با تعدیل فصلی) و نرخ اشتغال ۶۸٫۹ درصد و نرخ مشارکت نیروی کار ۷۵٫۵ درصد بوده است【32727063083057†L36-L55】【32727063083057†L52-L57】. این نشان‌دهندهٔ بازاری کار نسبتاً پایدار اما با بیکاری بالاتر از میانگین برخی کشورهای اروپایی است. سوئد با کمبود نیرو در حوزه‌های پزشکی، مهندسی و فناوری مواجه است و برای این مشاغل فهرست کمبود وجود دارد. سیستم آموزشی رایگان و با کیفیت بالا بوده و اقتصاد بر پایهٔ فناوری، خدمات و صنایع صادراتی مانند فناوری اطلاعات، خودرو و دارو استوار است.", 200000m, "کرون سوئد", "برای ویزای خوداشتغالی سوئد باید نشان دهید حداقل ۲۰۰٬۰۰۰ کرون برای خود، ۱۰۰٬۰۰۰ کرون برای همسر و ۵۰٬۰۰۰ کرون برای هر فرزند برای دو سال اول زندگی در اختیار دارید و همچنین سرمایه لازم برای خرید یا راه‌اندازی کسب‌وکار را فراهم کنید.", false, "Sweden", "در برنامه‌های مهاجرتی سوئد، مجرد یا متأهل بودن امتیاز جداگانه‌ای ایجاد نمی‌کند؛ تنها برای پیوست همسر و فرزندان اهمیت دارد.", "سوئد" },
                    { 8L, "گزارش Cedefop نشان می‌دهد که ایتالیا در مشاغل فناوری اطلاعات، توسعه‌دهندگان نرم‌افزار، ریاضیدانان و مهندسان الکترونیک با کمبود شدید نیرو مواجه است، زیرا تولید فارغ‌التحصیلان کافی نیست و تقاضا بالاست【982764113348400†L342-L378】. همچنین در حوزه‌های STEM و سلامت کمبود نیرو وجود دارد و تقاضا برای کارکنان سلامت هر سال حدود ۲ درصد افزایش می‌یابد【982764113348400†L408-L452】. اقتصاد ایتالیا بر پایهٔ تولید صنعتی و گردشگری استوار است و نرخ بیکاری جوانان نسبتاً بالاست. دانشگاه‌های این کشور به‌ویژه در رشته‌های مهندسی، طراحی و هنر شهرت دارند.", 250000m, "یورو", "برای دریافت ویزای سرمایه‌گذاری ایتالیا باید یکی از گزینه‌های زیر را انتخاب کنید: ۲ میلیون یورو سرمایه‌گذاری در اوراق قرضه دولتی، ۵۰۰ هزار یورو در یک شرکت ایتالیایی، ۲۵۰ هزار یورو در یک استارتاپ نوآور یا ۱ میلیون یورو کمک به یک پروژهٔ خیریه.", false, "Italy", "در برنامه‌های مهاجرتی ایتالیا وضعیت تأهل تأثیر مستقیمی بر امتیاز ندارد و صرفاً در ویزاهای پیوست خانواده و الحاق همسر اهمیت پیدا می‌کند.", "ایتالیا" },
                    { 9L, "عمان در چارچوب چشم‌انداز ۲۰۴۰ به‌دنبال تنوع‌بخشی به اقتصاد و آموزش مهارت‌های دیجیتال است و دولت برای توسعهٔ زیرساخت‌ها و صنایع غیرنفتی سرمایه‌گذاری می‌کند؛ گزارش‌ها نشان می‌دهد رشد اشتغال در سال ۲۰۲۴ حدود ۱۰٫۶ درصد بوده و تمرکز بر آموزش نیروهای عمانی در صنایع نوظهور است【107658387508375†L67-L100】. اقتصاد عمان همچنان به نفت وابسته است اما پروژه‌های بزرگ صنعتی و گردشگری در حال توسعه‌اند. نظام آموزشی در حال ارتقا است هرچند بسیاری از متخصصان برای تحصیلات عالی به خارج می‌روند. سیاست عمانیزه کردن سبب محدودیت برای استخدام خارجی‌ها شده است.", 200000m, "ریال عمانی", "برای دریافت ویزای سرمایه‌گذاری ۵ یا ۱۰ ساله عمان، متقاضی باید از طریق وزارت تجارت درخواست دهد و هزینهٔ صدور به ترتیب ۲۵۰ و ۵۰۰ ریال عمانی است. برای اقامت طلایی، سرمایه‌گذاری حداقل ۲۰۰٬۰۰۰ ریال عمانی از طریق خرید ملک، اوراق قرضه یا ایجاد اشتغال برای ۵۰ نفر لازم است.", false, "Oman", "در برنامهٔ اقامت سرمایه‌گذاری عمان، مجرد یا متأهل بودن امتیاز جداگانه‌ای ایجاد نمی‌کند؛ همسر و فرزندان می‌توانند به عنوان همراه درخواست دهند، اما امتیاز خاصی اضافه نمی‌شود.", "عمان" },
                    { 10L, "بازار کار هند به‌سرعت در حوزه‌های فناوری، هوش مصنوعی و یادگیری ماشین رشد می‌کند و براساس گزارش مجمع جهانی اقتصاد، دو سوم شرکت‌ها قصد دارند نیروی متخصص فناوری را افزایش دهند؛ اما شکاف مهارتی و نیاز به بازآموزی وجود دارد【565810706922104†L20-L77】. اقتصاد هند با رشد ۶ تا ۷ درصدی یکی از سریع‌ترین اقتصادهای جهان است، ولی بیکاری جوانان و نابرابری درآمدی همچنان چالش‌برانگیز است. نظام آموزشی بسیار وسیع بوده و دانشگاه‌های معتبر بسیاری در حوزه‌های مهندسی و فناوری وجود دارد، هرچند کیفیت در میان مؤسسات متفاوت است.", 100000000m, "روپیه هند", "برای دریافت وضعیت اقامت دائم (Permanent Residency Status) باید حداقل ۱۰ کرور روپیه ظرف ۱۸ ماه یا ۲۵ کرور روپیه ظرف ۳۶ ماه سرمایه‌گذاری کنید و حداقل برای ۲۰ نفر کار ایجاد کنید.", false, "India", "وضعیت تأهل در فرآیند مهاجرت به هند نقش مستقیمی ندارد؛ افراد متأهل می‌توانند همسر و فرزندان خود را به‌عنوان وابسته همراه داشته باشند و برایشان ویزای همراه صادر می‌شود. مهم‌ترین شرط، احراز شرایط سرمایه‌گذاری و سایر الزامات قانونی است.", "هند" }
                });

            migrationBuilder.InsertData(
                table: "CountryEducations",
                columns: new[] { "Id", "CountryId", "Description", "FieldName", "IsDeleted", "LanguageRequirement", "Level", "Score" },
                values: new object[,]
                {
                    { 20001L, 1L, "دانشجویان خارجی برای تحصیل در رشتهٔ مهندسی نرم‌افزار در کانادا باید نامهٔ پذیرش، پاسپورت معتبر و تمکن مالی ارائه کنند. پس از پایان تحصیل می‌توانند با ویزای کار پس از تحصیل (PGWP) در کانادا بمانند【665772236957633†L63-L90】.", "مهندسی نرم‌افزار", false, "نمرهٔ آیلتس حداقل ۶٫۵ یا معادل آن در تافل برای دوره‌های انگلیسی کافی است.", "کارشناسی", 90 },
                    { 20002L, 1L, "رشتهٔ مهندسی برق و الکترونیک در دانشگاه‌های کانادا محبوب است. دانشجویان از آزمایشگاه‌های مدرن و ارتباط با صنایع بهره‌مند می‌شوند و می‌توانند پس از تحصیل در پروژه‌های انرژی و مخابرات مشغول شوند.", "مهندسی برق و الکترونیک", false, "برای دوره‌های انگلیسی آیلتس ۶٫۵ و برای دوره‌های فرانسوی نمرهٔ معادل در TEF ضروری است.", "کارشناسی", 80 },
                    { 20003L, 1L, "رشتهٔ مهندسی مکانیک به دانشجویان مبانی طراحی و تولید را آموزش می‌دهد. فارغ‌التحصیلان می‌توانند در صنایع خودروسازی، هوافضا و تولید کار یابند.", "مهندسی مکانیک", false, "آیلتس ۶٫۵ و ارائهٔ رزومهٔ تحصیلی برای پذیرش لازم است.", "کارشناسی", 80 },
                    { 20004L, 1L, "مهندسی عمران در کانادا به دلیل پروژه‌های زیرساختی و شهری اهمیت دارد. دانشگاه‌ها دوره‌های عملی و کارآموزی ارائه می‌دهند تا دانشجویان برای بازار کار آماده شوند.", "مهندسی عمران", false, "آیلتس ۶٫۵ و ارزیابی مدارک تحصیلی پیشین لازم است.", "کارشناسی", 80 },
                    { 20005L, 1L, "علوم داده و یادگیری ماشین از رشته‌های نوظهور در کانادا هستند. دانشگاه‌ها برنامه‌های پژوهشی قوی دارند و فارغ‌التحصیلان در شرکت‌های فناوری و بخش دولتی جذب می‌شوند.", "علوم داده", false, "نمرهٔ آیلتس ۷ برای دوره‌های تحصیلات تکمیلی توصیه می‌شود.", "کارشناسی ارشد", 90 },
                    { 20006L, 1L, "برنامه‌های تحلیل و مدیریت کسب‌وکار به دانشجویان کمک می‌کند تا تصمیم‌گیری داده‌محور و مدیریت عملیات را یاد بگیرند. فارغ‌التحصیلان در شرکت‌های مشاوره و مالی استخدام می‌شوند.", "تحلیل و مدیریت کسب و کار", false, "آیلتس ۶٫۵ و ارائهٔ نتایج GMAT یا GRE برای برخی دانشگاه‌ها لازم است.", "کارشناسی ارشد", 80 },
                    { 20007L, 1L, "دوره‌های مدیریت پروژه بر برنامه‌ریزی، زمان‌بندی و بودجه‌بندی پروژه‌ها تمرکز دارد و دانشجویان را برای دریافت گواهینامه‌های PMP آماده می‌کند.", "مدیریت پروژه", false, "نمرهٔ آیلتس ۶٫۵ و تجربهٔ کاری مرتبط برای پذیرش ترجیح داده می‌شود.", "کارشناسی ارشد", 70 },
                    { 20008L, 1L, "رشتهٔ حسابداری و مالی در کانادا دانشجویان را برای اخذ مدارک CPA و کار در بخش مالی آماده می‌کند. بازار کار برای فارغ‌التحصیلان این حوزه مناسب است.", "حسابداری و مالی", false, "آیلتس ۶٫۵ و تسلط بر ریاضیات مالی ضروری است.", "کارشناسی", 70 },
                    { 20009L, 1L, "مدیریت منابع انسانی به دانشجویان مهارت‌های استخدام، آموزش و توسعهٔ کارکنان را آموزش می‌دهد. این رشته در سازمان‌های دولتی و خصوصی کاربرد دارد.", "مدیریت منابع انسانی", false, "آیلتس ۶٫۵ و مهارت‌های ارتباطی قوی مورد نیاز است.", "کارشناسی", 70 },
                    { 20010L, 1L, "رشتهٔ بازاریابی و دیجیتال مارکتینگ دانشجویان را با استراتژی‌های بازاریابی آنلاین، تحقیق بازار و تبلیغات دیجیتال آشنا می‌کند.", "بازاریابی و دیجیتال مارکتینگ", false, "آیلتس ۶٫۵ و آشنایی با رسانه‌های اجتماعی ضروری است.", "کارشناسی", 70 },
                    { 20011L, 1L, "کانادا به پرستاران و متخصصان سلامت نیاز دارد و فارغ‌التحصیلان این رشته‌ها به‌سادگی می‌توانند پس از تحصیل در بازار کار جذب شوند【164992063620704†L102-L116】.", "علوم بهداشتی و پرستاری", false, "نمرهٔ بالاتر آیلتس (۷ یا بالاتر) و در برخی استان‌ها مهارت فرانسوی لازم است.", "کارشناسی", 90 },
                    { 20012L, 1L, "رشتهٔ آموزش (تدریس) برای تربیت معلمان مدارس و دوره‌های متوسطه طراحی شده است. دانشجویان دروس روان‌شناسی آموزشی و روش‌های تدریس را می‌آموزند.", "آموزش", false, "آیلتس ۶٫۵ و گذراندن دوره‌های عملی تدریس الزامی است.", "کارشناسی", 70 },
                    { 20013L, 1L, "علوم پایه و پژوهش شامل فیزیک، شیمی و زیست‌شناسی است. دانشگاه‌های کانادا فرصت‌های تحقیقاتی فراوانی دارند و دانشجویان می‌توانند در مقاطع تحصیلات تکمیلی ادامه دهند.", "علوم پایه و پژوهش", false, "آیلتس ۷ و ارائهٔ پروپوزال تحقیقاتی قوی برای پذیرش دکترا مورد نیاز است.", "کارشناسی ارشد", 80 },
                    { 20014L, 1L, "رشتهٔ هنر و طراحی شامل گرافیک، هنرهای زیبا و طراحی صنعتی است. دانشگاه‌های کانادا امکانات کارگاهی و نمایشگاهی فراهم می‌کنند.", "هنر و طراحی", false, "نمونه کار هنری و نمرهٔ آیلتس ۶ برای پذیرش معمولاً لازم است.", "کارشناسی", 60 },
                    { 20015L, 1L, "رشتهٔ گردشگری و مهمان‌داری به دانشجویان مهارت‌های مدیریت هتل، پذیرایی و گردشگری را می‌آموزد. با توسعهٔ صنعت گردشگری، فرصت‌های شغلی وجود دارد.", "گردشگری و مهمان‌داری", false, "آیلتس ۶ و مهارت‌های ارتباطی با مشتری ضروری است.", "کارشناسی", 60 },
                    { 40001L, 2L, "رشتهٔ مهندسی نرم‌افزار و فناوری اطلاعات در استرالیا یکی از رشته‌های پرتقاضا است. بر اساس گزارش Jobs and Skills Australia، کمبود نیروی کار در حوزه‌های تکنسین و متخصص از جمله مهندسی و فناوری محسوس است【101680514043225†L790-L801】.", "مهندسی نرم‌افزار", false, "برای بسیاری از برنامه‌ها حداقل نمرهٔ آیلتس ۶٫۵ یا معادل آن در تافل مورد نیاز است.", "کارشناسی", 90 },
                    { 40002L, 2L, "دانشگاه‌های استرالیا دوره‌های تخصصی برق و الکترونیک ارائه می‌دهند و دانشجویان با شرکت در پروژه‌های صنعت انرژی و مخابرات، تجربهٔ عملی کسب می‌کنند.", "مهندسی برق و الکترونیک", false, "آیلتس ۶٫۵ و داشتن رزومهٔ تحصیلی قوی برای پذیرش نیاز است.", "کارشناسی", 80 },
                    { 40003L, 2L, "رشتهٔ مهندسی مکانیک در استرالیا بر طراحی و تولید ماشین‌آلات تمرکز دارد. فارغ‌التحصیلان می‌توانند در صنایع معدنی، خودروسازی و هوافضا به کار مشغول شوند.", "مهندسی مکانیک", false, "نمرهٔ آیلتس ۶٫۵ و ارائهٔ مدارک تحصیلی معتبر لازم است.", "کارشناسی", 80 },
                    { 40004L, 2L, "استرالیا به دلیل پروژه‌های زیرساختی بزرگ به مهندسان عمران نیاز دارد. دانشگاه‌ها دوره‌های ترکیبی از تئوری و کارآموزی ارائه می‌کنند.", "مهندسی عمران", false, "نمرهٔ آیلتس ۶٫۵ و در بعضی دانشگاه‌ها ارائهٔ نمونهٔ پروژهٔ دانشجویی لازم است.", "کارشناسی", 80 },
                    { 40005L, 2L, "علوم داده و تحلیل پیشرفته در استرالیا رو به رشد است و دانشگاه‌ها برنامه‌های قوی در یادگیری ماشین و هوش مصنوعی ارائه می‌دهند.", "علوم داده", false, "برای دوره‌های تحصیلات تکمیلی، آیلتس ۷ و نمرهٔ GRE برای برخی دانشگاه‌ها لازم است.", "کارشناسی ارشد", 90 },
                    { 40006L, 2L, "برنامه‌های تحلیل و مدیریت کسب‌وکار در استرالیا دانشجویان را برای تحلیل داده‌ها و تصمیم‌گیری استراتژیک در سازمان‌ها آماده می‌کند.", "تحلیل و مدیریت کسب و کار", false, "نمرهٔ GMAT یا GRE به همراه آیلتس ۶٫۵ برای پذیرش MBA مورد نیاز است.", "کارشناسی ارشد", 80 },
                    { 40007L, 2L, "دوره‌های مدیریت پروژه در استرالیا با تمرکز بر روش‌های چابک و استانداردهای جهانی، دانشجویان را برای مدیریت پروژه‌های بزرگ آماده می‌کند.", "مدیریت پروژه", false, "آیلتس ۶٫۵ و تجربهٔ کاری برای بسیاری از دوره‌های کارشناسی ارشد لازم است.", "کارشناسی ارشد", 70 },
                    { 40008L, 2L, "رشتهٔ حسابداری و مالی در استرالیا دانشجویان را برای دریافت مدارک حرفه‌ای (CPA, CA) و کار در بانک‌ها و شرکت‌های حسابرسی آماده می‌کند.", "حسابداری و مالی", false, "نمرهٔ آیلتس ۶٫۵ و تسلط بر مفاهیم حسابداری مالی لازم است.", "کارشناسی", 70 },
                    { 40009L, 2L, "رشتهٔ مدیریت منابع انسانی شامل جذب، آموزش و نگهداشت کارکنان است و در سازمان‌های دولتی و خصوصی مورد نیاز است.", "مدیریت منابع انسانی", false, "آیلتس ۶٫۵ و مهارت‌های ارتباطی قوی برای پذیرش ضروری است.", "کارشناسی", 70 },
                    { 40010L, 2L, "رشتهٔ بازاریابی و دیجیتال مارکتینگ در استرالیا روی استراتژی‌های تبلیغات آنلاین، رفتار مصرف‌کننده و مدیریت برند تمرکز دارد.", "بازاریابی و دیجیتال مارکتینگ", false, "آیلتس ۶٫۵ و تجربهٔ پروژه‌های عملی برای پذیرش مفید است.", "کارشناسی", 70 },
                    { 40011L, 2L, "استرالیا به پرستاران و متخصصان سلامت نیاز دارد و فارغ‌التحصیلان این رشته‌ها فرصت‌های شغلی مناسبی دارند. کیفیت آموزش و امکانات بهداشتی بالا است.", "علوم بهداشتی و پرستاری", false, "مدرک آیلتس حداقل ۷ و ثبت‌نام در AHPRA برای پرستاران الزامی است.", "کارشناسی", 80 },
                    { 40012L, 2L, "رشتهٔ آموزش برای تربیت معلمان دوره‌های ابتدایی و متوسطه طراحی شده است و در برخی مناطق استرالیا کمبود معلم وجود دارد.", "آموزش", false, "آیلتس ۷ و گذراندن دورهٔ عملی تدریس برای پذیرش لازم است.", "کارشناسی", 70 },
                    { 40013L, 2L, "علوم پایه شامل فیزیک، شیمی و زیست‌شناسی است. دانشگاه‌های استرالیا فرصت‌های تحقیقاتی و فاندینگ برای دانشجویان فراهم می‌کنند.", "علوم پایه و پژوهش", false, "برای پذیرش در مقطع دکترا، ارائهٔ پروپوزال تحقیقاتی و آیلتس ۷ لازم است.", "کارشناسی ارشد", 80 },
                    { 40014L, 2L, "دوره‌های هنر و طراحی شامل گرافیک، مد و طراحی صنعتی است و دانشگاه‌های استرالیا امکانات کارگاهی و نمایشگاهی فراهم می‌کنند.", "هنر و طراحی", false, "ارسال پرتفولیوی هنری و آیلتس ۶٫۵ برای پذیرش ضروری است.", "کارشناسی", 60 },
                    { 40015L, 2L, "رشتهٔ گردشگری و مهمان‌داری در استرالیا مهارت‌های مدیریت هتل، رستوران و برنامه‌ریزی رویدادها را به دانشجویان آموزش می‌دهد. این صنعت در شهرهای توریستی رشد یافته است.", "گردشگری و مهمان‌داری", false, "نمرهٔ آیلتس ۶٫۵ و مهارت‌های ارتباطی برای پذیرش این رشته لازم است.", "کارشناسی", 60 },
                    { 60001L, 3L, "بیشتر برنامه‌های مهندسی نرم‌افزار در آلمان به زبان آلمانی ارائه می‌شود و دانشجویان باید مهارت زبان خود را از طریق آزمون‌های TestDaF یا DSH اثبات کنند؛ برخی برنامه‌های بین‌المللی به زبان انگلیسی نیز موجود است【212200104484712†L278-L306】.", "مهندسی نرم‌افزار", false, "آزمون TestDaF یا DSH برای دوره‌های آلمانی و آیلتس/تافل برای دوره‌های انگلیسی لازم است.", "کارشناسی", 90 },
                    { 60002L, 3L, "دانشگاه‌های آلمان دوره‌های مهندسی برق و الکترونیک ارائه می‌دهند که روی Industry 4.0 و سیستم‌های هوشمند تمرکز دارند.", "مهندسی برق و الکترونیک", false, "مدرک زبان آلمانی سطح B2 یا بالاتر برای اکثر برنامه‌ها لازم است.", "کارشناسی", 90 },
                    { 60003L, 3L, "مهندسی مکانیک در آلمان به دلیل صنایع خودروسازی و تولیدی بسیار محبوب است و فرصت‌های کارآموزی در شرکت‌های بزرگ وجود دارد.", "مهندسی مکانیک", false, "زبان آلمانی سطح B2 و در برخی دانشگاه‌ها سطح C1 مورد نیاز است.", "کارشناسی", 90 },
                    { 60004L, 3L, "رشتهٔ مهندسی عمران در آلمان برای توسعهٔ زیرساخت‌ها و پروژه‌های ساخت‌وساز اهمیت دارد و دانشگاه‌ها دوره‌های عملی گسترده ارائه می‌کنند.", "مهندسی عمران", false, "مدرک زبان آلمانی سطح B2 و گذراندن دورهٔ مقدماتی برای دانشجویان بین‌المللی ضروری است.", "کارشناسی", 80 },
                    { 60005L, 3L, "علوم داده و تحلیل داده در آلمان به دلیل رشد اقتصاد دیجیتال و صنعت ۴.۰ پرطرفدار است و دانشگاه‌ها برنامه‌های پیشرفته‌ای ارائه می‌دهند【538120821009572†L329-L343】.", "علوم داده", false, "برای دوره‌های انگلیسی آیلتس ۶٫۵ یا تافل ۸۰ و برای دوره‌های آلمانی TestDaF لازم است.", "کارشناسی ارشد", 90 },
                    { 60006L, 3L, "رشتهٔ تحلیل و مدیریت کسب‌وکار در آلمان روی استراتژی‌های مدیریتی و تحلیل داده تمرکز دارد و فارغ‌التحصیلان در شرکت‌های صنعتی و خدماتی استخدام می‌شوند.", "تحلیل و مدیریت کسب و کار", false, "آیلتس ۶٫۵ یا TestDaF و در برخی موارد نیاز به نمرهٔ GMAT برای دوره‌های MBA.", "کارشناسی ارشد", 80 },
                    { 60007L, 3L, "دوره‌های مدیریت پروژه در آلمان با تمرکز بر استانداردهای بین‌المللی به دانشجویان کمک می‌کند تا پروژه‌های صنعتی و فناوری را به‌طور موثر مدیریت کنند.", "مدیریت پروژه", false, "زبان آلمانی یا انگلیسی و تجربهٔ کاری مرتبط در برخی برنامه‌ها لازم است.", "کارشناسی ارشد", 80 },
                    { 60008L, 3L, "رشتهٔ حسابداری و مالی در آلمان به دانشجویان مفاهیم مالیاتی و حسابرسی آلمانی را آموزش می‌دهد و آن‌ها را برای دریافت مدارک حرفه‌ای آماده می‌کند.", "حسابداری و مالی", false, "سطح B2 زبان آلمانی و در برخی برنامه‌ها آیلتس ۶٫۵ برای دوره‌های انگلیسی.", "کارشناسی", 80 },
                    { 60009L, 3L, "مدیریت منابع انسانی در آلمان شامل استخدام، توسعه و روابط کار است و دانشجویان با قوانین کار آلمان آشنا می‌شوند.", "مدیریت منابع انسانی", false, "مدرک زبان آلمانی و مهارت‌های ارتباطی برای پذیرش لازم است.", "کارشناسی", 70 },
                    { 60010L, 3L, "رشتهٔ بازاریابی و دیجیتال مارکتینگ در آلمان روی تبلیغات آنلاین، تحلیل بازار و مدیریت برند تمرکز دارد؛ بازار کار برای متخصصان دیجیتال مارکتینگ در حال گسترش است.", "بازاریابی و دیجیتال مارکتینگ", false, "آیلتس ۶٫۵ یا TestDaF و آشنایی با ابزارهای دیجیتال برای پذیرش لازم است.", "کارشناسی", 70 },
                    { 60011L, 3L, "آلمان با کمبود پرستاران روبه‌روست و برنامه‌های پرستاری و بهداشت فرصت‌های شغلی زیادی برای فارغ‌التحصیلان دارد. دانشجویان باید زبان آلمانی را برای ارتباط با بیماران بیاموزند.", "علوم بهداشتی و پرستاری", false, "مدرک TestDaF یا DSH و سطح B2 آلمانی برای پذیرش الزامی است.", "کارشناسی", 80 },
                    { 60012L, 3L, "رشتهٔ آموزش معلمان آینده را برای تدریس در مدارس آلمان تربیت می‌کند؛ تقاضا برای معلمان علوم و زبان بالاست.", "آموزش", false, "زبان آلمانی سطح C1 و گذراندن دوره‌های تربیت معلم مورد نیاز است.", "کارشناسی", 70 },
                    { 60013L, 3L, "علوم پایه شامل فیزیک، شیمی و زیست‌شناسی است و آلمان مؤسسات تحقیقاتی برجسته‌ای دارد که فرصت‌های پژوهشی فراوانی ارائه می‌دهند【538120821009572†L351-L353】.", "علوم پایه و پژوهش", false, "برای پذیرش در دوره‌های تحصیلات تکمیلی، ارائهٔ پروپوزال تحقیقاتی و مدرک زبان معتبر لازم است.", "کارشناسی ارشد", 90 },
                    { 60014L, 3L, "رشتهٔ هنر و طراحی در آلمان شامل گرافیک، معماری داخلی و طراحی صنعتی است و دانشجویان از سنت هنری و امکانات مدرن بهره‌مند می‌شوند.", "هنر و طراحی", false, "نمونهٔ کار هنری و مدرک زبان B2 آلمانی یا آیلتس ۶٫۵ لازم است.", "کارشناسی", 70 },
                    { 60015L, 3L, "گردشگری و مهمان‌داری در آلمان شامل مدیریت هتل‌ها، رستوران‌ها و رویدادهای فرهنگی است؛ اما نسبت به رشته‌های مهندسی امتیاز کمتری دارد.", "گردشگری و مهمان‌داری", false, "زبان آلمانی و انگلیسی و مهارت‌های بین‌فرهنگی برای پذیرش لازم است.", "کارشناسی", 60 },
                    { 80001L, 4L, "دانشگاه‌های آمریکا در رشته‌های مهندسی نرم‌افزار و علوم رایانه شهرت جهانی دارند. برای اخذ ویزای دانشجویی (F‑1) باید نامهٔ پذیرش، اثبات تمکن مالی و مدرک زبان انگلیسی ارائه شود؛ همچنین برای برخی برنامه‌های مهاجرتی مانند لاتاری، داشتن مدرک دبیرستان یا سابقهٔ کار الزامی است【551578366031283†L74-L88】.", "مهندسی نرم‌افزار", false, "حداقل نمرهٔ TOEFL iBT ۸۰ یا IELTS ۶٫۵ برای دوره‌های کارشناسی لازم است.", "کارشناسی", 90 },
                    { 80002L, 4L, "رشتهٔ مهندسی برق و الکترونیک در آمریکا شامل دروس الکترونیک، سیستم‌های قدرت و مخابرات است. فارغ‌التحصیلان می‌توانند در شرکت‌های فناوری، انرژی و خودرو مشغول شوند.", "مهندسی برق و الکترونیک", false, "نمرهٔ TOEFL iBT بالای ۸۰ یا IELTS ۶٫۵ و ارائهٔ مدارک تحصیلی پیشین لازم است.", "کارشناسی", 80 },
                    { 80003L, 4L, "مهندسی مکانیک در آمریکا بر طراحی، تحلیل و ساخت قطعات و سیستم‌ها تمرکز دارد. دانشگاه‌ها به دانشجویان فرصت‌های تحقیقاتی و کارآموزی ارائه می‌دهند.", "مهندسی مکانیک", false, "TOEFL iBT ۸۰ یا IELTS ۶٫۵ برای پذیرش کافی است.", "کارشناسی", 80 },
                    { 80004L, 4L, "مهندسی عمران در آمریکا به دلیل پروژه‌های زیرساختی عظیم اهمیت دارد. دانشجویان در زمینه‌های طراحی سازه، راه‌سازی و مدیریت ساخت آموزش می‌بینند.", "مهندسی عمران", false, "نمرهٔ زبان مشابه سایر رشته‌های مهندسی و ارائهٔ مدارک علمی مورد نیاز است.", "کارشناسی", 80 },
                    { 80005L, 4L, "علوم داده و یادگیری ماشین از رشته‌های پررونق در آمریکا هستند. دانشگاه‌ها دوره‌های پیشرفته‌ای در تحلیل داده و هوش مصنوعی ارائه می‌کنند و فارغ‌التحصیلان در شرکت‌های فناوری جذب می‌شوند.", "علوم داده", false, "برای دوره‌های تحصیلات تکمیلی معمولاً نمرهٔ GRE و TOEFL iBT بالای ۹۰ توصیه می‌شود.", "کارشناسی ارشد", 90 },
                    { 80006L, 4L, "رشتهٔ تحلیل و مدیریت کسب‌وکار به دانشجویان ابزارهای تحلیل داده، مدیریت مالی و استراتژی سازمانی را آموزش می‌دهد؛ فارغ‌التحصیلان می‌توانند در شرکت‌های مشاوره و مالی فعالیت کنند.", "تحلیل و مدیریت کسب و کار", false, "نمرهٔ GMAT یا GRE و مدرک زبان انگلیسی برای پذیرش برنامه‌های MBA لازم است.", "کارشناسی ارشد", 80 },
                    { 80007L, 4L, "دوره‌های مدیریت پروژه در آمریکا بر برنامه‌ریزی، بودجه‌بندی و رهبری پروژه‌ها تمرکز دارد و دانشجویان را برای گواهی‌های حرفه‌ای آماده می‌کند.", "مدیریت پروژه", false, "آیلتس ۶٫۵ یا TOEFL iBT ۸۰ و تجربهٔ کاری مرتبط برای پذیرش مزیت محسوب می‌شود.", "کارشناسی ارشد", 80 },
                    { 80008L, 4L, "برنامه‌های حسابداری و مالی در آمریکا دانشجویان را برای اخذ مدارک حرفه‌ای (CPA, CFA) و کار در بانک‌ها و شرکت‌های مالی آماده می‌کنند.", "حسابداری و مالی", false, "TOEFL iBT ۸۰ یا IELTS ۶٫۵ و تسلط بر ریاضیات مالی ضروری است.", "کارشناسی", 70 },
                    { 80009L, 4L, "رشتهٔ مدیریت منابع انسانی مهارت‌های استخدام، ارزیابی عملکرد و توسعهٔ کارکنان را آموزش می‌دهد. فارغ‌التحصیلان در سازمان‌های دولتی و خصوصی به کار گرفته می‌شوند.", "مدیریت منابع انسانی", false, "TOEFL iBT ۷۹ یا IELTS ۶٫۵ به همراه مهارت‌های ارتباطی نیاز است.", "کارشناسی", 70 },
                    { 80010L, 4L, "برنامه‌های بازاریابی و دیجیتال مارکتینگ در آمریکا تکنیک‌های تبلیغات آنلاین، تحلیل بازار و مدیریت برند را آموزش می‌دهند.", "بازاریابی و دیجیتال مارکتینگ", false, "TOEFL iBT ۷۹ یا IELTS ۶٫۵ و تجربهٔ کارآموزی در حوزهٔ بازاریابی مفید است.", "کارشناسی", 80 },
                    { 80011L, 4L, "دانشکده‌های پرستاری آمریکا از کیفیت بالایی برخوردارند و به دلیل کمبود نیروی انسانی در بخش سلامت، فارغ‌التحصیلان این رشته‌ها می‌توانند پس از تحصیل از طریق دورهٔ کار عملی (OPT) به کار در بیمارستان‌ها و مراکز درمانی مشغول شوند.", "علوم بهداشتی و پرستاری", false, "مدرک IELTS یا TOEFL و پس از پایان دوره، شرکت در آزمون NCLEX برای اخذ مجوز پرستاری لازم است.", "کارشناسی", 80 },
                    { 80012L, 4L, "رشتهٔ آموزش در آمریکا معلمان آینده را برای تدریس در مدارس ابتدایی و متوسطه آماده می‌کند. دوره‌ها شامل روان‌شناسی آموزشی و مدیریت کلاس است.", "آموزش", false, "آیلتس ۶٫۵ یا TOEFL ۸۰ و گذراندن دورهٔ کارآموزی تدریس ضروری است.", "کارشناسی", 70 },
                    { 80013L, 4L, "علوم پایه شامل فیزیک، شیمی و زیست‌شناسی است. دانشگاه‌های آمریکا فرصت‌های تحقیقاتی و فاندینگ مناسب ارائه می‌کنند و فارغ‌التحصیلان می‌توانند در مقاطع تحصیلات تکمیلی ادامه دهند.", "علوم پایه و پژوهش", false, "نمرهٔ GRE و TOEFL بالا به‌علاوهٔ رزومهٔ پژوهشی قوی برای پذیرش دکترا توصیه می‌شود.", "کارشناسی ارشد", 80 },
                    { 80014L, 4L, "دوره‌های هنر و طراحی در آمریکا شامل گرافیک، فیلم‌سازی و طراحی صنعتی است. دانشجویان از امکانات کارگاهی و شبکه‌های حرفه‌ای بهره‌مند می‌شوند.", "هنر و طراحی", false, "ارائهٔ نمونهٔ آثار هنری و نمرهٔ زبان مورد قبول برای پذیرش ضروری است.", "کارشناسی", 70 },
                    { 80015L, 4L, "رشتهٔ گردشگری و مهمان‌داری در آمریکا مهارت‌های مدیریت هتل، سرویس‌دهی و برنامه‌ریزی رویدادها را آموزش می‌دهد. این صنعت در شهرهای گردشگری رونق دارد.", "گردشگری و مهمان‌داری", false, "آیلتس ۶٫۵ یا TOEFL iBT ۷۹ و مهارت‌های بین‌فرهنگی لازم است.", "کارشناسی", 60 },
                    { 100001L, 5L, "با توجه به کمبود نیروی ICT در هلند، رشته مهندسی نرم‌افزار و علوم کامپیوتر پرطرفدار است【394404756887990†L194-L222】.", "مهندسی نرم‌افزار", false, "بسیاری از برنامه‌های کارشناسی به زبان انگلیسی و هلندی ارائه می‌شوند و نیازمند مدرک زبان C1 (آیلتس ۷ یا تافل ۱۰۰) هستند【226732374735180†L61-L100】【226732374735180†L126-L128】.", "کارشناسی", 90 },
                    { 100002L, 5L, "رشته مهندسی برق و الکترونیک در صنایع انرژی و فناوری هلند اهمیت دارد؛ دانشجویان با سیستم‌های قدرت و الکترونیک صنعتی آشنا می‌شوند.", "مهندسی برق و الکترونیک", false, "نیاز به مدرک زبان انگلیسی C1 و در برخی برنامه‌ها زبان هلندی سطح B2.", "کارشناسی", 80 },
                    { 100003L, 5L, "مهندسی مکانیک در صنایع تولیدی و ماشین‌سازی هلند از جمله کشتی‌سازی و تجهیزات صنعتی کاربرد دارد.", "مهندسی مکانیک", false, "مدرک زبان انگلیسی C1 و گاهی مدرک زبان هلندی برای دوره‌های مشترک.", "کارشناسی", 80 },
                    { 100004L, 5L, "رشته مهندسی عمران در هلند بر مهندسی سازه، راه‌سازی و مدیریت آب تمرکز دارد؛ کشور به مهندسان عمران برای پروژه‌های زیربنایی نیاز دارد【394404756887990†L194-L222】.", "مهندسی عمران", false, "مدرک زبان انگلیسی یا هلندی (سطح B2/C1) و در برخی موارد آزمون ریاضی ورودی.", "کارشناسی", 80 },
                    { 100005L, 5L, "علوم داده و هوش مصنوعی در دانشگاه‌های هلند بسیار رو به رشد است و برای تحلیل داده‌های بزرگ در صنایع مالی و فناوری کاربرد دارد.", "علوم داده", false, "مدرک زبان انگلیسی C1، آشنایی با برنامه‌نویسی و گاهی آزمون GRE یا GMAT.", "کارشناسی ارشد", 80 },
                    { 100006L, 5L, "این رشته به دانشجویان روش‌های تحلیل داده، مدیریت فرآیندها و تصمیم‌گیری استراتژیک می‌آموزد و تقاضای بالایی در شرکت‌های هلندی دارد.", "تحلیل و مدیریت کسب و کار", false, "مدرک زبان انگلیسی C1 و در برخی برنامه‌ها نمرهٔ GMAT.", "کارشناسی ارشد", 70 },
                    { 100007L, 5L, "دوره‌های مدیریت پروژه اصول برنامه‌ریزی، کنترل و اجرای پروژه‌های بزرگ را ارائه می‌دهند؛ این رشته برای پروژه‌های مهندسی و فناوری کاربرد دارد.", "مدیریت پروژه", false, "مدرک زبان انگلیسی و تجربه کاری مرتبط برای برخی دانشگاه‌ها الزامی است.", "کارشناسی ارشد", 70 },
                    { 100008L, 5L, "رشته حسابداری و مالی در هلند دانشجویان را با قوانین مالیاتی اروپا و گزارش‌گری بین‌المللی آشنا می‌کند و برای کار در شرکت‌های چندملیتی مناسب است【394404756887990†L194-L222】.", "حسابداری و مالی", false, "مدرک زبان انگلیسی C1 و در برخی برنامه‌ها آشنایی با زبان هلندی سطح B2.", "کارشناسی", 70 },
                    { 100009L, 5L, "مدیریت منابع انسانی در هلند بر جذب و توسعهٔ کارکنان، روابط کاری و قوانین کار تمرکز دارد؛ بازار کار این رشته متوسط است.", "مدیریت منابع انسانی", false, "مدرک زبان انگلیسی و گاهی مدرک زبان هلندی برای کارآموزی.", "کارشناسی", 70 },
                    { 100010L, 5L, "رشته بازاریابی و دیجیتال مارکتینگ دانشجویان را برای تبلیغات آنلاین و مدیریت برند آماده می‌کند؛ به ویژه در صنایع گردشگری و تجارت الکترونیک کاربرد دارد.", "بازاریابی و دیجیتال مارکتینگ", false, "مدرک زبان انگلیسی و آشنایی با زبان هلندی در سطح B2.", "کارشناسی", 70 },
                    { 100011L, 5L, "رشته‌های علوم سلامت و پرستاری در هلند به دلیل کمبود کارکنان بخش سلامت اهمیت دارند و فارغ‌التحصیلان در بیمارستان‌ها و خانه‌های سالمندان به کار گرفته می‌شوند【394404756887990†L194-L222】.", "علوم بهداشتی و پرستاری", false, "مدرک زبان هلندی در سطح B2/C1 و قبولی در آزمون‌های ورودی پرستاری.", "کارشناسی", 80 },
                    { 100012L, 5L, "برنامه‌های آموزش معلم در هلند معلمان را برای تدریس در مدارس ابتدایی و متوسطه آماده می‌کنند؛ نرخ خالی مشاغل آموزش بالا است【394404756887990†L194-L222】.", "آموزش", false, "مدرک زبان هلندی سطح C1 و شرکت در دوره‌های تربیت معلم.", "کارشناسی", 80 },
                    { 100013L, 5L, "علوم پایه و پژوهش شامل فیزیک، شیمی و زیست‌شناسی است؛ دانشگاه‌های هلند پروژه‌های تحقیقاتی با بودجه اتحادیه اروپا ارائه می‌دهند.", "علوم پایه و پژوهش", false, "مدرک زبان انگلیسی C1 و در صورت نیاز، ارائهٔ پیشنهاد پژوهشی.", "کارشناسی ارشد", 70 },
                    { 100014L, 5L, "رشته هنر و طراحی در هلند شامل معماری، طراحی گرافیک و مد است و دانشجویان از محیط خلاق و فرهنگی بهره‌مند می‌شوند.", "هنر و طراحی", false, "نمونهٔ کار هنری و مدرک زبان انگلیسی یا هلندی (سطح B2) مورد نیاز است.", "کارشناسی", 60 },
                    { 100015L, 5L, "هلند به عنوان مقصد گردشگری و مرکز کنفرانس‌های بین‌المللی، دوره‌های گردشگری و مهمان‌داری برای مدیریت هتل، رویداد و خدمات گردشگری ارائه می‌دهد.", "گردشگری و مهمان‌داری", false, "مدرک زبان انگلیسی و هلندی در سطح B2 و مهارت‌های ارتباطی لازم است.", "کارشناسی", 60 },
                    { 120001L, 6L, "طبق گزارش Cedefop، اسپانیا با کمبود نیرو در حوزه‌های ICT و توسعه‌دهندگان نرم‌افزار روبه‌روست و دانشگاه‌ها برنامه‌های مهندسی نرم‌افزار را به زبان اسپانیایی و انگلیسی ارائه می‌دهند【189557896059467†L342-L375】.", "مهندسی نرم‌افزار", false, "مدرک زبان DELE سطح B2 برای دوره‌های اسپانیایی و آیلتس ۶٫۵ یا تافل ۸۰ برای دوره‌های انگلیسی لازم است.", "کارشناسی", 80 },
                    { 120002L, 6L, "دوره‌های مهندسی برق و الکترونیک در اسپانیا به دانشجویان آموزش سیستم‌های قدرت و مخابرات می‌دهد و فرصت‌های شغلی در شرکت‌های انرژی فراهم می‌کند.", "مهندسی برق و الکترونیک", false, "زبان اسپانیایی یا انگلیسی با سطح B2 و بالاتر مورد نیاز است.", "کارشناسی", 70 },
                    { 120003L, 6L, "رشتهٔ مهندسی مکانیک در اسپانیا در صنایع خودروسازی و تولید اهمیت دارد؛ دانشگاه‌ها دوره‌های عملی و کارآموزی ارائه می‌کنند.", "مهندسی مکانیک", false, "مدرک زبان B2 اسپانیایی یا آیلتس ۶ برای پذیرش لازم است.", "کارشناسی", 70 },
                    { 120004L, 6L, "به دلیل پروژه‌های ساخت‌وساز و احیای زیرساخت‌ها، اسپانیا به مهندسان عمران نیاز دارد【189557896059467†L408-L433】.", "مهندسی عمران", false, "مدرک زبان اسپانیایی سطح B2 و قبولی در آزمون ورودی دانشگاه ضروری است.", "کارشناسی", 70 },
                    { 120005L, 6L, "علوم داده در اسپانیا در حال رشد است و دانشگاه‌ها برنامه‌های مرتبط با تحلیل داده و هوش مصنوعی ارائه می‌دهند.", "علوم داده", false, "برای پذیرش در مقاطع تکمیلی، آیلتس ۶٫۵ یا تافل ۸۰ و در برخی دانشگاه‌ها GRE لازم است.", "کارشناسی ارشد", 70 },
                    { 120006L, 6L, "رشتهٔ تحلیل و مدیریت کسب‌وکار در اسپانیا به دانشجویان کمک می‌کند داده‌ها را تحلیل و تصمیمات استراتژیک بگیرند؛ بازار کار در حال توسعه است.", "تحلیل و مدیریت کسب و کار", false, "مدرک زبان و در برخی برنامه‌ها نمرهٔ GMAT برای دوره‌های MBA لازم است.", "کارشناسی ارشد", 70 },
                    { 120007L, 6L, "دوره‌های مدیریت پروژه در اسپانیا اصول برنامه‌ریزی و کنترل پروژه را آموزش می‌دهند و فارغ‌التحصیلان در شرکت‌های ساختمانی و فناوری به کار گرفته می‌شوند.", "مدیریت پروژه", false, "مدرک زبان و تجربهٔ کاری در برخی برنامه‌ها الزامی است.", "کارشناسی ارشد", 70 },
                    { 120008L, 6L, "رشتهٔ حسابداری و مالی دانشجویان را با قوانین مالیاتی اسپانیا و اتحادیه اروپا آشنا می‌کند و برای کار در شرکت‌های داخلی و بین‌المللی آماده می‌سازد.", "حسابداری و مالی", false, "مدرک زبان اسپانیایی سطح B2 و در برخی برنامه‌ها انگلیسی لازم است.", "کارشناسی", 70 },
                    { 120009L, 6L, "مدیریت منابع انسانی در اسپانیا بر استخدام، توسعه و روابط صنعتی تمرکز دارد و فارغ‌التحصیلان می‌توانند در شرکت‌های مختلف کار کنند.", "مدیریت منابع انسانی", false, "مدرک زبان اسپانیایی و مهارت‌های ارتباطی برای پذیرش ضروری است.", "کارشناسی", 70 },
                    { 120010L, 6L, "رشتهٔ بازاریابی و دیجیتال مارکتینگ در اسپانیا شامل تبلیغات آنلاین و مدیریت برند است و به دلیل رشد گردشگری و تجارت الکترونیک اهمیت دارد.", "بازاریابی و دیجیتال مارکتینگ", false, "مدرک زبان B2 اسپانیایی یا آیلتس ۶٫۵ و تجربهٔ کارآموزی مفید است.", "کارشناسی", 70 },
                    { 120011L, 6L, "کمبود پرستاران و کارکنان سلامت در اسپانیا موجب شده است رشتهٔ پرستاری و علوم بهداشت فرصت‌های شغلی خوبی داشته باشد. دانشجویان باید آزمون ورودی و مدرک زبان اسپانیایی داشته باشند.", "علوم بهداشتی و پرستاری", false, "مدرک زبان اسپانیایی سطح B2 یا بالاتر و قبولی در آزمون ورودی لازم است.", "کارشناسی", 80 },
                    { 120012L, 6L, "رشتهٔ آموزش برای تربیت معلمان در مدارس اسپانیا طراحی شده است؛ کمبود معلم در برخی مناطق به خصوص زبان‌های خارجی وجود دارد.", "آموزش", false, "مدرک زبان اسپانیایی سطح C1 و گذراندن دوره‌های تربیت معلم لازم است.", "کارشناسی", 70 },
                    { 120013L, 6L, "علوم پایه شامل رشته‌های فیزیک، شیمی و زیست‌شناسی است. دانشگاه‌های اسپانیا فرصت‌های تحقیقاتی خوبی ارائه می‌دهند، هرچند بودجه‌ها محدودتر از بعضی کشورهاست.", "علوم پایه و پژوهش", false, "برای تحصیلات تکمیلی، آیلتس ۶٫۵ یا معادل آن و ارائهٔ پروپوزال تحقیقاتی لازم است.", "کارشناسی ارشد", 70 },
                    { 120014L, 6L, "رشتهٔ هنر و طراحی در اسپانیا شامل معماری، گرافیک و طراحی مد است و دانشجویان از میراث هنری غنی این کشور بهره‌مند می‌شوند.", "هنر و طراحی", false, "نمونهٔ کار هنری و مدرک زبان اسپانیایی یا انگلیسی لازم است.", "کارشناسی", 70 },
                    { 120015L, 6L, "اسپانیا یکی از مقاصد گردشگری محبوب است و رشتهٔ گردشگری و مهمان‌داری دانشجویان را برای مدیریت هتل‌ها و برنامه‌ریزی رویدادها آماده می‌کند.", "گردشگری و مهمان‌داری", false, "مدرک زبان اسپانیایی و انگلیسی و مهارت‌های ارتباطی برای پذیرش لازم است.", "کارشناسی", 60 },
                    { 140001L, 7L, "دانشگاه‌های سوئد دوره‌های مهندسی نرم‌افزار و علوم رایانه را ارائه می‌کنند. این رشته‌ها به دلیل کمبود برنامه‌نویسان و متخصصان IT بسیار پرطرفدار هستند【707620709545585†L39-L83】.", "مهندسی نرم‌افزار", false, "نیاز به مدرک زبان انگلیسی معادل English 6 یا آیلتس ۶٫۵ و در برخی موارد زبان سوئدی سطح B2 است【687246862216066†screenshot】.", "کارشناسی", 90 },
                    { 140002L, 7L, "رشته مهندسی برق در سوئد در صنایع انرژی و مخابرات اهمیت دارد. دانشگاه‌ها تحقیق و پروژه‌های عملی فراهم می‌کنند.", "مهندسی برق و الکترونیک", false, "مدرک زبان انگلیسی و گاهی سوئدی در سطح B2 برای پذیرش لازم است.", "کارشناسی", 80 },
                    { 140003L, 7L, "مهندسی مکانیک در صنایع خودروسازی و فناوری پیشرفته سوئد مانند ولوو و اسکانیا جایگاه ویژه‌ای دارد.", "مهندسی مکانیک", false, "مدرک زبان انگلیسی و سوئدی سطح B2 برای بسیاری از برنامه‌ها نیاز است.", "کارشناسی", 70 },
                    { 140004L, 7L, "رشته مهندسی عمران با تمرکز بر ساخت‌وساز پایدار، توسعه شهری و مدیریت آب در سوئد تدریس می‌شود.", "مهندسی عمران", false, "برای تحصیل در این رشته مدرک زبان سوئدی یا انگلیسی در سطح B2/C1 ضروری است.", "کارشناسی", 80 },
                    { 140005L, 7L, "به دلیل گسترش هوش مصنوعی و تحلیل داده، رشته علوم داده در سوئد رو به رشد است و دانشگاه‌ها دوره‌های متعدد ارائه می‌دهند.", "علوم داده", false, "نیاز به مدرک زبان انگلیسی (آیلتس ۶٫۵) و در برخی برنامه‌ها تجربه برنامه‌نویسی.", "کارشناسی ارشد", 80 },
                    { 140006L, 7L, "رشته تحلیل و مدیریت کسب‌وکار به دانشجویان ابزارهای تحلیل داده و مدیریت پروژه را آموزش می‌دهد و بازار کار مناسبی دارد.", "تحلیل و مدیریت کسب و کار", false, "مدرک زبان انگلیسی و در برخی دوره‌ها حداقل سابقه کاری الزامی است.", "کارشناسی ارشد", 70 },
                    { 140007L, 7L, "دوره‌های مدیریت پروژه در سوئد بر برنامه‌ریزی، کنترل و اجرای پروژه‌های فناوری و عمرانی تمرکز دارند.", "مدیریت پروژه", false, "مدرک زبان انگلیسی و تجربه کاری مرتبط در برخی دانشگاه‌ها.", "کارشناسی ارشد", 70 },
                    { 140008L, 7L, "رشته حسابداری و مالی دانشجویان را با قوانین مالیاتی سوئد و اتحادیه اروپا آشنا می‌کند و برای کار در شرکت‌های بین‌المللی آماده می‌سازد.", "حسابداری و مالی", false, "مدرک زبان انگلیسی و گاهی سوئدی سطح B2 لازم است.", "کارشناسی", 70 },
                    { 140009L, 7L, "در رشته مدیریت منابع انسانی، دانشجویان با مدیریت کارکنان، قوانین کار و توسعه سازمانی آشنا می‌شوند.", "مدیریت منابع انسانی", false, "مدرک زبان انگلیسی و مهارت‌های ارتباطی برای پذیرش ضروری است.", "کارشناسی", 60 },
                    { 140010L, 7L, "این رشته برای آموزش تکنیک‌های تبلیغات آنلاین، برندینگ و بازاریابی بین‌المللی طراحی شده است.", "بازاریابی و دیجیتال مارکتینگ", false, "مدرک زبان انگلیسی و در برخی موارد سوئدی (سطح B2) لازم است.", "کارشناسی", 70 },
                    { 140011L, 7L, "به دلیل کمبود پرستار، دانشگاه‌های سوئد دوره‌های بهداشت و پرستاری ارائه می‌دهند؛ فارغ‌التحصیلان سریعاً جذب بازار کار می‌شوند【707620709545585†L39-L83】.", "علوم بهداشتی و پرستاری", false, "مدرک زبان سوئدی سطح B2/C1 و قبولی در آزمون‌های ورودی پرستاری.", "کارشناسی", 90 },
                    { 140012L, 7L, "رشته آموزش معلمان را برای تدریس در مدارس ابتدایی و متوسطه آماده می‌کند و کمبود معلم در برخی مناطق وجود دارد【707620709545585†L39-L83】.", "آموزش", false, "مدرک زبان سوئدی سطح C1 و گذراندن دوره‌های تربیت معلم.", "کارشناسی", 80 },
                    { 140013L, 7L, "علوم پایه و پژوهش شامل فیزیک، شیمی و زیست‌شناسی است و دانشگاه‌های سوئد پروژه‌های پژوهشی بین‌المللی ارائه می‌دهند.", "علوم پایه و پژوهش", false, "مدرک زبان انگلیسی و ارائهٔ پیشنهاد تحقیقاتی برای پذیرش در مقاطع تکمیلی.", "کارشناسی ارشد", 80 },
                    { 140014L, 7L, "رشته هنر و طراحی در سوئد شامل معماری، طراحی صنعتی و مد است و دانشجویان از نوآوری‌های اسکاندیناوی بهره‌مند می‌شوند.", "هنر و طراحی", false, "نمونهٔ کار هنری و مدرک زبان انگلیسی یا سوئدی (B2) مورد نیاز است.", "کارشناسی", 60 },
                    { 140015L, 7L, "با رشد صنعت گردشگری و هتل‌داری، رشته گردشگری و مهمان‌داری دانشجویان را برای مدیریت مهمان‌پذیری، هتل و خدمات گردشگری آماده می‌کند.", "گردشگری و مهمان‌داری", false, "مدرک زبان انگلیسی یا سوئدی و مهارت‌های ارتباطی برای پذیرش لازم است.", "کارشناسی", 60 },
                    { 160001L, 8L, "طبق گزارش Cedefop، ایتالیا با کمبود فارغ‌التحصیلان ICT و STEM مواجه است و در رشته‌های مهندسی نرم‌افزار و علوم رایانه تقاضای زیادی وجود دارد【982764113348400†L342-L378】.", "مهندسی نرم‌افزار", false, "برای دوره‌های ایتالیایی سطح B2 زبان ایتالیایی و برای دوره‌های انگلیسی حداقل آیلتس ۶ لازم است.", "کارشناسی", 80 },
                    { 160002L, 8L, "دانشگاه‌های ایتالیا مانند پلی‌تکنیک تورین دوره‌های مهندسی برق و الکترونیک ارائه می‌دهند و دانشجویان می‌توانند در صنایع انرژی و الکترونیک مشغول شوند.", "مهندسی برق و الکترونیک", false, "سطح B2 ایتالیایی یا آیلتس ۶٫۵ برای دوره‌های انگلیسی لازم است.", "کارشناسی", 80 },
                    { 160003L, 8L, "مهندسی مکانیک در ایتالیا به دلیل صنایع خودروسازی و ماشین‌سازی اهمیت دارد. دانشجویان دروس طراحی و تولید را فرا می‌گیرند.", "مهندسی مکانیک", false, "مدرک زبان ایتالیایی یا انگلیسی و ارائهٔ مدارک تحصیلی مورد نیاز است.", "کارشناسی", 80 },
                    { 160004L, 8L, "رشتهٔ مهندسی عمران برای مرمت بناهای تاریخی و پروژه‌های زیربنایی در ایتالیا مهم است و دانشگاه‌ها برنامه‌های ترکیبی ارائه می‌دهند.", "مهندسی عمران", false, "مدرک B2 ایتالیایی یا آیلتس ۶٫۵ بسته به زبان دوره لازم است.", "کارشناسی", 70 },
                    { 160005L, 8L, "علوم داده در ایتالیا در حال رشد است؛ اما نسبت به کشورهای پیشرفته کوچک‌تر است. این رشته امکان کار در شرکت‌های فناوری و بانک‌ها را فراهم می‌کند.", "علوم داده", false, "برای پذیرش در مقاطع تحصیلات تکمیلی آیلتس ۶٫۵ یا معادل آن و sometimes GRE لازم است.", "کارشناسی ارشد", 80 },
                    { 160006L, 8L, "برنامه‌های تحلیل و مدیریت کسب‌وکار در ایتالیا دانشجویان را برای بهینه‌سازی فرایندها در شرکت‌ها و بانک‌ها آماده می‌کند؛ بازار کار نسبتاً محدود است.", "تحلیل و مدیریت کسب و کار", false, "آیلتس ۶ یا مدرک معادل به همراه GMAT برای دوره‌های MBA لازم است.", "کارشناسی ارشد", 70 },
                    { 160007L, 8L, "دوره‌های مدیریت پروژه در ایتالیا به اصول برنامه‌ریزی و کنترل پروژه می‌پردازند؛ این رشته در صنایع ساختمانی و فناوری کاربرد دارد.", "مدیریت پروژه", false, "مدرک زبان و سابقهٔ کاری برای برخی برنامه‌ها لازم است.", "کارشناسی ارشد", 70 },
                    { 160008L, 8L, "رشتهٔ حسابداری و مالی در ایتالیا دانشجویان را با قوانین مالیاتی و حسابرسی ایتالیایی آشنا می‌کند و امکان کار در شرکت‌ها و بانک‌ها را فراهم می‌کند.", "حسابداری و مالی", false, "سطح B2 ایتالیایی برای دوره‌های محلی و آیلتس ۶ برای دوره‌های انگلیسی مورد نیاز است.", "کارشناسی", 70 },
                    { 160009L, 8L, "مدیریت منابع انسانی در ایتالیا روی استخدام، آموزش و روابط کار تمرکز دارد. این رشته در سازمان‌های کوچک و بزرگ کاربرد دارد.", "مدیریت منابع انسانی", false, "مدرک زبان و مهارت‌های ارتباطی برای پذیرش لازم است.", "کارشناسی", 70 },
                    { 160010L, 8L, "به دلیل اهمیت صنعت مد و گردشگری، بازاریابی و دیجیتال مارکتینگ در ایتالیا جایگاه ویژه‌ای دارد. دانشجویان با استراتژی‌های تبلیغات و برندینگ آشنا می‌شوند.", "بازاریابی و دیجیتال مارکتینگ", false, "آیلتس ۶ و آشنایی با زبان ایتالیایی مزیت محسوب می‌شود.", "کارشناسی", 70 },
                    { 160011L, 8L, "بخش سلامت ایتالیا با کمبود پرستاران مواجه است و برنامه‌های پرستاری و بهداشت فرصت‌های شغلی خوبی ارائه می‌دهند【982764113348400†L408-L452】.", "علوم بهداشتی و پرستاری", false, "آزمون ورودی، مدرک زبان ایتالیایی سطح B2 و ثبت‌نام در نظام پرستاری الزامی است.", "کارشناسی", 80 },
                    { 160012L, 8L, "رشتهٔ آموزش در ایتالیا معلمان را برای تدریس در مدارس ابتدایی و متوسطه تربیت می‌کند؛ تقاضا برای معلمان در برخی مناطق پایدار است.", "آموزش", false, "داشتن مدرک زبان ایتالیایی و گذراندن دوره‌های تربیت معلم ضروری است.", "کارشناسی", 70 },
                    { 160013L, 8L, "علوم پایه و پژوهش در ایتالیا شامل فیزیک، شیمی و زیست‌شناسی است. دانشگاه‌ها فرصت‌های تحقیقاتی در مقاطع تکمیلی دارند.", "علوم پایه و پژوهش", false, "آیلتس ۶٫۵ و ارائهٔ پروپوزال تحقیقاتی برای پذیرش دکترا لازم است.", "کارشناسی ارشد", 70 },
                    { 160014L, 8L, "ایتالیا مرکز هنر، مد و طراحی است و دانشگاه‌ها برنامه‌های غنی در زمینهٔ طراحی صنعتی، مد و هنرهای زیبا ارائه می‌دهند.", "هنر و طراحی", false, "نمونه کار هنری و مهارت زبان برای پذیرش الزامی است.", "کارشناسی", 80 },
                    { 160015L, 8L, "گردشگری و مهمان‌داری از صنایع اصلی ایتالیا است و دانشجویان این رشته مهارت‌های مدیریت هتل، رستوران و راهنمایی گردشگران را فرا می‌گیرند.", "گردشگری و مهمان‌داری", false, "دانش زبان ایتالیایی و انگلیسی و مهارت در ارتباط با مشتری برای پذیرش لازم است.", "کارشناسی", 70 },
                    { 180001L, 9L, "رشته مهندسی نرم‌افزار در عمان در حال رشد است و دانشگاه‌ها دوره‌های مبتنی بر مهارت‌های برنامه‌نویسی و توسعه سیستم را ارائه می‌دهند؛ دولت برای آموزش نیروی بومی در حوزه دیجیتال سرمایه‌گذاری کرده است【107658387508375†L67-L100】.", "مهندسی نرم‌افزار", false, "مدرک زبان انگلیسی (آیلتس یا تافل) یا عربی بسته به زبان تدریس لازم است.", "کارشناسی", 60 },
                    { 180002L, 9L, "دانشگاه‌های عمان دوره‌هایی در مهندسی برق و الکترونیک با تمرکز بر سیستم‌های قدرت و ارتباطات ارائه می‌دهند.", "مهندسی برق و الکترونیک", false, "تسلط به زبان انگلیسی یا عربی و داشتن مدرک پیش‌دانشگاهی مرتبط برای پذیرش لازم است.", "کارشناسی", 60 },
                    { 180003L, 9L, "رشته مهندسی مکانیک در عمان به ویژه در صنایع نفت، گاز و تولید کاربرد دارد و فرصت‌های شغلی در پروژه‌های صنعتی فراهم می‌کند.", "مهندسی مکانیک", false, "مدرک زبان انگلیسی یا عربی و گذراندن واحدهای پایهٔ ریاضیات و فیزیک الزامی است.", "کارشناسی", 60 },
                    { 180004L, 9L, "با توسعهٔ زیرساخت‌ها و پروژه‌های مسکن، مهندسی عمران در عمان یکی از رشته‌های پرتقاضا است.", "مهندسی عمران", false, "مدرک زبان انگلیسی یا عربی و شرط معدل برای پذیرش لازم است.", "کارشناسی", 70 },
                    { 180005L, 9L, "رشته علوم داده هنوز نوپا است؛ دانشگاه‌ها در تلاشند برنامه‌های مرتبط با تحلیل داده و هوش مصنوعی راه‌اندازی کنند تا نیاز بازار را پاسخ دهند.", "علوم داده", false, "مدرک زبان انگلیسی و سابقه تحصیلی قوی در علوم رایانه برای پذیرش الزامی است.", "کارشناسی ارشد", 60 },
                    { 180006L, 9L, "دانشگاه‌های عمان رشته تحلیل و مدیریت کسب‌وکار را با تأکید بر مهارت‌های مدیریتی و تحلیل داده ارائه می‌دهند؛ این رشته برای توسعه کسب‌وکارهای داخلی اهمیت دارد.", "تحلیل و مدیریت کسب و کار", false, "مدرک زبان انگلیسی و گاهی عربی و داشتن سابقه کاری یا دورهٔ کارآموزی الزامی است.", "کارشناسی ارشد", 60 },
                    { 180007L, 9L, "رشته مدیریت پروژه دانشجویان را برای برنامه‌ریزی و اجرای پروژه‌های عمرانی و صنعتی در عمان آماده می‌کند.", "مدیریت پروژه", false, "مدرک زبان انگلیسی و تجربه کاری مرتبط برای بسیاری از برنامه‌ها لازم است.", "کارشناسی ارشد", 60 },
                    { 180008L, 9L, "رشته حسابداری و مالی به آموزش قوانین مالیاتی و استانداردهای حسابداری در عمان و منطقه می‌پردازد.", "حسابداری و مالی", false, "مدرک زبان انگلیسی و عربی و در برخی دانشگاه‌ها آزمون ورودی لازم است.", "کارشناسی", 60 },
                    { 180009L, 9L, "این رشته دانشجویان را برای مدیریت کارکنان و توسعه سرمایه انسانی در شرکت‌های عمانی آماده می‌کند.", "مدیریت منابع انسانی", false, "مدرک زبان انگلیسی یا عربی و داشتن مهارت‌های ارتباطی برای پذیرش ضروری است.", "کارشناسی", 60 },
                    { 180010L, 9L, "رشد تدریجی تجارت الکترونیک و گردشگری در عمان باعث شده رشته بازاریابی و دیجیتال مارکتینگ اهمیت بیشتری پیدا کند.", "بازاریابی و دیجیتال مارکتینگ", false, "مدرک زبان انگلیسی یا عربی و آشنایی با ابزارهای بازاریابی دیجیتال لازم است.", "کارشناسی", 60 },
                    { 180011L, 9L, "به دلیل توسعه بخش سلامت، رشته‌های پرستاری و علوم بهداشتی در عمان آینده کاری خوبی دارند؛ اما ظرفیت دانشگاه‌ها محدود است.", "علوم بهداشتی و پرستاری", false, "مدرک زبان عربی و انگلیسی و قبولی در آزمون‌های ورودی پرستاری لازم است.", "کارشناسی", 70 },
                    { 180012L, 9L, "رشته آموزش معلمان را برای مدارس ابتدایی و متوسطه عمان آماده می‌کند؛ دولت به ارتقای نظام آموزشی توجه دارد.", "آموزش", false, "مدرک زبان عربی یا انگلیسی و داشتن مدرک دیپلم یا معادل آن برای پذیرش ضروری است.", "کارشناسی", 60 },
                    { 180013L, 9L, "رشته‌های علوم پایه مانند فیزیک و شیمی در دانشگاه‌های عمان کمتر توسعه یافته‌اند، اما همکاری با دانشگاه‌های خارجی افزایش یافته است.", "علوم پایه و پژوهش", false, "مدرک زبان انگلیسی و ارائهٔ پیشنهاد تحقیقاتی برای تحصیلات تکمیلی لازم است.", "کارشناسی ارشد", 60 },
                    { 180014L, 9L, "رشته هنر و طراحی در عمان شامل طراحی داخلی، مد و هنرهای سنتی است؛ بازار کار محدود اما در حال رشد است.", "هنر و طراحی", false, "نمونهٔ کار هنری و مدرک زبان انگلیسی یا عربی برای پذیرش لازم است.", "کارشناسی", 50 },
                    { 180015L, 9L, "با توجه به رشد صنعت گردشگری و پروژه‌های توسعه‌ای، رشته گردشگری و مهمان‌داری در عمان فرصت‌های شغلی مناسبی فراهم می‌کند.", "گردشگری و مهمان‌داری", false, "مدرک زبان انگلیسی یا عربی و مهارت‌های خدمات مشتری برای پذیرش لازم است.", "کارشناسی", 70 },
                    { 200001L, 10L, "رشته مهندسی نرم‌افزار و علوم کامپیوتر در هند از پرطرفدارترین رشته‌هاست و فارغ‌التحصیلان آن در شرکت‌های فناوری و استارتاپ‌ها استخدام می‌شوند【565810706922104†L20-L77】.", "مهندسی نرم‌افزار", false, "مدرک زبان انگلیسی (آیلتس یا تافل) و گاهی شرکت در آزمون‌های سراسری مانند JEE برای پذیرش لازم است.", "کارشناسی", 80 },
                    { 200002L, 10L, "رشته مهندسی برق و الکترونیک در هند در صنایع برق، مخابرات و انرژی‌های نو کاربرد دارد؛ دانشگاه‌ها دوره‌های تئوری و عملی ارائه می‌دهند.", "مهندسی برق و الکترونیک", false, "مدرک زبان انگلیسی و شرکت در آزمون ورودی مانند JEE یا GATE بسته به مقطع.", "کارشناسی", 70 },
                    { 200003L, 10L, "مهندسی مکانیک از رشته‌های قدیمی و معتبر هند است و در صنایع خودروسازی، هوافضا و تولید نقش مهمی دارد.", "مهندسی مکانیک", false, "مدرک زبان انگلیسی و شرکت در آزمون‌های ورودی مانند JEE برای کارشناسی الزامی است.", "کارشناسی", 70 },
                    { 200004L, 10L, "رشته مهندسی عمران به دلیل پروژه‌های زیرساختی گسترده در هند تقاضای بالایی دارد و دانشگاه‌ها دوره‌های پیشرفته ارائه می‌دهند.", "مهندسی عمران", false, "مدرک زبان انگلیسی و قبولی در آزمون‌های ورودی مانند JEE برای پذیرش الزامی است.", "کارشناسی", 70 },
                    { 200005L, 10L, "علوم داده، کلان‌داده و هوش مصنوعی در هند رو به رشد است؛ دانشگاه‌ها دوره‌های تحصیلات تکمیلی در این حوزه ارائه می‌دهند【565810706922104†L20-L77】.", "علوم داده", false, "مدرک زبان انگلیسی، نمرهٔ مناسب در آزمون‌هایی مانند GATE یا GRE و سابقهٔ برنامه‌نویسی برای پذیرش لازم است.", "کارشناسی ارشد", 80 },
                    { 200006L, 10L, "رشته تحلیل و مدیریت کسب‌وکار به دانشجویان مهارت‌های تصمیم‌گیری داده‌محور و مدیریت پروژه می‌آموزد؛ شرکت‌ها به چنین فارغ‌التحصیلانی نیاز دارند.", "تحلیل و مدیریت کسب و کار", false, "مدرک زبان انگلیسی و در بسیاری از دانشگاه‌ها نمرهٔ CAT یا GMAT برای دوره‌های MBA لازم است.", "کارشناسی ارشد", 70 },
                    { 200007L, 10L, "این رشته برای تربیت مدیران پروژه در بخش‌های فناوری و زیرساختی طراحی شده و به دانشجویان ابزارهای مدیریت و کنترل پروژه می‌آموزد.", "مدیریت پروژه", false, "مدرک زبان انگلیسی و در برخی دانشگاه‌ها سابقه کاری مرتبط شرط پذیرش است.", "کارشناسی ارشد", 70 },
                    { 200008L, 10L, "رشته حسابداری و مالی دانشجویان را با قوانین مالیاتی و بازار سرمایه آشنا می‌کند؛ فرصت‌های شغلی در مؤسسات مالی و شرکت‌ها وجود دارد.", "حسابداری و مالی", false, "مدرک زبان انگلیسی و شرکت در آزمون‌های ورودی دانشگاه برای پذیرش لازم است.", "کارشناسی", 60 },
                    { 200009L, 10L, "مدیریت منابع انسانی بر جذب، آموزش و نگه‌داشت کارکنان تمرکز دارد و در شرکت‌های هندی اهمیت پیدا کرده است.", "مدیریت منابع انسانی", false, "مدرک زبان انگلیسی و گذراندن آزمون‌های ورودی برای پذیرش در دانشگاه‌ها.", "کارشناسی", 60 },
                    { 200010L, 10L, "به دلیل رشد سریع تجارت الکترونیک و استارتاپ‌ها، رشته بازاریابی و دیجیتال مارکتینگ در هند اهمیت ویژه‌ای پیدا کرده است【565810706922104†L20-L77】.", "بازاریابی و دیجیتال مارکتینگ", false, "مدرک زبان انگلیسی و در برخی موارد شرکت در آزمون‌های ورودی دانشگاه لازم است.", "کارشناسی", 70 },
                    { 200011L, 10L, "رشته‌های علوم بهداشتی و پرستاری برای پاسخگویی به نیازهای روزافزون نظام سلامت هند اهمیت دارند؛ دانشگاه‌ها دوره‌های عملی و بالینی ارائه می‌دهند.", "علوم بهداشتی و پرستاری", false, "مدرک زبان انگلیسی و شرکت در آزمون‌های ورودی مانند NEET برای پذیرش در رشته‌های سلامت لازم است.", "کارشناسی", 70 },
                    { 200012L, 10L, "رشته آموزش معلمان را برای تدریس در مدارس و آموزش و پرورش تربیت می‌کند؛ کمبود معلم در مناطق روستایی فرصت‌های شغلی ایجاد کرده است.", "آموزش", false, "مدرک زبان انگلیسی و قبولی در آزمون ورودی یا دوره تربیت معلم برای پذیرش لازم است.", "کارشناسی", 60 },
                    { 200013L, 10L, "علوم پایه و پژوهش شامل فیزیک، شیمی و زیست‌شناسی است؛ دولت هند برای تحقیق و توسعه در این حوزه‌ها سرمایه‌گذاری می‌کند.", "علوم پایه و پژوهش", false, "مدرک زبان انگلیسی و ارائهٔ طرح پژوهشی برای پذیرش در مقاطع تحصیلات تکمیلی.", "کارشناسی ارشد", 70 },
                    { 200014L, 10L, "رشته هنر و طراحی در هند شامل طراحی صنعتی، مد و هنرهای زیبا است؛ بازار کار این حوزه متنوع و وابسته به پروژه‌هاست.", "هنر و طراحی", false, "نمونهٔ کار هنری و مدرک زبان انگلیسی برای پذیرش لازم است.", "کارشناسی", 60 },
                    { 200015L, 10L, "هند با داشتن آثار تاریخی و تنوع فرهنگی، فرصت‌های خوبی در رشته گردشگری و مهمان‌داری فراهم کرده است؛ فارغ‌التحصیلان در هتل‌ها و شرکت‌های گردشگری مشغول می‌شوند.", "گردشگری و مهمان‌داری", false, "مدرک زبان انگلیسی و در برخی دوره‌ها گذراندن مصاحبه یا آزمون ورودی لازم است.", "کارشناسی", 60 }
                });

            migrationBuilder.InsertData(
                table: "CountryJobs",
                columns: new[] { "Id", "CountryId", "Description", "ExperienceImpact", "IsDeleted", "Score", "Title" },
                values: new object[,]
                {
                    { 10001L, 1L, "طبق گزارش بانک کار کانادا، آیندهٔ شغلی توسعه‌دهندگان نرم‌افزار در اکثر استان‌ها متوسط تا خوب است و نیاز به نیروی متخصص همچنان وجود دارد【811497086229718†L80-L116】.", "تجربهٔ کار در پروژه‌های بزرگ و تسلط بر فریم‌ورک‌های مدرن مانند دات‌نت و کلود امتیاز را افزایش می‌دهد.", false, 90, "توسعه‌دهنده نرم‌افزار" },
                    { 10002L, 1L, "مهندسان برق در پروژه‌های انرژی، ارتباطات و زیرساخت کانادا فعالیت می‌کنند. بازار کار برای برخی استان‌ها متوسط تا خوب بوده و کمبود نیروی متخصص وجود دارد【811497086229718†L80-L116】.", "سابقهٔ کار در پروژه‌های صنعتی و داشتن مدرک حرفه‌ای P.Eng. امتیاز متقاضی را افزایش می‌دهد.", false, 80, "مهندس برق و الکترونیک" },
                    { 10003L, 1L, "کانادا با پروژه‌های زیربنایی و مسکن روبه‌رو است؛ مهندسان عمران نقش کلیدی دارند و در برخی مناطق کمبود نیرو مشاهده می‌شود. این شغل در رتبه‌بندی جهانی کانادا جایگاه نسبتاً بالایی دارد.", "تجربهٔ مدیریت پروژه‌های ساختمانی و آشنایی با استانداردهای کانادایی (CSA) امتیاز را افزایش می‌دهد.", false, 80, "مهندس عمران و ساخت" },
                    { 10004L, 1L, "طبق گزارش بانک کار، کانادا با کمبود جدی پرستاران و سایر کارکنان سلامت روبه‌روست و چشم‌انداز شغلی این حوزه بسیار خوب است【164992063620704†L102-L116】.", "سابقهٔ کار بیمارستانی، داشتن گواهینامهٔ RN و تسلط به زبان انگلیسی/فرانسوی باعث افزایش امتیاز می‌شود.", false, 90, "پرستار و متخصص سلامت" },
                    { 10005L, 1L, "بازار کار علوم داده در کانادا رو به رشد است؛ شرکت‌ها برای توسعهٔ هوش مصنوعی و تحلیل داده به متخصصان ماهر نیاز دارند.", "تسلط به یادگیری ماشین، پایتون و تجربهٔ پروژه‌های دادهٔ بزرگ امتیاز را افزایش می‌دهد.", false, 88, "دانشمند داده و تحلیل داده" },
                    { 10006L, 1L, "شرکت‌های کانادایی به تحلیل‌گران کسب‌وکار برای بهبود فرایندها و تصمیم‌گیری داده‌محور نیاز دارند. این شغل بازار کار مناسبی دارد.", "تجربه در تحلیل سیستم‌ها، مدیریت پروژه و تسلط به ابزارهای BI امتیاز را افزایش می‌دهد.", false, 80, "تحلیلگر کسب و کار" },
                    { 10007L, 1L, "مدیران پروژه در صنایع فناوری، ساخت و بهداشت وظیفهٔ برنامه‌ریزی و هماهنگی پروژه‌ها را دارند. بازار کار متعادل است.", "گواهینامهٔ PMP و تجربهٔ رهبری تیم‌های بین‌المللی امتیاز را افزایش می‌دهد.", false, 80, "مدیر پروژه" },
                    { 10008L, 1L, "با رشد فضای مجازی، نیاز به متخصصان امنیت شبکه و سایبری در کانادا افزایش یافته است؛ بازار کار این حوزه رقابتی و پرتقاضا است.", "گواهینامه‌های امنیت مانند CISSP و تجربهٔ کار در سازمان‌های بزرگ امتیاز را افزایش می‌دهد.", false, 88, "متخصص شبکه و امنیت" },
                    { 10009L, 1L, "مهندسان مکانیک در صنایع خودروسازی، هوافضا و تولید کانادا نقش مهمی دارند؛ بازار کار متعادل و بعضاً رو به رشد است.", "تجربهٔ کار در خطوط تولید و دانش نرم‌افزارهای طراحی مهندسی (CAD/CAM) امتیاز را افزایش می‌دهد.", false, 80, "مهندس مکانیک و تولید" },
                    { 10010L, 1L, "نیاز به معلمان در برخی استان‌های کانادا به‌ویژه در مناطق دورافتاده وجود دارد؛ شرایط کاری نسبتاً پایدار است.", "مدرک رسمی تدریس و تجربهٔ تدریس در مدارس کانادا امتیاز را افزایش می‌دهد.", false, 75, "معلم و آموزش" },
                    { 10011L, 1L, "حسابداران و متخصصان مالی در شرکت‌های بزرگ و کوچک کانادا مورد نیازند؛ بازار کار متوسط تا خوب است.", "دارا بودن مدارک CPA و تجربه در حسابرسی یا مالیات باعث افزایش امتیاز می‌شود.", false, 75, "حسابدار و امور مالی" },
                    { 10012L, 1L, "مدیران منابع انسانی در سازمان‌ها نقش مهمی در مدیریت کارکنان و توسعهٔ منابع انسانی دارند؛ این شغل تقاضای نسبی دارد.", "تجربهٔ حل منازعات و دانش قوانین کار کانادا امتیاز را افزایش می‌دهد.", false, 75, "مدیر منابع انسانی" },
                    { 10013L, 1L, "شرکت‌ها برای توسعهٔ بازار و فروش به متخصصان بازاریابی دیجیتال نیازمندند؛ این حوزه با رشد تجارت الکترونیکی اهمیت یافته است.", "تجربهٔ اجرای کمپین‌های آنلاین، SEO و شبکه‌های اجتماعی امتیاز را افزایش می‌دهد.", false, 80, "متخصص بازاریابی و دیجیتال مارکتینگ" },
                    { 10014L, 1L, "کانادا به پژوهشگران در زمینه‌های علوم پایه و کاربردی اهمیت می‌دهد؛ فرصت‌های تحقیقاتی در دانشگاه‌ها و مراکز تحقیقاتی وجود دارد.", "انتشار مقالات علمی و شرکت در پروژه‌های بین‌المللی امتیاز را افزایش می‌دهد.", false, 80, "پژوهشگر علمی و دانشگاهی" },
                    { 10015L, 1L, "صنعت گردشگری و رستوران‌داری کانادا به آشپزها و کارکنان خدمات نیاز دارد؛ بازار کار متوسط است.", "تجربهٔ کار در رستوران‌های معتبر و مهارت در غذاهای بین‌المللی امتیاز را افزایش می‌دهد.", false, 70, "آشپز و خدمات رستوران" },
                    { 20001L, 2L, "بخش فناوری استرالیا در حال رشد است و نیاز به توسعه‌دهندگان نرم‌افزار در حوزه‌های ابری، هوش مصنوعی و امنیت افزایش یافته است. بسیاری از نقش‌های فناوری در فهرست کمبود مشاغل قرار دارند【101680514043225†L790-L801】.", "تجربه کار با چارچوب‌های نوین و برنامه‌نویسی موبایل یا ابری امتیاز فرد را افزایش می‌دهد.", false, 85, "توسعه‌دهنده نرم‌افزار" },
                    { 20002L, 2L, "پروژه‌های انرژی و زیرساخت‌های استرالیا به مهندسان برق نیاز دارند. این حوزه در فهرست کمبود شغل قرار دارد و بازار کار مناسبی دارد【101680514043225†L790-L801】.", "سابقه در سیستم‌های قدرت و گواهی حرفه‌ای مانند CPEng امتیاز را افزایش می‌دهد.", false, 80, "مهندس برق و الکترونیک" },
                    { 20003L, 2L, "زیرساخت‌های حمل‌ونقل و مسکن در استرالیا گسترش یافته و نیاز به مهندسان عمران و ساخت‌وساز در پروژه‌های ملی بسیار است【101680514043225†L790-L801】.", "تجربه مدیریت پروژه‌های بزرگ و آشنایی با استانداردهای زیست‌محیطی امتیاز را افزایش می‌دهد.", false, 80, "مهندس عمران و ساخت" },
                    { 20004L, 2L, "به دلیل کمبود نیروی انسانی، بخش سلامت استرالیا همچنان به پرستاران و متخصصان بهداشت نیاز دارد؛ این نقش‌ها در فهرست کمبود قرار دارند【101680514043225†L790-L801】.", "تجربه بالینی و ثبت‌نام در انجمن AHPRA امتیاز را افزایش می‌دهد.", false, 80, "پرستار و متخصص سلامت" },
                    { 20005L, 2L, "شرکت‌های استرالیایی برای رقابت جهانی در حوزهٔ هوش مصنوعی و تجزیه و تحلیل داده به متخصصان علوم داده نیاز دارند.", "تجربه در یادگیری ماشین، پایتون و پروژه‌های داده بزرگ امتیاز فرد را افزایش می‌دهد.", false, 85, "دانشمند داده و تحلیل داده" },
                    { 20006L, 2L, "نیاز شرکت‌ها به تحلیل‌گران کسب‌وکار برای بهینه‌سازی فرایندها و تصمیم‌گیری داده‌محور در حال افزایش است.", "تجربه در مدل‌سازی داده و استفاده از نرم‌افزارهای BI امتیاز را افزایش می‌دهد.", false, 78, "تحلیلگر کسب و کار" },
                    { 20007L, 2L, "پروژه‌های متعدد عمرانی و فناوری در استرالیا نیازمند مدیران پروژه کارآزموده هستند؛ این شغل تقاضای ثابت دارد.", "گواهی PMP و تجربه مدیریت تیم‌های چندفرهنگی امتیاز را افزایش می‌دهد.", false, 78, "مدیر پروژه" },
                    { 20008L, 2L, "افزایش حملات سایبری باعث شده نیاز به متخصصان شبکه و امنیت در استرالیا افزایش یابد؛ این حوزه به ویژه در بانکداری و دولت اهمیت دارد.", "گواهی‌های امنیتی و تجربه کار در مراکز داده یا سازمان‌های حساس امتیاز را افزایش می‌دهد.", false, 85, "متخصص شبکه و امنیت" },
                    { 20009L, 2L, "صنایع تولیدی و معدنی استرالیا به مهندسان مکانیک و تولید نیاز دارند؛ این بخش در فهرست کمبود شغل قرار دارد【101680514043225†L790-L801】.", "تجربه در طراحی مکانیکی و فرایندهای تولید و استفاده از ابزار CAD/CAM امتیاز را افزایش می‌دهد.", false, 78, "مهندس مکانیک و تولید" },
                    { 20010L, 2L, "در برخی مناطق به ویژه مناطق روستایی استرالیا کمبود معلم وجود دارد و حرفهٔ آموزش در فهرست کمبود درج شده است【101680514043225†L790-L801】.", "گواهی تدریس و تجربه در مدارس ایالتی یا بومی امتیاز را افزایش می‌دهد.", false, 75, "معلم و آموزش" },
                    { 20011L, 2L, "حرفهٔ حسابداری و امور مالی در استرالیا برای شرکت‌ها و مؤسسات مالی ضروری است؛ تقاضا متعادل است.", "دارا بودن مدارک CA یا CPA و تجربه در حسابرسی باعث افزایش امتیاز می‌شود.", false, 75, "حسابدار و امور مالی" },
                    { 20012L, 2L, "مدیران منابع انسانی نقش مهمی در جذب و نگهداشت کارکنان دارند؛ این شغل در استرالیا جایگاه متوسطی دارد.", "تجربه در سیاست‌گذاری منابع انسانی و حل اختلافات امتیاز فرد را افزایش می‌دهد.", false, 75, "مدیر منابع انسانی" },
                    { 20013L, 2L, "با رشد تجارت الکترونیک و رقابت در بازار، شرکت‌های استرالیایی به متخصصان بازاریابی دیجیتال نیاز دارند.", "تجربه در مدیریت کمپین‌های آنلاین، SEO و شبکه‌های اجتماعی امتیاز را افزایش می‌دهد.", false, 78, "متخصص بازاریابی و دیجیتال مارکتینگ" },
                    { 20014L, 2L, "استرالیا دانشگاه‌ها و موسسات پژوهشی بسیاری دارد و پژوهشگران در رشته‌های علوم، مهندسی و پزشکی از فرصت‌های شغلی برخوردارند.", "انتشار مقالات و مشارکت در پروژه‌های بین‌المللی امتیاز را افزایش می‌دهد.", false, 78, "پژوهشگر علمی و دانشگاهی" },
                    { 20015L, 2L, "صنعت گردشگری و رستوران‌داری استرالیا به آشپزها و کارکنان خدمات نیاز دارد؛ اما نسبت به سایر مشاغل امتیاز کمتری دارد.", "تجربه در آشپزخانه‌های حرفه‌ای و مهارت در غذاهای چندفرهنگی امتیاز را افزایش می‌دهد.", false, 70, "آشپز و خدمات رستوران" },
                    { 30001L, 3L, "طبق وب‌سایت Make it in Germany، بخش فناوری اطلاعات آلمان در حال رونق است و سالانه هزاران شغل جدید برای متخصصان نرم‌افزار ایجاد می‌شود【538120821009572†L329-L343】.", "تجربه در توسعه نرم‌افزار، امنیت سایبری و تسلط به زبان آلمانی و انگلیسی امتیاز فرد را افزایش می‌دهد.", false, 90, "توسعه‌دهنده نرم‌افزار" },
                    { 30002L, 3L, "استفاده از فناوری‌های دیجیتال و Industry 4.0 موجب شده شرکت‌های آلمانی به شدت به مهندسان برق و الکترونیک نیاز داشته باشند【538120821009572†L329-L343】.", "سابقهٔ کار در صنایع تولیدی، انرژی یا خودروسازی و آشنایی با استانداردهای آلمان (DIN) امتیاز را افزایش می‌دهد.", false, 90, "مهندس برق و الکترونیک" },
                    { 30003L, 3L, "آلمان برای ساخت‌وساز و نگهداری زیرساخت‌ها به مهندسان عمران نیاز دارد؛ تقاضا در شهرسازی و پروژه‌های راه‌آهن بالا است.", "تجربه در پروژه‌های بزرگ و توانایی مدیریت پروژه و آشنایی با مقررات ساختمانی آلمان امتیاز را افزایش می‌دهد.", false, 85, "مهندس عمران و ساخت" },
                    { 30004L, 3L, "بخش بهداشت و درمان آلمان با کمبود پرستاران و متخصصان سلامت روبه‌روست؛ این شغل‌ها در اولویت مهاجرتی قرار دارند.", "تسلط به زبان آلمانی و داشتن تجربه بالینی در بیمارستان‌ها یا خانه‌های سالمندان امتیاز را افزایش می‌دهد.", false, 85, "پرستار و متخصص سلامت" },
                    { 30005L, 3L, "آلمان برای توسعهٔ صنعت ۴ و اقتصاد دیجیتال به متخصصان داده و هوش مصنوعی نیاز دارد؛ شرکت‌ها به دنبال استعدادهای این حوزه هستند【538120821009572†L329-L343】.", "تجربه در یادگیری ماشین، تحلیل داده و آشنایی با زبان‌های برنامه‌نویسی مانند Python و R امتیاز را افزایش می‌دهد.", false, 90, "دانشمند داده و تحلیل داده" },
                    { 30006L, 3L, "شرکت‌های آلمانی برای بهبود فرآیندهای خود به تحلیل‌گران کسب‌وکار نیاز دارند؛ این شغل در حوزه‌های صنعتی و خدماتی اهمیت دارد.", "تجربه در مدیریت فرآیند و استفاده از ابزارهای تحلیلی مانند SAP و Tableau امتیاز را افزایش می‌دهد.", false, 80, "تحلیلگر کسب و کار" },
                    { 30007L, 3L, "مدیران پروژه برای هماهنگی پروژه‌های بزرگ در صنایع مهندسی و فناوری نقش مهمی دارند؛ تقاضای ثابت برای این نقش وجود دارد.", "گواهی PMP و تجربه مدیریت تیم‌های چندملیتی و دانش زبان آلمانی امتیاز را افزایش می‌دهد.", false, 80, "مدیر پروژه" },
                    { 30008L, 3L, "آلمان اهمیت زیادی به امنیت سایبری می‌دهد و شرکت‌ها به متخصصان شبکه و امنیت برای محافظت از داده‌ها و زیرساخت‌ها نیاز دارند.", "دارا بودن گواهی‌های امنیتی (مثل CISSP) و تجربه در مراکز داده یا شرکت‌های فناوری اطلاعات امتیاز را افزایش می‌دهد.", false, 90, "متخصص شبکه و امنیت" },
                    { 30009L, 3L, "آلمان به عنوان رهبر صنعت تولید و خودروسازی، به مهندسان مکانیک و تولید نیاز فراوانی دارد؛ این مشاغل در کنار صنعت ۴.۰ پیشرفت می‌کنند【538120821009572†L329-L343】.", "سابقه در طراحی ماشین‌آلات و استفاده از نرم‌افزارهای CAD/CAM و آشنایی با سیستم‌های تولید ناب امتیاز را افزایش می‌دهد.", false, 90, "مهندس مکانیک و تولید" },
                    { 30010L, 3L, "نظام آموزشی آلمان به معلمان واجد شرایط نیاز دارد؛ به ویژه در رشته‌های علوم و زبان، تقاضا رو به افزایش است.", "گواهی تدریس و مهارت زبان آلمانی و انگلیسی، و سابقه تدریس در مدارس آلمان امتیاز را افزایش می‌دهد.", false, 75, "معلم و آموزش" },
                    { 30011L, 3L, "در صنایع تولیدی و خدماتی آلمان، حسابداران و متخصصان مالی برای مدیریت هزینه‌ها و گزارش‌های مالی ضروری‌اند.", "دارا بودن مدارک حسابداری (مثلاً Steuerberater) و تجربه در قوانین مالیاتی آلمان امتیاز را افزایش می‌دهد.", false, 80, "حسابدار و امور مالی" },
                    { 30012L, 3L, "مدیران منابع انسانی برای جذب و نگه‌داشت کارکنان و ایجاد محیط کاری سالم نقش دارند؛ این شغل تقاضای متوسطی دارد.", "تجربه در حل اختلافات و تسلط بر قوانین کار آلمان امتیاز را افزایش می‌دهد.", false, 78, "مدیر منابع انسانی" },
                    { 30013L, 3L, "شرکت‌های آلمانی به ویژه در بخش تکنولوژی و خودرو برای بازاریابی محصولات خود به متخصصان دیجیتال مارکتینگ نیاز دارند.", "تجربه در مدیریت کمپین‌های دیجیتال و مهارت در SEO و شبکه‌های اجتماعی امتیاز را افزایش می‌دهد.", false, 75, "متخصص بازاریابی و دیجیتال مارکتینگ" },
                    { 30014L, 3L, "آلمان میزبان مؤسسات تحقیقاتی و دانشگاه‌های مطرح است و پژوهشگران علوم پایه و کاربردی به شدت مورد تقاضا هستند【538120821009572†L351-L353】.", "داشتن مدرک دکتری و سابقه انتشار مقالات در مجلات علمی و همکاری با برنامه‌های اروپایی امتیاز را افزایش می‌دهد.", false, 90, "پژوهشگر علمی و دانشگاهی" },
                    { 30015L, 3L, "آلمان دارای رستوران‌ها و هتل‌های متنوعی است اما بازار کار آشپز در مقایسه با سایر مشاغل محدودتر و رقابتی‌تر است.", "تجربه در آشپزخانه‌های حرفه‌ای و آشنایی با غذاهای آلمانی و بین‌المللی امتیاز را افزایش می‌دهد.", false, 65, "آشپز و خدمات رستوران" },
                    { 40001L, 4L, "دفتر آمار کار آمریکا پیش‌بینی می‌کند اشتغال توسعه‌دهندگان نرم‌افزار بین سال‌های ۲۰۲۴ تا ۲۰۳۴ حدود ۱۵ درصد رشد کند و هر سال حدود ۱۲۹٬۲۰۰ فرصت شغلی ایجاد شود【512303356091045†L249-L289】.", "تجربه در حوزه‌هایی مانند هوش مصنوعی، یادگیری ماشین و خدمات ابری سبب افزایش امتیاز می‌شود.", false, 95, "توسعه‌دهنده نرم‌افزار" },
                    { 40002L, 4L, "مهندسان برق در پروژه‌های صنعتی و فناوری آمریکا نقش حیاتی دارند و تقاضا برای آنان در حوزه‌هایی مانند انرژی تجدیدپذیر و سخت‌افزار افزایش یافته است.", "تجربه در طراحی سیستم‌های الکتریکی و داشتن مجوز حرفه‌ای (PE) امتیاز متقاضی را افزایش می‌دهد.", false, 85, "مهندس برق و الکترونیک" },
                    { 40003L, 4L, "آمریکا با پروژه‌های زیرساختی بزرگ و توسعه شهرها نیاز به مهندسان عمران دارد؛ بخش ساخت‌وساز یکی از حوزه‌های مهم شغلی است.", "تجربه در پروژه‌های راه‌سازی و مدیریت ساختمان و داشتن گواهی PMP یا PE باعث افزایش امتیاز می‌شود.", false, 82, "مهندس عمران و ساخت" },
                    { 40004L, 4L, "اشتغال پرستاران ثبت‌ شده در دههٔ ۲۰۲۴ تا ۲۰۳۴ حدود ۵ درصد رشد می‌کند و هر سال تقریباً ۱۸۹٬۱۰۰ فرصت شغلی وجود دارد【52862489964824†L248-L279】.", "داشتن تجربه بالینی و گواهینامه‌های تخصصی مانند NCLEX-RN امتیاز را افزایش می‌دهد.", false, 85, "پرستار و متخصص سلامت" },
                    { 40005L, 4L, "آمریکا پیشتاز توسعهٔ هوش مصنوعی و علوم داده است؛ تقاضای بسیار بالا برای متخصصان داده و یادگیری ماشین وجود دارد.", "تسلط به الگوریتم‌های یادگیری ماشین، پایتون و تجربه در پروژه‌های داده بزرگ امتیاز را افزایش می‌دهد.", false, 95, "دانشمند داده و تحلیل داده" },
                    { 40006L, 4L, "شرکت‌ها برای تصمیم‌گیری و بهینه‌سازی فرایندها به تحلیل‌گران کسب‌وکار نیاز دارند؛ این شغل در ایالات متحده جایگاه قابل توجهی دارد.", "تجربه در تحلیل فرآیندها، مدیریت داده و آشنایی با SQL یا ابزارهای BI امتیاز را افزایش می‌دهد.", false, 85, "تحلیلگر کسب و کار" },
                    { 40007L, 4L, "مدیران پروژه در صنایع فناوری، ساخت و بخش سلامت نقشی کلیدی ایفا می‌کنند؛ به ویژه در پروژه‌های بزرگ مهندسی نیاز بالایی وجود دارد.", "گواهی PMP و تجربه رهبری تیم‌های چندملیتی امتیاز قابل توجهی دارد.", false, 85, "مدیر پروژه" },
                    { 40008L, 4L, "افزایش حملات سایبری موجب شده نیاز به متخصصان امنیت شبکه در آمریکا بسیار بالا باشد؛ این رشته با رشد فناوری اهمیت بیشتری یافته است.", "گواهی‌هایی مانند CISSP و تجربه در تحلیل نفوذ و امنیت سایبری امتیاز را افزایش می‌دهد.", false, 92, "متخصص شبکه و امنیت" },
                    { 40009L, 4L, "مهندسان مکانیک در صنایع خودروسازی، هوافضا و تولید آمریکا نقش مهمی دارند؛ نیاز به نوآوری در تولید باعث افزایش تقاضا شده است.", "تجربه در طراحی مکانیکی، کنترل کیفیت و نرم‌افزارهای CAD/CAM امتیاز را افزایش می‌دهد.", false, 85, "مهندس مکانیک و تولید" },
                    { 40010L, 4L, "سیستم آموزشی آمریکا نیازمند معلمان واجد شرایط است؛ در برخی مناطق کمبود نیرو وجود دارد اما در سطح ملی تقاضا متوسط است.", "مدرک Teaching Credential و تجربه تدریس در مدارس عمومی امتیاز را افزایش می‌دهد.", false, 70, "معلم و آموزش" },
                    { 40011L, 4L, "حسابداران و متخصصان مالی در شرکت‌های آمریکایی نقش کلیدی در گزارشگری مالی و انطباق با قوانین مالیاتی دارند؛ تقاضا نسبتاً بالاست.", "دارا بودن مدارک CPA یا CMA و تجربه در حسابرسی و بودجه‌بندی امتیاز را افزایش می‌دهد.", false, 80, "حسابدار و امور مالی" },
                    { 40012L, 4L, "مدیران منابع انسانی برای جذب و نگه‌داشت کارکنان در شرکت‌ها اهمیت دارند؛ این حرفه در بازار کار آمریکا جایگاه متوسطی دارد.", "تجربه در توسعه سیاست‌های HR و حل و فصل اختلافات امتیاز را افزایش می‌دهد.", false, 78, "مدیر منابع انسانی" },
                    { 40013L, 4L, "با رشد تجارت الکترونیک، شرکت‌های آمریکایی به متخصصان بازاریابی دیجیتال نیاز مبرم دارند؛ بهینه‌سازی کمپین‌های آنلاین اهمیت دارد.", "تجربه در SEO، تبلیغات آنلاین و تحلیل داده‌های مشتری امتیاز را افزایش می‌دهد.", false, 85, "متخصص بازاریابی و دیجیتال مارکتینگ" },
                    { 40014L, 4L, "آمریکا میزبان دانشگاه‌ها و مراکز تحقیقاتی پیشرو است و پژوهشگران در علوم پایه و کاربردی فرصت‌های زیادی دارند.", "انتشار مقاله در مجلات معتبر و همکاری با تیم‌های تحقیقاتی بین‌المللی امتیاز را افزایش می‌دهد.", false, 85, "پژوهشگر علمی و دانشگاهی" },
                    { 40015L, 4L, "صنعت رستوران و گردشگری آمریکا به آشپزها و کارکنان خدمات نیاز دارد؛ اما این شغل نسبت به سایر مشاغل دارای امتیاز متوسط است.", "تجربه در رستوران‌های سطح بالا و مهارت در غذاهای بین‌المللی امتیاز را افزایش می‌دهد.", false, 70, "آشپز و خدمات رستوران" },
                    { 50001L, 5L, "براساس آمار EURES، بخش ICT در هلند با کمبود نیروی کار مواجه است و شرکت‌ها به دنبال توسعه‌دهندگان نرم‌افزار و متخصصان فناوری اطلاعات هستند【394404756887990†L194-L222】.", "تجربه در برنامه‌نویسی، توسعه وب و زبان‌های مدرن و تسلط به زبان انگلیسی و هلندی امتیاز را افزایش می‌دهد.", false, 88, "توسعه‌دهنده نرم‌افزار" },
                    { 50002L, 5L, "شرکت‌های هلندی در حوزه‌های انرژی و الکترونیک به مهندسان برق و الکترونیک نیاز دارند؛ این شغل‌ها در گروه کمبود نیروی کار قرار دارند【394404756887990†L194-L222】.", "تجربه در طراحی سیستم‌های الکتریکی و مهارت در اتوماسیون صنعتی و دانش استانداردهای اروپا امتیاز را افزایش می‌دهد.", false, 88, "مهندس برق و الکترونیک" },
                    { 50003L, 5L, "به علت نیاز به توسعه زیرساخت‌ها، هلند به مهندسان عمران به ویژه در ساخت پل‌ها، جاده‌ها و کنترل آب نیاز دارد【394404756887990†L194-L222】.", "تجربه در پروژه‌های زیربنایی و دانش استانداردهای مهندسی آب امتیاز را افزایش می‌دهد.", false, 83, "مهندس عمران و ساخت" },
                    { 50004L, 5L, "بخش‌های سلامت و مراقبت‌های سالمندان در هلند با کمبود نیروی کار روبه‌رو هستند؛ پرستاران فرصت‌های شغلی خوبی دارند【394404756887990†L194-L222】.", "سابقهٔ کار در بیمارستان‌ها و توانایی ارتباط با بیماران هلندی به زبان هلندی امتیاز را افزایش می‌دهد.", false, 85, "پرستار و متخصص سلامت" },
                    { 50005L, 5L, "شرکت‌های هلندی به ویژه در بخش‌های مالی و فناوری به متخصصان علوم داده و تحلیل‌گر داده نیاز دارند تا به تصمیم‌گیری مبتنی بر داده کمک کنند.", "تسلط به یادگیری ماشین، تحلیل داده و زبان‌های برنامه‌نویسی مانند Python یا R امتیاز را افزایش می‌دهد.", false, 90, "دانشمند داده و تحلیل داده" },
                    { 50006L, 5L, "مشاغل مدیریت و بازرگانی در هلند با کمبود نیرو مواجه‌اند؛ تحلیل‌گران کسب‌وکار برای بهبود فرآیندها و سودآوری شرکت‌ها ضروری‌اند【394404756887990†L194-L222】.", "تجربه در تحلیل داده، مدیریت فرآیند و توانایی ارتباط با ذینفعان مختلف امتیاز را افزایش می‌دهد.", false, 80, "تحلیلگر کسب و کار" },
                    { 50007L, 5L, "مدیران پروژه در هلند پروژه‌های بزرگ مهندسی، فناوری و توسعه شهری را هدایت می‌کنند؛ تقاضا برای این نقش‌ها بالاست.", "گواهی PMP و تجربه مدیریت تیم‌های چندملیتی و تسلط به زبان هلندی و انگلیسی امتیاز را افزایش می‌دهد.", false, 80, "مدیر پروژه" },
                    { 50008L, 5L, "با پیشرفت دیجیتال‌سازی، نیاز به متخصصان شبکه و امنیت سایبری در هلند افزایش یافته است؛ به ویژه در بخش‌های مالی و دولتی.", "گواهی‌های امنیتی مانند CISSP و تجربه در مدیریت شبکه‌های بزرگ امتیاز را افزایش می‌دهد.", false, 90, "متخصص شبکه و امنیت" },
                    { 50009L, 5L, "صنایع تولیدی و ماشین‌سازی هلند به مهندسان مکانیک نیاز دارند؛ این حرفه در گروه کمبود نیروی کار قرار دارد【394404756887990†L194-L222】.", "تجربه در طراحی مکانیکی، تولید و آشنایی با استانداردهای اروپایی امتیاز را افزایش می‌دهد.", false, 83, "مهندس مکانیک و تولید" },
                    { 50010L, 5L, "نرخ خالی مشاغل در بخش آموزش هلند بالا است؛ مدارس به خصوص در دوره‌های ابتدایی و متوسطه به معلمان نیاز دارند【394404756887990†L194-L222】.", "دارا بودن گواهی تدریس و تسلط به زبان هلندی و انگلیسی امتیاز را افزایش می‌دهد.", false, 80, "معلم و آموزش" },
                    { 50011L, 5L, "کمبود نیروی متخصص در امور مالی و حسابداری وجود دارد؛ شرکت‌ها به حسابداران با مهارت‌های بین‌المللی نیاز دارند【394404756887990†L194-L222】.", "دارا بودن مدارک ACCA/CPA و تجربه در شرکت‌های چندملیتی امتیاز را افزایش می‌دهد.", false, 80, "حسابدار و امور مالی" },
                    { 50012L, 5L, "مدیران منابع انسانی در جذب و نگه‌داشت کارکنان در شرکت‌های هلندی نقش دارند؛ این شغل تقاضای متوسطی دارد.", "تجربه در سیاست‌گذاری HR و آشنایی با قوانین کار هلند امتیاز را افزایش می‌دهد.", false, 78, "مدیر منابع انسانی" },
                    { 50013L, 5L, "شرکت‌های هلندی به ویژه در بخش فناوری و تجارت الکترونیک به متخصصان بازاریابی دیجیتال نیاز دارند.", "تجربه در مدیریت کمپین‌های آنلاین، SEO و تحلیل بازار امتیاز را افزایش می‌دهد.", false, 80, "متخصص بازاریابی و دیجیتال مارکتینگ" },
                    { 50014L, 5L, "هلند با داشتن دانشگاه‌ها و مراکز تحقیقاتی پیشرفته، به پژوهشگران در علوم، مهندسی و علوم اجتماعی نیاز دارد؛ این مشاغل در سطح بالایی قرار دارند.", "داشتن مدرک دکتری و سابقه انتشار مقالات علمی و همکاری بین‌المللی امتیاز را افزایش می‌دهد.", false, 88, "پژوهشگر علمی و دانشگاهی" },
                    { 50015L, 5L, "صنعت گردشگری و غذا در هلند رو به رشد است و آشپزها و کارکنان رستوران فرصت‌های شغلی دارند؛ اما این شغل نسبت به سایر مشاغل امتیاز متوسطی دارد.", "تجربه در آشپزخانه‌های حرفه‌ای و مهارت در غذاهای بین‌المللی امتیاز را افزایش می‌دهد.", false, 75, "آشپز و خدمات رستوران" },
                    { 60001L, 6L, "به دلیل دیجیتال‌سازی و کمبود نیروی متخصص، مشاغل ICT مانند برنامه‌نویسان و توسعه‌دهندگان نرم‌افزار در اسپانیا با کمبود نیروی کار روبه‌رو هستند【189557896059467†L342-L375】.", "تجربهٔ کار در شرکت‌های فناوری و تسلط به فریمورک‌های جدید و زبان انگلیسی امتیاز فرد را افزایش می‌دهد.", false, 75, "توسعه‌دهنده نرم‌افزار" },
                    { 60002L, 6L, "مهندسان برق و الکترونیک در صنایع انرژی و مخابرات اسپانیا تقاضا دارند؛ اما کمبود متخصصان هنوز وجود دارد【189557896059467†L342-L375】.", "سابقهٔ کار در شرکت‌های مهندسی و آشنایی با استانداردهای اروپایی امتیاز را افزایش می‌دهد.", false, 70, "مهندس برق و الکترونیک" },
                    { 60003L, 6L, "اسپانیا برای پروژه‌های ساخت‌وساز و احیای زیرساخت‌ها به مهندسان عمران نیاز دارد؛ اما تعداد فارغ‌التحصیلان کافی نیست【189557896059467†L408-L433】.", "تجربه در پروژه‌های ساختمانی و تسلط به آیین‌نامه‌های ساختمانی اروپا امتیاز را افزایش می‌دهد.", false, 76, "مهندس عمران و ساخت" },
                    { 60004L, 6L, "کمبود پرستاران و کارکنان سلامت در اسپانیا و رشد جمعیت سالمند باعث شده این حرفه‌ها بسیار پرتقاضا باشند.", "تجربهٔ کار در بیمارستان‌ها و تسلط به زبان اسپانیایی به‌ویژه برای ارتباط با بیماران امتیاز را افزایش می‌دهد.", false, 78, "پرستار و متخصص سلامت" },
                    { 60005L, 6L, "شرکت‌های اسپانیایی در حال گذار دیجیتال هستند و نیاز به تحلیل‌گران داده برای بهینه‌سازی عملیات و اتخاذ تصمیمات دارند.", "تجربه در یادگیری ماشین، تحلیل داده و آشنایی با زبان‌های برنامه‌نویسی مانند Python و R امتیاز را افزایش می‌دهد.", false, 70, "دانشمند داده و تحلیل داده" },
                    { 60006L, 6L, "شرکت‌های اسپانیایی برای بهبود فرایندها و رقابت در بازارهای بین‌المللی به تحلیل‌گران کسب‌وکار نیاز دارند.", "تجربه در تحلیل فرآیند و استفاده از ابزارهای تحلیلی مانند Power BI یا Tableau امتیاز را افزایش می‌دهد.", false, 72, "تحلیلگر کسب و کار" },
                    { 60007L, 6L, "مدیران پروژه در بخش‌های مهندسی، فناوری و ساخت‌وساز نقش مهمی دارند؛ اما بازار کار نسبتاً رقابتی است.", "گواهی PMP و تجربه مدیریت تیم‌های چندملیتی امتیاز را افزایش می‌دهد.", false, 72, "مدیر پروژه" },
                    { 60008L, 6L, "با افزایش تهدیدات سایبری، نیاز به متخصصان امنیت و شبکه در اسپانیا افزایش یافته است؛ اما حجم بازار نسبتاً کوچک‌تر از کشورهایی مانند آلمان یا آمریکا است.", "گواهی‌های امنیتی و تجربه در حفاظت از شبکه‌های بزرگ امتیاز را افزایش می‌دهد.", false, 72, "متخصص شبکه و امنیت" },
                    { 60009L, 6L, "مهندسان تولید و مکانیک در صنایع خودروسازی و تولید اسپانیا پرتقاضا هستند اما تعداد فارغ‌التحصیلان کم است【189557896059467†L408-L433】.", "تجربه در خطوط تولید و آشنایی با استانداردهای صنعتی اروپا امتیاز را افزایش می‌دهد.", false, 70, "مهندس مکانیک و تولید" },
                    { 60010L, 6L, "سیستم آموزشی اسپانیا در برخی مناطق به ویژه مدارس ابتدایی و زبان خارجی به معلمان بیشتری نیاز دارد؛ حقوق و مزایا محدود است.", "مدرک رسمی تدریس و تسلط به زبان اسپانیایی و انگلیسی امتیاز را افزایش می‌دهد.", false, 70, "معلم و آموزش" },
                    { 60011L, 6L, "حسابداران و متخصصان مالی برای شرکت‌های اسپانیایی ضروری‌اند؛ اما رقابت در این حوزه بالاست و فرصت‌ها محدودتر هستند.", "داشتن مدارک حرفه‌ای حسابداری و تجربه با قوانین مالیاتی اسپانیا امتیاز را افزایش می‌دهد.", false, 70, "حسابدار و امور مالی" },
                    { 60012L, 6L, "مدیران منابع انسانی در شرکت‌های اسپانیایی مسئول استخدام و مدیریت کارکنان هستند؛ این شغل امتیاز متوسطی دارد.", "تجربه در توسعه سیاست‌های HR و آشنایی با قانون کار اسپانیا امتیاز را افزایش می‌دهد.", false, 70, "مدیر منابع انسانی" },
                    { 60013L, 6L, "با رشد تجارت الکترونیک و گردشگری، نیاز به متخصصان بازاریابی دیجیتال در اسپانیا افزایش یافته است.", "تجربه در کمپین‌های آنلاین، SEO و شبکه‌های اجتماعی امتیاز را افزایش می‌دهد.", false, 72, "متخصص بازاریابی و دیجیتال مارکتینگ" },
                    { 60014L, 6L, "پژوهشگران در دانشگاه‌ها و مؤسسات تحقیقاتی اسپانیا به فعالیت می‌پردازند؛ بودجه‌های محدود باعث می‌شود امتیاز این شغل نسبت به کشورهایی مانند آلمان کمتر باشد.", "انتشار مقاله در مجلات علمی و شرکت در پروژه‌های اروپایی امتیاز را افزایش می‌دهد.", false, 70, "پژوهشگر علمی و دانشگاهی" },
                    { 60015L, 6L, "صنعت گردشگری و رستوران‌داری اسپانیا بسیار قوی است؛ آشپزها و کارکنان رستوران‌ها با تقاضای بالا مواجه‌اند.", "تجربه در آشپزخانه‌های حرفه‌ای و تخصص در غذاهای اسپانیایی و بین‌المللی امتیاز را افزایش می‌دهد.", false, 78, "آشپز و خدمات رستوران" },
                    { 70001L, 7L, "در فهرست کمبود نیروی اداره مهاجرت سوئد، توسعه‌دهندگان نرم‌افزار و سیستم و معماران IT جزو مشاغل کمیاب هستند و شرکت‌ها به دنبال برنامه‌نویسان ماهر می‌گردند【707620709545585†L39-L83】.", "سابقه کار در شرکت‌های فناوری و تسلط بر زبان‌های برنامه‌نویسی مدرن و معماری سیستم امتیاز را افزایش می‌دهد.", false, 92, "توسعه‌دهنده نرم‌افزار" },
                    { 70002L, 7L, "صنایع انرژی و فناوری سوئد به مهندسان برق و الکترونیک نیاز دارند؛ این شغل در فهرست کمبود نیرو است و مهندسان واجد شرایط فرصت‌های زیادی دارند【707620709545585†L39-L83】.", "تجربه در پروژه‌های صنعتی و دانش فناوری‌های نو مانند انرژی‌های تجدیدپذیر امتیاز را افزایش می‌دهد.", false, 85, "مهندس برق و الکترونیک" },
                    { 70003L, 7L, "در سوئد، پروژه‌های ساخت‌وساز و توسعه شهرها نیاز به مهندسان عمران دارد؛ بازار کار این رشته به‌ویژه در پروژه‌های زیرساختی خوب است.", "تجربه در مدیریت پروژه و آشنایی با قوانین ساختمانی سوئد امتیاز را افزایش می‌دهد.", false, 78, "مهندس عمران و ساخت" },
                    { 70004L, 7L, "سوئد به دلیل افزایش جمعیت سالمند به پرستاران و کارکنان سلامت نیاز دارد؛ این حرفه‌ها در فهرست کمبود رسمی هستند【707620709545585†L39-L83】.", "سابقه بالینی و تسلط به زبان سوئدی و انگلیسی امتیاز را افزایش می‌دهد.", false, 88, "پرستار و متخصص سلامت" },
                    { 70005L, 7L, "شرکت‌های سوئدی در حوزه‌های فناوری و مالی به تحلیل‌گران داده و دانشمندان داده نیاز دارند تا به تصمیم‌گیری مبتنی بر داده کمک کنند.", "تسلط به یادگیری ماشین، تحلیل داده و زبان‌های برنامه‌نویسی مانند Python و R امتیاز را افزایش می‌دهد.", false, 90, "دانشمند داده و تحلیل داده" },
                    { 70006L, 7L, "شرکت‌های سوئدی به تحلیل‌گران کسب‌وکار برای بهبود فرآیندها و نوآوری نیاز دارند؛ این حرفه در بازار کار جایگاه خوبی دارد.", "تجربه در تحلیل فرآیند و استفاده از ابزارهای BI مانند Power BI امتیاز را افزایش می‌دهد.", false, 77, "تحلیلگر کسب و کار" },
                    { 70007L, 7L, "مدیران پروژه نقش کلیدی در اجرای پروژه‌های فناوری و زیرساختی در سوئد دارند؛ تقاضا در سطح متوسط تا خوب است.", "گواهی PMP و تجربه مدیریت تیم‌های چندفرهنگی و تسلط به زبان انگلیسی و سوئدی امتیاز را افزایش می‌دهد.", false, 78, "مدیر پروژه" },
                    { 70008L, 7L, "با رشد دیجیتال‌سازی، نیاز به متخصصان امنیت شبکه و سایبری در سوئد افزایش یافته است؛ این حوزه اهمیت زیادی دارد【707620709545585†L39-L83】.", "گواهی‌های امنیتی مانند CISSP و تجربه کار در شبکه‌های بزرگ امتیاز را افزایش می‌دهد.", false, 90, "متخصص شبکه و امنیت" },
                    { 70009L, 7L, "صنایع تولیدی و خودرو در سوئد به مهندسان مکانیک و تولید نیاز دارند؛ به خصوص در شرکت‌های خودروسازی و شرکت‌های فناوری پیشرفته.", "تجربه در طراحی مکانیکی و استفاده از نرم‌افزارهای CAD/CAM و دانش Lean Manufacturing امتیاز را افزایش می‌دهد.", false, 80, "مهندس مکانیک و تولید" },
                    { 70010L, 7L, "سوئد در برخی مناطق به معلمان به ویژه در رشته‌های STEM و زبان‌های خارجی نیاز دارد؛ این حرفه در فهرست کمبود نیروی کار قرار دارد【707620709545585†L39-L83】.", "گواهی تدریس و تجربه کار در مدارس سوئد و تسلط به زبان سوئدی امتیاز را افزایش می‌دهد.", false, 80, "معلم و آموزش" },
                    { 70011L, 7L, "حسابداران و متخصصان مالی برای شرکت‌های سوئدی در گزارش‌دهی مالی و انطباق با قوانین مالیاتی ضروری هستند؛ تقاضا متوسط است.", "دارا بودن مدارک ACCA یا CPA و تجربه با سیستم مالی سوئد امتیاز را افزایش می‌دهد.", false, 75, "حسابدار و امور مالی" },
                    { 70012L, 7L, "مدیران منابع انسانی نقش مهمی در جذب و نگه‌داشت کارکنان دارند؛ بازار کار این حرفه در سوئد متوسط است.", "تجربه در توسعه سیاست‌های HR و دانش قانون کار سوئد امتیاز را افزایش می‌دهد.", false, 75, "مدیر منابع انسانی" },
                    { 70013L, 7L, "شرکت‌ها و استارتاپ‌های سوئدی به متخصصان بازاریابی دیجیتال برای رقابت در بازار جهانی نیاز دارند.", "تجربه در تبلیغات آنلاین، SEO و شبکه‌های اجتماعی و تسلط به زبان انگلیسی و سوئدی امتیاز را افزایش می‌دهد.", false, 75, "متخصص بازاریابی و دیجیتال مارکتینگ" },
                    { 70014L, 7L, "سوئد با دانشگاه‌ها و مؤسسات پژوهشی پیشرفته، به پژوهشگران در علوم پایه و مهندسی نیاز دارد؛ بسیاری از پروژه‌ها بین‌المللی هستند.", "داشتن مدرک دکتری و انتشار مقالات علمی و همکاری با پروژه‌های اروپایی امتیاز را افزایش می‌دهد.", false, 90, "پژوهشگر علمی و دانشگاهی" },
                    { 70015L, 7L, "صنعت رستوران و هتل‌داری سوئد نیازمند آشپزهای ماهر است اما بازار نسبت به سایر کشورها محدودتر می‌باشد.", "تجربه در آشپزخانه‌های حرفه‌ای و مهارت در غذاهای اسکاندیناویایی و بین‌المللی امتیاز را افزایش می‌دهد.", false, 65, "آشپز و خدمات رستوران" },
                    { 80001L, 8L, "گزارش Cedefop نشان می‌دهد مشاغل ICT از جمله توسعه‌دهندگان نرم‌افزار به دلیل کمبود فارغ‌التحصیلان و تقاضای بالا در ایتالیا کمیاب هستند【982764113348400†L342-L378】.", "تجربه در زبان‌های برنامه‌نویسی و تسلط به زبان انگلیسی و ایتالیایی، همراه با دانش هوش مصنوعی، امتیاز را افزایش می‌دهد.", false, 70, "توسعه‌دهنده نرم‌افزار" },
                    { 80002L, 8L, "مهندسان برق و الکترونیک در صنایع انرژی و تولید ایتالیا فرصت‌های شغلی دارند؛ کمبود نیروی متخصص در زمینه‌های دیجیتال و اتوماسیون وجود دارد【982764113348400†L342-L378】.", "سابقهٔ کار در پروژه‌های صنعتی و آشنایی با استانداردهای اروپا (CE) امتیاز را افزایش می‌دهد.", false, 75, "مهندس برق و الکترونیک" },
                    { 80003L, 8L, "ایتالیا برای پروژه‌های زیربنایی و مرمت بناهای تاریخی به مهندسان عمران نیاز دارد؛ اما بازار کار نسبت به کشورهایی مثل آلمان محدودتر است.", "تجربه در مدیریت پروژه‌های ساختمانی و آشنایی با قوانین شهرسازی ایتالیا امتیاز را افزایش می‌دهد.", false, 73, "مهندس عمران و ساخت" },
                    { 80004L, 8L, "بخش سلامت ایتالیا با کمبود پرستاران و متخصصان بهداشت روبه‌روست و رشد این بخش حدود دو درصد در سال است【982764113348400†L408-L452】.", "سابقهٔ کار بیمارستانی و تسلط به زبان ایتالیایی امتیاز را افزایش می‌دهد.", false, 75, "پرستار و متخصص سلامت" },
                    { 80005L, 8L, "اگرچه بازار علوم داده در ایتالیا در حال رشد است، اما نسبت به کشورهای پیشرفته کوچک‌تر است؛ شرکت‌ها به متخصصان داده نیاز دارند.", "تسلط به یادگیری ماشین و تحلیل داده و آشنایی با زبان انگلیسی فرصت‌های شغلی بیشتری ایجاد می‌کند.", false, 70, "دانشمند داده و تحلیل داده" },
                    { 80006L, 8L, "شرکت‌های ایتالیایی به تحلیل‌گران کسب‌وکار برای بهینه‌سازی فرایندها نیاز دارند، اما بازار کار محدودتر از کشورهای شمال اروپا است.", "تجربه در مدیریت فرآیندها و استفاده از ابزارهای تحلیلی امتیاز را افزایش می‌دهد.", false, 70, "تحلیلگر کسب و کار" },
                    { 80007L, 8L, "مدیران پروژه در بخش‌های مهندسی و فناوری ایتالیا نقش دارند؛ با این حال فرصت‌های محدودتری نسبت به آمریکا یا کانادا وجود دارد.", "گواهی PMP و تجربه مدیریت تیم‌های چندفرهنگی امتیاز را افزایش می‌دهد.", false, 70, "مدیر پروژه" },
                    { 80008L, 8L, "با افزایش دیجیتال‌سازی، نیاز به متخصصان امنیت سایبری در ایتالیا در حال افزایش است، اما نسبت به کشورهای پیشرفته کمتر است.", "گواهی‌های امنیتی و تجربه کار در شرکت‌های فناوری اطلاعات امتیاز را افزایش می‌دهد.", false, 70, "متخصص شبکه و امنیت" },
                    { 80009L, 8L, "مهندسان مکانیک در صنایع خودروسازی و ماشین‌آلات ایتالیا با کمبود مواجه‌اند و فرصت‌های خوبی دارند【982764113348400†L342-L378】.", "تجربه در طراحی و تولید قطعات صنعتی و آشنایی با استانداردهای مهندسی اروپا امتیاز را افزایش می‌دهد.", false, 76, "مهندس مکانیک و تولید" },
                    { 80010L, 8L, "تقاضا برای معلمان در ایتالیا نسبتاً پایدار است؛ حقوق و مزایا نسبت به کشورهای شمال اروپا کمتر است.", "تجربه تدریس و مدرک آموزشی رسمی به افزایش امتیاز کمک می‌کند.", false, 70, "معلم و آموزش" },
                    { 80011L, 8L, "حسابداران و متخصصان مالی در شرکت‌های ایتالیایی مورد نیازند، اما بازار رقابتی است و فرصت‌ها نسبتاً محدود است.", "دارا بودن مدارک رسمی حسابداری و تجربه مالیاتی در شرکت‌های ایتالیایی امتیاز را افزایش می‌دهد.", false, 70, "حسابدار و امور مالی" },
                    { 80012L, 8L, "مدیران منابع انسانی در ایتالیا برای ادارهٔ کارکنان و بهبود شرایط کار اهمیت دارند ولی تقاضا نسبتاً محدود است.", "تجربه در مدیریت تعارض و دانش قانون کار ایتالیا امتیاز را افزایش می‌دهد.", false, 70, "مدیر منابع انسانی" },
                    { 80013L, 8L, "شرکت‌های ایتالیایی به‌ویژه در حوزهٔ مد و گردشگری به متخصصان بازاریابی دیجیتال نیاز دارند اما تقاضا نسبتاً محدود است.", "تجربه در تجارت الکترونیک، شبکه‌های اجتماعی و آشنایی با زبان انگلیسی و ایتالیایی امتیاز را افزایش می‌دهد.", false, 70, "متخصص بازاریابی و دیجیتال مارکتینگ" },
                    { 80014L, 8L, "در دانشگاه‌ها و مؤسسات تحقیقاتی ایتالیا فرصت‌هایی برای پژوهشگران وجود دارد، اما بودجه‌های محدود و رقابت باعث می‌شود امتیاز این شغل متوسط باشد.", "انتشار مقاله در مجلات علمی و همکاری با پروژه‌های اروپایی امتیاز را افزایش می‌دهد.", false, 70, "پژوهشگر علمی و دانشگاهی" },
                    { 80015L, 8L, "ایتالیا با صنعت گردشگری و غذاهای مشهور به آشپزهای ماهر نیاز دارد؛ فرصت‌های زیادی در رستوران‌ها و هتل‌ها وجود دارد.", "تجربه در آشپزخانه‌های حرفه‌ای و مهارت در غذاهای ایتالیایی امتیاز را افزایش می‌دهد.", false, 75, "آشپز و خدمات رستوران" },
                    { 90001L, 9L, "به گفتهٔ گزارش‌های خبری، عمان تلاش می‌کند نیروی کار خود را در حوزه‌های فناوری اطلاعات و دیجیتال آموزش دهد؛ این حوزه در حال رشد است اما در مقایسه با کشورهای پیشرفته کوچک‌تر است【107658387508375†L67-L100】.", "تجربه برنامه‌نویسی و آشنایی با زبان انگلیسی یا عربی و مدیریت پروژه‌های فناوری اطلاعات امتیاز را افزایش می‌دهد.", false, 60, "توسعه‌دهنده نرم‌افزار" },
                    { 90002L, 9L, "پروژه‌های صنعتی و توسعه زیرساخت‌ها در عمان به مهندسان برق و الکترونیک نیاز دارند؛ اما بازار کار نسبت به کشورهای صنعتی محدودتر است.", "تجربه کار در پروژه‌های انرژی و تسلط به استانداردهای منطقه‌ای امتیاز را افزایش می‌دهد.", false, 60, "مهندس برق و الکترونیک" },
                    { 90003L, 9L, "طرح‌های زیرساختی و توسعهٔ صنعتی در عمان فرصت‌های شغلی برای مهندسان عمران ایجاد کرده است؛ دولت به توسعه شهرها و پروژه‌های مسکن توجه دارد.", "سابقهٔ کار در پروژه‌های ساخت‌وساز و آشنایی با مقررات عمرانی عمان امتیاز را افزایش می‌دهد.", false, 65, "مهندس عمران و ساخت" },
                    { 90004L, 9L, "بخش سلامت عمان در حال توسعه است و نیاز به پرستاران و کارکنان بهداشت دارد؛ با این حال حجم بازار نسبت به کشورهایی مانند کانادا کمتر است.", "تجربه بالینی و تسلط به زبان عربی و انگلیسی و دریافت مجوزهای حرفه‌ای باعث افزایش امتیاز می‌شود.", false, 70, "پرستار و متخصص سلامت" },
                    { 90005L, 9L, "اقتصاد عمان در حال گذار به سمت دیجیتال است؛ اما فرصت‌های مربوط به علوم داده محدود و در حال توسعه است.", "تجربه در تحلیل داده، آشنایی با زبان‌های برنامه‌نویسی و توانایی ایجاد راهکارهای داده‌محور امتیاز را افزایش می‌دهد.", false, 60, "دانشمند داده و تحلیل داده" },
                    { 90006L, 9L, "شرکت‌های عمانی برای بهبود کارایی و رقابت‌پذیری به تحلیل‌گران کسب‌وکار نیاز دارند؛ اما تقاضا نسبت به کشورهای پیشرفته کمتر است.", "تجربه در تحلیل فرآیندها و آشنایی با ابزارهای مدیریتی امتیاز را افزایش می‌دهد.", false, 65, "تحلیلگر کسب و کار" },
                    { 90007L, 9L, "پروژه‌های عمرانی و صنعتی عمان نیاز به مدیران پروژه ماهر دارند؛ این شغل در حال رشد است.", "گواهی PMP و تجربه مدیریت تیم‌های چندملیتی و آشنایی با فرهنگ عمان امتیاز را افزایش می‌دهد.", false, 65, "مدیر پروژه" },
                    { 90008L, 9L, "دولت عمان در زمینه امنیت سایبری سرمایه‌گذاری می‌کند؛ اما بازار کار نسبت به کشورهای غربی محدود است.", "تجربه در مدیریت شبکه و گواهی‌های امنیتی و توانایی آموزش نیروهای بومی امتیاز را افزایش می‌دهد.", false, 60, "متخصص شبکه و امنیت" },
                    { 90009L, 9L, "صنایع نفت، گاز و تولید عمان به مهندسان مکانیک نیاز دارند؛ به ویژه پروژه‌های صنعتی در حال توسعه هستند.", "تجربه در سیستم‌های تولید و آشنایی با تجهیزات نفت و گاز امتیاز را افزایش می‌دهد.", false, 65, "مهندس مکانیک و تولید" },
                    { 90010L, 9L, "عمان به معلمان برای برنامه‌های آموزشی و زبان انگلیسی نیاز دارد؛ دولت به ارتقای نظام آموزشی توجه دارد.", "گواهی تدریس و تسلط به انگلیسی و عربی امتیاز را افزایش می‌دهد.", false, 68, "معلم و آموزش" },
                    { 90011L, 9L, "شرکت‌ها و بانک‌های عمان به حسابداران و متخصصان مالی نیاز دارند؛ اما بازار کار نسبت به کشورهای بزرگ محدودتر است.", "مدارک حرفه‌ای حسابداری و تجربه در بازارهای خاورمیانه امتیاز را افزایش می‌دهد.", false, 68, "حسابدار و امور مالی" },
                    { 90012L, 9L, "مدیران منابع انسانی در شرکت‌های عمانی مسئول استخدام و نگه‌داشت کارکنان هستند؛ این شغل تقاضای متوسط دارد.", "تجربه در سیاست‌گذاری HR و شناخت فرهنگ کاری عمان امتیاز را افزایش می‌دهد.", false, 68, "مدیر منابع انسانی" },
                    { 90013L, 9L, "اقتصاد عمان به تدریج به سمت تجارت الکترونیک حرکت می‌کند؛ نیاز به متخصصان بازاریابی دیجیتال در حال افزایش است اما هنوز در حال شکل‌گیری است.", "تجربه در کمپین‌های دیجیتال، SEO و آشنایی با فرهنگ بازار عمان امتیاز را افزایش می‌دهد.", false, 65, "متخصص بازاریابی و دیجیتال مارکتینگ" },
                    { 90014L, 9L, "مراکز تحقیقاتی در عمان محدود هستند؛ فرصت‌های پژوهشی عمدتاً در دانشگاه‌های دولتی یا پروژه‌های مشترک با دانشگاه‌های خارجی موجود است.", "داشتن مدرک دکتری و همکاری با مؤسسات بین‌المللی امتیاز را افزایش می‌دهد.", false, 60, "پژوهشگر علمی و دانشگاهی" },
                    { 90015L, 9L, "عمان با رشد گردشگری و هتل‌سازی نیاز به آشپز و کارکنان رستوران دارد؛ صنعت خدمات غذایی در حال رشد است.", "تجربه در آشپزی محلی و بین‌المللی و مهارت‌های خدمات مشتری امتیاز را افزایش می‌دهد.", false, 65, "آشپز و خدمات رستوران" },
                    { 100001L, 10L, "در هند رشد سریع در بخش‌های فناوری پیشرفته مانند هوش مصنوعی و یادگیری ماشین باعث شده مشاغل فناوری اطلاعات و برنامه‌نویسی جزء سریع‌ترین مشاغل درحال رشد باشند【565810706922104†L20-L77】.", "تجربه در AI/ML، توسعه نرم‌افزار و زبان‌های برنامه‌نویسی مدرن مانند Python و Java امتیاز را افزایش می‌دهد.", false, 80, "توسعه‌دهنده نرم‌افزار" },
                    { 100002L, 10L, "با توسعه صنعت برق، مخابرات و انرژی‌های نو در هند، مهندسان برق و الکترونیک فرصت‌های شغلی زیادی دارند؛ اما نسبت به کشورهایی مانند آمریکا پایین‌تر است.", "تجربه در پروژه‌های برق قدرت و الکترونیک و آشنایی با فناوری‌های نو مانند انرژی خورشیدی امتیاز را افزایش می‌دهد.", false, 75, "مهندس برق و الکترونیک" },
                    { 100003L, 10L, "با رشد زیرساخت‌ها و پروژه‌های عمرانی، مهندسان عمران در هند تقاضای بالایی دارند؛ پروژه‌های مسکن و راه‌سازی فرصت‌های شغلی فراوان ایجاد کرده‌اند.", "تجربه در مدیریت پروژه‌های عمرانی و دانش آیین‌نامه‌های ساختمانی هند امتیاز را افزایش می‌دهد.", false, 75, "مهندس عمران و ساخت" },
                    { 100004L, 10L, "سیستم سلامت هند به کارکنان حرفه‌ای در حوزه‌های پزشکی و پرستاری نیاز دارد؛ به‌ویژه در مناطق روستایی کمبود نیرو محسوس است.", "تجربه بالینی و مدارک بین‌المللی مانند IELTS برای پرستاران امتیاز را افزایش می‌دهد.", false, 65, "پرستار و متخصص سلامت" },
                    { 100005L, 10L, "رشد هوش مصنوعی و کلان داده در هند موجب شده مشاغل علوم داده و تحلیل‌گر داده پرتقاضا باشند؛ شرکت‌ها به دنبال استعدادهای این حوزه هستند【565810706922104†L20-L77】.", "تجربه در یادگیری ماشین، علم داده و استفاده از ابزارهای داده مانند Hadoop و Spark امتیاز را افزایش می‌دهد.", false, 80, "دانشمند داده و تحلیل داده" },
                    { 100006L, 10L, "با رشد اقتصاد دیجیتال هند، شرکت‌ها به تحلیل‌گران کسب‌وکار برای تصمیم‌گیری داده‌محور و بهبود فرایندها نیاز دارند.", "تجربه در تحلیل فرآیند، مدیریت داده و تسلط به ابزارهای BI امتیاز را افزایش می‌دهد.", false, 75, "تحلیلگر کسب و کار" },
                    { 100007L, 10L, "پروژه‌های فناوری و عمرانی هند نیاز به مدیران پروژه ماهر دارند؛ بازار کار این شغل در حال رشد است.", "گواهی PMP و تجربه مدیریت تیم‌های بزرگ و چندفرهنگی امتیاز را افزایش می‌دهد.", false, 75, "مدیر پروژه" },
                    { 100008L, 10L, "با توسعهٔ اینترنت و دیجیتال‌سازی، تقاضا برای متخصصان امنیت سایبری و شبکه در هند افزایش یافته است【565810706922104†L20-L77】.", "گواهی‌های امنیتی و تجربه در مدیریت شبکه و امنیت سایبری امتیاز را افزایش می‌دهد.", false, 80, "متخصص شبکه و امنیت" },
                    { 100009L, 10L, "برنامه‌های دولت برای ارتقای تولید داخلی (Make in India) به مهندسان مکانیک و تولید فرصت‌های بیشتری داده است؛ این رشته‌ها همچنان پرتقاضا هستند.", "تجربه در خطوط تولید و استفاده از نرم‌افزارهای CAD/CAM و دانش مهندسی مکانیک امتیاز را افزایش می‌دهد.", false, 75, "مهندس مکانیک و تولید" },
                    { 100010L, 10L, "سیستم آموزشی هند، به‌خصوص در روستاها و مناطق محروم، به معلمان نیاز دارد؛ با این حال حقوق و امکانات ممکن است محدود باشد.", "مدرک تربیت‌معلم و تسلط به زبان‌های محلی و انگلیسی امتیاز را افزایش می‌دهد.", false, 60, "معلم و آموزش" },
                    { 100011L, 10L, "شرکت‌ها و موسسات مالی در هند به حسابداران و متخصصان مالی نیاز دارند؛ بازار کار این حوزه نسبتاً رقابتی است.", "دارا بودن مدارک حرفه‌ای حسابداری (CA/CPA) و تجربه در مالیات و حسابرسی امتیاز را افزایش می‌دهد.", false, 65, "حسابدار و امور مالی" },
                    { 100012L, 10L, "مدیران منابع انسانی در شرکت‌های هندی مسئول مدیریت کارکنان و ایجاد فرهنگ سازمانی هستند؛ این شغل جایگاه متوسطی دارد.", "تجربه در حل تعارضات و دانش قانون کار هند امتیاز را افزایش می‌دهد.", false, 70, "مدیر منابع انسانی" },
                    { 100013L, 10L, "با رشد سریع استارتاپ‌ها و تجارت الکترونیک در هند، نیاز به متخصصان بازاریابی دیجیتال و مدیریت شبکه‌های اجتماعی افزایش یافته است【565810706922104†L20-L77】.", "تجربه در کمپین‌های تبلیغاتی دیجیتال و آشنایی با پلتفرم‌های بازاریابی امتیاز را افزایش می‌دهد.", false, 75, "متخصص بازاریابی و دیجیتال مارکتینگ" },
                    { 100014L, 10L, "هند به پژوهشگران در حوزه‌های علوم پایه و کاربردی، به‌ویژه در فناوری‌های نوین و علوم پزشکی، نیاز دارد؛ با این حال امکانات پژوهشی محدود است.", "داشتن مدرک دکتری و انتشار مقاله در مجلات علمی بین‌المللی امتیاز را افزایش می‌دهد.", false, 78, "پژوهشگر علمی و دانشگاهی" },
                    { 100015L, 10L, "صنعت گردشگری و رستوران‌داری هند در حال رشد است؛ آشپزها و کارکنان خدمات فرصت‌های شغلی دارند ولی رقابت بالاست.", "تجربه در آشپزی و سرویس‌دهی و مهارت در غذاهای هندی و بین‌المللی امتیاز را افزایش می‌دهد.", false, 65, "آشپز و خدمات رستوران" }
                });

            migrationBuilder.InsertData(
                table: "CountryLivingCosts",
                columns: new[] { "Id", "CountryId", "IsDeleted", "Type", "Value" },
                values: new object[,]
                {
                    { 1001L, 1L, false, 1, "هزینه ماهانه خانواده چهار نفره (بدون اجاره): ۵٬۲۲۴٫۱ دلار کانادا ≈ ۳٬۷۳۸ دلار آمریکا" },
                    { 1002L, 1L, false, 2, "هزینه ماهانه یک نفر (بدون اجاره): ۱٬۴۳۲٫۷ دلار کانادا ≈ ۱٬۰۲۵ دلار آمریکا" },
                    { 1003L, 1L, false, 3, "حمل‌ونقل: کارت ماهانه حدود ۱۰۴٫۵ C$ (≈۷۴٫۸ $)" },
                    { 1004L, 1L, false, 4, "برق، آب و گاز برای آپارتمان ۸۵ مترمربع: حدود ۲۰۸٫۷۸ C$ (≈۱۴۹٫۴ $)" },
                    { 1005L, 1L, false, 5, "تفریح: عضویت باشگاه ۵۹٫۰۱ C$ (≈۴۲٫۲ $) و بلیط سینما ۱۵٫۸۲ C$ (≈۱۱٫۳ $)" },
                    { 1006L, 1L, false, 6, "میانگین اجاره آپارتمان ۱ خوابه در سه شهر بزرگ: ≈ ۲٬۲۵۱ C$ (≈۱٬۶۱۱ $)" },
                    { 1007L, 1L, false, 7, "میانگین اجاره آپارتمان ۳ خوابه در سه شهر بزرگ: ≈ ۳٬۸۴۱ C$ (≈۲٬۷۴۹ $)" },
                    { 1008L, 1L, false, 8, "اینترنت نامحدود: حدود ۸۵٫۶۱ C$ (≈۶۱٫۳ $) در ماه" },
                    { 2001L, 2L, false, 1, "هزینه ماهانه خانواده چهار نفره (بدون اجاره): ۵٬۶۹۴٫۵ دلار استرالیا ≈ ۳٬۷۳۲ دلار آمریکا" },
                    { 2002L, 2L, false, 2, "هزینه ماهانه یک نفر (بدون اجاره): ۱٬۵۸۰٫۵ دلار استرالیا ≈ ۱٬۰۳۶ دلار آمریکا" },
                    { 2003L, 2L, false, 3, "حمل‌ونقل: کارت ماهانه حدود ۱۴۰ A$ (≈۹۱٫۸ $)" },
                    { 2004L, 2L, false, 4, "برق، آب، گاز و زباله برای آپارتمان ۸۵ مترمربع: حدود ۲۷۴٫۴۲ A$ (≈۱۷۹٫۹ $)" },
                    { 2005L, 2L, false, 5, "تفریح: عضویت باشگاه ≈ ۷۳ A$ (≈۴۷٫۸ $) و بلیط سینما ۲۰ A$ (≈۱۳٫۱ $)" },
                    { 2006L, 2L, false, 6, "میانگین اجاره آپارتمان ۱ خوابه در سه شهر بزرگ: ≈ ۲٬۷۴۳ A$ (≈۱٬۷۹۸ $)" },
                    { 2007L, 2L, false, 7, "میانگین اجاره آپارتمان ۳ خوابه در سه شهر بزرگ: ≈ ۵٬۲۶۸ A$ (≈۳٬۴۵۳ $)" },
                    { 2008L, 2L, false, 8, "اینترنت نامحدود: حدود ۸۲٫۱۷ A$ (≈۵۳٫۹ $) در ماه" },
                    { 3001L, 3L, false, 1, "هزینه ماهانه خانواده چهار نفره (بدون اجاره): ۳٬۴۰۷٫۶ یورو ≈ ۳٬۹۵۲ $" },
                    { 3002L, 3L, false, 2, "هزینه ماهانه یک نفر (بدون اجاره): ۹۸۵٫۶ یورو ≈ ۱٬۱۴۳ $" },
                    { 3003L, 3L, false, 3, "حمل‌ونقل: کارت ماهانه حدود ۵۸ € (≈۶۷٫۳ $)" },
                    { 3004L, 3L, false, 4, "برق، آب و گاز برای آپارتمان ۸۵ مترمربع: حدود ۳۰۳٫۰۸ € (≈۳۵۱٫۶ $)" },
                    { 3005L, 3L, false, 5, "تفریح: عضویت باشگاه ۳۵٫۴۲ € (≈۴۱٫۱ $) و بلیط سینما ۱۲ € (≈۱۳٫۹ $)" },
                    { 3006L, 3L, false, 6, "میانگین اجاره آپارتمان ۱ خوابه در سه شهر بزرگ: ≈ ۱٬۲۹۹ € (≈۱٬۵۰۶ $)" },
                    { 3007L, 3L, false, 7, "میانگین اجاره آپارتمان ۳ خوابه در سه شهر بزرگ: ≈ ۲٬۴۴۵ € (≈۲٬۸۳۶ $)" },
                    { 3008L, 3L, false, 8, "اینترنت نامحدود: حدود ۴۳٫۲۲ € (≈۵۰٫۱ $) در ماه" },
                    { 4001L, 4L, false, 1, "هزینه ماهانه خانواده چهار نفره (بدون اجاره): ۴٬۲۳۱٫۳ دلار آمریکا" },
                    { 4002L, 4L, false, 2, "هزینه ماهانه یک نفر (بدون اجاره): ۱٬۱۷۳٫۴ دلار آمریکا" },
                    { 4003L, 4L, false, 3, "حمل‌ونقل: کارت ماهانه حمل‌ونقل عمومی حدود ۶۵ $" },
                    { 4004L, 4L, false, 4, "برق، آب و گاز برای آپارتمان ۸۵ مترمربع: حدود ۲۱۰٫۳۷ $" },
                    { 4005L, 4L, false, 5, "تفریح: عضویت باشگاه ۴۵٫۵۱ $ و بلیط سینما ۱۵ $" },
                    { 4006L, 4L, false, 6, "میانگین اجاره آپارتمان ۱ خوابه در نیویورک، لس‌آنجلس و شیکاگو: ≈ ۳٬۰۷۲ $" },
                    { 4007L, 4L, false, 7, "میانگین اجاره آپارتمان ۳ خوابه در سه شهر: ≈ ۵٬۸۸۶ $" },
                    { 4008L, 4L, false, 8, "اینترنت نامحدود: ۷۲٫۴۳ $ در ماه" },
                    { 5001L, 5L, false, 1, "هزینه ماهانه خانواده چهار نفره (بدون اجاره): ۳٬۶۲۱٫۸ یورو ≈ ۴٬۲۰۱ $" },
                    { 5002L, 5L, false, 2, "هزینه ماهانه یک نفر (بدون اجاره): ۱٬۰۱۳٫۹ یورو ≈ ۱٬۱۷۶ $" },
                    { 5003L, 5L, false, 3, "حمل‌ونقل: کارت ماهانه ۹۰ € (≈۱۰۴٫۴ $)" },
                    { 5004L, 5L, false, 4, "برق، آب و گاز: حدود ۲۲۶٫۷ € (≈۲۶۲٫۹ $)" },
                    { 5005L, 5L, false, 5, "تفریح: عضویت باشگاه ۳۷٫۰۵ € (≈۴۳٫۰ $) و بلیط سینما ۱۴ € (≈۱۶٫۲ $)" },
                    { 5006L, 5L, false, 6, "میانگین اجاره آپارتمان ۱ خوابه در آمستردام، روتردام و لاهه: ≈ ۱٬۷۰۲ € (≈۱٬۹۷۴ $)" },
                    { 5007L, 5L, false, 7, "میانگین اجاره آپارتمان ۳ خوابه در سه شهر: ≈ ۲٬۹۱۵ € (≈۳٬۳۸۲ $)" },
                    { 5008L, 5L, false, 8, "اینترنت نامحدود: ۴۳٫۰۷ € (≈۵۰٫۰ $) در ماه" },
                    { 6001L, 6L, false, 1, "هزینه ماهانه خانواده چهار نفره (بدون اجاره): ۲٬۵۵۳٫۷ یورو ≈ ۲٬۹۶۲ $" },
                    { 6002L, 6L, false, 2, "هزینه ماهانه یک نفر (بدون اجاره): ۷۰۶٫۷ یورو ≈ ۸۱۹٫۸ $" },
                    { 6003L, 6L, false, 3, "حمل‌ونقل: کارت ماهانه ۳۰ € (≈۳۴٫۸ $)" },
                    { 6004L, 6L, false, 4, "برق، آب و گاز: حدود ۱۳۱٫۶۶ € (≈۱۵۲٫۷ $)" },
                    { 6005L, 6L, false, 5, "تفریح: عضویت باشگاه ۴۰٫۶۶ € (≈۴۷٫۲ $) و بلیط سینما ۸ € (≈۹٫۳ $)" },
                    { 6006L, 6L, false, 6, "میانگین اجاره آپارتمان ۱ خوابه در مادرید، بارسلونا و والنسیا: ≈ ۱٬۲۴۸ € (≈۱٬۴۴۷ $)" },
                    { 6007L, 6L, false, 7, "میانگین اجاره آپارتمان ۳ خوابه در سه شهر: ≈ ۲٬۰۸۱ € (≈۲٬۴۱۴ $)" },
                    { 6008L, 6L, false, 8, "اینترنت نامحدود: حدود ۲۹٫۰۱ € (≈۳۳٫۷ $) در ماه" },
                    { 7001L, 7L, false, 1, "هزینه ماهانه خانواده چهار نفره (بدون اجاره): ۳۷٬۹۱۸٫۹ کرون سوئد ≈ ۴٬۰۱۲ دلار آمریکا" },
                    { 7002L, 7L, false, 2, "هزینه ماهانه یک نفر (بدون اجاره): ۱۰٬۲۱۶٫۳ کرون ≈ ۱٬۰۸۱ دلار آمریکا" },
                    { 7003L, 7L, false, 3, "حمل‌ونقل: کارت ماهانه ۸۶۰ kr (≈۹۱٫۰ $)" },
                    { 7004L, 7L, false, 4, "برق، آب و گاز: حدود ۱٬۲۴۱٫۵۸ kr (≈۱۳۱٫۴ $)" },
                    { 7005L, 7L, false, 5, "تفریح: عضویت باشگاه ۳۹۵٫۱۲ kr (≈۴۱٫۸ $) و بلیط سینما ۱۵۹ kr (≈۱۶٫۸ $)" },
                    { 7006L, 7L, false, 6, "میانگین اجاره آپارتمان ۱ خوابه در استکهلم، مالمو و گوتنبرگ: ≈ ۱۰٬۹۵۴ kr (≈۱٬۱۵۹ $)" },
                    { 7007L, 7L, false, 7, "میانگین اجاره آپارتمان ۳ خوابه در سه شهر: ≈ ۱۹٬۹۶۱ kr (≈۲٬۱۱۲ $)" },
                    { 7008L, 7L, false, 8, "اینترنت نامحدود: ۳۷۷٫۶۳ kr (≈۴۰٫۰ $) در ماه" },
                    { 8001L, 8L, false, 1, "هزینه ماهانه خانواده چهار نفره (بدون اجاره): ۳٬۱۰۸٫۱ یورو ≈ ۳٬۶۰۵ $" },
                    { 8002L, 8L, false, 2, "هزینه ماهانه یک نفر (بدون اجاره): ۸۶۹٫۷ یورو ≈ ۱٬۰۰۹ $" },
                    { 8003L, 8L, false, 3, "حمل‌ونقل: کارت ماهانه حدود ۳۸ € (≈۴۴٫۱ $)" },
                    { 8004L, 8L, false, 4, "برق، آب، گاز و زباله برای آپارتمان ۸۵ مترمربع: حدود ۱۹۵٫۸۶ € (≈۲۲۷٫۲ $)" },
                    { 8005L, 8L, false, 5, "تفریح: عضویت باشگاه ۴۹٫۳۴ € (≈۵۷٫۲ $) و بلیط سینما ۹ € (≈۱۰٫۴ $)" },
                    { 8006L, 8L, false, 6, "میانگین اجاره آپارتمان ۱ خوابه در رم، میلان و ناپل: ≈ ۱٬۲۲۲ € (≈۱٬۴۱۷ $)" },
                    { 8007L, 8L, false, 7, "میانگین اجاره آپارتمان ۳ خوابه در سه شهر: ≈ ۲٬۳۶۹ € (≈۲٬۷۴۸ $)" },
                    { 8008L, 8L, false, 8, "اینترنت نامحدود: ۲۷٫۱۸ € (≈۳۱٫۵ $) در ماه" },
                    { 9001L, 9L, false, 1, "هزینه ماهانه خانواده چهار نفره (بدون اجاره): ۱٬۰۲۲٫۵ ﷼ عمان ≈ ۲٬۶۵۵ دلار آمریکا" },
                    { 9002L, 9L, false, 2, "هزینه ماهانه یک نفر (بدون اجاره): ۲۸۷٫۱ ﷼ عمان ≈ ۷۴۶ دلار آمریکا" },
                    { 9003L, 9L, false, 3, "حمل‌ونقل: کارت ماهانه ۲۵ ﷼ (≈۶۴٫۹ $)" },
                    { 9004L, 9L, false, 4, "برق، آب و گاز: حدود ۳۸٫۸۸ ﷼ (≈۱۰۱٫۰ $)" },
                    { 9005L, 9L, false, 5, "تفریح: عضویت باشگاه ۲۰٫۰۳ ﷼ (≈۵۲٫۰ $) و بلیط سینما ۴ ﷼ (≈۱۰٫۴ $)" },
                    { 9006L, 9L, false, 6, "میانگین اجاره آپارتمان ۱ خوابه در مسقط، صلاله و صحار: ≈ ۱۶۱٫۳ ﷼ (≈۴۱۹ $)" },
                    { 9007L, 9L, false, 7, "میانگین اجاره آپارتمان ۳ خوابه در سه شهر: ≈ ۳۱۴٫۷ ﷼ (≈۸۱۷ $)" },
                    { 9008L, 9L, false, 8, "اینترنت نامحدود: ۲۷٫۷۴ ﷼ (≈۷۲٫۰ $) در ماه" },
                    { 10001L, 10L, false, 1, "هزینه ماهانه خانواده چهار نفره (بدون اجاره): ۹۸٬۷۷۹٫۵ روپیه ≈ ۱٬۱۰۵ دلار آمریکا" },
                    { 10002L, 10L, false, 2, "هزینه ماهانه یک نفر (بدون اجاره): ۲۷٬۵۱۸٫۲ روپیه ≈ ۳۰۸ دلار آمریکا" },
                    { 10003L, 10L, false, 3, "حمل‌ونقل: کارت ماهانه حمل‌ونقل عمومی حدود ۷۶۴ ₹ (≈۸٫۵ $)" },
                    { 10004L, 10L, false, 4, "برق، آب و گاز برای آپارتمان ۸۵ مترمربع: حدود ۳٬۵۱۶٫۳۵ ₹ (≈۳۹٫۳ $)" },
                    { 10005L, 10L, false, 5, "تفریح: عضویت باشگاه ۱٬۳۶۷٫۱۳ ₹ (≈۱۵٫۳ $) و بلیط سینما ۳۰۰ ₹ (≈۳٫۴ $)" },
                    { 10006L, 10L, false, 6, "میانگین اجاره آپارتمان ۱ خوابه در بمبئی، دهلی و بنگلور: ≈ ۳۷٬۴۳۷ ₹ (≈۴۱۸٫۹ $)" },
                    { 10007L, 10L, false, 7, "میانگین اجاره آپارتمان ۳ خوابه در سه شهر: ≈ ۱۰۰٬۱۲۵ ₹ (≈۱٬۱۲۰٫۴ $)" },
                    { 10008L, 10L, false, 8, "اینترنت نامحدود: حدود ۶۸۴٫۶ ₹ (≈۷٫۷ $) در ماه" }
                });

            migrationBuilder.InsertData(
                table: "CountryRestrictions",
                columns: new[] { "Id", "CountryId", "Description", "IsDeleted" },
                values: new object[,]
                {
                    { 101L, 1L, "بر اساس قانون مهاجرت کانادا، افرادی که به دلایل امنیتی، نقض حقوق بشر، جرایم یا تحت تحریم هستند ممکن است غیرقابل پذیرش شناخته شوند؛ در سال‌های اخیر رژیم ایران به عنوان ناقض حقوق بشر تحت قانون معرفی شده و برخی مقامات ارشد رژیم به همین دلیل از ورود به کانادا منع شده‌اند【129470507908352†L44-L61】【129470507908352†L107-L124】.", false },
                    { 102L, 1L, "ادارهٔ مهاجرت کانادا تأکید می‌کند که همهٔ متقاضیان ایرانی باید معیارهای پذیرش از جمله قانون‌مداری، سلامت، عدم خطر امنیتی و داشتن تمکن مالی کافی را رعایت کنند و تنها در صورت تأمین این شرایط می‌توانند وضعیت اقامت خود را تمدید کنند【923856638287588†L55-L175】.", false },
                    { 201L, 2L, "سایت رسمی وزارت کشور استرالیا اعلام کرده که به دلیل تعطیلی موقت سفارت این کشور در تهران، ایرانیان باید درخواست‌های ویزا را به صورت آنلاین ارائه دهند؛ درخواست‌ها بر اساس معیارهای سلامت، شخصیت و امنیتی ارزیابی می‌شود و ممکن است به ارائهٔ بیومتریک نیاز باشد【65972592913445†L149-L187】.", false },
                    { 202L, 2L, "با وجود عدم ممنوعیت رسمی برای ایرانیان، فرایند بررسی ویزا ممکن است به دلیل بررسی‌های امنیتی طولانی‌تر شود و متقاضیان باید مدارک کاملی مانند گواهی عدم سوءپیشینه، آزمایش‌های پزشکی و اسناد مالی ارائه دهند.", false },
                    { 301L, 3L, "به‌دلیل کاهش ظرفیت سفارت آلمان در تهران، وقت‌دهی برای ویزاهای کاری و تخصصی بسیار محدود شده و پرونده‌های ایرانیان با تأخیر طولانی بررسی می‌شوند؛ اولویت به ویزاهای خانوادگی و انسان‌دوستانه داده می‌شود【974668712062734†L41-L63】.", false },
                    { 302L, 3L, "برای دریافت اجازهٔ اقامت خوداشتغالی، متقاضیان باید طرح تجاری معتبر، تضمین مالی و مدارک امنیتی ارائه دهند؛ عدم ارائهٔ این مدارک می‌تواند منجر به رد درخواست شود【496840517904274†L302-L313】.", false },
                    { 401L, 4L, "یک فرمان ریاست‌جمهوری ایالات متحده در ۴ ژوئن ۲۰۲۵ ورود اتباع ایران را به عنوان مهاجر و غیرمهاجر به‌دلیل نگرانی‌های امنیتی و حمایت دولتی از تروریسم تعلیق کرده است【652412899581844†L326-L335】.", false },
                    { 402L, 4L, "خدمت یا عضویت در سپاه پاسداران انقلاب اسلامی می‌تواند موجب عدم واجد شرایط بودن برای ویزا یا ورود به آمریکا شود و برای ویزاهای مهاجرتی هیچ بخشودگی وجود ندارد【594094986034718†L369-L385】.", false },
                    { 403L, 4L, "متقاضیان ایرانی باید در مصاحبهٔ حضوری و مراحل بررسی اداری شرکت کنند و ممکن است به دلیل بررسی‌های امنیتی، روند پردازش ویزا طولانی‌تر باشد【594094986034718†L288-L329】.", false },
                    { 501L, 5L, "سفارت هلند در تهران به‌دلیل وضعیت امنیتی تنها خدمات محدودی مانند دریافت استیکر MVV، آزمون هم‌پیوندی و گذرنامه ارائه می‌دهد و وقت‌ها بسیار محدود است؛ این شرایط موجب تاخیر در رسیدگی به درخواست‌های ایرانیان می‌شود【244439518350853†L95-L104】【244439518350853†L184-L188】.", false },
                    { 502L, 5L, "در مرکز VFS Global تهران امکان درخواست ویزای گردشگری برای هلند وجود ندارد و فقط درخواست‌های خاص مانند دیدار خانواده و تجارت پذیرفته می‌شود【348312902467940†L40-L42】.", false },
                    { 601L, 6L, "برنامهٔ ویزای سرمایه‌گذاری اسپانیا (گلدن ویزا) در آوریل ۲۰۲۵ لغو شد و بنابراین مسیر سرمایه‌گذاری برای اتباع خارجی از جمله ایرانیان بسته شده است【810607001739387†L84-L124】.", false },
                    { 602L, 6L, "سایت مرکز خدمات ویزای اسپانیا در ایران هشدار می‌دهد که وقت‌های ویزا باید فقط از طریق سایت رسمی رزرو شود و پرداخت به دلالان ممنوع است؛ عدم رعایت این موارد می‌تواند باعث مسدود شدن دسترسی به وقت‌ها شود【791952424432128†L124-L172】.", false },
                    { 701L, 7L, "سفارت سوئد در تهران اعلام کرده که متقاضیان ایرانی باید مدارک کامل شامل گذرنامه، فرم درخواست، اثر انگشت، هدف سفر، محل اقامت و تمکن مالی را ارائه کنند و در صورت نقص مدارک، درخواست پذیرفته نمی‌شود【473232319044766†L151-L182】.", false },
                    { 702L, 7L, "به دلیل حجم بالای درخواست‌ها و ضرورت ثبت بیومتریک، زمان انتظار برای صدور ویزاهای سوئد برای ایرانیان ممکن است طولانی شود و متقاضیان باید زودتر از موعد برنامه‌ریزی کنند.", false },
                    { 801L, 8L, "سفارت ایتالیا در تهران در ۲۴ ژوئن ۲۰۲۵ به دلیل شرایط امنیتی، خدمات کنسولی خود را به طور موقت به حالت تعلیق درآورد؛ این وضعیت باعث محدود شدن دسترسی و افزایش تأخیر در بررسی ویزاها شده و در حال حاضر تمرکز بر پرونده‌های دانشجویان ایرانی پذیرفته‌شده است【22804495354170†L179-L190】.", false },
                    { 802L, 8L, "به دلیل تعلیق خدمات در تهران، متقاضیان ایرانی ممکن است مجبور شوند از طریق سایر سفارت‌های ایتالیا در منطقه درخواست دهند و انتظار برای دریافت وقت سفارت و پردازش ویزا طولانی‌تر است.", false },
                    { 901L, 9L, "برای دریافت اقامت سرمایه‌گذاری عمان، متقاضی باید حداقل ۲۰۰٬۰۰۰ ریال عمانی سرمایه‌گذاری کند و از طریق وزارت تجارت درخواست دهد؛ همچنین سن متقاضی باید حداقل ۲۱ سال باشد و گواهی عدم سوءپیشینه ارائه دهد【309023576662590†L44-L98】.", false },
                    { 902L, 9L, "عمان ورود گردشگران ایرانی را بدون ویزا تا ۱۰ روز مجاز کرده است، اما مسافران باید رزرو هتل، بیمه درمانی و بلیط برگشت داشته باشند؛ عدم رعایت این شرایط موجب جریمهٔ روزانه می‌شود【237265316935786†L33-L43】.", false },
                    { 1001L, 10L, "اتباع ایرانی باید درخواست ویزای هند را به‌صورت آنلاین تکمیل کنند و سپس نسخهٔ چاپی فرم را همراه با پاسپورت و مدارک لازم به مرکز درخواست کنسولی هند در ایران (ICAC) تحویل دهند؛ فرم‌ها باید به زبان انگلیسی تکمیل شوند و فرم‌های ناقص یا دارای اطلاعات غلط توسط مرکز یا سفارت رد می‌شوند【909243431442982†L692-L706】.", false },
                    { 1002L, 10L, "هر درخواست ویزا به‌صورت موردی بررسی می‌شود و سفارت ممکن است در هر مرحله از روند بررسی اطلاعات اضافی مطالبه کند که می‌تواند زمان پردازش را افزایش دهد؛ ارائهٔ درخواست و پرداخت هزینه تضمینی برای دریافت ویزا نیست و درخواست صدور زودتر ویزا پذیرفته نمی‌شود【909243431442982†L705-L715】.", false },
                    { 1003L, 10L, "زمان صدور ویزا بسته به نوع ویزا متفاوت است: ویزای تجاری معمولاً طی ۳ روز کاری، ویزای توریستی و کنفرانس طی ۴ روز کاری و سایر ویزاها (از جمله پژوهشی) برای متقاضیان دارای ملیت ایرانی در صورت تکمیل مدارک طی ۷ روز کاری صادر می‌شود【909243431442982†L719-L723】.", false },
                    { 1004L, 10L, "برای دریافت اقامت دائم سرمایه‌گذار، متقاضیان باید دست‌کم ۱۰ کرور روپیه ظرف ۱۸ ماه یا ۲۵ کرور روپیه ظرف ۳۶ ماه سرمایه‌گذاری کنند و حداقل ۲۰ شغل ایجاد کنند【147254951936675†screenshot】. این شرط سرمایه‌گذاری برای بسیاری از ایرانیان بسیار بالا است و یکی از موانع مهم محسوب می‌شود.", false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleComment_ArticleId",
                table: "ArticleComment",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleComment_UserId",
                table: "ArticleComment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Key",
                table: "Countries",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CountryEducations_CountryId",
                table: "CountryEducations",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryJobs_CountryId",
                table: "CountryJobs",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryLivingCosts_CountryId",
                table: "CountryLivingCosts",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryRestrictions_CountryId",
                table: "CountryRestrictions",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodes_Code",
                table: "DiscountCodes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImmigrationEvaluations_CreatedAtUtc",
                table: "ImmigrationEvaluations",
                column: "CreatedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_ImmigrationEvaluations_CustomerId",
                table: "ImmigrationEvaluations",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRequests_CustomerId_Status",
                table: "PaymentRequests",
                columns: new[] { "CustomerId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRequests_GatewayTrackId",
                table: "PaymentRequests",
                column: "GatewayTrackId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentResponses_PaymentRequestId",
                table: "PaymentResponses",
                column: "PaymentRequestId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Role",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_ParentId",
                table: "RolePermission",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleId",
                table: "RolePermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "User",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleComment");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CountryEducations");

            migrationBuilder.DropTable(
                name: "CountryJobs");

            migrationBuilder.DropTable(
                name: "CountryLivingCosts");

            migrationBuilder.DropTable(
                name: "CountryRestrictions");

            migrationBuilder.DropTable(
                name: "DiscountCodes");

            migrationBuilder.DropTable(
                name: "ImmigrationEvaluations");

            migrationBuilder.DropTable(
                name: "PaymentResponses");

            migrationBuilder.DropTable(
                name: "RolePermission");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Article");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "PaymentRequests");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
