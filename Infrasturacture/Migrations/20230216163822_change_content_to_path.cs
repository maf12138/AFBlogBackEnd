using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrasturacture.Migrations
{
    public partial class change_content_to_path : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "T_Articles",
                newName: "Path");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "T_Articles",
                newName: "Content");
        }
    }
}
