using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrasturacture.Migrations
{
    public partial class changeArticleContentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Content",
                table: "T_Articles",
                type: "BLOB",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "T_Articles");
        }
    }
}
