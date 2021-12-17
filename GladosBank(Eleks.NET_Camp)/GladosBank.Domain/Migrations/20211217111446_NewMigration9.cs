using Microsoft.EntityFrameworkCore.Migrations;

namespace GladosBank.Domain.Migrations
{
    public partial class NewMigration9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Currency",
                keyColumn: "Code",
                keyValue: "UAN",
                column: "Coefficient",
                value: 1m);

            migrationBuilder.UpdateData(
                table: "Currency",
                keyColumn: "Code",
                keyValue: "USD",
                column: "Coefficient",
                value: 28m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
