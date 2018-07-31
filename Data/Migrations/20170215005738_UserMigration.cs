using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodFinder.Data.Migrations
{
    public partial class UserMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "newUserIdId",
                table: "newReviews",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_newReviews_newUserIdId",
                table: "newReviews",
                column: "newUserIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_newReviews_AspNetUsers_newUserIdId",
                table: "newReviews",
                column: "newUserIdId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_newReviews_AspNetUsers_newUserIdId",
                table: "newReviews");

            migrationBuilder.DropIndex(
                name: "IX_newReviews_newUserIdId",
                table: "newReviews");

            migrationBuilder.DropColumn(
                name: "newUserIdId",
                table: "newReviews");
        }
    }
}
