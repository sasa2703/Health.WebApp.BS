using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Health.WebApp.BS.Repository.Migrations
{
    /// <inheritdoc />
    public partial class first_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HealthCollection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayIndex = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthCollection", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserCategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Health",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrialId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Participants = table.Column<int>(type: "int", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    ClinicalTrialsCollectionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Health", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Health_HealthCollection_ClinicalTrialsCollectionId",
                        column: x => x.ClinicalTrialsCollectionId,
                        principalTable: "HealthCollection",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubscriptionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DtCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DtLastUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IUserCategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscription_UserCategory_IUserCategoryId",
                        column: x => x.IUserCategoryId,
                        principalTable: "UserCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApiAccessToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubscriptionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubscriptionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoginId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DtCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DtExpireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KeyVaultSecretId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubscriptionId1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiAccessToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiAccessToken_Subscription_SubscriptionId1",
                        column: x => x.SubscriptionId1,
                        principalTable: "Subscription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubscriptionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserCategory = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeZone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    DtCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DtLastUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserCategoryNavigationId = table.Column<int>(type: "int", nullable: false),
                    SubscriptionId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Subscription_SubscriptionId1",
                        column: x => x.SubscriptionId1,
                        principalTable: "Subscription",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_User_UserCategory_UserCategoryNavigationId",
                        column: x => x.UserCategoryNavigationId,
                        principalTable: "UserCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_UserStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "UserStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiAccessToken_SubscriptionId1",
                table: "ApiAccessToken",
                column: "SubscriptionId1");

            migrationBuilder.CreateIndex(
                name: "IX_Health_ClinicalTrialsCollectionId",
                table: "Health",
                column: "ClinicalTrialsCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_IUserCategoryId",
                table: "Subscription",
                column: "IUserCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_User_StatusId",
                table: "User",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_User_SubscriptionId1",
                table: "User",
                column: "SubscriptionId1");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserCategoryNavigationId",
                table: "User",
                column: "UserCategoryNavigationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiAccessToken");

            migrationBuilder.DropTable(
                name: "Health");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "HealthCollection");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "UserStatus");

            migrationBuilder.DropTable(
                name: "UserCategory");
        }
    }
}
