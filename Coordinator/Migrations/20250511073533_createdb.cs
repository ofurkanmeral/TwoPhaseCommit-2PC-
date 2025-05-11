using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Coordinator.Migrations
{
    /// <inheritdoc />
    public partial class createdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "Id", "name" },
                values: new object[,]
                {
                    { new Guid("35105515-6955-4fa5-8c73-a47d8e4f5d31"), "PaymentAPI" },
                    { new Guid("63c4d47a-75cd-4688-932f-f168f8c0e319"), "StockAPI" },
                    { new Guid("966c1571-2f27-45e8-bcb5-98a11c69053b"), "OrderAPI" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("35105515-6955-4fa5-8c73-a47d8e4f5d31"));

            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("63c4d47a-75cd-4688-932f-f168f8c0e319"));

            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("966c1571-2f27-45e8-bcb5-98a11c69053b"));

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
    }
}
