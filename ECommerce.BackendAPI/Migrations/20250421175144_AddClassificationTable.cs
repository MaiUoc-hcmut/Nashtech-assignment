using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddClassificationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductClassifications",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ClassificationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductClassifications", x => new { x.ProductId, x.ClassificationId });
                    table.ForeignKey(
                        name: "FK_ProductClassifications_Classifications_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Classifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductClassifications_Products_ClassificationId",
                        column: x => x.ClassificationId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductClassifications_ClassificationId",
                table: "ProductClassifications",
                column: "ClassificationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductClassifications");

            migrationBuilder.DropTable(
                name: "Classifications");
        }
    }
}
