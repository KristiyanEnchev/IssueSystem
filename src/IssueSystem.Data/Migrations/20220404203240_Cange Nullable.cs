using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueSystem.Data.Migrations
{
    public partial class CangeNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AcceptantId",
                schema: "Identity",
                table: "Tickets",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AcceptantId",
                schema: "Identity",
                table: "Tickets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
