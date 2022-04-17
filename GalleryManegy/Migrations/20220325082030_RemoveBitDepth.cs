using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GalleryManegy.Migrations
{
    public partial class RemoveBitDepth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BitDepth",
                table: "Images");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BitDepth",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
