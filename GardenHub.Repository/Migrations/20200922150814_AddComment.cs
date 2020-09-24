using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GardenHub.Repository.Migrations
{
    public partial class AddComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Comment");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountOwnerId",
                table: "Comment",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "PostedTime",
                table: "Comment",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountOwnerId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "PostedTime",
                table: "Comment");

            migrationBuilder.AddColumn<Guid>(
                name: "Owner",
                table: "Comment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
