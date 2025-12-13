using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IndoorLocalization.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "floormaps",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: true),
                    image_width_px = table.Column<int>(type: "integer", nullable: true),
                    image_height_px = table.Column<int>(type: "integer", nullable: true),
                    width_m = table.Column<double>(type: "double precision", nullable: false),
                    height_m = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("floormaps_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    passwordhash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    firstname = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    lastname = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    refreshtoken = table.Column<string>(type: "text", nullable: true),
                    refreshtokenexpiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "assets",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    x = table.Column<double>(type: "double precision", nullable: true),
                    y = table.Column<double>(type: "double precision", nullable: true),
                    lastsync = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    floormapid = table.Column<long>(type: "bigint", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("assets_pkey", x => x.id);
                    table.ForeignKey(
                        name: "assets_floormapid_fkey",
                        column: x => x.floormapid,
                        principalTable: "floormaps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "zones",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    points = table.Column<string>(type: "jsonb", nullable: true),
                    userid = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("zones_pkey", x => x.id);
                    table.ForeignKey(
                        name: "zones_userid_fkey",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "assetpositionhistory",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    assetid = table.Column<long>(type: "bigint", nullable: true),
                    x = table.Column<double>(type: "double precision", nullable: false),
                    y = table.Column<double>(type: "double precision", nullable: false),
                    datetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    floormapid = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("assetpositionhistory_pkey", x => x.id);
                    table.ForeignKey(
                        name: "assetpositionhistory_assetid_fkey",
                        column: x => x.assetid,
                        principalTable: "assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "assetpositionhistory_floormapid_fkey",
                        column: x => x.floormapid,
                        principalTable: "floormaps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "assetzonehistory",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    assetid = table.Column<long>(type: "bigint", nullable: true),
                    zoneid = table.Column<long>(type: "bigint", nullable: true),
                    enterdatetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    exitdatetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    retentiontime = table.Column<TimeSpan>(type: "interval", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("assetzonehistory_pkey", x => x.id);
                    table.ForeignKey(
                        name: "assetzonehistory_assetid_fkey",
                        column: x => x.assetid,
                        principalTable: "assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "assetzonehistory_zoneid_fkey",
                        column: x => x.zoneid,
                        principalTable: "zones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_assetpositionhistory_assetid",
                table: "assetpositionhistory",
                column: "assetid");

            migrationBuilder.CreateIndex(
                name: "IX_assetpositionhistory_floormapid",
                table: "assetpositionhistory",
                column: "floormapid");

            migrationBuilder.CreateIndex(
                name: "IX_assets_floormapid",
                table: "assets",
                column: "floormapid");

            migrationBuilder.CreateIndex(
                name: "IX_assetzonehistory_assetid",
                table: "assetzonehistory",
                column: "assetid");

            migrationBuilder.CreateIndex(
                name: "IX_assetzonehistory_zoneid",
                table: "assetzonehistory",
                column: "zoneid");

            migrationBuilder.CreateIndex(
                name: "users_email_key",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "users_username_key",
                table: "users",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_zones_userid",
                table: "zones",
                column: "userid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assetpositionhistory");

            migrationBuilder.DropTable(
                name: "assetzonehistory");

            migrationBuilder.DropTable(
                name: "assets");

            migrationBuilder.DropTable(
                name: "zones");

            migrationBuilder.DropTable(
                name: "floormaps");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
