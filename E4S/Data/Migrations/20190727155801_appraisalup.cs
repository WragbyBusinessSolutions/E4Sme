using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace E4S.Data.Migrations
{
    public partial class appraisalup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "AppraisalAssignedTemplates",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppraisalAssignedTemplates_AppraisalId",
                table: "AppraisalAssignedTemplates",
                column: "AppraisalId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppraisalAssignedTemplates_Appraisals_AppraisalId",
                table: "AppraisalAssignedTemplates",
                column: "AppraisalId",
                principalTable: "Appraisals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppraisalAssignedTemplates_Appraisals_AppraisalId",
                table: "AppraisalAssignedTemplates");

            migrationBuilder.DropIndex(
                name: "IX_AppraisalAssignedTemplates_AppraisalId",
                table: "AppraisalAssignedTemplates");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AppraisalAssignedTemplates");
        }
    }
}
