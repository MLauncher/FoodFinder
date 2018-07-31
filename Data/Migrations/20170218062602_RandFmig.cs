using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodFinder.Data.Migrations
{
    public partial class RandFmig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         

          

           
            

         
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_newReviews_AspNetUsers_newUserId",
                table: "newReviews");

            migrationBuilder.DropIndex(
                name: "IX_newReviews_newUserId",
                table: "newReviews");

            migrationBuilder.DropColumn(
                name: "newUserId",
                table: "newReviews");


            
        }
    }
}
