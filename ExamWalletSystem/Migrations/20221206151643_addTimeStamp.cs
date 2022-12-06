using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExamWalletSystem.Migrations
{
    public partial class addTimeStamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "tblUser",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "tblTransaction",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "tblUser");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "tblTransaction");
        }
    }
}
