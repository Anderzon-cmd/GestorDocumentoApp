using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestorDocumentoApp.Migrations
{
    /// <inheritdoc />
    public partial class AddChangeRequestToVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChangeRequestId",
                table: "Versions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Versions_ChangeRequestId",
                table: "Versions",
                column: "ChangeRequestId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Versions_ChangeRequests_ChangeRequestId",
                table: "Versions",
                column: "ChangeRequestId",
                principalTable: "ChangeRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Versions_ChangeRequests_ChangeRequestId",
                table: "Versions");

            migrationBuilder.DropIndex(
                name: "IX_Versions_ChangeRequestId",
                table: "Versions");

            migrationBuilder.DropColumn(
                name: "ChangeRequestId",
                table: "Versions");
        }
    }
}
