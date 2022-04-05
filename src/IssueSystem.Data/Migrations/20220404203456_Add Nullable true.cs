using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueSystem.Data.Migrations
{
    public partial class AddNullabletrue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Images_ImageId",
                schema: "Identity",
                table: "Tickets");

            migrationBuilder.AlterColumn<int>(
                name: "ImageId",
                schema: "Identity",
                table: "Tickets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Images_ImageId",
                schema: "Identity",
                table: "Tickets",
                column: "ImageId",
                principalSchema: "Identity",
                principalTable: "Images",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Images_ImageId",
                schema: "Identity",
                table: "Tickets");

            migrationBuilder.AlterColumn<int>(
                name: "ImageId",
                schema: "Identity",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Images_ImageId",
                schema: "Identity",
                table: "Tickets",
                column: "ImageId",
                principalSchema: "Identity",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
