using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace E4S.Data.Migrations
{
    public partial class paygradeupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Salaries_PayGrades_PayGradeId",
                table: "Salaries");

            migrationBuilder.DropIndex(
                name: "IX_Salaries_PayGradeId",
                table: "Salaries");

            migrationBuilder.DropColumn(
                name: "PayGradeId",
                table: "Salaries");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PayGradeId",
                table: "Salaries",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_PayGradeId",
                table: "Salaries",
                column: "PayGradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Salaries_PayGrades_PayGradeId",
                table: "Salaries",
                column: "PayGradeId",
                principalTable: "PayGrades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
