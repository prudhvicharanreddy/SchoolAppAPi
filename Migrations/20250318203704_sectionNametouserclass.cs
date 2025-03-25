using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolApplication.Migrations
{
    /// <inheritdoc />
    public partial class sectionNametouserclass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SectionName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SectionName",
                table: "AspNetUsers");
        }
    }
}
