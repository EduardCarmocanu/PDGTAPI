using Microsoft.EntityFrameworkCore.Migrations;

namespace PDGTAPI.Migrations
{
    public partial class AddNullables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_RandomisationGroup_RandomisationGroupID",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "RedCapRecordId",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "RandomisationGroupID",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_RandomisationGroup_RandomisationGroupID",
                table: "AspNetUsers",
                column: "RandomisationGroupID",
                principalTable: "RandomisationGroup",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_RandomisationGroup_RandomisationGroupID",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "RedCapRecordId",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RandomisationGroupID",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_RandomisationGroup_RandomisationGroupID",
                table: "AspNetUsers",
                column: "RandomisationGroupID",
                principalTable: "RandomisationGroup",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
