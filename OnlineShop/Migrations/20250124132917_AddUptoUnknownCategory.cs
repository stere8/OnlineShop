using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.Migrations
{
    /// <inheritdoc />
    public partial class AddUptoUnknownCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            // Check if Category 8 already exists before inserting
            migrationBuilder.Sql(@"
    SET IDENTITY_INSERT Categories ON;

    IF NOT EXISTS (SELECT 1 FROM Categories WHERE CategoryId = 8)
    BEGIN
        INSERT INTO Categories (CategoryId, Name, ImageUrl, Summary)
        VALUES (8, 'Unknown', NULL, NULL);
    END

    SET IDENTITY_INSERT Categories OFF;
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
