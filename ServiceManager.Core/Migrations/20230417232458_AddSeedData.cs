using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceManager.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("0d0996ae-ba1c-4dba-ba91-bdada6efb79f"), "0d0996ae-ba1c-4dba-ba91-bdada6efb79f", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("cf16bab6-31c5-45dd-ad82-0e83fe52856b"), "cf16bab6-31c5-45dd-ad82-0e83fe52856b", "Employee", "EMPLOYEE" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "FirstName", "LastName" },
                values: new object[] { new Guid("064c112d-2be0-4345-b8eb-229728d91ea1"), 0, "00000000-0000-0000-0000-000000000000", "", true, false, null, "", "ADMIN", "AQAAAAEAACcQAAAAELjsp8caSGh7rknzSNuXIFQLhNVS482xEYlsNBD3V1HsOIyUGWrmoGO7AQCKlheRyg==", null, true, "00000000-0000-0000-0000-000000000000", false, "Admin", "Админ", "Админович" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("0d0996ae-ba1c-4dba-ba91-bdada6efb79f"), new Guid("064c112d-2be0-4345-b8eb-229728d91ea1") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("cf16bab6-31c5-45dd-ad82-0e83fe52856b"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("0d0996ae-ba1c-4dba-ba91-bdada6efb79f"), new Guid("064c112d-2be0-4345-b8eb-229728d91ea1") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0d0996ae-ba1c-4dba-ba91-bdada6efb79f"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("064c112d-2be0-4345-b8eb-229728d91ea1"));
        }
    }
}
