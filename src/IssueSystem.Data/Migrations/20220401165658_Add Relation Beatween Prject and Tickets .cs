using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueSystem.Data.Migrations
{
    public partial class AddRelationBeatweenPrjectandTickets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectId",
                schema: "Identity",
                table: "Tickets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ProjectId",
                schema: "Identity",
                table: "Tickets",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Projects_ProjectId",
                schema: "Identity",
                table: "Tickets",
                column: "ProjectId",
                principalSchema: "Identity",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Projects_ProjectId",
                schema: "Identity",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ProjectId",
                schema: "Identity",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                schema: "Identity",
                table: "Tickets");
        }
    }
}
