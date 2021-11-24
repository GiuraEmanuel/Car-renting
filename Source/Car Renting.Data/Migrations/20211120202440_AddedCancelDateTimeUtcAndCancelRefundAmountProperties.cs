using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Car_Renting.Data.Migrations
{
    public partial class AddedCancelDateTimeUtcAndCancelRefundAmountProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CancelDateTimeUtc",
                table: "Bookings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CancelRefundAmount",
                table: "Bookings",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelDateTimeUtc",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "CancelRefundAmount",
                table: "Bookings");
        }
    }
}
