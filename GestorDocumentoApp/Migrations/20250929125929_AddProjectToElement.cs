using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestorDocumentoApp.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectToElement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Elements",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Elements_ProjectId",
                table: "Elements",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Elements_Projects_ProjectId",
                table: "Elements",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Elements_Projects_ProjectId",
                table: "Elements");

            migrationBuilder.DropIndex(
                name: "IX_Elements_ProjectId",
                table: "Elements");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Elements");
        }
    }
}
