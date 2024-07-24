using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication103434929_VT.Migrations
{
    /// <inheritdoc />
    public partial class calendar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClassTime",
                table: "Enrollments",
                newName: "Time");

            migrationBuilder.AddColumn<string>(
                name: "DayOfWeek",
                table: "Enrollments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "Enrollments");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Enrollments",
                newName: "ClassTime");
        }
    }
}
