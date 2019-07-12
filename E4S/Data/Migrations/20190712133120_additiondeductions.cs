using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace E4S.Data.Migrations
{
    public partial class additiondeductions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Additions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AdditionType = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrganisationId = table.Column<Guid>(nullable: false),
                    Others = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Additions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Deductions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    DeductionType = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrganisationId = table.Column<Guid>(nullable: false),
                    Others = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deductions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalaryAdditions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AdditionId = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    EmployeeDetailId = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrganisationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryAdditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryAdditions_Additions_AdditionId",
                        column: x => x.AdditionId,
                        principalTable: "Additions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalaryAdditions_EmployeeDetails_EmployeeDetailId",
                        column: x => x.EmployeeDetailId,
                        principalTable: "EmployeeDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalaryDeductions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    DeductionId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    EmployeeDetailId = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrganisationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryDeductions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryDeductions_Deductions_DeductionId",
                        column: x => x.DeductionId,
                        principalTable: "Deductions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalaryDeductions_EmployeeDetails_EmployeeDetailId",
                        column: x => x.EmployeeDetailId,
                        principalTable: "EmployeeDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalaryAdditions_AdditionId",
                table: "SalaryAdditions",
                column: "AdditionId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryAdditions_EmployeeDetailId",
                table: "SalaryAdditions",
                column: "EmployeeDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryDeductions_DeductionId",
                table: "SalaryDeductions",
                column: "DeductionId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryDeductions_EmployeeDetailId",
                table: "SalaryDeductions",
                column: "EmployeeDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalaryAdditions");

            migrationBuilder.DropTable(
                name: "SalaryDeductions");

            migrationBuilder.DropTable(
                name: "Additions");

            migrationBuilder.DropTable(
                name: "Deductions");
        }
    }
}
