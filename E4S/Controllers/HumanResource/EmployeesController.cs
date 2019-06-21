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

namespace E4S.Controllers.HumanResource
{
  [Authorize]
    public class EmployeesController : Controller
    {
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender _emailSender;

    public EmployeesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
    {
      _context = context;
      _userManager = userManager;
      _emailSender = emailSender;

    }

    private Guid getOrg()
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var orgId = _context.Users.Where(x => x.Id == userId).FirstOrDefault().OrganisationId;

      return orgId;
    }

    public IActionResult Index()
        {
      var orgId = getOrg();

      var employeeListDb = _context.EmployeeDetails.Where(x => x.OrganisationId == orgId).ToList();
      List<EmployeeListViewModel> employeeList = new List<EmployeeListViewModel>();

      EmployeeListViewModel singleEmployee;

      foreach (var item in employeeListDb)
      {
        singleEmployee = new EmployeeListViewModel()
        {
          Id = item.Id,
          EmployeeName = item.FirstName + " " + item.LastName,
          Department = "",
          EmployeeStatus = "",
          JobTitle = "",
          Supervisor = ""
        };

        employeeList.Add(singleEmployee);
      }



      return View(employeeList);
        }
    
        public IActionResult AddEmployee()
        {
            return View();
        }

        public IActionResult EmployeeDetails(Guid id)
        {
      var singleEmployee = _context.EmployeeDetails.Where(x => x.Id == id).FirstOrDefault();
          return View(singleEmployee);
        }
    [HttpGet]
    public async Task<IActionResult> EmployeeInfo (Guid id)
    {
      if (id == null)
      {
        return NotFound();
      }
      return View();
    }

        [HttpPost]
        public async Task<IActionResult> postNewEmployee([FromBody]PostNewEmployee postNewEmployee)
        {
      if (postNewEmployee == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      var orgId = getOrg();
      var organisationDetails = _context.Organisations.Where(x => x.Id == orgId).FirstOrDefault();
      int noOfEmployee = _context.Users.Where(x => x.OrganisationId == orgId).Count();

      try
      {
        var userId = Guid.NewGuid();
        EmployeeDetail newEmployee = new EmployeeDetail()
        {
          Id = Guid.NewGuid(),
          FirstName = postNewEmployee.FirstName,
          LastName = postNewEmployee.LastName,
          Email = postNewEmployee.PersonalEmail,
          PhoneNumber = postNewEmployee.PhoneNumber,
          EmployeeId = organisationDetails.OrganisationPrefix + (noOfEmployee + 1).ToString(),
          OrganisationId = orgId,
          UserId = userId
        };

        _context.Add(newEmployee);
        _context.SaveChanges();

        var user = new ApplicationUser
        {
          Id = userId.ToString(),
          UserName = newEmployee.Email,
          Email = newEmployee.Email,
          OrganisationId = orgId,
          PhoneNumber = newEmployee.PhoneNumber,
          UserRole = "Employee",
          EmployeeName = newEmployee.LastName + " " + newEmployee.FirstName,
        };

        var result = await _userManager.CreateAsync(user);
        if (result.Succeeded)
        {
          await _userManager.AddToRoleAsync(user, user.UserRole);

          var Email = user.Email;
          string Code = await _userManager.GeneratePasswordResetTokenAsync(user);
          var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, Code, Request.Scheme);

          var response = _emailSender.SendGridEmailAsync(user.Email, "Reset Password",
             $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");

          //var response = _emailSender.GmailSendEmail(user.Email, callbackUrl, user.UserRole);


          return Json(new
          {
            msg = "Success"
          }
         );


        }
      }
      catch(Exception ee)
      {

      }


      return Json(new
      {
        msg = "Fail"
      }
     );

    }
  }
}