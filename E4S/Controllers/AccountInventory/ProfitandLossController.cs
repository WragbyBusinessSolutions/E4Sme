using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using E4S.Data;
using E4S.Models;
using E4S.ViewModel.AccountVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E4S.Controllers.AccountInventory
{
    public class ProfitandLossController : Controller
    {
    [TempData]
    public string StatusMessage { get; set; }

    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfitandLossController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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

    public IActionResult Index()
        {
      var orgId = getOrg();

      ProfitLossViewModel plVM = new ProfitLossViewModel();

      plVM.TotalIncome = _context.Transactions
        .Where(x => x.OrganisationId == orgId)
        .Where(x => x.DateCreated.Month == DateTime.Now.Month && x.DateCreated.Year == DateTime.Now.Year)
        .Where(x => x.TransactionType == "IN")
        .Sum(x => x.Amount) * 100/105;

      plVM.CostOfGoods = _context.CostOfGoods
        .Where(x => x.OrganisationId == orgId)
        .Where(x => x.DateCreated.Month == DateTime.Now.Month && x.DateCreated.Year == DateTime.Now.Year)
        .Sum(x => x.TotalCost);

      plVM.OperatingExpenses = _context.Transactions
        .Where(x => x.OrganisationId == orgId)
        .Where(x => x.DateCreated.Month == DateTime.Now.Month && x.DateCreated.Year == DateTime.Now.Year)
        .Where(x => x.TransactionType == "OE")
        .Sum(x => x.Amount);

      plVM.NetProfit = plVM.TotalIncome - plVM.CostOfGoods - plVM.OperatingExpenses;
      plVM.GrossProfit = plVM.TotalIncome - plVM.CostOfGoods;

      return View(plVM);
        }
    }
}
