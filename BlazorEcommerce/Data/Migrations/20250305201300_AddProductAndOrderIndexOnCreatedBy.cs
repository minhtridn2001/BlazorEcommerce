using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorEcommerce.Migrations
{
    /// <inheritdoc />
    public partial class AddProductAndOrderIndexOnCreatedBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Product_CreatedDate",
                table: "Product",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CreatedDate",
                table: "Order",
                column: "CreatedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Product_CreatedDate",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Order_CreatedDate",
                table: "Order");
        }
    }
}
