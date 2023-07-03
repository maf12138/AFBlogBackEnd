using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrasturacture.Migrations
{
    public partial class add_user_properties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "T_Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "sex",
                table: "T_Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NickName",
                table: "T_Users");

            migrationBuilder.DropColumn(
                name: "sex",
                table: "T_Users");
        }
    }
}
