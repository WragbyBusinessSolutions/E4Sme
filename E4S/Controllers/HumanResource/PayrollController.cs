using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using E4S.Data;
using E4S.Models;
using E4S.Models.HumanResource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E4S.Controllers.HumanResource
{
    [Authorize]
    public class PayrollController : Controller
    {

    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public PayrollController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
      _context = context;
      _userManager = userManager;

    }
    private Guid getOrg()
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var orgId = _context.Users.Where(x => x.Id == userId).FirstOrDefault().OrganisationId;

      var orgdetails = _context.Organisations.Where(x => x.Id == orgId).FirstOrDefault();
      ViewData["OrganisationName"] = orgdetails.OrganisationName;
      ViewData["OrganisationImage"] = orgdetails.ImageUrl;

      return orgId;
    }

        [Produces("application/json")]
        [HttpGet("search")]
        [Route("/api/Payroll/search")]
        public async Task<IActionResult> Search()
        {

      var orgId = getOrg();


            //var otheruser =  _context.Organisations.Where(x => x.Id == orgId).FirstOrDefault();

            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var names = _context.EmployeeDetails.Where(x => x.OrganisationId == orgId).Where(p => p.FirstName.Contains(term) || p.LastName.Contains(term)).Select(p => p.FirstName + " " +  p.LastName).ToList();
                return Ok(names);
            }
            catch
            {
                return BadRequest();
            }
        }

        public IActionResult Index()
        {
          var orgId = getOrg();

          return View();
        }
        public IActionResult SalaryAdditions()
        {
          var orgId = getOrg();

          return View();
        }
        public IActionResult SalaryDeductions()
        {
          var orgId = getOrg();

          return View();
        }

    public async Task<IActionResult> GeneratePayroll()
    {
      var orgId = getOrg();

      var AllPayrollUser = _context.EmployeeDetails.Where(x => x.OrganisationId == orgId).ToList();
      var GenId = Guid.NewGuid();

      foreach (var item in AllPayrollUser)
      {
        var result = this.ComputePaySlip(item, DateTime.Now.Month.ToString(), GenId);
      }

      var payrolllist = await _context.Payrolls.Where(x => x.GenerationId == GenId).ToListAsync();

      return View(payrolllist);
    }


    private async Task ComputePaySlip(EmployeeDetail employeeInformation, string Paymonth, Guid genId)
    {
      Payroll payrollDetails = new Payroll();
      var employeeSalaryDetails = _context.Salaries.Where(x => x.EmployeeDetailId == employeeInformation.Id).FirstOrDefault();
      var salary = employeeSalaryDetails.Amount;

      payrollDetails.GenerationId = genId;
      payrollDetails.EmployeeName = employeeInformation.FirstName + " " + employeeInformation.LastName;
      payrollDetails.Basic = ((float)((salary * 12) * 0.5));
      payrollDetails.Housing = ((float)((salary * 12) * 0.3));
      payrollDetails.Transport = ((float)((salary * 12) * 0.12));
      payrollDetails.Meal = ((float)((salary * 12) * 0.08));
      payrollDetails.BankName = employeeSalaryDetails.BankName;
      payrollDetails.AccountNo = employeeSalaryDetails.AccountNo;
      payrollDetails.MonthlyBasic = salary;
      payrollDetails.AccountName = employeeSalaryDetails.AccountName;
      payrollDetails.AnnualGrossSalary = (payrollDetails.MonthlyBasic * 12);
      payrollDetails.Overtime = 0;
      payrollDetails.Arrears = 0;
      payrollDetails.AnnualOTArrears = (payrollDetails.Overtime * 12) + (payrollDetails.Arrears * 12);
      payrollDetails.GrossMonthlyEmolument = ((payrollDetails.AnnualGrossSalary / 12) + payrollDetails.Overtime + payrollDetails.Arrears);
      payrollDetails.GrossAnnualEmolument = payrollDetails.GrossMonthlyEmolument * 12;
      payrollDetails.PensionFund = (float)((payrollDetails.Basic + payrollDetails.Housing + payrollDetails.Transport) * 0.08);
      payrollDetails.NHIS = 0;
      payrollDetails.Gratuities = 0;
      payrollDetails.LifeAssurance = 0;

      if ((payrollDetails.GrossAnnualEmolument * 0.01) > 200000)
      {
        payrollDetails.ConsolidatedReliefsAllowance = (float)((payrollDetails.GrossAnnualEmolument * 0.01) + (payrollDetails.GrossAnnualEmolument * 0.2));
      }
      else
      {
        payrollDetails.ConsolidatedReliefsAllowance = (float)(200000 + (payrollDetails.GrossAnnualEmolument * 0.2));
      }

      payrollDetails.TotalAnnualRellief = payrollDetails.PensionFund + payrollDetails.NHIS + payrollDetails.Gratuities + payrollDetails.LifeAssurance + payrollDetails.ConsolidatedReliefsAllowance;
      payrollDetails.TaxableIncome = payrollDetails.GrossAnnualEmolument - payrollDetails.TotalAnnualRellief;
      payrollDetails.ComputedAnnualTax = this.ComputeTax(payrollDetails.TaxableIncome);
      payrollDetails.MinimumTax = (float)(payrollDetails.GrossAnnualEmolument * 0.01);
      payrollDetails.MonthlyMinimumTax = payrollDetails.MinimumTax / 12;

      if (payrollDetails.ComputedAnnualTax < payrollDetails.MinimumTax)
      {
        payrollDetails.AnnualPayableTax = payrollDetails.MinimumTax;
      }
      else
      {
        payrollDetails.AnnualPayableTax = payrollDetails.ComputedAnnualTax;
      }
      IList<float> intList = new List<float>() { payrollDetails.ComputedAnnualTax, payrollDetails.MinimumTax };

      payrollDetails.AnnualActualTaxPayable = intList.Max();

      payrollDetails.MonthlyActualTaxPayable = payrollDetails.AnnualActualTaxPayable / 12;

      payrollDetails.PensionDeductionMonthly = payrollDetails.PensionFund / 12;

      payrollDetails.StaffLoan = 0;

      payrollDetails.AbsentDeductions = 0;

      payrollDetails.CooperativeDeduction = 0;

      payrollDetails.OtherDeductions = 0;

      IList<float> intDeductionList = new List<float>() { payrollDetails.MonthlyActualTaxPayable, payrollDetails.PensionDeductionMonthly ,
                payrollDetails.StaffLoan, payrollDetails.AbsentDeductions,  payrollDetails.CooperativeDeduction, payrollDetails.OtherDeductions   };

      payrollDetails.TotalDeduction = intDeductionList.Sum();

      payrollDetails.Addition = 0;
      payrollDetails.PayrollMonth = Paymonth;

      payrollDetails.OrganisationId = employeeInformation.OrganisationId;

      payrollDetails.PayableToStaff = payrollDetails.GrossMonthlyEmolument - (payrollDetails.TotalDeduction + payrollDetails.Addition);

      _context.Add(payrollDetails);
      await _context.SaveChangesAsync();

    }


    private float ComputeTax(float TaxiableIncome)
    {
      float result1 = 0;

      if (TaxiableIncome <= 300000)
      {
        return result1 = (float)(0.07 * TaxiableIncome);

      }
      else if (TaxiableIncome > 300000 && TaxiableIncome <= 600000)
      {
        return result1 = (float)(21000 + (TaxiableIncome - 300000) * 0.11);
      }
      else if (TaxiableIncome > 600000 && TaxiableIncome <= 1100000)
      {
        return result1 = (float)(54000 + (TaxiableIncome - 600000) * 0.15);
      }

      else if (TaxiableIncome > 1100000 && TaxiableIncome <= 1600000)
      {
        return result1 = (float)(129000 + (TaxiableIncome - 1100000) * 0.19);

      }
      else if (TaxiableIncome > 1600000 && TaxiableIncome <= 3200000)
      {
        return result1 = (float)(224000 + (TaxiableIncome - 1600000) * 0.21);
      }
      else if (TaxiableIncome > 3200000)
      {
        return result1 = (float)(560000 + (TaxiableIncome - 3200000) * 0.24);

      }
      else
      {
        return result1;
      }
    }

  }
}