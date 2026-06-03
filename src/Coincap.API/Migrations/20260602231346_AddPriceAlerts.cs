using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coincap.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceAlerts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PriceAlerts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AssetId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    AssetName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    AssetSymbol = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    PriceBefore = table.Column<decimal>(type: "TEXT", precision: 36, scale: 18, nullable: false),
                    PriceAfter = table.Column<decimal>(type: "TEXT", precision: 36, scale: 18, nullable: false),
                    ChangePercent = table.Column<decimal>(type: "TEXT", precision: 18, scale: 8, nullable: false),
                    Direction = table.Column<string>(type: "TEXT", maxLength: 4, nullable: false),
                    DetectedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceAlerts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceAlerts_AssetId",
                table: "PriceAlerts",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceAlerts_DetectedAt",
                table: "PriceAlerts",
                column: "DetectedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceAlerts");
        }
    }
}
