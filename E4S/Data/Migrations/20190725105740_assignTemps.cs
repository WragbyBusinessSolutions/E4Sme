using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace E4S.Data.Migrations
{
    public partial class assignTemps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppraisalAssignedTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AppraisalTemplateId = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    EmployeeDetailId = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrganisationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppraisalAssignedTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppraisalAssignedTemplates_AppraisalTemplates_AppraisalTemplateId",
                        column: x => x.AppraisalTemplateId,
                        principalTable: "AppraisalTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppraisalAssignedTemplates_EmployeeDetails_EmployeeDetailId",
                        column: x => x.EmployeeDetailId,
                        principalTable: "EmployeeDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppraisalAssignedTemplates_AppraisalTemplateId",
                table: "AppraisalAssignedTemplates",
                column: "AppraisalTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_AppraisalAssignedTemplates_EmployeeDetailId",
                table: "AppraisalAssignedTemplates",
                column: "EmployeeDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppraisalAssignedTemplates");
        }
    }
}
