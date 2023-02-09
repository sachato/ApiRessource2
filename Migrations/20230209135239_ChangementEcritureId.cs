using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiRessource2.Migrations
{
    public partial class ChangementEcritureId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Voteds",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "IdRessource",
                table: "Voteds",
                newName: "RessourceId");

            migrationBuilder.RenameColumn(
                name: "IdZoneGeo",
                table: "Users",
                newName: "ZoneGeoId");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Resources",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "ReportedRessources",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "IdRessource",
                table: "ReportedRessources",
                newName: "RessourceId");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "ReportedComments",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "IdComment",
                table: "ReportedComments",
                newName: "CommentId");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Favoris",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "IdRessource",
                table: "Favoris",
                newName: "RessourceId");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Consultations",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "IdRessource",
                table: "Consultations",
                newName: "RessourceId");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Comments",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "IdRessource",
                table: "Comments",
                newName: "RessourceId");

            migrationBuilder.RenameColumn(
                name: "IdDeleted",
                table: "Comments",
                newName: "IsDeleted");

            migrationBuilder.UpdateData(
                table: "ZoneGeos",
                keyColumn: "Name",
                keyValue: null,
                column: "Name",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ZoneGeos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Voteds",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "RessourceId",
                table: "Voteds",
                newName: "IdRessource");

            migrationBuilder.RenameColumn(
                name: "ZoneGeoId",
                table: "Users",
                newName: "IdZoneGeo");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Resources",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ReportedRessources",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "RessourceId",
                table: "ReportedRessources",
                newName: "IdRessource");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ReportedComments",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "ReportedComments",
                newName: "IdComment");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Favoris",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "RessourceId",
                table: "Favoris",
                newName: "IdRessource");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Consultations",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "RessourceId",
                table: "Consultations",
                newName: "IdRessource");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Comments",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "RessourceId",
                table: "Comments",
                newName: "IdRessource");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Comments",
                newName: "IdDeleted");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ZoneGeos",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
