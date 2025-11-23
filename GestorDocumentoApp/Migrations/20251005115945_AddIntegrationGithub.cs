using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestorDocumentoApp.Migrations
{
    /// <inheritdoc />
    public partial class AddIntegrationGithub : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternaCodeElement",
                table: "Elements",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalUrlElement",
                table: "Elements",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternaCodeElement",
                table: "Elements");

            migrationBuilder.DropColumn(
                name: "ExternalUrlElement",
                table: "Elements");
        }
    }
}
