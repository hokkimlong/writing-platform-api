using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationWebApi.Migrations
{
    public partial class updatedb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleTag_Article_ArticleId",
                table: "ArticleTag");

            migrationBuilder.DropForeignKey(
                name: "FK_ArticleTag_Tag_TagId",
                table: "ArticleTag");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "ArticleTag",
                newName: "TagsId");

            migrationBuilder.RenameColumn(
                name: "ArticleId",
                table: "ArticleTag",
                newName: "ArticlesId");

            migrationBuilder.RenameIndex(
                name: "IX_ArticleTag_TagId",
                table: "ArticleTag",
                newName: "IX_ArticleTag_TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleTag_Article_ArticlesId",
                table: "ArticleTag",
                column: "ArticlesId",
                principalTable: "Article",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleTag_Tag_TagsId",
                table: "ArticleTag",
                column: "TagsId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleTag_Article_ArticlesId",
                table: "ArticleTag");

            migrationBuilder.DropForeignKey(
                name: "FK_ArticleTag_Tag_TagsId",
                table: "ArticleTag");

            migrationBuilder.RenameColumn(
                name: "TagsId",
                table: "ArticleTag",
                newName: "TagId");

            migrationBuilder.RenameColumn(
                name: "ArticlesId",
                table: "ArticleTag",
                newName: "ArticleId");

            migrationBuilder.RenameIndex(
                name: "IX_ArticleTag_TagsId",
                table: "ArticleTag",
                newName: "IX_ArticleTag_TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleTag_Article_ArticleId",
                table: "ArticleTag",
                column: "ArticleId",
                principalTable: "Article",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleTag_Tag_TagId",
                table: "ArticleTag",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
