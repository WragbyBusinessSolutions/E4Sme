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

namespace E4S.Controllers
{
  [Authorize]
  public class HomeController : Controller
  {
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender _emailSender;
    private readonly SignInManager<ApplicationUser> _signInManager;

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

      return orgId;
    }

    public async Task<IActionResult> Index()
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var user = await _signInManager.UserManager.FindByIdAsync(userId);
      var userRoles = await _signInManager.UserManager.GetRolesAsync(user);

      if (userRoles.Contains("Employee"))
      {
        return RedirectToAction("Index", "EmployeeProfiel");
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
      return View();
    }
  }
}
