using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImHungryLibrary.Migrations
{
    public partial class RestaurantOwnerAndIsActiveAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Restaurants",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<long>(
                name: "OwnerId",
                table: "Restaurants",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_OwnerId",
                table: "Restaurants",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Users_OwnerId",
                table: "Restaurants",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Users_OwnerId",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_OwnerId",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Restaurants");
        }
    }
}
