using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GardenHub.Repository.Migrations
{
    public partial class updateaDbComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Post",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "AccountEmail",
                table: "Post",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PostIdFromRoute",
                table: "Comment",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountEmail",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "PostIdFromRoute",
                table: "Comment");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Post",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
