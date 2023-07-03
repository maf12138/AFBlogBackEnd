using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrasturacture.Migrations
{
    public partial class Rename文章类型toCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Articles_T_Types_TypeId",
                table: "T_Articles");

            migrationBuilder.DropTable(
                name: "T_Types");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "T_Articles",
                newName: "CategoriesId");

            migrationBuilder.RenameIndex(
                name: "IX_T_Articles_TypeId",
                table: "T_Articles",
                newName: "IX_T_Articles_CategoriesId");

            migrationBuilder.CreateTable(
                name: "T_Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Categories", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Articles_T_Categories_CategoriesId",
                table: "T_Articles",
                column: "CategoriesId",
                principalTable: "T_Categories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Articles_T_Categories_CategoriesId",
                table: "T_Articles");

            migrationBuilder.DropTable(
                name: "T_Categories");

            migrationBuilder.RenameColumn(
                name: "CategoriesId",
                table: "T_Articles",
                newName: "TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_T_Articles_CategoriesId",
                table: "T_Articles",
                newName: "IX_T_Articles_TypeId");

            migrationBuilder.CreateTable(
                name: "T_Types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TypeName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Types", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Articles_T_Types_TypeId",
                table: "T_Articles",
                column: "TypeId",
                principalTable: "T_Types",
                principalColumn: "Id");
        }
    }
}
