using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FoodFinder.Data.Migrations
{
    public partial class Another : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "newFoodTable",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    FoodName = table.Column<string>(nullable: true),
                    PicturesPath = table.Column<string>(nullable: true),
                    RId = table.Column<int>(nullable: true),
                    foodColumn = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_newFoodTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "newReviews",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FId = table.Column<int>(nullable: true),
                    Review1 = table.Column<string>(nullable: true),
                    UId = table.Column<string>(nullable: true),
                    newColumn = table.Column<string>(nullable: true),
                    newFoodTableId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_newReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_newReviews_newFoodTable_newFoodTableId",
                        column: x => x.newFoodTableId,
                        principalTable: "newFoodTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });


            migrationBuilder.CreateIndex(
                name: "IX_newReviews_newFoodTableId",
                table: "newReviews",
                column: "newFoodTableId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropTable(
                name: "newReviews");

            migrationBuilder.DropTable(
                name: "newFoodTable");
        }
    }
}
