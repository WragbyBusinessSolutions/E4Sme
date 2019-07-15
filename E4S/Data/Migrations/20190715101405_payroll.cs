using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace E4S.Data.Migrations
{
    public partial class payroll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payrolls",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AbsentDeductions = table.Column<float>(nullable: false),
                    AccountName = table.Column<string>(nullable: true),
                    AccountNo = table.Column<string>(nullable: true),
                    Addition = table.Column<float>(nullable: false),
                    AnnualActualTaxPayable = table.Column<float>(nullable: false),
                    AnnualGrossSalary = table.Column<float>(nullable: false),
                    AnnualOTArrears = table.Column<float>(nullable: false),
                    AnnualPayableTax = table.Column<float>(nullable: false),
                    Arrears = table.Column<float>(nullable: false),
                    BankName = table.Column<string>(nullable: true),
                    Basic = table.Column<float>(nullable: false),
                    ComputedAnnualTax = table.Column<float>(nullable: false),
                    ConsolidatedReliefsAllowance = table.Column<float>(nullable: false),
                    CooperativeDeduction = table.Column<float>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    EmployeeId = table.Column<Guid>(nullable: false),
                    EmployeeName = table.Column<string>(nullable: true),
                    Gratuities = table.Column<float>(nullable: false),
                    GrossAnnualEmolument = table.Column<float>(nullable: false),
                    GrossMonthlyEmolument = table.Column<float>(nullable: false),
                    Housing = table.Column<float>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    JobId = table.Column<Guid>(nullable: false),
                    LifeAssurance = table.Column<float>(nullable: false),
                    Meal = table.Column<float>(nullable: false),
                    MinimumTax = table.Column<float>(nullable: false),
                    Month = table.Column<string>(nullable: true),
                    MonthlyActualTaxPayable = table.Column<float>(nullable: false),
                    MonthlyBasic = table.Column<float>(nullable: false),
                    MonthlyMinimumTax = table.Column<float>(nullable: false),
                    NHIS = table.Column<float>(nullable: false),
                    OrganisationId = table.Column<Guid>(nullable: false),
                    OtherDeductions = table.Column<float>(nullable: false),
                    Overtime = table.Column<float>(nullable: false),
                    PayableToStaff = table.Column<float>(nullable: false),
                    PayrollMonth = table.Column<string>(nullable: true),
                    PensionDeductionMonthly = table.Column<float>(nullable: false),
                    PensionFund = table.Column<float>(nullable: false),
                    SalaryId = table.Column<Guid>(nullable: false),
                    StaffLoan = table.Column<float>(nullable: false),
                    TaxableIncome = table.Column<float>(nullable: false),
                    TotalAnnualRellief = table.Column<float>(nullable: false),
                    TotalDeduction = table.Column<float>(nullable: false),
                    Transport = table.Column<float>(nullable: false),
                    Year = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payrolls", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payrolls");
        }
    }
}
