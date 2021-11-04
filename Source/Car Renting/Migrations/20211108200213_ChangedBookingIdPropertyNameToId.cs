using Microsoft.EntityFrameworkCore.Migrations;

namespace Car_Renting.Migrations
{
    public partial class ChangedBookingIdPropertyNameToId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BookingId",
                table: "Bookings",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Bookings",
                newName: "BookingId");
        }
    }
}
