using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using E4S.Data;
using E4S.Models;
using E4S.Models.HumanResource;
using E4S.Services;
using E4S.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E4S.Controllers.HumanResource
{
    [Authorize]
    public class LeavesController : Controller
    {
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender _emailSender;
    private EmployeeDetail employeeDetails;

        [TempData]
    public string StatusMessage { get; set; }

    public LeavesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
    {
      _context = context;
      _userManager = userManager;
      _emailSender = emailSender;


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

      var leaveList = _context.Leaves.Where(x => x.OrganisationId == orgId).Include(x => x.EmployeeDetail).OrderByDescending(x => x.DateCreated).ToList();

            ViewData["StatusMessage"] = StatusMessage;
            ViewData["StatusMessage"] = StatusMessage;
            return View(leaveList);
        }

    [HttpPost]
    public async Task<IActionResult> ApproveLeave([FromBody]PostApproveLeave postApprove)
    {
      if (postApprove == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      var orgId = getOrg();
      var organisationDetails = _context.Organisations.Where(x => x.Id == orgId).FirstOrDefault();


      try
      {
        var leave = _context.Leaves.Where(x => x.Id == postApprove.Id).Include(c => c.EmployeeDetail).FirstOrDefault();
        leave.Comment = postApprove.Comment;
        leave.ApproveDate = DateTime.Now;
        leave.Status = "Approved";

        _context.Update(leave);
        _context.SaveChanges();
       

         
         var response = _emailSender.SendGridLeaveApprovalAsync(leave.EmployeeDetail.Email, "Approved Leave Request", "/EmployeeProfile/Leave", leave.EmployeeDetail.FirstName, "approvalLeave", organisationDetails.OrganisationName, leave);

                


        StatusMessage = "Leave has been successfully Approved!.";

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

    [HttpPost]
    public async Task<IActionResult> DeclineLeave([FromBody]PostApproveLeave postApprove)
    {
      if (postApprove == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      var orgId = getOrg();
      var organisationDetails = _context.Organisations.Where(x => x.Id == orgId).FirstOrDefault();

      try
      {
        var leave = _context.Leaves.Where(x => x.Id == postApprove.Id).Include(y => y.EmployeeDetail).FirstOrDefault();
        leave.Comment = postApprove.Comment;
        leave.ApproveDate = DateTime.Now;
        leave.Status = "Declined";

        _context.Update(leave);
        _context.SaveChanges();

        var response = _emailSender.SendGridLeaveDeclinedAsync(leave.EmployeeDetail.Email, "Declined Leave Request", "/EmployeeProfile/Leave", leave.EmployeeDetail.FirstName, "declinedLeave", organisationDetails.OrganisationName, leave);


        StatusMessage = "Leave has been successfully Declined!.";

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



    }
}