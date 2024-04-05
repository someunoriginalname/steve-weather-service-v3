using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CountryModel.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    CountryID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(400)", unicode: false, maxLength: 400, nullable: false),
                    Iso2 = table.Column<string>(type: "char(900)", unicode: false, fixedLength: true, nullable: false),
                    Iso3 = table.Column<string>(type: "char(900)", unicode: false, fixedLength: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.CountryID);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    CityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Latitude = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    Longitude = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    CountryID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "char(30)", unicode: false, fixedLength: true, maxLength: 30, nullable: false),
                    Population = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Table__F2D21A965FAEB50E", x => x.CityID);
                    table.ForeignKey(
                        name: "FK_City_Country",
                        column: x => x.CountryID,
                        principalTable: "Country",
                        principalColumn: "CountryID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_City_CountryID",
                table: "City",
                column: "CountryID");

            migrationBuilder.CreateIndex(
                name: "IX_City_Latitude",
                table: "City",
                column: "Latitude");

            migrationBuilder.CreateIndex(
                name: "IX_City_Longitude",
                table: "City",
                column: "Longitude");

            migrationBuilder.CreateIndex(
                name: "IX_City_Name",
                table: "City",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_City_Population",
                table: "City",
                column: "Population");

            migrationBuilder.CreateIndex(
                name: "IX_Country_Iso2",
                table: "Country",
                column: "Iso2");

            migrationBuilder.CreateIndex(
                name: "IX_Country_Iso3",
                table: "Country",
                column: "Iso3");

            migrationBuilder.CreateIndex(
                name: "IX_Country_Name",
                table: "Country",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Country");
        }
    }
}
