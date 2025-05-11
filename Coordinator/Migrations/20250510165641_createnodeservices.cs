using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Coordinator.Migrations
{
    /// <inheritdoc />
    public partial class createnodeservices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "Id", "name" },
                values: new object[,]
                {
                    { new Guid("2e3c3672-5b26-4fda-a86f-7f14016cee79"), "StockAPI" },
                    { new Guid("412fd884-e89a-4c10-aa8e-4848d89ed204"), "OrderAPI" },
                    { new Guid("8a1d8354-9af9-4340-991f-1b82f78ce662"), "PaymentAPI" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("2e3c3672-5b26-4fda-a86f-7f14016cee79"));

            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("412fd884-e89a-4c10-aa8e-4848d89ed204"));

            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("8a1d8354-9af9-4340-991f-1b82f78ce662"));
        }
    }
}
