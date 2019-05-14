using Microsoft.EntityFrameworkCore.Migrations;

namespace PDGTAPI.Migrations
{
    public partial class PatientHasDoctor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DoctorID",
                table: "AspNetUsers",
                maxLength: 450,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DoctorID",
                table: "AspNetUsers",
                column: "DoctorID");

            migrationBuilder.AddForeignKey(
                name: "FK__AspNetUse__Docto__01142BA1",
                table: "AspNetUsers",
                column: "DoctorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__AspNetUse__Docto__01142BA1",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DoctorID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DoctorID",
                table: "AspNetUsers");
        }
    }
}
