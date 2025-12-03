using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BulkyBook.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addsubmittedAt1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AncestralPositions",
                columns: table => new
                {
                    AncestralPositionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Side = table.Column<string>(type: "TEXT", nullable: true),
                    Section = table.Column<string>(type: "TEXT", nullable: true),
                    Level = table.Column<string>(type: "TEXT", nullable: true),
                    Position = table.Column<string>(type: "TEXT", nullable: true),
                    Applicant = table.Column<string>(type: "TEXT", nullable: true),
                    Relation = table.Column<string>(type: "TEXT", nullable: true),
                    Mobile_Tel = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<int>(type: "INTEGER", nullable: true),
                    PositionId = table.Column<string>(type: "TEXT", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AncestralPositions", x => x.AncestralPositionId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    StreetAddress = table.Column<string>(type: "TEXT", nullable: true),
                    City = table.Column<string>(type: "TEXT", nullable: true),
                    State = table.Column<string>(type: "TEXT", nullable: true),
                    PostalCode = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KindnessPositions",
                columns: table => new
                {
                    KindnessPositionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Floor = table.Column<string>(type: "TEXT", nullable: true),
                    Section = table.Column<string>(type: "TEXT", nullable: true),
                    Level = table.Column<string>(type: "TEXT", nullable: true),
                    Position = table.Column<string>(type: "TEXT", nullable: true),
                    Applicant = table.Column<string>(type: "TEXT", nullable: true),
                    Relation = table.Column<string>(type: "TEXT", nullable: true),
                    Mobile_Tel = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<int>(type: "INTEGER", nullable: true),
                    PositionId = table.Column<string>(type: "TEXT", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KindnessPositions", x => x.KindnessPositionId);
                });

            migrationBuilder.CreateTable(
                name: "SurveyResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LoginTime = table.Column<string>(type: "TEXT", nullable: false),
                    UsageCount = table.Column<string>(type: "TEXT", nullable: false),
                    ErrorCount = table.Column<string>(type: "TEXT", nullable: false),
                    Continent = table.Column<string>(type: "TEXT", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyResponses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 21, nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    StreetAddress = table.Column<string>(type: "TEXT", nullable: true),
                    City = table.Column<string>(type: "TEXT", nullable: true),
                    State = table.Column<string>(type: "TEXT", nullable: true),
                    PostalCode = table.Column<string>(type: "TEXT", nullable: true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ISBN = table.Column<string>(type: "TEXT", nullable: false),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    HDate = table.Column<string>(type: "TEXT", nullable: true),
                    HeldYN = table.Column<char>(type: "TEXT", nullable: false),
                    ListPrice = table.Column<double>(type: "REAL", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApplicationUserId = table.Column<string>(type: "TEXT", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ShippingDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OrderTotal = table.Column<double>(type: "REAL", nullable: false),
                    OrderStatus = table.Column<string>(type: "TEXT", nullable: true),
                    PaymentStatus = table.Column<string>(type: "TEXT", nullable: true),
                    TrackingNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Carrier = table.Column<string>(type: "TEXT", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PaymentDueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SessionId = table.Column<string>(type: "TEXT", nullable: true),
                    PaymentIntentId = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    StreetAddress = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    State = table.Column<string>(type: "TEXT", nullable: false),
                    PostalCode = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderHeaders_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventRegistrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    PaymentTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RegistrationTime = table.Column<string>(type: "TEXT", nullable: false),
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    Table = table.Column<int>(type: "INTEGER", nullable: false),
                    Senior80 = table.Column<int>(type: "INTEGER", nullable: false),
                    Vegetarian = table.Column<int>(type: "INTEGER", nullable: false),
                    Volunteer = table.Column<int>(type: "INTEGER", nullable: false),
                    PreAdult = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalNumberJoined = table.Column<int>(type: "INTEGER", nullable: false),
                    PaymentAmount = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventRegistrations_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    Table = table.Column<int>(type: "INTEGER", nullable: false),
                    Senior80 = table.Column<int>(type: "INTEGER", nullable: false),
                    Volunteer = table.Column<int>(type: "INTEGER", nullable: false),
                    Vegetarian = table.Column<int>(type: "INTEGER", nullable: false),
                    PreAdult = table.Column<int>(type: "INTEGER", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "TEXT", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderHeaderId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    Senior80 = table.Column<int>(type: "INTEGER", nullable: false),
                    Volunteer = table.Column<int>(type: "INTEGER", nullable: false),
                    Vegetarian = table.Column<int>(type: "INTEGER", nullable: false),
                    PreAdult = table.Column<int>(type: "INTEGER", nullable: false),
                    Table = table.Column<int>(type: "INTEGER", nullable: false),
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_OrderHeaders_OrderHeaderId",
                        column: x => x.OrderHeaderId,
                        principalTable: "OrderHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "DisplayOrder", "Name", "SubmittedAt" },
                values: new object[,]
                {
                    { 1, 1, "祭祖、團拜 (新年、清明、中秋、冬至)", new DateTime(2025, 8, 22, 5, 7, 16, 914, DateTimeKind.Utc).AddTicks(636) },
                    { 2, 2, "年度大掃除 (懷恩塔、純篤公墓園、宗祠)", new DateTime(2025, 8, 22, 5, 7, 16, 914, DateTimeKind.Utc).AddTicks(994) },
                    { 3, 3, "參訪、春酒、尾牙", new DateTime(2025, 8, 22, 5, 7, 16, 914, DateTimeKind.Utc).AddTicks(996) }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "City", "Name", "PhoneNumber", "PostalCode", "State", "StreetAddress", "SubmittedAt" },
                values: new object[,]
                {
                    { 1, "Taichung City", "台中市銀同碧湖陳氏宗親會", "6669990000", "12121", "ROC", "123 Tech St", new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(2529) },
                    { 2, "KinMen", "金門湖前陳氏宗親會", "7779990000", "66666", "ROC", "999 Vid St", new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(2867) },
                    { 3, "KinMen", "金門塔后陳氏宗親會\n", "1113335555", "99999", "ROC", "999 Main St", new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(2869) },
                    { 4, "Taina", "台南學甲中洲陳桂記大宗祠\n", "1113335555", "99999", "ROC", "999 Main St", new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(2871) },
                    { 5, "Kao-chung", "高雄巿學甲中洲陳桂記宗親會\n", "1113335555", "99999", "Kaoh", "999 Main St", new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(2874) }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CompanyId", "Description", "HDate", "HeldYN", "ISBN", "ListPrice", "SubmittedAt", "Title" },
                values: new object[,]
                {
                    { 1, 1, 1, "正月初一新春祭祖團拜\r\n\r\n(農曆正月初一,宗祠會館三樓). ", "正月初一", 'N', "正月初一新春祭祖團拜", 0.0, new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(6137), "正月初一新春祭祖團拜\r\n" },
                    { 2, 1, 1, "清明節祭祖大典及聯誼餐會.\r\n\r\n(清明節,宗祠會館). ", "清明節", 'N', "清明節祭祖大典及聯誼餐會", 600.0, new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(6460), "清明節祭祖大典及聯誼餐會" },
                    { 3, 1, 1, "中秋節祭祖大典及聯誼餐會.\r\n\r\n(中秋節前一週日,宗祠會館). ", "中秋節前一週日", 'N', "中秋節祭祖大典及聯誼餐會", 0.0, new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(6463), "中秋節祭祖大典及聯誼餐會" },
                    { 4, 1, 1, "冬至搓湯圓活動.\r\n\r\n(冬至前一週日或週末,宗祠會館). ", "冬至前一週日或週末", 'N', "冬至搓湯圓活動", 0.0, new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(6465), "冬至搓湯圓活動" },
                    { 5, 1, 4, "台南學甲中洲大桂記大宗祠祭祖大典.\r\n\r\n(中秋節後週日或週末). ", "中秋節後週日或週末", 'N', "台南學甲中洲大桂記大宗祠祭祖大典", 0.0, new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(6532), "台南學甲中洲大桂記大宗祠祭祖大典" },
                    { 6, 1, 2, "金門祭祖大典.\r\n\r\n(冬至前一週日或週末). ", "冬至前一週日或週末", 'N', "金門祭祖大典", 0.0, new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(6535), "金門祭祖大典" },
                    { 7, 1, 1, "志工隊年度旅遊活動.\r\n\r\n(活動日期及費用依主辦單位公佈為主). ", "依主辦單位公佈為主", 'N', "志工隊年度旅遊活動", 0.0, new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(6537), "志工隊年度旅遊活動" }
                });

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "ImageUrl", "ProductId", "SubmittedAt" },
                values: new object[,]
                {
                    { 1, "\\images\\products\\product-4\\caca0d62-34b0-47e4-bd22-3d4f439367cf.jpg", 4, new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8485) },
                    { 2, "\\images\\products\\product-1\\06e30a51-4abe-4e4b-9f98-f4235bbd0ac9.jpg", 1, new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8803) },
                    { 3, "\\images\\products\\product-2\\3dcd5a56-fac1-4730-a2f4-7c04baf99689.jpg", 2, new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8805) },
                    { 4, "\\images\\products\\product-3\\142d5816-7199-491c-b606-ca3d1ed5d976.jpg", 3, new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8806) },
                    { 5, "\\images\\products\\product-4\\bc8e2d1b-25d6-4174-b687-a556d871d0d5.jpg", 4, new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8808) },
                    { 6, "\\images\\products\\product-2\\f8bb9335-585b-4ae1-9e3c-6f2cd0af5b4e.jpg", 2, new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8810) },
                    { 7, "\\images\\products\\product-1\\cef81a2a-95fa-4c01-8afb-4cfabba061db.jpg", 1, new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8811) },
                    { 8, "\\images\\products\\product-5\\12eb4930-cf74-4f69-9092-c10e07703d77.jpg", 5, new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8813) },
                    { 9, "\\images\\products\\product-5\\eb17ea7a-91f4-43b5-bc47-58b77559fd8c.jpg", 5, new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8814) },
                    { 10, "\\images\\products\\product-5\\80534b19-a460-49da-b989-cdaabeb322f2.jpg", 5, new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8816) },
                    { 12, "\\images\\products\\product-6\\8951128a-5f5a-40cd-bb98-7a44eace559c.jpg", 6, new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8817) },
                    { 13, "\\images\\products\\product-6\\f207d7b2-bef7-47a4-b92e-d5e064d07969.jpg", 6, new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8819) },
                    { 14, "\\images\\products\\product-6\\b24faf65-a7a8-4a7f-a6d6-111596bfe469.jpg", 6, new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8820) },
                    { 15, "\\images\\products\\product-7\\5be2d6ab-cddf-40fe-b56d-7bf4df9a7744.jpg", 7, new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8822) },
                    { 16, "\\images\\products\\product-7\\8434a454-da5b-496f-8bfc-773e2b9df6d7.jpg", 7, new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8823) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistrations_ProductId_UserId",
                table: "EventRegistrations",
                columns: new[] { "ProductId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderHeaderId",
                table: "OrderDetails",
                column: "OrderHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeaders_ApplicationUserId",
                table: "OrderHeaders",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CompanyId",
                table: "Products",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_ApplicationUserId",
                table: "ShoppingCarts",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_ProductId",
                table: "ShoppingCarts",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AncestralPositions");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "EventRegistrations");

            migrationBuilder.DropTable(
                name: "KindnessPositions");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");

            migrationBuilder.DropTable(
                name: "SurveyResponses");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "OrderHeaders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
