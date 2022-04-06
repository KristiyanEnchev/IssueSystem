using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueSystem.Data.Migrations
{
    public partial class Somefix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Identity",
                table: "Projects",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Identity",
                table: "Projects");
        }
    }
}
