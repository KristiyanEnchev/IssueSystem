using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueSystem.Data.Migrations
{
    public partial class RemoveCollection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_TicketCategories_TicketCategoryId",
                schema: "Identity",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_TicketCategoryId",
                schema: "Identity",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "TicketCategoryId",
                schema: "Identity",
                table: "Employee");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TicketCategoryId",
                schema: "Identity",
                table: "Employee",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_TicketCategoryId",
                schema: "Identity",
                table: "Employee",
                column: "TicketCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_TicketCategories_TicketCategoryId",
                schema: "Identity",
                table: "Employee",
                column: "TicketCategoryId",
                principalSchema: "Identity",
                principalTable: "TicketCategories",
                principalColumn: "TicketCategoryId");
        }
    }
}
