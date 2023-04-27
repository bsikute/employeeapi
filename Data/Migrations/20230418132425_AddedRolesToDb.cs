using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Employee.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedRolesToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5b06ca0a-3448-46c2-915c-fa8f88809fd3", "7fc0409d-33fb-499b-9883-b14b71e9a0a3", "Manager", "MANAGER" },
                    { "cf2d38a4-6f6a-458f-8925-7585961472f3", "7dc731e5-5fcb-4d14-9f1e-29bca0f70b50", "Administrator", "ADMINISTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5b06ca0a-3448-46c2-915c-fa8f88809fd3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cf2d38a4-6f6a-458f-8925-7585961472f3");
        }
    }
}
