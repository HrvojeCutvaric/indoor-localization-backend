using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IndoorLocalization.Migrations
{
    /// <inheritdoc />
    public partial class AddFloorMapIdToZones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FloorMapId",
                table: "zones",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_zones_FloorMapId",
                table: "zones",
                column: "FloorMapId");

            migrationBuilder.AddForeignKey(
                name: "FK_zones_floormaps_FloorMapId",
                table: "zones",
                column: "FloorMapId",
                principalTable: "floormaps",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_zones_floormaps_FloorMapId",
                table: "zones");

            migrationBuilder.DropIndex(
                name: "IX_zones_FloorMapId",
                table: "zones");

            migrationBuilder.DropColumn(
                name: "FloorMapId",
                table: "zones");
        }
    }
}
