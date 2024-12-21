using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectIO.Migrations
{
    /// <inheritdoc />
    public partial class TestAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "userID", "userName", "userPassword" },
                values: new object[] { 1, "admin", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "userID",
                keyValue: 1);
        }
    }
}
