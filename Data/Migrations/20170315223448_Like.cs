using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodFinder.Data.Migrations
{
    public partial class Like : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Something",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "Like",
                table: "newReviews",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReviewDate",
                table: "newReviews",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "newFoodTable",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Like",
                table: "newReviews");

            migrationBuilder.DropColumn(
                name: "ReviewDate",
                table: "newReviews");

            migrationBuilder.AddColumn<string>(
                name: "Something",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Rating",
                table: "newFoodTable",
                nullable: false);
        }
    }
}
