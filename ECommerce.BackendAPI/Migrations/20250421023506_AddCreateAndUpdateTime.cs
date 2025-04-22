using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCreateAndUpdateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VariantCart_Carts_CartId",
                table: "VariantCart");

            migrationBuilder.DropForeignKey(
                name: "FK_VariantCart_Variants_VariantId",
                table: "VariantCart");

            migrationBuilder.DropForeignKey(
                name: "FK_VariantCategory_Categories_CategoryId",
                table: "VariantCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_VariantCategory_Variants_VariantId",
                table: "VariantCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_VariantOrder_Orders_OrderId",
                table: "VariantOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_VariantOrder_Variants_VariantId",
                table: "VariantOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VariantOrder",
                table: "VariantOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VariantCategory",
                table: "VariantCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VariantCart",
                table: "VariantCart");

            migrationBuilder.RenameTable(
                name: "VariantOrder",
                newName: "VariantOrders");

            migrationBuilder.RenameTable(
                name: "VariantCategory",
                newName: "VariantCategories");

            migrationBuilder.RenameTable(
                name: "VariantCart",
                newName: "VariantCarts");

            migrationBuilder.RenameIndex(
                name: "IX_VariantOrder_OrderId",
                table: "VariantOrders",
                newName: "IX_VariantOrders_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_VariantCategory_CategoryId",
                table: "VariantCategories",
                newName: "IX_VariantCategories_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_VariantCart_CartId",
                table: "VariantCarts",
                newName: "IX_VariantCarts_CartId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Variants",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Variants",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Reviews",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Reviews",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "VariantOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "VariantOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "VariantCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "VariantCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "VariantCarts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "VariantCarts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_VariantOrders",
                table: "VariantOrders",
                columns: new[] { "VariantId", "OrderId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_VariantCategories",
                table: "VariantCategories",
                columns: new[] { "VariantId", "CategoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_VariantCarts",
                table: "VariantCarts",
                columns: new[] { "VariantId", "CartId" });

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_VariantCarts_Carts_CartId",
                table: "VariantCarts",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VariantCarts_Variants_VariantId",
                table: "VariantCarts",
                column: "VariantId",
                principalTable: "Variants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VariantCategories_Categories_CategoryId",
                table: "VariantCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VariantCategories_Variants_VariantId",
                table: "VariantCategories",
                column: "VariantId",
                principalTable: "Variants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VariantOrders_Orders_OrderId",
                table: "VariantOrders",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VariantOrders_Variants_VariantId",
                table: "VariantOrders",
                column: "VariantId",
                principalTable: "Variants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VariantCarts_Carts_CartId",
                table: "VariantCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_VariantCarts_Variants_VariantId",
                table: "VariantCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_VariantCategories_Categories_CategoryId",
                table: "VariantCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_VariantCategories_Variants_VariantId",
                table: "VariantCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_VariantOrders_Orders_OrderId",
                table: "VariantOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_VariantOrders_Variants_VariantId",
                table: "VariantOrders");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VariantOrders",
                table: "VariantOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VariantCategories",
                table: "VariantCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VariantCarts",
                table: "VariantCarts");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "VariantOrders");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "VariantOrders");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "VariantCategories");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "VariantCategories");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "VariantCarts");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "VariantCarts");

            migrationBuilder.RenameTable(
                name: "VariantOrders",
                newName: "VariantOrder");

            migrationBuilder.RenameTable(
                name: "VariantCategories",
                newName: "VariantCategory");

            migrationBuilder.RenameTable(
                name: "VariantCarts",
                newName: "VariantCart");

            migrationBuilder.RenameIndex(
                name: "IX_VariantOrders_OrderId",
                table: "VariantOrder",
                newName: "IX_VariantOrder_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_VariantCategories_CategoryId",
                table: "VariantCategory",
                newName: "IX_VariantCategory_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_VariantCarts_CartId",
                table: "VariantCart",
                newName: "IX_VariantCart_CartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VariantOrder",
                table: "VariantOrder",
                columns: new[] { "VariantId", "OrderId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_VariantCategory",
                table: "VariantCategory",
                columns: new[] { "VariantId", "CategoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_VariantCart",
                table: "VariantCart",
                columns: new[] { "VariantId", "CartId" });

            migrationBuilder.AddForeignKey(
                name: "FK_VariantCart_Carts_CartId",
                table: "VariantCart",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VariantCart_Variants_VariantId",
                table: "VariantCart",
                column: "VariantId",
                principalTable: "Variants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VariantCategory_Categories_CategoryId",
                table: "VariantCategory",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VariantCategory_Variants_VariantId",
                table: "VariantCategory",
                column: "VariantId",
                principalTable: "Variants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VariantOrder_Orders_OrderId",
                table: "VariantOrder",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VariantOrder_Variants_VariantId",
                table: "VariantOrder",
                column: "VariantId",
                principalTable: "Variants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
