using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IndoorLocalization.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "floormaps",
                columns: new[] { "id", "name", "image_url", "image_width_px", "image_height_px", "width_m", "height_m" },
                values: new object[]
                {
                    1,
                    "Test-1",
                    "/images/test.jpg",
                    750,
                    666,
                    60.0,
                    53.3
                }
            );

            migrationBuilder.InsertData(
                table: "assets",
                columns: new[] { "id", "name", "x", "y", "floormapid", "active", "color" },
                values: new object[,]
                {
                    {
                        1,
                        "forklift",
                        20.0,
                        30.0,
                        1,
                        true,
                        "#FF5733"
                    },
                    {
                        2,
                        "forklift 1",
                        1.0,
                        2.0,
                        1,
                        true,
                        "#33A1FF"
                    }
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData("assets", "id", 1);
            migrationBuilder.DeleteData("assets", "id", 2);
            migrationBuilder.DeleteData("floormaps", "id", 1);
        }
    }
}
