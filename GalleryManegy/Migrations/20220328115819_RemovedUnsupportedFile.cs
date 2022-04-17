using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GalleryManegy.Migrations
{
    public partial class RemovedUnsupportedFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnsupportedFiles");

            migrationBuilder.AddColumn<bool>(
                name: "Unsupported",
                table: "Images",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unsupported",
                table: "Images");

            migrationBuilder.CreateTable(
                name: "UnsupportedFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Scanned = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnsupportedFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnsupportedFiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnsupportedFiles_UserId",
                table: "UnsupportedFiles",
                column: "UserId");
        }
    }
}
