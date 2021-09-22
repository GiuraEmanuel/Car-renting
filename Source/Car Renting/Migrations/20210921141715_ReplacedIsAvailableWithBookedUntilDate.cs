using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Car_Renting.Migrations
{
    public partial class ReplacedIsAvailableWithBookedUntilDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Cars");

            migrationBuilder.AddColumn<DateTime>(
                name: "BookedUntilDate",
                table: "Cars",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookedUntilDate",
                table: "Cars");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Cars",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
