using Microsoft.EntityFrameworkCore.Migrations;

namespace Car_Renting.Migrations
{
    public partial class CarsAddedToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey("PK_AspNetUserLogins", "AspNetUserLogins");
            migrationBuilder.DropPrimaryKey("PK_AspNetUserTokens", "AspNetUserTokens");
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "IsAvailable", "Manufacturer", "Model", "Name", "Year" },
                values: new object[,]
                {
                    { 1, false, "Audi", "A6", "Audi A6", 1994 },
                    { 2, false, "BMW", "X6", "BMW X6", 2008 },
                    { 3, false, "Citroen", "A", "Citroen A", 2020 },
                    { 4, false, "Chevrolet", "Volt", "Chevrolet Volt", 2011 },
                    { 5, false, "Chrysler", "300C Sedan", "Chrysler Sedan", 2004 },
                    { 6, false, "Lamborghini", "Diablo Coupe", "Lamborghini Diablo", 2000 },
                    { 7, false, "Kia", "Optima Sedan", "Kia Optima", 2018 },
                    { 8, false, "Jaguar", "XK Coupe", "Jaguar XK Coupe", 2011 },
                    { 9, false, "Porsche", "Taycan Hatchback", "Porsche Taycan", 2019 },
                    { 10, false, "Mitsubishi", "ASX SUV", "Mitsubishi ASX SUV", 2019 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);
            migrationBuilder.AddPrimaryKey("PK_AspNetUserTokens", "AspNetUserTokens", new[] { "UserId", "LoginProvider", "Name" });
            migrationBuilder.AddPrimaryKey("PK_AspNetUserLogins", "AspNetUserLogins", new string[] { "LoginProvider", "ProviderKey" });
        }
    }
}
