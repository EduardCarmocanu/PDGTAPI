using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PDGTAPI.Migrations
{
    public partial class RemoveWQSRecommendedWeightExGuide : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Exercise__GuideI__619B8048",
                table: "Exercise");

            migrationBuilder.DropTable(
                name: "Guide");

            migrationBuilder.DropTable(
                name: "WeeklyQuestionnaire");

            migrationBuilder.DropIndex(
                name: "IX_Exercise_GuideID",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "UserExerciseWeight",
                table: "UserHasExerciseWeightInTimeRange");

            migrationBuilder.DropColumn(
                name: "GuideID",
                table: "Exercise");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "UserExerciseWeight",
                table: "UserHasExerciseWeightInTimeRange",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "GuideID",
                table: "Exercise",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Guide",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GuideDescription = table.Column<string>(maxLength: 2048, nullable: false),
                    GuideImage = table.Column<byte[]>(type: "image", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guide", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WeeklyQuestionnaire",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Completed = table.Column<bool>(nullable: false, defaultValue: false),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyQuestionnaire", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WeeklyQuestionnaire_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_GuideID",
                table: "Exercise",
                column: "GuideID");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyQuestionnaire_UserId",
                table: "WeeklyQuestionnaire",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK__Exercise__GuideI__619B8048",
                table: "Exercise",
                column: "GuideID",
                principalTable: "Guide",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
