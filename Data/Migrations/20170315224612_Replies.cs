using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FoodFinder.Data.Migrations
{
    public partial class Replies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "newReviewReply",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    agree = table.Column<bool>(nullable: false),
                    newReviewId = table.Column<int>(nullable: true),
                    newUserId = table.Column<string>(nullable: true),
                    reply = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_newReviewReply", x => x.Id);
                    table.ForeignKey(
                        name: "FK_newReviewReply_newReviews_newReviewId",
                        column: x => x.newReviewId,
                        principalTable: "newReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_newReviewReply_AspNetUsers_newUserId",
                        column: x => x.newUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_newReviewReply_newReviewId",
                table: "newReviewReply",
                column: "newReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_newReviewReply_newUserId",
                table: "newReviewReply",
                column: "newUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "newReviewReply");
        }
    }
}
