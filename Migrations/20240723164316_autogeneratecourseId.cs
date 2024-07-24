using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication103434929_VT.Migrations
{
    /// <inheritdoc />
    public partial class autogeneratecourseId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Courses_CourseTeacherId_CourseClassId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Rooms_RoomId",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_CourseTeacherId_CourseClassId",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_RoomId",
                table: "Enrollments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courses",
                table: "Courses");

            // Add a new temporary column to store the old IDs
            migrationBuilder.AddColumn<int>(
                name: "TempId",
                table: "Courses",
                nullable: false,
                defaultValue: 0);

            // Copy the old IDs to the new temporary column
            migrationBuilder.Sql("UPDATE Courses SET TempId = Id");

            // Drop the original Id column
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Courses");

            // Add a new Id column with the IDENTITY property
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Courses",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            // Copy the IDs from the temporary column to the new Id column
            migrationBuilder.Sql("SET IDENTITY_INSERT Courses ON");
            migrationBuilder.Sql("INSERT INTO Courses (Id, TeacherId, ClassId, DayOfWeek, Time, StartDate, EndDate, TestDate, RoomLocation, RoomId) SELECT TempId, TeacherId, ClassId, DayOfWeek, Time, StartDate, EndDate, TestDate, RoomLocation, RoomId FROM Courses");
            migrationBuilder.Sql("SET IDENTITY_INSERT Courses OFF");

            // Drop the temporary column
            migrationBuilder.DropColumn(
                name: "TempId",
                table: "Courses");

            // Set the new Id column as the primary key
            migrationBuilder.AddPrimaryKey(
                name: "PK_Courses",
                table: "Courses",
                column: "Id");

            // Recreate indexes and constraints
            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_CourseId",
                table: "Enrollments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherId",
                table: "Courses",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Courses_CourseId",
                table: "Enrollments",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Rooms_RoomId",
                table: "Enrollments",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse the changes made in the Up method

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Courses_CourseId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Rooms_RoomId",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_CourseId",
                table: "Enrollments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courses",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Courses");

            migrationBuilder.AddColumn<int>(
                name: "OldId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courses",
                table: "Courses",
                columns: new[] { "TeacherId", "ClassId" });

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_CourseTeacherId_CourseClassId",
                table: "Enrollments",
                columns: new[] { "CourseTeacherId", "CourseClassId" });

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_RoomId",
                table: "Enrollments",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Courses_CourseTeacherId_CourseClassId",
                table: "Enrollments",
                columns: new[] { "CourseTeacherId", "CourseClassId" },
                principalTable: "Courses",
                principalColumns: new[] { "TeacherId", "ClassId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Rooms_RoomId",
                table: "Enrollments",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }
    }
}
