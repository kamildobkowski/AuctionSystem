using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auctions.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MovePicturesToDifferentService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Auctions_AuctionId",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Pictures");

            migrationBuilder.AlterColumn<Guid>(
                name: "AuctionId",
                table: "Pictures",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "BuyNowAuctions",
                type: "numeric(2)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(2,0)",
                oldPrecision: 2);

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Auctions_AuctionId",
                table: "Pictures",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Auctions_AuctionId",
                table: "Pictures");

            migrationBuilder.AlterColumn<Guid>(
                name: "AuctionId",
                table: "Pictures",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Pictures",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Pictures",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "BuyNowAuctions",
                type: "numeric(2,0)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(2)",
                oldPrecision: 2);

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Auctions_AuctionId",
                table: "Pictures",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "Id");
        }
    }
}
