﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EllaSciFair.Migrations
{
    public partial class Public : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "SignUp",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "SignUp");
        }
    }
}
