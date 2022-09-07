using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyPollAPI.Migrations
{
    public partial class CurrentQuestionMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentQuestionNumber",
                table: "PollGames",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentQuestionNumber",
                table: "PollGames");
        }
    }
}
