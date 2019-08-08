using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace E4S.Data.Migrations
{
    public partial class expenseTypeot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BankId",
                table: "OperatingExpenses",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "isPaid",
                table: "OperatingExpenses",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankId",
                table: "OperatingExpenses");

            migrationBuilder.DropColumn(
                name: "isPaid",
                table: "OperatingExpenses");
        }
    }
}
