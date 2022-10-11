using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Otto.models.Migrations
{
    public partial class StockRequests2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "StockRequests");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "ProductsInStock");

            migrationBuilder.RenameColumn(
                name: "SellerIdMail",
                table: "StockRequests",
                newName: "UserIdMail");

            migrationBuilder.RenameColumn(
                name: "SellerIdMail",
                table: "ProductsInStock",
                newName: "UserIdMail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserIdMail",
                table: "StockRequests",
                newName: "SellerIdMail");

            migrationBuilder.RenameColumn(
                name: "UserIdMail",
                table: "ProductsInStock",
                newName: "SellerIdMail");

            migrationBuilder.AddColumn<string>(
                name: "SellerId",
                table: "StockRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SellerId",
                table: "ProductsInStock",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
