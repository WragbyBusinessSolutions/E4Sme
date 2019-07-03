using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace E4S.Data.Migrations
{
    public partial class accountSalary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountName",
                table: "Salaries",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountNo",
                table: "Salaries",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "Salaries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountName",
                table: "Salaries");

            migrationBuilder.DropColumn(
                name: "AccountNo",
                table: "Salaries");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "Salaries");
        }
    }
}
