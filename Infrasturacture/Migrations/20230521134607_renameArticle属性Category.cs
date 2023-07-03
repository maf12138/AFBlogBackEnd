using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrasturacture.Migrations
{
    public partial class renameArticle属性Category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Articles_T_Categories_CategoriesId",
                table: "T_Articles");

            migrationBuilder.RenameColumn(
                name: "CategoriesId",
                table: "T_Articles",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_T_Articles_CategoriesId",
                table: "T_Articles",
                newName: "IX_T_Articles_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Articles_T_Categories_CategoryId",
                table: "T_Articles",
                column: "CategoryId",
                principalTable: "T_Categories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Articles_T_Categories_CategoryId",
                table: "T_Articles");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "T_Articles",
                newName: "CategoriesId");

            migrationBuilder.RenameIndex(
                name: "IX_T_Articles_CategoryId",
                table: "T_Articles",
                newName: "IX_T_Articles_CategoriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Articles_T_Categories_CategoriesId",
                table: "T_Articles",
                column: "CategoriesId",
                principalTable: "T_Categories",
                principalColumn: "Id");
        }
    }
}
