using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DictionaryAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddPartOfSpeechNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Words_PartsOfSpeech_PartOfSpeechId",
                table: "Words");

            migrationBuilder.AlterColumn<int>(
                name: "PartOfSpeechId",
                table: "Words",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Words_PartsOfSpeech_PartOfSpeechId",
                table: "Words",
                column: "PartOfSpeechId",
                principalTable: "PartsOfSpeech",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Words_PartsOfSpeech_PartOfSpeechId",
                table: "Words");

            migrationBuilder.AlterColumn<int>(
                name: "PartOfSpeechId",
                table: "Words",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Words_PartsOfSpeech_PartOfSpeechId",
                table: "Words",
                column: "PartOfSpeechId",
                principalTable: "PartsOfSpeech",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
