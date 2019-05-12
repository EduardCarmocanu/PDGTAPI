using Microsoft.EntityFrameworkCore.Migrations;

namespace PDGTAPI.Migrations
{
    public partial class SimplifySchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupHasExerciseInTimeRange");

            migrationBuilder.AddColumn<int>(
                name: "RandomisationGroupID",
                table: "TimeRange",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TimeRangeHasExecise",
                columns: table => new
                {
                    ExerciseID = table.Column<int>(nullable: false),
                    TimeRangeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeRangeHasExecise", x => new { x.ExerciseID, x.TimeRangeID });
                    table.ForeignKey(
                        name: "FK__TimeRang__HasE__73BA3083",
                        column: x => x.ExerciseID,
                        principalTable: "Exercise",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__TimeRang__HasE__72C60C4A",
                        column: x => x.TimeRangeID,
                        principalTable: "TimeRange",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeRange_RandomisationGroupID",
                table: "TimeRange",
                column: "RandomisationGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_TimeRangeHasExecise_TimeRangeID",
                table: "TimeRangeHasExecise",
                column: "TimeRangeID");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeRange_RandomisationGroup_RandomisationGroupID",
                table: "TimeRange",
                column: "RandomisationGroupID",
                principalTable: "RandomisationGroup",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeRange_RandomisationGroup_RandomisationGroupID",
                table: "TimeRange");

            migrationBuilder.DropTable(
                name: "TimeRangeHasExecise");

            migrationBuilder.DropIndex(
                name: "IX_TimeRange_RandomisationGroupID",
                table: "TimeRange");

            migrationBuilder.DropColumn(
                name: "RandomisationGroupID",
                table: "TimeRange");

            migrationBuilder.CreateTable(
                name: "GroupHasExerciseInTimeRange",
                columns: table => new
                {
                    GroupID = table.Column<int>(nullable: false),
                    ExerciseID = table.Column<int>(nullable: false),
                    TimeRangeID = table.Column<int>(nullable: false)
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
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupHasExerciseInTimeRange_ExerciseID",
                table: "GroupHasExerciseInTimeRange",
                column: "ExerciseID");

            migrationBuilder.CreateIndex(
                name: "IX_GroupHasExerciseInTimeRange_TimeRangeID",
                table: "GroupHasExerciseInTimeRange",
                column: "TimeRangeID");
        }
    }
}
