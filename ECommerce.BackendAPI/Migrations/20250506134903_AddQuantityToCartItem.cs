using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddQuantityToCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "VariantCarts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "VariantCarts");
        }
    }
}
