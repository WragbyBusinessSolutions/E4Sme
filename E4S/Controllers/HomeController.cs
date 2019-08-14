using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using E4S.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using E4S.Data;
using Microsoft.AspNetCore.Identity;
using E4S.Services;
using E4S.ViewModel;
using E4S.ViewModel.AccountVM;
using Microsoft.EntityFrameworkCore;

namespace E4S.Controllers
{
  [Authorize]
  public class HomeController : Controller
  {
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender _emailSender;
    private readonly SignInManager<ApplicationUser> _signInManager;

    [TempData]
    public string StatusMessage { get; set; }

    public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailSender emailSender, SignInManager<ApplicationUser> signInManager)
    {
      _context = context;
      _userManager = userManager;
      _emailSender = emailSender;
      _signInManager = signInManager;
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

    public void orgDetails()
    {
      var orgdetails = _context.Organisations.Where(x => x.Id == getOrg()).FirstOrDefault();
      ViewData["OrganisationName"] = orgdetails.OrganisationName;
      ViewData["OrganisationImage"] = orgdetails.ImageUrl;
    }

    public async Task<IActionResult> Index()
    {
      orgDetails();

      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var user = await _signInManager.UserManager.FindByIdAsync(userId);
      var userRoles = await _signInManager.UserManager.GetRolesAsync(user);
      ViewData["StatusMessage"] = StatusMessage;

      if (userRoles.Contains("Employee"))
      {
        return RedirectToAction("Index", "EmployeeProfile");
      }

      return View();
    }

    public IActionResult About()
    {
      ViewData["Message"] = "Your application description page.";

      return View();
    }

    public IActionResult Contact()
    {
      ViewData["Message"] = "Your contact page.";

      return View();
    }

    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult DashboardHR()
    {
      HRDashboardViewModel HRDVM = new HRDashboardViewModel();
      List<LatestEmployeeVM> lastestEmployeeVM = new List<LatestEmployeeVM>();
      List<LeaveDashVM> leaveDashVM = new List<LeaveDashVM>();
      List<RecentApplication> recentAppVM = new List<RecentApplication>();

      LatestEmployeeVM latestEmployee;
      LeaveDashVM leaveDash;
      RecentApplication recentApp;

      var orgId = getOrg();
      var empList = _context.Users.Where(x => x.OrganisationId == orgId).ToList();
      var empDetails = _context.EmployeeDetails.Where(x => x.OrganisationId == orgId).ToList();
      var jobs = _context.Jobs.Where(x => x.OrganisationId == orgId).ToList();

        foreach (var item in empList)
        {
          var empdet = empDetails.Where(x => x.UserId == Guid.Parse(item.Id)).FirstOrDefault();
          if (empdet != null)
          {
          //try
          //{
            latestEmployee = new LatestEmployeeVM()
            {
              Id = empdet.Id,
              EmployeeName = empdet.FirstName + " " + empdet.LastName,
              //Department = jobs.Where(x => x.EmployeeDetailId == empdet.Id).FirstOrDefault().Department.DepartmentName,
              //JobTitle = jobs.Where(x => x.EmployeeDetailId == empdet.Id).FirstOrDefault().JobTitle.JobTitleName,
              //IsActive = item.Status
            };

            lastestEmployeeVM.Add(latestEmployee);

          //}
          //catch
          //{

          //}

          }
        }

      List<HeadCount> headCounts = new List<HeadCount>();
      var department = _context.Departments.Where(x => x.OrganisationId == orgId).ToList();

      var allJobs = _context.Jobs.Where(x => x.OrganisationId == orgId);
      HeadCount hC;
      foreach (var item in department)
      {
        hC = new HeadCount();

        hC.Department = item.DepartmentName;
        hC.Female = allJobs.Where(x => x.DepartmentId == item.Id).Where(x => x.EmployeeDetail.Gender == "Female").ToList().Count();
        hC.Male = allJobs.Where(x => x.DepartmentId == item.Id).Where(x => x.EmployeeDetail.Gender == "Male").ToList().Count();

        headCounts.Add(hC);
      }

      var leaves = _context.Leaves.Where(x => x.OrganisationId == orgId);

      HRDVM.ActiveLeaves = leaves.Where(c => c.Status == "Pending").ToList();
      HRDVM.PendingLeave = leaves.Where(c => c.Status == "Approved").Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now).ToList().Count();



      HRDVM.LatestEmployeeVMs = lastestEmployeeVM;
      HRDVM.TotalEmployee = empDetails.Count();
      HRDVM.HeadCounts = headCounts;

      return View(HRDVM);
    }

    public IActionResult DashboardAC()
    {
      var orgId = getOrg();
      var allyearexpense = _context.OperatingExpenses.Where(x => x.OrganisationId == orgId);
      var allyearsales = _context.Transactions.Where(x => x.OrganisationId == orgId);
      var allyearCostOfGoods = _context.CostOfGoods.Where(x => x.OrganisationId == orgId);

      List<ExpenseBD> expenseList = new List<ExpenseBD>();
      List<ProfitLossBD> prolossList = new List<ProfitLossBD>();

      DashboardACViewModel dbAcVM = new DashboardACViewModel();

      dbAcVM.Customers = _context.Customers.Where(x => x.OrganisationId == orgId).Count();
      dbAcVM.Sales = allyearsales
                        .Where(x => x.TransactionType == "IN")
                        .Where(x => x.DateCreated.Year == DateTime.Now.Year)
                        .Where(x => x.DateCreated.Month == DateTime.Now.Month).Sum(x => x.Amount) * 100 / 105;

      dbAcVM.Expenses = allyearsales
                        .Where(x => x.DebitCredit == "C")
                        .Where(x => x.DateCreated.Year == DateTime.Now.Year)
                        .Where(x => x.DateCreated.Month == DateTime.Now.Month).Sum(x => x.Amount);



      var allexpense = allyearexpense
                        .Where(x => x.DateCreated.Year == DateTime.Now.Year)
                        .Where(x => x.DateCreated.Month == DateTime.Now.Month).Include(x => x.Expense).ToList();

      ExpenseBD exp;
      foreach (var item in allexpense.Select(x => x.Expense.ExpenseType).Distinct())
      {
        exp = new ExpenseBD();
        exp.ExpenseName = item;
        exp.Amount = allexpense.Where(x => x.Expense.ExpenseType == item).Sum(x => x.Amount);

        expenseList.Add(exp);
      }

      ProfitLossBD pl;
      for (int i = 0; i < 6; i++)
      {
        pl = new ProfitLossBD();
        pl.Date = DateTime.Now.AddMonths(-i).ToString("MMM yy");
        pl.Income = allyearsales.Where(x => x.DebitCredit == "D")
                      .Where(x => x.DateCreated.Month == DateTime.Now.AddMonths(-i).Month && x.DateCreated.Year == DateTime.Now.AddMonths(-i).Year)
                      .Sum(x => x.Amount) * 100/105;
        pl.Expenses = allyearexpense
                        .Where(x => x.DateCreated.Month == DateTime.Now.AddMonths(-i).Month && x.DateCreated.Year == DateTime.Now.AddMonths(-i).Year)
                        .Sum(x => x.Amount) + 
                      
                        allyearCostOfGoods
                        .Where(x => x.DateCreated.Month == DateTime.Now.AddMonths(-i).Month && x.DateCreated.Year == DateTime.Now.AddMonths(-i).Year)
                        .Sum(x => x.TotalCost);

        prolossList.Add(pl);

      }

      dbAcVM.ProfitLossChart = prolossList;
      dbAcVM.ExpenseChart = expenseList;

      return View(dbAcVM);
    }

    
  }
}
