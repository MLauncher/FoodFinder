using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodFinder.Data.Migrations
{
    public partial class Rating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "newFoodTable",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "newFoodTable");

            migrationBuilder.AddColumn<string>(
                name: "newColumn",
                table: "newReviews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "foodColumn",
                table: "newFoodTable",
                nullable: true);
        }
    }
}
