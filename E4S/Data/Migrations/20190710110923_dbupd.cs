using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace E4S.Data.Migrations
{
    public partial class dbupd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Branches_BranchId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_EmploymentStatuses_EmploymentStatusId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobCategories_JobCategoryId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_BranchId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_EmploymentStatusId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_JobCategoryId",
                table: "Jobs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Jobs_BranchId",
                table: "Jobs",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_EmploymentStatusId",
                table: "Jobs",
                column: "EmploymentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_JobCategoryId",
                table: "Jobs",
                column: "JobCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Branches_BranchId",
                table: "Jobs",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_EmploymentStatuses_EmploymentStatusId",
                table: "Jobs",
                column: "EmploymentStatusId",
                principalTable: "EmploymentStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_JobCategories_JobCategoryId",
                table: "Jobs",
                column: "JobCategoryId",
                principalTable: "JobCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
