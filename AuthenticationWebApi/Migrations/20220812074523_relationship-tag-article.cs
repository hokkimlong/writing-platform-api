using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationWebApi.Migrations
{
    public partial class relationshiptagarticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Article_ArticleId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_ArticleId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "Tag");

            migrationBuilder.CreateTable(
                name: "ArticleTag",
                columns: table => new
                {
                    ArticlesId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTag", x => new { x.ArticlesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_ArticleTag_Article_ArticlesId",
                        column: x => x.ArticlesId,
                        principalTable: "Article",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleTag_Tag_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                table: "Tag",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_ArticleId",
                table: "Tag",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Article_ArticleId",
                table: "Tag",
                column: "ArticleId",
                principalTable: "Article",
                principalColumn: "Id");
        }
    }
}
