using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coincap.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Symbol = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Rank = table.Column<int>(type: "INTEGER", nullable: false),
                    PriceUsd = table.Column<decimal>(type: "TEXT", precision: 36, scale: 18, nullable: false),
                    MarketCapUsd = table.Column<decimal>(type: "TEXT", precision: 36, scale: 18, nullable: false),
                    VolumeUsd24Hr = table.Column<decimal>(type: "TEXT", precision: 36, scale: 18, nullable: false),
                    ChangePercent24Hr = table.Column<decimal>(type: "TEXT", precision: 18, scale: 8, nullable: false),
                    LastSyncAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assets");
        }
    }
}
