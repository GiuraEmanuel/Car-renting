using Microsoft.EntityFrameworkCore.Migrations;

namespace Car_Renting.Data.Migrations
{
    public partial class FixBookingDateNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BookingStart",
                table: "Bookings",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "BookingEnd",
                table: "Bookings",
                newName: "EndDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Bookings",
                newName: "BookingStart");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Bookings",
                newName: "BookingEnd");
        }
    }
}
