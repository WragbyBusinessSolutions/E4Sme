using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace E4S.Data.Migrations
{
    public partial class institutionqualification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InstitutionQualifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CourseOfStudy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    Degree = table.Column<string>(nullable: true),
                    EmployeeDetailId = table.Column<Guid>(nullable: false),
                    Grade = table.Column<string>(nullable: true),
                    ImageURL = table.Column<string>(nullable: true),
                    Institution = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrganisationId = table.Column<Guid>(nullable: false),
                    YearCompleted = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionQualifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstitutionQualifications_EmployeeDetails_EmployeeDetailId",
                        column: x => x.EmployeeDetailId,
                        principalTable: "EmployeeDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionQualifications_EmployeeDetailId",
                table: "InstitutionQualifications",
                column: "EmployeeDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstitutionQualifications");
        }
    }
}
