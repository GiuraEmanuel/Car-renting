using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Car_Renting.Migrations
{
    public partial class DeletedNameAndBookedUntilDateProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DropColumn(
                name: "BookedUntilDate",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Cars");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BookedUntilDate",
                table: "Cars",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "BookedUntilDate", "Manufacturer", "Model", "Name", "PricePerDay", "Status", "Year" },
                values: new object[,]
                {
                    { 1, null, "Audi", "A6", "Audi A6", 0m, 0, 1994 },
                    { 2, null, "BMW", "X6", "BMW X6", 0m, 0, 2008 },
                    { 3, null, "Citroen", "A", "Citroen A", 0m, 0, 2020 },
                    { 4, null, "Chevrolet", "Volt", "Chevrolet Volt", 0m, 0, 2011 },
                    { 5, null, "Chrysler", "300C Sedan", "Chrysler Sedan", 0m, 0, 2004 },
                    { 6, null, "Lamborghini", "Diablo Coupe", "Lamborghini Diablo", 0m, 0, 2000 },
                    { 7, null, "Kia", "Optima Sedan", "Kia Optima", 0m, 0, 2018 },
                    { 8, null, "Jaguar", "XK Coupe", "Jaguar XK Coupe", 0m, 0, 2011 },
                    { 9, null, "Porsche", "Taycan Hatchback", "Porsche Taycan", 0m, 0, 2019 },
                    { 10, null, "Mitsubishi", "ASX SUV", "Mitsubishi ASX SUV", 0m, 0, 2019 }
                });
        }
    }
}
