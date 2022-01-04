using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyPWS.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pws",
                columns: table => new
                {
                    IdPws = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Pwd = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Lat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Lon = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Alt = table.Column<short>(type: "smallint", nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pws", x => x.IdPws);
                });

            migrationBuilder.CreateTable(
                name: "Configwunderground",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPws = table.Column<int>(type: "int", nullable: false),
                    Wuid = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Pwd = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PwsIdPws = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configwunderground", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configwunderground_Pws_PwsIdPws",
                        column: x => x.PwsIdPws,
                        principalTable: "Pws",
                        principalColumn: "IdPws");
                });

            migrationBuilder.CreateTable(
                name: "Weather",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPws = table.Column<int>(type: "int", nullable: false),
                    Dateutc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tempc = table.Column<decimal>(type: "decimal(3,1)", precision: 3, scale: 1, nullable: true),
                    Humidity = table.Column<short>(type: "smallint", precision: 3, nullable: true),
                    Dewptc = table.Column<decimal>(type: "decimal(3,1)", precision: 3, scale: 1, nullable: true),
                    Baromhpa = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true),
                    Winddir = table.Column<short>(type: "smallint", precision: 3, nullable: true),
                    Windspeedkmh = table.Column<decimal>(type: "decimal(4,1)", precision: 4, scale: 1, nullable: true),
                    Windgustkmh = table.Column<decimal>(type: "decimal(4,1)", precision: 4, scale: 1, nullable: true),
                    Uv = table.Column<decimal>(type: "decimal(3,1)", precision: 3, scale: 1, nullable: true),
                    Rainmm = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true),
                    Dailyrainmm = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true),
                    Indoortempc = table.Column<decimal>(type: "decimal(3,1)", precision: 3, scale: 1, nullable: true),
                    Indoorhumidity = table.Column<short>(type: "smallint", precision: 3, nullable: true),
                    PwsIdPws = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weather", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Weather_Pws_PwsIdPws",
                        column: x => x.PwsIdPws,
                        principalTable: "Pws",
                        principalColumn: "IdPws");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Configwunderground_PwsIdPws",
                table: "Configwunderground",
                column: "PwsIdPws");

            migrationBuilder.CreateIndex(
                name: "IX_Weather_PwsIdPws",
                table: "Weather",
                column: "PwsIdPws");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configwunderground");

            migrationBuilder.DropTable(
                name: "Weather");

            migrationBuilder.DropTable(
                name: "Pws");
        }
    }
}
