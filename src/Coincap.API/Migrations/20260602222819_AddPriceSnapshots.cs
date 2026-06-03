using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coincap.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceSnapshots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PriceSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AssetId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    AssetSymbol = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    PriceUsd = table.Column<decimal>(type: "TEXT", precision: 36, scale: 18, nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceSnapshots", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceSnapshots_AssetId_RecordedAt",
                table: "PriceSnapshots",
                columns: new[] { "AssetId", "RecordedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceSnapshots");
        }
    }
}
