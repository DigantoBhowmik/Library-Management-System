using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Diganto.Migrations
{
    public partial class ssf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PublisherId",
                table: "AppBooks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AppBooks_PublisherId",
                table: "AppBooks",
                column: "PublisherId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppBooks_AppPublishers_PublisherId",
                table: "AppBooks",
                column: "PublisherId",
                principalTable: "AppPublishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppBooks_AppPublishers_PublisherId",
                table: "AppBooks");

            migrationBuilder.DropIndex(
                name: "IX_AppBooks_PublisherId",
                table: "AppBooks");

            migrationBuilder.DropColumn(
                name: "PublisherId",
                table: "AppBooks");
        }
    }
}
