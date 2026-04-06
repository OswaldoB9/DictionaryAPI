using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DictionaryAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSynonyms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WordSynonyms",
                columns: table => new
                {
                    WordId = table.Column<int>(type: "INTEGER", nullable: false),
                    SynonymId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordSynonyms", x => new { x.WordId, x.SynonymId });
                    table.ForeignKey(
                        name: "FK_WordSynonyms_Words_SynonymId",
                        column: x => x.SynonymId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WordSynonyms_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WordSynonyms_SynonymId",
                table: "WordSynonyms",
                column: "SynonymId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WordSynonyms");
        }
    }
}
