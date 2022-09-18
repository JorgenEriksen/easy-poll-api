using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyPollAPI.Migrations
{
    public partial class StatusMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasStarted",
                table: "PollGames");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "PollGames",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_PollGames_StatusId",
                table: "PollGames",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_PollGames_PollGameStatusTypes_StatusId",
                table: "PollGames",
                column: "StatusId",
                principalTable: "PollGameStatusTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PollGames_PollGameStatusTypes_StatusId",
                table: "PollGames");

            migrationBuilder.DropTable(
                name: "PollGameStatusTypes");

            migrationBuilder.DropIndex(
                name: "IX_PollGames_StatusId",
                table: "PollGames");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "PollGames");

            migrationBuilder.AddColumn<bool>(
                name: "HasStarted",
                table: "PollGames",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
