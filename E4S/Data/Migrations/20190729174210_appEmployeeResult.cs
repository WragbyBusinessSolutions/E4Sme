using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace E4S.Data.Migrations
{
    public partial class appEmployeeResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppraisalEmployeeResults",
                columns: table => new
                {
                    OrganisationId = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    AppraisalId = table.Column<Guid>(nullable: false),
                    EmployeeDetailId = table.Column<Guid>(nullable: false),
                    AppraisalScore = table.Column<float>(nullable: false),
                    EmployeePerformanceReview = table.Column<string>(nullable: true),
                    DateSubmited = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppraisalEmployeeResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KPIEmployeeResults",
                columns: table => new
                {
                    OrganisationId = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    AppraisalId = table.Column<Guid>(nullable: false),
                    EmployeeDetailId = table.Column<Guid>(nullable: false),
                    AppraisalEmployeeResultId = table.Column<Guid>(nullable: false),
                    KPIId = table.Column<Guid>(nullable: false),
                    KPISelection = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KPIEmployeeResults", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppraisalEmployeeResults");

            migrationBuilder.DropTable(
                name: "KPIEmployeeResults");
        }
    }
}
