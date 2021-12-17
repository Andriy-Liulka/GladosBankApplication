using Microsoft.EntityFrameworkCore.Migrations;

namespace GladosBank.Domain.Migrations
{
    public partial class NewMigration8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Coefficient",
                table: "Currency",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Currency",
                keyColumn: "Code",
                keyValue: "EUR",
                column: "Coefficient",
                value: 32m);

            migrationBuilder.UpdateData(
                table: "Currency",
                keyColumn: "Code",
                keyValue: "UAN",
                column: "Coefficient",
                value: 28m);

            migrationBuilder.UpdateData(
                table: "Currency",
                keyColumn: "Code",
                keyValue: "USD",
                column: "Coefficient",
                value: 1m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coefficient",
                table: "Currency");
        }
    }
}
