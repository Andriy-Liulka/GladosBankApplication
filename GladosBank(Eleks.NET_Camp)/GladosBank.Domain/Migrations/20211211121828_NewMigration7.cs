using Microsoft.EntityFrameworkCore.Migrations;

namespace GladosBank.Domain.Migrations
{
    public partial class NewMigration7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "Code", "Symbol" },
                values: new object[,]
                {
                    { "EUR", "€" },
                    { "UAN", "₴" },
                    { "USD", "$" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsActive", "Login", "PasswordHash", "Phone" },
                values: new object[] { 1024, "admin@example.com", true, "admin", "5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5", "06866414" });

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "UserId" },
                values: new object[] { 1, 1024 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Code",
                keyValue: "EUR");

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Code",
                keyValue: "UAN");

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Code",
                keyValue: "USD");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1024);
        }
    }
}
