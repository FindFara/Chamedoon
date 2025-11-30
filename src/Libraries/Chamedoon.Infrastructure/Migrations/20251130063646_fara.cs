using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Chamedoon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fara : Migration
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
                    ArticleTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Writer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArticleDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ReaedAdmin = table.Column<bool>(type: "bit", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArticleId1 = table.Column<long>(type: "bigint", nullable: false),
                    UserId1 = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleComment_Article_ArticleId1",
                        column: x => x.ArticleId1,
                        principalTable: "Article",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleComment_User_UserId1",
                        column: x => x.UserId1,
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
                    InvestmentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JobCategory = table.Column<int>(type: "int", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    WorkExperienceYears = table.Column<int>(type: "int", nullable: false),
                    FieldCategory = table.Column<int>(type: "int", nullable: false),
                    DegreeLevel = table.Column<int>(type: "int", nullable: false),
                    LanguageCertificate = table.Column<int>(type: "int", nullable: false),
                    WillingToStudy = table.Column<bool>(type: "bit", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
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
                name: "IX_ArticleComment_ArticleId1",
                table: "ArticleComment",
                column: "ArticleId1");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleComment_UserId1",
                table: "ArticleComment",
                column: "UserId1");

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
                name: "IX_CountryLivingCosts_CountryId",
                table: "CountryLivingCosts",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryRestrictions_CountryId",
                table: "CountryRestrictions",
                column: "CountryId");

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
                name: "CountryLivingCosts");

            migrationBuilder.DropTable(
                name: "CountryRestrictions");

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
