using Microsoft.EntityFrameworkCore.Migrations;

namespace BNS.Migrations
{
    public partial class Receiver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Receiver",
                table: "Transactions",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Receiver",
                table: "Transactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
