using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PDGTAPI.Migrations
{
    public partial class EntityRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RedCapGroup",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "RandomisationGroup",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RandomisationGroupId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RedCapBaseline",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Guide",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GuideImage = table.Column<byte[]>(type: "image", nullable: false),
                    GuideDescription = table.Column<string>(maxLength: 2048, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guide", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RandomisationGroup",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GroupName = table.Column<string>(unicode: false, maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RandomisationGroup", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Session",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompletionTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Session", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TimeRange",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StartWeek = table.Column<byte>(nullable: false),
                    EndWeek = table.Column<byte>(nullable: false),
                    SetsAmount = table.Column<byte>(nullable: false),
                    RepsAmount = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeRange", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Exercise",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExerciseName = table.Column<string>(maxLength: 255, nullable: false),
                    GuideID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercise", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Exercise__GuideI__619B8048",
                        column: x => x.GuideID,
                        principalTable: "Guide",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupHasExerciseInTimeRange",
                columns: table => new
                {
                    GroupID = table.Column<int>(nullable: false),
                    ExerciseID = table.Column<int>(nullable: false),
                    TimeRangeID = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupHasExerciseInTimeRange", x => new { x.GroupID, x.ExerciseID, x.TimeRangeID });
                    table.ForeignKey(
                        name: "FK__GroupHasE__Exerc__73BA3083",
                        column: x => x.ExerciseID,
                        principalTable: "Exercise",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__GroupHasE__Group__72C60C4A",
                        column: x => x.GroupID,
                        principalTable: "RandomisationGroup",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__GroupHasE__TimeR__74AE54BC",
                        column: x => x.TimeRangeID,
                        principalTable: "TimeRange",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupHasExerciseInTimeRange_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserHasExerciseWeightInTimeRange",
                columns: table => new
                {
                    UserID = table.Column<string>(nullable: false),
                    ExerciseID = table.Column<int>(nullable: false),
                    TimeRangeID = table.Column<int>(nullable: false),
                    UserExerciseWeight = table.Column<byte>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHasExerciseWeightInTimeRange", x => new { x.UserID, x.ExerciseID, x.TimeRangeID });
                    table.ForeignKey(
                        name: "FK__UserHasEx__Exerc__6EF57B66",
                        column: x => x.ExerciseID,
                        principalTable: "Exercise",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__UserHasEx__TimeR__6FE99F9F",
                        column: x => x.TimeRangeID,
                        principalTable: "TimeRange",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__UserHasEx__UserI__6E01572D",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RandomisationGroupId",
                table: "AspNetUsers",
                column: "RandomisationGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_GuideID",
                table: "Exercise",
                column: "GuideID");

            migrationBuilder.CreateIndex(
                name: "IX_GroupHasExerciseInTimeRange_ExerciseID",
                table: "GroupHasExerciseInTimeRange",
                column: "ExerciseID");

            migrationBuilder.CreateIndex(
                name: "IX_GroupHasExerciseInTimeRange_TimeRangeID",
                table: "GroupHasExerciseInTimeRange",
                column: "TimeRangeID");

            migrationBuilder.CreateIndex(
                name: "IX_GroupHasExerciseInTimeRange_UserId",
                table: "GroupHasExerciseInTimeRange",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHasExerciseWeightInTimeRange_ExerciseID",
                table: "UserHasExerciseWeightInTimeRange",
                column: "ExerciseID");

            migrationBuilder.CreateIndex(
                name: "IX_UserHasExerciseWeightInTimeRange_TimeRangeID",
                table: "UserHasExerciseWeightInTimeRange",
                column: "TimeRangeID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_RandomisationGroup_RandomisationGroupId",
                table: "AspNetUsers",
                column: "RandomisationGroupId",
                principalTable: "RandomisationGroup",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_RandomisationGroup_RandomisationGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "GroupHasExerciseInTimeRange");

            migrationBuilder.DropTable(
                name: "Session");

            migrationBuilder.DropTable(
                name: "UserHasExerciseWeightInTimeRange");

            migrationBuilder.DropTable(
                name: "RandomisationGroup");

            migrationBuilder.DropTable(
                name: "Exercise");

            migrationBuilder.DropTable(
                name: "TimeRange");

            migrationBuilder.DropTable(
                name: "Guide");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RandomisationGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RandomisationGroup",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RandomisationGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RedCapBaseline",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RedCapGroup",
                table: "AspNetUsers",
                nullable: true);
        }
    }
}
