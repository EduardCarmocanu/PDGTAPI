using Microsoft.EntityFrameworkCore.Migrations;

namespace PDGTAPI.Migrations
{
    public partial class UserHasExerciseInTimeRange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserHasExerciseWeightInTimeRange");

            migrationBuilder.CreateTable(
                name: "UserHasExerciseInTimeRange",
                columns: table => new
                {
                    UserID = table.Column<string>(nullable: false),
                    ExerciseID = table.Column<int>(nullable: false),
                    TimeRangeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHasExerciseInTimeRange", x => new { x.UserID, x.ExerciseID, x.TimeRangeID });
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
                name: "IX_UserHasExerciseInTimeRange_ExerciseID",
                table: "UserHasExerciseInTimeRange",
                column: "ExerciseID");

            migrationBuilder.CreateIndex(
                name: "IX_UserHasExerciseInTimeRange_TimeRangeID",
                table: "UserHasExerciseInTimeRange",
                column: "TimeRangeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserHasExerciseInTimeRange");

            migrationBuilder.CreateTable(
                name: "UserHasExerciseWeightInTimeRange",
                columns: table => new
                {
                    UserID = table.Column<string>(nullable: false),
                    ExerciseID = table.Column<int>(nullable: false),
                    TimeRangeID = table.Column<int>(nullable: false)
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
                name: "IX_UserHasExerciseWeightInTimeRange_ExerciseID",
                table: "UserHasExerciseWeightInTimeRange",
                column: "ExerciseID");

            migrationBuilder.CreateIndex(
                name: "IX_UserHasExerciseWeightInTimeRange_TimeRangeID",
                table: "UserHasExerciseWeightInTimeRange",
                column: "TimeRangeID");
        }
    }
}
