using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuctionApp.Migrations.auction
{
    /// <inheritdoc />
    public partial class Initial_auction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Auctions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    User = table.Column<string>(type: "longtext", nullable: false),
                    OpeningBid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auctions", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Bids",
                columns: table => new
                {
                    BidId = table.Column<Guid>(type: "char(36)", nullable: false),
                    User = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    BidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BidDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AuctionId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bids", x => x.BidId);
                    table.ForeignKey(
                        name: "FK_Bids_Auctions_AuctionId",
                        column: x => x.AuctionId,
                        principalTable: "Auctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Auctions",
                columns: new[] { "Id", "Description", "ExpirationDate", "Name", "OpeningBid", "User" },
                values: new object[] { new Guid("18357f3f-feb9-4a21-903b-fb8192c969c8"), "New Auction description", new DateTime(2024, 10, 18, 2, 52, 14, 370, DateTimeKind.Local).AddTicks(1784), "New Auction", 1m, "seed@kth.se" });

            migrationBuilder.InsertData(
                table: "Bids",
                columns: new[] { "BidId", "AuctionId", "BidAmount", "BidDate", "User" },
                values: new object[,]
                {
                    { new Guid("0598c2bf-5add-43bd-a506-2004cb697240"), new Guid("18357f3f-feb9-4a21-903b-fb8192c969c8"), 1m, new DateTime(2024, 10, 11, 2, 52, 14, 370, DateTimeKind.Local).AddTicks(2099), "bidder@kth.se" },
                    { new Guid("da00fe98-e4f4-49d1-a5f2-12eeb2b8d753"), new Guid("18357f3f-feb9-4a21-903b-fb8192c969c8"), 3m, new DateTime(2024, 10, 11, 2, 52, 14, 370, DateTimeKind.Local).AddTicks(2104), "bidder@kth.se" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bids_AuctionId",
                table: "Bids",
                column: "AuctionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bids");

            migrationBuilder.DropTable(
                name: "Auctions");
        }
    }
}
