using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyPollAPI.Migrations
{
    public partial class CurrentQuestionOrderMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentQuestionNumber",
                table: "PollGames",
                newName: "CurrentQuestionOrder");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentQuestionOrder",
                table: "PollGames",
                newName: "CurrentQuestionNumber");
        }
    }
}
