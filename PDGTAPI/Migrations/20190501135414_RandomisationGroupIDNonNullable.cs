using Microsoft.EntityFrameworkCore.Migrations;

namespace PDGTAPI.Migrations
{
    public partial class RandomisationGroupIDNonNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_RandomisationGroup_RandomisationGroupId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "RandomisationGroupId",
                table: "AspNetUsers",
                newName: "RandomisationGroupID");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_RandomisationGroupId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_RandomisationGroupID");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_RandomisationGroup_RandomisationGroupID",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "RandomisationGroupID",
                table: "AspNetUsers",
                newName: "RandomisationGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_RandomisationGroupID",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_RandomisationGroupId");

            migrationBuilder.AlterColumn<int>(
                name: "RandomisationGroupId",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_RandomisationGroup_RandomisationGroupId",
                table: "AspNetUsers",
                column: "RandomisationGroupId",
                principalTable: "RandomisationGroup",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
