using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using E4S.Data;
using E4S.Models;
using E4S.Models.HumanResource;
using E4S.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

      var orgPayrollId = _context.Payrolls.Where(z => z.OrganisationId == orgId).Select(x => x.GenerationId).Distinct().ToList();
      List<PayrollSummaryViewModel> psVM = new List<PayrollSummaryViewModel>();
      PayrollSummaryViewModel pSummary;

      foreach (var item in orgPayrollId)
      {
        pSummary = new PayrollSummaryViewModel();
        var payrolls = _context.Payrolls.Where(x => x.GenerationId == item).ToList();

        pSummary.Id = item;
        pSummary.MonthYear = payrolls.FirstOrDefault().DateCreated;
        pSummary.NumberOfEmployee = payrolls.Count();
        pSummary.TotalMonthlySalary = payrolls.Sum(x => x.MonthlyBasic);
        pSummary.TotalPayables = payrolls.Sum(x => x.PayableToStaff);

        psVM.Add(pSummary);

      }

          return View(psVM.OrderByDescending(x => x.MonthYear));
        }

    public IActionResult ViewPayroll(Guid id)
    {
      var orgId = getOrg();

      var payrolllist = _context.Payrolls.Where(x => x.GenerationId == id).ToList();

      return View(payrolllist);

    }
        public IActionResult SalaryAdditions()
        {
            var ordId = getOrg();
            ViewData["EmployeeDetails"] = new SelectList(_context.EmployeeDetails.Where(x => x.OrganisationId == ordId), "Id", "FirstName", "LastName");
            ViewData["Additions"] = new SelectList(_context.Additions.Where(x => x.OrganisationId == ordId), "Id", "AdditionType");
            var orgId = getOrg();

            var salaryAdditonslist = _context.SalaryAdditions.Where(x => x.OrganisationId == ordId).ToList();

            return View(salaryAdditonslist);
        }


        [HttpPost]
        public async Task<IActionResult> postSalaryAdditions([FromBody]PostNewSalaryAdditions postNewSalaryAdditions)
        {
            if (postNewSalaryAdditions == null)
            {
                return Json(new
                {
                    msg = "No Data"
                }
               );
            }

            var orgId = getOrg();           

            try
            {
                SalaryAddition orgSalaryAddition = new SalaryAddition()
                {
                    Id = Guid.NewGuid(),
                    EmployeeDetailId = postNewSalaryAdditions.EmployeeDetailsId,
                    AdditionId = postNewSalaryAdditions.AdditonId,
                    Amount = postNewSalaryAdditions.Amount,
                    Description = postNewSalaryAdditions.Description,
                    OrganisationId = orgId,

                };

                _context.Add(orgSalaryAddition);
                _context.SaveChanges();


                return Json(new
                {
                    msg = "Success"
                }
             );
            }
            catch (Exception ee)
            {

            }

            return Json(
            new
            {
                msg = "Fail"
            });
        }



        public IActionResult SalaryDeductions()
        {
            var ordId = getOrg();
            ViewData["EmployeeDetails"] = new SelectList(_context.EmployeeDetails.Where(x => x.OrganisationId == ordId), "Id", "FirstName", "LastName");
            ViewData["Deductions"] = new SelectList(_context.Deductions.Where(x => x.OrganisationId == ordId), "Id", "DeductionType");
            var orgId = getOrg();
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> postSalaryDeductions([FromBody]PostNewSalaryDeductions postNewSalaryDeductions)
        {
            if (postNewSalaryDeductions == null)
            {
                return Json(new
                {
                    msg = "No Data"
                }
               );
            }

            var orgId = getOrg();

            try
            {
                SalaryDeduction orgSalaryDeduction = new SalaryDeduction()
                {
                    Id = Guid.NewGuid(),
                    EmployeeDetailId = postNewSalaryDeductions.EmployeeDetailsId,
                    DeductionId = postNewSalaryDeductions.DeductionId,
                    //Amount = postNewSalaryDeductions.Amount,
                    Description = postNewSalaryDeductions.Description,
                    OrganisationId = orgId,

                };

                _context.Add(orgSalaryDeduction);
                _context.SaveChanges();


                return Json(new
                {
                    msg = "Success"
                }
             );
            }
            catch (Exception ee)
            {

            }

            return Json(
            new
            {
                msg = "Fail"
            });
        }




        public async Task<IActionResult> GeneratePayroll()
        {
          var orgId = getOrg();

          var AllPayrollUser = _context.Salaries.Where(x => x.OrganisationId == orgId).Include(x => x.EmployeeDetail).ToList();
          var GenId = Guid.NewGuid();

          foreach (var item in AllPayrollUser)
          {
            var result = this.ComputePaySlip(item, DateTime.Now.Month.ToString(), GenId);
          }

          var payrolllist = await _context.Payrolls.Where(x => x.GenerationId == GenId).ToListAsync();

          return View(payrolllist);
        }


    private async Task ComputePaySlip(Salary employeeInformation, string Paymonth, Guid genId)
    {
      Payroll payrollDetails = new Payroll();
      //var employeeSalaryDetails = await _context.Salaries.Where(x => x.EmployeeDetailId == employeeInformation.Id).FirstOrDefaultAsync();
      var salary = employeeInformation.Amount;

      payrollDetails.GenerationId = genId;
      payrollDetails.EmployeeName = employeeInformation.EmployeeDetail.FirstName + " " + employeeInformation.EmployeeDetail.LastName;
      payrollDetails.Basic = ((float)((salary * 12) * 0.5));
      payrollDetails.Housing = ((float)((salary * 12) * 0.3));
      payrollDetails.Transport = ((float)((salary * 12) * 0.12));
      payrollDetails.Meal = ((float)((salary * 12) * 0.08));
      payrollDetails.BankName = employeeInformation.BankName;
      payrollDetails.AccountNo = employeeInformation.AccountNo;
      payrollDetails.MonthlyBasic = salary;
      payrollDetails.AccountName = employeeInformation.AccountName;
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
      _context.SaveChanges();

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