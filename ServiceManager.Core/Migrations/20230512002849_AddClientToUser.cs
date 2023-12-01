using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceManager.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddClientToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Client_UserId",
                table: "Client");

            migrationBuilder.CreateIndex(
                name: "IX_Client_UserId",
                table: "Client",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Client_UserId",
                table: "Client");

            migrationBuilder.CreateIndex(
                name: "IX_Client_UserId",
                table: "Client",
                column: "UserId");
        }
    }
}
