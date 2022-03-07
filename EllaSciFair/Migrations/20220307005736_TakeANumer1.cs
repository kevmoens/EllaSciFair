using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EllaSciFair.Migrations
{
    public partial class TakeANumer1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "SignUp",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TakeANumbers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CurrentNumber = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TakeANumbers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TakeANumbers");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "SignUp");
        }
    }
}
