using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DictionaryAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddPartOfSpeech : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PartOfSpeechId",
                table: "Words",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PartsOfSpeech",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartsOfSpeech", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PartsOfSpeech",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Sustantivo" },
                    { 2, "Verbo" },
                    { 3, "Adjetivo" },
                    { 4, "Adverbio" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Words_PartOfSpeechId",
                table: "Words",
                column: "PartOfSpeechId");

            migrationBuilder.CreateIndex(
                name: "IX_PartsOfSpeech_Name",
                table: "PartsOfSpeech",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Words_PartsOfSpeech_PartOfSpeechId",
                table: "Words",
                column: "PartOfSpeechId",
                principalTable: "PartsOfSpeech",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Words_PartsOfSpeech_PartOfSpeechId",
                table: "Words");

            migrationBuilder.DropTable(
                name: "PartsOfSpeech");

            migrationBuilder.DropIndex(
                name: "IX_Words_PartOfSpeechId",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "PartOfSpeechId",
                table: "Words");
        }
    }
}
