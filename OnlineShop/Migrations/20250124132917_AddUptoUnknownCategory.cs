﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.Migrations
{
    /// <inheritdoc />
    public partial class AddUptoUnknownCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add ImageUrl and Summary columns
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            // Check if Category 8 already exists before inserting
            migrationBuilder.Sql(@"
        IF NOT EXISTS (SELECT 1 FROM Categories WHERE CategoryId = 8)
        BEGIN
            INSERT INTO Categories (CategoryId, Name, ImageUrl, Summary)
            VALUES (8, 'Unknown', NULL, NULL);
        END
    ");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 8);
        }
    }
}
