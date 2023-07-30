using Microsoft.EntityFrameworkCore.Migrations;

namespace Diganto.Migrations
{
    public partial class Updateusertable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "AbpUsers",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "AbpUsers");
        }
    }
}
