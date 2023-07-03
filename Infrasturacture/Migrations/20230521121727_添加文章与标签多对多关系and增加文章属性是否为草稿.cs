using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrasturacture.Migrations
{
    public partial class 添加文章与标签多对多关系and增加文章属性是否为草稿 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleTag_T_Articles_ArticlesId",
                table: "ArticleTag");

            migrationBuilder.DropForeignKey(
                name: "FK_ArticleTag_T_Tags_TagsId",
                table: "ArticleTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArticleTag",
                table: "ArticleTag");

            migrationBuilder.RenameTable(
                name: "ArticleTag",
                newName: "T_ArticleTags");

            migrationBuilder.RenameIndex(
                name: "IX_ArticleTag_TagsId",
                table: "T_ArticleTags",
                newName: "IX_T_ArticleTags_TagsId");

            migrationBuilder.AddColumn<bool>(
                name: "IsDraft",
                table: "T_Articles",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_T_ArticleTags",
                table: "T_ArticleTags",
                columns: new[] { "ArticlesId", "TagsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_T_ArticleTags_T_Articles_ArticlesId",
                table: "T_ArticleTags",
                column: "ArticlesId",
                principalTable: "T_Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_ArticleTags_T_Tags_TagsId",
                table: "T_ArticleTags",
                column: "TagsId",
                principalTable: "T_Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_ArticleTags_T_Articles_ArticlesId",
                table: "T_ArticleTags");

            migrationBuilder.DropForeignKey(
                name: "FK_T_ArticleTags_T_Tags_TagsId",
                table: "T_ArticleTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_T_ArticleTags",
                table: "T_ArticleTags");

            migrationBuilder.DropColumn(
                name: "IsDraft",
                table: "T_Articles");

            migrationBuilder.RenameTable(
                name: "T_ArticleTags",
                newName: "ArticleTag");

            migrationBuilder.RenameIndex(
                name: "IX_T_ArticleTags_TagsId",
                table: "ArticleTag",
                newName: "IX_ArticleTag_TagsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArticleTag",
                table: "ArticleTag",
                columns: new[] { "ArticlesId", "TagsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleTag_T_Articles_ArticlesId",
                table: "ArticleTag",
                column: "ArticlesId",
                principalTable: "T_Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleTag_T_Tags_TagsId",
                table: "ArticleTag",
                column: "TagsId",
                principalTable: "T_Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
