using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication103434929_VT.Migrations
{
    /// <inheritdoc />
    public partial class updatestudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Rooms_RoomId",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_RoomId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Classes");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Enrollments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_RoomId",
                table: "Enrollments",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Rooms_RoomId",
                table: "Enrollments",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Rooms_RoomId",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_RoomId",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Enrollments");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_RoomId",
                table: "Classes",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Rooms_RoomId",
                table: "Classes",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
