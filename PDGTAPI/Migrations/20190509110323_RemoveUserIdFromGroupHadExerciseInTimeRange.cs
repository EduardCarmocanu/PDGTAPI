using Microsoft.EntityFrameworkCore.Migrations;

namespace PDGTAPI.Migrations
{
    public partial class RemoveUserIdFromGroupHadExerciseInTimeRange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupHasExerciseInTimeRange_AspNetUsers_UserId",
                table: "GroupHasExerciseInTimeRange");

            migrationBuilder.DropIndex(
                name: "IX_GroupHasExerciseInTimeRange_UserId",
                table: "GroupHasExerciseInTimeRange");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GroupHasExerciseInTimeRange");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "GroupHasExerciseInTimeRange",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupHasExerciseInTimeRange_UserId",
                table: "GroupHasExerciseInTimeRange",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupHasExerciseInTimeRange_AspNetUsers_UserId",
                table: "GroupHasExerciseInTimeRange",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
