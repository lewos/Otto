using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Otto.models.Migrations
{
    public partial class StockRequests3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "StockRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "ProductsInStock",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "StockRequests");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "ProductsInStock");
        }
    }
}
