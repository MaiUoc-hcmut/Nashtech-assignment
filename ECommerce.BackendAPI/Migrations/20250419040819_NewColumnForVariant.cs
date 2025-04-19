using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class NewColumnForVariant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Variants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Variants");
        }
    }
}
