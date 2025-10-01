using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorcycleRental.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "delivery_drivers",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    cnpj = table.Column<string>(type: "text", nullable: false),
                    birth_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    cnh_type = table.Column<string>(type: "text", nullable: false),
                    cnh_number = table.Column<string>(type: "text", nullable: false),
                    cnh_image_path = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_delivery_drivers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "motorcycles",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    model = table.Column<string>(type: "text", nullable: false),
                    license_plate = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_motorcycles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rentals",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    delivery_driver_id = table.Column<string>(type: "text", nullable: false),
                    motorcycle_id = table.Column<string>(type: "text", nullable: false),
                    initial_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expected_end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    return_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    plan = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rentals", x => x.id);
                    table.ForeignKey(
                        name: "fk_rentals_delivery_drivers_delivery_driver_id",
                        column: x => x.delivery_driver_id,
                        principalTable: "delivery_drivers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_rentals_motorcycles_motorcycle_id",
                        column: x => x.motorcycle_id,
                        principalTable: "motorcycles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_delivery_drivers_cnh_number",
                table: "delivery_drivers",
                column: "cnh_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_delivery_drivers_cnpj",
                table: "delivery_drivers",
                column: "cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_motorcycles_license_plate",
                table: "motorcycles",
                column: "license_plate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_rentals_delivery_driver_id",
                table: "rentals",
                column: "delivery_driver_id");

            migrationBuilder.CreateIndex(
                name: "ix_rentals_motorcycle_id",
                table: "rentals",
                column: "motorcycle_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rentals");

            migrationBuilder.DropTable(
                name: "delivery_drivers");

            migrationBuilder.DropTable(
                name: "motorcycles");
        }
    }
}
