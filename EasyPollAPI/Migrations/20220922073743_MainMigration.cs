using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyPollAPI.Migrations
{
    public partial class MainMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PollGameStatusTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollGameStatusTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PollGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    InviteCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentQuestionOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PollGames_PollGameStatusTypes_StatusId",
                        column: x => x.StatusId,
                        principalTable: "PollGameStatusTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionOrder = table.Column<int>(type: "int", nullable: false),
                    PollGameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_PollGames_PollGameId",
                        column: x => x.PollGameId,
                        principalTable: "PollGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TempUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isAdmin = table.Column<bool>(type: "bit", nullable: false),
                    PollGameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TempUsers_PollGames_PollGameId",
                        column: x => x.PollGameId,
                        principalTable: "PollGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionAlternatives",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlternativeText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAlternatives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionAlternatives_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionAlternativeId = table.Column<int>(type: "int", nullable: false),
                    TempUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAnswers_QuestionAlternatives_QuestionAlternativeId",
                        column: x => x.QuestionAlternativeId,
                        principalTable: "QuestionAlternatives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAnswers_TempUsers_TempUserId",
                        column: x => x.TempUserId,
                        principalTable: "TempUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PollGames_StatusId",
                table: "PollGames",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAlternatives_QuestionId",
                table: "QuestionAlternatives",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_PollGameId",
                table: "Questions",
                column: "PollGameId");

            migrationBuilder.CreateIndex(
                name: "IX_TempUsers_PollGameId",
                table: "TempUsers",
                column: "PollGameId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_QuestionAlternativeId",
                table: "UserAnswers",
                column: "QuestionAlternativeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_TempUserId",
                table: "UserAnswers",
                column: "TempUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAnswers");

            migrationBuilder.DropTable(
                name: "QuestionAlternatives");

            migrationBuilder.DropTable(
                name: "TempUsers");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "PollGames");

            migrationBuilder.DropTable(
                name: "PollGameStatusTypes");
        }
    }
}
