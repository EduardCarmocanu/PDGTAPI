using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PDGTAPI.Migrations
{
    public partial class NUllableGuideImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "GuideImage",
                table: "Guide",
                type: "image",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "image");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "GuideImage",
                table: "Guide",
                type: "image",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "image",
                oldNullable: true);
        }
    }
}
