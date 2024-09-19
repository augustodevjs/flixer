using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flixer.Catalog.Infra.Data.EF.Migrations
{
    public partial class removeEventsToMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Media_MediaId",
                table: "Videos");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Media_TrailerId",
                table: "Videos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Media",
                table: "Media");

            migrationBuilder.RenameTable(
                name: "Media",
                newName: "Medias");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Medias",
                table: "Medias",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Medias_MediaId",
                table: "Videos",
                column: "MediaId",
                principalTable: "Medias",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Medias_TrailerId",
                table: "Videos",
                column: "TrailerId",
                principalTable: "Medias",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Medias_MediaId",
                table: "Videos");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Medias_TrailerId",
                table: "Videos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Medias",
                table: "Medias");

            migrationBuilder.RenameTable(
                name: "Medias",
                newName: "Media");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Media",
                table: "Media",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Media_MediaId",
                table: "Videos",
                column: "MediaId",
                principalTable: "Media",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Media_TrailerId",
                table: "Videos",
                column: "TrailerId",
                principalTable: "Media",
                principalColumn: "Id");
        }
    }
}
