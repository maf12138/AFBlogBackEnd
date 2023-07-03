using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrasturacture.Migrations
{
    public partial class modify_Models : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TagName",
                table: "T_Types",
                newName: "TypeName");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "T_Comments",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationTime",
                table: "T_Articles",
                type: "datetime(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "T_Comments");

            migrationBuilder.DropColumn(
                name: "ModificationTime",
                table: "T_Articles");

            migrationBuilder.RenameColumn(
                name: "TypeName",
                table: "T_Types",
                newName: "TagName");
        }
    }
}
