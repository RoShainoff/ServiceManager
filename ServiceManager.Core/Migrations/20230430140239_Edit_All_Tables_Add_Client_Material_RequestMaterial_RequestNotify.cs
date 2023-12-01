using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceManager.Core.Migrations
{
    /// <inheritdoc />
    public partial class Edit_All_Tables_Add_Client_Material_RequestMaterial_RequestNotify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_AspNetUsers_CreatorId",
                table: "Request");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestHistory_AspNetUsers_UserId",
                table: "RequestHistory");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ServiceType");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Request");

            migrationBuilder.DropColumn(
                name: "RoomName",
                table: "Request");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Request",
                newName: "Priority");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Request",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Request_CreatorId",
                table: "Request",
                newName: "IX_Request_ClientId");

            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                table: "Service",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Hours",
                table: "Service",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "RequestHistory",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Request",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Request",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceTypeId",
                table: "Request",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Client_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Material",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Material", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestNotify",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    RequestHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestNotify", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestNotify_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestNotify_RequestHistory_RequestHistoryId",
                        column: x => x.RequestHistoryId,
                        principalTable: "RequestHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestMaterial",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestMaterial_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestMaterial_Request_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Request",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Request_ServiceTypeId",
                table: "Request",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_UserId",
                table: "Client",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestMaterial_MaterialId",
                table: "RequestMaterial",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestMaterial_RequestId",
                table: "RequestMaterial",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestNotify_RequestHistoryId",
                table: "RequestNotify",
                column: "RequestHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestNotify_UserId",
                table: "RequestNotify",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_AspNetUsers_ClientId",
                table: "Request",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Request_ServiceType_ServiceTypeId",
                table: "Request",
                column: "ServiceTypeId",
                principalTable: "ServiceType",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestHistory_AspNetUsers_UserId",
                table: "RequestHistory",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_AspNetUsers_ClientId",
                table: "Request");

            migrationBuilder.DropForeignKey(
                name: "FK_Request_ServiceType_ServiceTypeId",
                table: "Request");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestHistory_AspNetUsers_UserId",
                table: "RequestHistory");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "RequestMaterial");

            migrationBuilder.DropTable(
                name: "RequestNotify");

            migrationBuilder.DropTable(
                name: "Material");

            migrationBuilder.DropIndex(
                name: "IX_Request_ServiceTypeId",
                table: "Request");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "Hours",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Request");

            migrationBuilder.DropColumn(
                name: "ServiceTypeId",
                table: "Request");

            migrationBuilder.RenameColumn(
                name: "Priority",
                table: "Request",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Request",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Request_ClientId",
                table: "Request",
                newName: "IX_Request_CreatorId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ServiceType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "RequestHistory",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Request",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Request",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RoomName",
                table: "Request",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_AspNetUsers_CreatorId",
                table: "Request",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestHistory_AspNetUsers_UserId",
                table: "RequestHistory",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
