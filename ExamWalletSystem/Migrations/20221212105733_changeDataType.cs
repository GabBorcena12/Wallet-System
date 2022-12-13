using Microsoft.EntityFrameworkCore.Migrations;

namespace ExamWalletSystem.Migrations
{
    public partial class changeDataType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tblUser_AccountNumber",
                table: "tblUser");

            migrationBuilder.AlterColumn<long>(
                name: "AccountNumber",
                table: "tblUser",
                type: "bigint",
                maxLength: 12,
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "AccountNumberTo",
                table: "tblTransaction",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "AccountNumberFrom",
                table: "tblTransaction",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblUser_AccountNumber",
                table: "tblUser",
                column: "AccountNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tblUser_AccountNumber",
                table: "tblUser");

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "tblUser",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldMaxLength: 12);

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumberTo",
                table: "tblTransaction",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumberFrom",
                table: "tblTransaction",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_tblUser_AccountNumber",
                table: "tblUser",
                column: "AccountNumber",
                unique: true,
                filter: "[AccountNumber] IS NOT NULL");
        }
    }
}
