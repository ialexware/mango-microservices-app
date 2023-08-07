using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mango.Services.CouponAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedCouponTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "coupons",
                columns: new[] { "CouponId", "CouponCode", "DsicountAmount", "MinAmount" },
                values: new object[,]
                {
                    { 1, "10OFF", 10.0, 100 },
                    { 2, "20OFF", 20.0, 200 },
                    { 3, "30OFF", 30.0, 300 },
                    { 4, "40OFF", 40.0, 400 },
                    { 5, "50OFF", 50.0, 500 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "coupons",
                keyColumn: "CouponId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "coupons",
                keyColumn: "CouponId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "coupons",
                keyColumn: "CouponId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "coupons",
                keyColumn: "CouponId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "coupons",
                keyColumn: "CouponId",
                keyValue: 5);
        }
    }
}
