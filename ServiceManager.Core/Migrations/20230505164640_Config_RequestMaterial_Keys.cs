using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceManager.Core.Migrations
{
    /// <inheritdoc />
    public partial class Config_RequestMaterial_Keys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestMaterial",
                table: "RequestMaterial");

            migrationBuilder.DropIndex(
                name: "IX_RequestMaterial_RequestId",
                table: "RequestMaterial");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RequestMaterial");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestMaterial",
                table: "RequestMaterial",
                columns: new[] { "RequestId", "MaterialId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestMaterial",
                table: "RequestMaterial");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "RequestMaterial",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestMaterial",
                table: "RequestMaterial",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RequestMaterial_RequestId",
                table: "RequestMaterial",
                column: "RequestId");
        }
    }
}
