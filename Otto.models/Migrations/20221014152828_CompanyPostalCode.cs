﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Otto.models.Migrations
{
    public partial class CompanyPostalCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Companies");
        }
    }
}
