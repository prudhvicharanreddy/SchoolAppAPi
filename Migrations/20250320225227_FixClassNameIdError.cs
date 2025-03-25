using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolApplication.Migrations
{
    /// <inheritdoc />
    public partial class FixClassNameIdError : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherClasses_AspNetUsers_TeacherNameId",
                table: "TeacherClasses");

            migrationBuilder.DropIndex(
                name: "IX_TeacherClasses_TeacherNameId",
                table: "TeacherClasses");

            migrationBuilder.DropColumn(
                name: "TeacherNameId",
                table: "TeacherClasses");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Subjects",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherClasses_AspNetUsers_TeacherId",
                table: "TeacherClasses",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherClasses_AspNetUsers_TeacherId",
                table: "TeacherClasses");

            migrationBuilder.AddColumn<int>(
                name: "TeacherNameId",
                table: "TeacherClasses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherClasses_TeacherNameId",
                table: "TeacherClasses",
                column: "TeacherNameId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherClasses_AspNetUsers_TeacherNameId",
                table: "TeacherClasses",
                column: "TeacherNameId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
