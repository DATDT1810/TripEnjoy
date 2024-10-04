using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripEnjoy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHotel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Hotels_HotelId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_HotelId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Hotels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_CategoryId",
                table: "Hotels",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_Categories_CategoryId",
                table: "Hotels",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_Categories_CategoryId",
                table: "Hotels");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_CategoryId",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Hotels");

            migrationBuilder.AddColumn<int>(
                name: "HotelId",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_HotelId",
                table: "Categories",
                column: "HotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Hotels_HotelId",
                table: "Categories",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "HotelId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
