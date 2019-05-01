using Microsoft.EntityFrameworkCore.Migrations;

namespace PDGTAPI.Migrations
{
    public partial class RemoveRandomisationGroupAndNullableColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RandomisationGroup",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<byte>(
                name: "UserExerciseWeight",
                table: "UserHasExerciseWeightInTimeRange",
                nullable: false,
                oldClrType: typeof(byte),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RedCapRecordId",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "UserExerciseWeight",
                table: "UserHasExerciseWeightInTimeRange",
                nullable: true,
                oldClrType: typeof(byte));

            migrationBuilder.AlterColumn<int>(
                name: "RedCapRecordId",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "RandomisationGroup",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");
        }
    }
}
