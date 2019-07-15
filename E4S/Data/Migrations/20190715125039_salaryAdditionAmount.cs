using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace E4S.Data.Migrations
{
    public partial class salaryAdditionAmount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Amount",
                table: "SalaryAdditions",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "SalaryAdditions");
        }
    }
}
