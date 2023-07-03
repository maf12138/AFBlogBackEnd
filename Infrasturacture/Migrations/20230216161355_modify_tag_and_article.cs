using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrasturacture.Migrations
{
    public partial class modify_tag_and_article : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Tags_T_Articles_ArticleId",
                table: "T_Tags");

            migrationBuilder.DropIndex(
                name: "IX_T_Tags_ArticleId",
                table: "T_Tags");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "T_Tags");

            migrationBuilder.CreateTable(
                name: "ArticleTag",
                columns: table => new
                {
                    ArticlesId = table.Column<int>(type: "int", nullable: false),
                    TagsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTag", x => new { x.ArticlesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_ArticleTag_T_Articles_ArticlesId",
                        column: x => x.ArticlesId,
                        principalTable: "T_Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleTag_T_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "T_Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTag_TagsId",
                table: "ArticleTag",
                column: "TagsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleTag");

            migrationBuilder.AddColumn<int>(
                name: "ArticleId",
                table: "T_Tags",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_Tags_ArticleId",
                table: "T_Tags",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Tags_T_Articles_ArticleId",
                table: "T_Tags",
                column: "ArticleId",
                principalTable: "T_Articles",
                principalColumn: "Id");
        }
    }
}
