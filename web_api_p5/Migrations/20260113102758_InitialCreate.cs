using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web_api_p5.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    email = table.Column<string>(type: "TEXT", nullable: false),
                    password = table.Column<string>(type: "TEXT", nullable: false),
                    full_name = table.Column<string>(type: "TEXT", nullable: false),
                    phone_number = table.Column<string>(type: "TEXT", nullable: false),
                    address = table.Column<string>(type: "TEXT", nullable: false),
                    loyalty_points = table.Column<int>(type: "INTEGER", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "menu_items",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: false),
                    category = table.Column<string>(type: "TEXT", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    image_url = table.Column<string>(type: "TEXT", nullable: false),
                    preparation_time = table.Column<int>(type: "INTEGER", nullable: false),
                    is_vegetarian = table.Column<bool>(type: "INTEGER", nullable: false),
                    is_spicy = table.Column<bool>(type: "INTEGER", nullable: false),
                    is_available = table.Column<bool>(type: "INTEGER", nullable: false),
                    rating = table.Column<decimal>(type: "decimal(3, 1)", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menu_items", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tables",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    table_number = table.Column<string>(type: "TEXT", nullable: false),
                    capacity = table.Column<int>(type: "INTEGER", nullable: false),
                    is_available = table.Column<bool>(type: "INTEGER", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tables", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "reservations",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    customer_id = table.Column<int>(type: "INTEGER", nullable: false),
                    reservation_number = table.Column<string>(type: "TEXT", nullable: false),
                    reservation_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    number_of_guests = table.Column<int>(type: "INTEGER", nullable: false),
                    table_number = table.Column<string>(type: "TEXT", nullable: false),
                    status = table.Column<string>(type: "TEXT", nullable: false),
                    special_requests = table.Column<string>(type: "TEXT", nullable: false),
                    subtotal = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    service_charge = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    discount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    total = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    payment_method = table.Column<string>(type: "TEXT", nullable: false),
                    payment_status = table.Column<string>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservations", x => x.id);
                    table.ForeignKey(
                        name: "FK_reservations_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "reservation_items",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    reservation_id = table.Column<int>(type: "INTEGER", nullable: false),
                    menu_item_id = table.Column<int>(type: "INTEGER", nullable: false),
                    quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservation_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_reservation_items_menu_items_menu_item_id",
                        column: x => x.menu_item_id,
                        principalTable: "menu_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reservation_items_reservations_reservation_id",
                        column: x => x.reservation_id,
                        principalTable: "reservations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_customers_email",
                table: "customers",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reservation_items_menu_item_id",
                table: "reservation_items",
                column: "menu_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservation_items_reservation_id",
                table: "reservation_items",
                column: "reservation_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservations_customer_id",
                table: "reservations",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservations_reservation_number",
                table: "reservations",
                column: "reservation_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tables_table_number",
                table: "tables",
                column: "table_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reservation_items");

            migrationBuilder.DropTable(
                name: "tables");

            migrationBuilder.DropTable(
                name: "menu_items");

            migrationBuilder.DropTable(
                name: "reservations");

            migrationBuilder.DropTable(
                name: "customers");
        }
    }
}
