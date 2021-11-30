using Microsoft.EntityFrameworkCore.Migrations;

namespace GladosBank.Domain.Migrations
{
    public partial class NewMigration4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "OperationsHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OperationsHistory_CustomerId",
                table: "OperationsHistory",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationsHistory_Customers_CustomerId",
                table: "OperationsHistory",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperationsHistory_Customers_CustomerId",
                table: "OperationsHistory");

            migrationBuilder.DropIndex(
                name: "IX_OperationsHistory_CustomerId",
                table: "OperationsHistory");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "OperationsHistory");
        }
    }
}
