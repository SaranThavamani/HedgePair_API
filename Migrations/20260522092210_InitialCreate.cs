using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HedgePair.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Financial_Instrument",
                columns: table => new
                {
                    FIN_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deal_Number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Notional_Amt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    deal_type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    deal_status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Financial_Instrument", x => x.FIN_ID);
                });

            migrationBuilder.CreateTable(
                name: "Hedge_Pair",
                columns: table => new
                {
                    HEDGE_PAIR_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BOND_FIN_ID = table.Column<int>(type: "int", nullable: false),
                    SWAP_FIN_ID = table.Column<int>(type: "int", nullable: false),
                    BOND_NOTIONAL_AMT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SWAP_NOTIONAL_AMT = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hedge_Pair", x => x.HEDGE_PAIR_ID);
                    table.ForeignKey(
                        name: "FK_Hedge_Pair_Financial_Instrument_BOND_FIN_ID",
                        column: x => x.BOND_FIN_ID,
                        principalTable: "Financial_Instrument",
                        principalColumn: "FIN_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hedge_Pair_Financial_Instrument_SWAP_FIN_ID",
                        column: x => x.SWAP_FIN_ID,
                        principalTable: "Financial_Instrument",
                        principalColumn: "FIN_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Financial_Instrument",
                columns: new[] { "FIN_ID", "Deal_Number", "deal_status", "deal_type", "Notional_Amt" },
                values: new object[,]
                {
                    { 1, "DL1234567", "Active", "BOND", 100000.50m },
                    { 2, "DL2345678", "Active", "BOND", 200000.75m },
                    { 3, "DL3456789", "Active", "BOND", 150000.00m },
                    { 4, "DL4567890", "Active", "BOND", 250000.25m },
                    { 5, "DL5678901", "Inactive", "BOND", 300000.10m },
                    { 6, "DL1234568", "Active", "SWAP", 100000.50m },
                    { 7, "DL2345679", "Active", "SWAP", 200000.75m },
                    { 8, "DL3456780", "Active", "SWAP", 150000.00m },
                    { 9, "DL4567810", "Active", "SWAP", 250000.25m },
                    { 10, "DL5678911", "Inactive", "SWAP", 300000.10m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Financial_Instrument_Deal_Number",
                table: "Financial_Instrument",
                column: "Deal_Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hedge_Pair_BOND_FIN_ID",
                table: "Hedge_Pair",
                column: "BOND_FIN_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hedge_Pair_SWAP_FIN_ID",
                table: "Hedge_Pair",
                column: "SWAP_FIN_ID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hedge_Pair");

            migrationBuilder.DropTable(
                name: "Financial_Instrument");
        }
    }
}
