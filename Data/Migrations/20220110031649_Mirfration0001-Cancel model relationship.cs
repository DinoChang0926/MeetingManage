using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetingManage.Migrations
{
    public partial class Mirfration0001Cancelmodelrelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_UserData_user_id",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_user_id",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "Meetings");

            migrationBuilder.AddColumn<string>(
                name: "user_Account",
                table: "Meetings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user_Account",
                table: "Meetings");

            migrationBuilder.AddColumn<long>(
                name: "user_id",
                table: "Meetings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_user_id",
                table: "Meetings",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_UserData_user_id",
                table: "Meetings",
                column: "user_id",
                principalTable: "UserData",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
