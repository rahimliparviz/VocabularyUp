using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class add_user_phrase_relationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPhrase_Phrases_PhaseId",
                table: "UserPhrase");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPhrase_AspNetUsers_UserId",
                table: "UserPhrase");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPhrase",
                table: "UserPhrase");

            migrationBuilder.RenameTable(
                name: "UserPhrase",
                newName: "UserPhrases");

            migrationBuilder.RenameIndex(
                name: "IX_UserPhrase_PhaseId",
                table: "UserPhrases",
                newName: "IX_UserPhrases_PhaseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPhrases",
                table: "UserPhrases",
                columns: new[] { "UserId", "PhaseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserPhrases_Phrases_PhaseId",
                table: "UserPhrases",
                column: "PhaseId",
                principalTable: "Phrases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPhrases_AspNetUsers_UserId",
                table: "UserPhrases",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPhrases_Phrases_PhaseId",
                table: "UserPhrases");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPhrases_AspNetUsers_UserId",
                table: "UserPhrases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPhrases",
                table: "UserPhrases");

            migrationBuilder.RenameTable(
                name: "UserPhrases",
                newName: "UserPhrase");

            migrationBuilder.RenameIndex(
                name: "IX_UserPhrases_PhaseId",
                table: "UserPhrase",
                newName: "IX_UserPhrase_PhaseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPhrase",
                table: "UserPhrase",
                columns: new[] { "UserId", "PhaseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserPhrase_Phrases_PhaseId",
                table: "UserPhrase",
                column: "PhaseId",
                principalTable: "Phrases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPhrase_AspNetUsers_UserId",
                table: "UserPhrase",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
