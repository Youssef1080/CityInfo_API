using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityInfo.API.Migrations
{
    /// <inheritdoc />
    public partial class ModifyCityDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointOfInterest_Cities_cityId",
                table: "PointOfInterest");

            migrationBuilder.RenameColumn(
                name: "cityId",
                table: "PointOfInterest",
                newName: "CityId");

            migrationBuilder.RenameIndex(
                name: "IX_PointOfInterest_cityId",
                table: "PointOfInterest",
                newName: "IX_PointOfInterest_CityId");

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "PointOfInterest",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PointOfInterest_Cities_CityId",
                table: "PointOfInterest",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointOfInterest_Cities_CityId",
                table: "PointOfInterest");

            migrationBuilder.RenameColumn(
                name: "CityId",
                table: "PointOfInterest",
                newName: "cityId");

            migrationBuilder.RenameIndex(
                name: "IX_PointOfInterest_CityId",
                table: "PointOfInterest",
                newName: "IX_PointOfInterest_cityId");

            migrationBuilder.AlterColumn<int>(
                name: "cityId",
                table: "PointOfInterest",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_PointOfInterest_Cities_cityId",
                table: "PointOfInterest",
                column: "cityId",
                principalTable: "Cities",
                principalColumn: "Id");
        }
    }
}
