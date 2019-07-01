﻿using System;
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


      HRDVM.LatestEmployeeVMs = lastestEmployeeVM;


      return View(HRDVM);
    }
  }
}
