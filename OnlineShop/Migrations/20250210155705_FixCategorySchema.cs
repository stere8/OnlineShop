using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnlineShop.Migrations
{
    /// <inheritdoc />
    public partial class FixCategorySchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 20);

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.CartId);
                    table.ForeignKey(
                        name: "FK_Carts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    CartItemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.CartItemID);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "CartId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CategoryId", "Description", "ImageUrl", "Name", "Price", "StockQuantity" },
                values: new object[,]
                {
                    { 1, 1, "High-performance laptop", "laptop.jpg", "Laptop", 1500.00m, 10 },
                    { 2, 1, "Latest smartphone with 5G", "smartphone.jpg", "Smartphone", 800.00m, 15 },
                    { 3, 1, "Portable Bluetooth speaker with high-quality sound", "speaker.jpg", "Bluetooth Speaker", 100.00m, 20 },
                    { 4, 1, "Waterproof smartwatch with health tracking features", "smartwatch.jpg", "Smartwatch", 200.00m, 30 },
                    { 5, 1, "Noise-cancelling over-ear headphones", "headphones.jpg", "Headphones", 75.00m, 25 },
                    { 6, 2, "100% cotton T-shirt", "tshirt.jpg", "T-shirt", 20.00m, 50 },
                    { 7, 2, "Classic blue jeans", "jeans.jpg", "Jeans", 45.00m, 40 },
                    { 8, 2, "Warm winter jacket", "jacket.jpg", "Jacket", 100.00m, 15 },
                    { 9, 2, "Comfortable running shoes", "sneakers.jpg", "Sneakers", 60.00m, 35 },
                    { 10, 2, "Elegant evening dress", "dress.jpg", "Dress", 80.00m, 20 },
                    { 11, 3, "Compact microwave oven", "microwave.jpg", "Microwave Oven", 150.00m, 10 },
                    { 12, 3, "Double-door refrigerator with smart features", "refrigerator.jpg", "Refrigerator", 1200.00m, 5 },
                    { 13, 3, "Front-loading washing machine", "washingmachine.jpg", "Washing Machine", 500.00m, 8 },
                    { 14, 3, "Energy-efficient air conditioner", "airconditioner.jpg", "Air Conditioner", 400.00m, 7 },
                    { 15, 3, "Multi-functional kitchen blender", "blender.jpg", "Blender", 30.00m, 50 },
                    { 16, 4, "Classic novel by F. Scott Fitzgerald", "greatgatsby.jpg", "Novel - The Great Gatsby", 10.00m, 100 },
                    { 17, 4, "Comprehensive high school science textbook", "sciencetextbook.jpg", "Science Textbook", 50.00m, 30 },
                    { 18, 4, "Illustrated story book for kids", "storybook.jpg", "Children's Story Book", 15.00m, 70 },
                    { 19, 5, "500-piece puzzle game", "puzzle.jpg", "Puzzle Game", 25.00m, 60 },
                    { 20, 5, "Remote-controlled toy car", "toycar.jpg", "Toy Car", 10.00m, 45 }
                });
        }
    }
}
