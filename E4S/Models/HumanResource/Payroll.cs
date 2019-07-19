using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.HumanResource
{
  public class Payroll : BaseClass
  {
    public Guid Id { get; set; }
    public Guid GenerationId { get; set; }

    public string EmployeeName { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid JobId { get; set; }
    public Guid SalaryId { get; set; }
    public float Basic { get; set; }
    public float Housing { get; set; }
    public float Transport { get; set; }
    public float Meal { get; set; }
    public float MonthlyBasic { get; set; }
    public float AnnualGrossSalary { get; set; }
    public float Overtime { get; set; }
    public float Arrears { get; set; }
    public float AnnualOTArrears { get; set; }
    public float GrossMonthlyEmolument { get; set; }
    public float GrossAnnualEmolument { get; set; }
    public float PensionFund { get; set; }
    public float NHIS { get; set; }
    public float Gratuities { get; set; }
    public float LifeAssurance { get; set; }
    public float ConsolidatedReliefsAllowance { get; set; }
    public float TotalAnnualRellief { get; set; }
    public float TaxableIncome { get; set; }
    public float ComputedAnnualTax { get; set; }
    public float MinimumTax { get; set; }
    public float MonthlyMinimumTax { get; set; }
    public float AnnualPayableTax { get; set; }
    public float AnnualActualTaxPayable { get; set; }
    public float MonthlyActualTaxPayable { get; set; }
    public float PensionDeductionMonthly { get; set; }
    public float StaffLoan { get; set; }
    public float AbsentDeductions { get; set; }
    public float CooperativeDeduction { get; set; }
    public float OtherDeductions { get; set; }
    public float TotalDeduction { get; set; }
    public float Addition { get; set; }
    public float PayableToStaff { get; set; }
    public string PayrollMonth { get; set; }
    public string AccountNo { get; set; }
    public string BankName { get; set; }
    public string AccountName { get; set; }

    public string Month { get; set; }
    public string Year { get; set; }
  }
}
