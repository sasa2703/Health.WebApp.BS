using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Health.WebApp.BS.Repository.Migrations
{
    /// <inheritdoc />
    public partial class table_removed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Health_HealthCollection_ClinicalTrialsCollectionId",
                table: "Health");

            migrationBuilder.DropTable(
                name: "HealthCollection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Health",
                table: "Health");

            migrationBuilder.DropIndex(
                name: "IX_Health_ClinicalTrialsCollectionId",
                table: "Health");

            migrationBuilder.DropColumn(
                name: "ClinicalTrialsCollectionId",
                table: "Health");

            migrationBuilder.RenameTable(
                name: "Health",
                newName: "ClinicalTrial");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClinicalTrial",
                table: "ClinicalTrial",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ClinicalTrial",
                table: "ClinicalTrial");

            migrationBuilder.RenameTable(
                name: "ClinicalTrial",
                newName: "Health");

            migrationBuilder.AddColumn<int>(
                name: "ClinicalTrialsCollectionId",
                table: "Health",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Health",
                table: "Health",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "HealthCollection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayIndex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthCollection", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Health_ClinicalTrialsCollectionId",
                table: "Health",
                column: "ClinicalTrialsCollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Health_HealthCollection_ClinicalTrialsCollectionId",
                table: "Health",
                column: "ClinicalTrialsCollectionId",
                principalTable: "HealthCollection",
                principalColumn: "Id");
        }
    }
}
