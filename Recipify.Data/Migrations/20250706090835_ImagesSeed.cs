using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recipify.Data.Migrations
{
    /// <inheritdoc />
    public partial class ImagesSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://leonardobansko.bg/wp-content/uploads/2020/08/2020-08-26_15h07_08.png");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://gotvach.bg/files/lib/400x296/curry-chicken1.webp");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://www.whiskware.com/cdn/shop/articles/yagv0pn15omlwkgpgrbk.jpg?v=1617665208&width=2000");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: null);
        }
    }
}
