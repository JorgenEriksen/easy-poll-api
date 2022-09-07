using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyPollAPI.Migrations
{
    public partial class InviteCodeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InviteCode",
                table: "PollGames",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InviteCode",
                table: "PollGames");
        }
    }
}
