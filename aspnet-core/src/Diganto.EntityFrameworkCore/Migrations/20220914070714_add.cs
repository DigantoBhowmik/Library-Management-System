using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Diganto.Migrations
{
    public partial class add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BookId",
                table: "AppAuthors",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppAuthors_BookId",
                table: "AppAuthors",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppAuthors_AppBooks_BookId",
                table: "AppAuthors",
                column: "BookId",
                principalTable: "AppBooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppAuthors_AppBooks_BookId",
                table: "AppAuthors");

            migrationBuilder.DropIndex(
                name: "IX_AppAuthors_BookId",
                table: "AppAuthors");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "AppAuthors");
        }
    }
}
