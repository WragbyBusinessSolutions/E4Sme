using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using E4S.Data;
using E4S.Models;
using E4S.Services;
using E4S.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E4S.Controllers
{
  [Authorize]
  public class UsersController : Controller
    {
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender _emailSender;


    public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
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

      var userList = _context.Users.Where(x => x.OrganisationId == orgId).ToList();

      return View(userList);
    }

    public IActionResult AddUser()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddUser(UsersViewModel newuser)
    {
      var orgId = getOrg();

      var user = new ApplicationUser
      {
        Id = Guid.NewGuid().ToString(),
        UserName = newuser.Email,
        Email = newuser.Email,
        OrganisationId = orgId,
        PhoneNumber = newuser.PhoneNumber,
        UserRole = newuser.UserRole,
        EmployeeName = newuser.Username
        
      };

      var result = await _userManager.CreateAsync(user);
      if (result.Succeeded)
      {
        await _userManager.AddToRoleAsync(user, user.UserRole);

        var Email = user.Email;
        string Code = await _userManager.GeneratePasswordResetTokenAsync(user);
        var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, Code, Request.Scheme);

        var response = _emailSender.SendPlainEmailAsync(user.Email, "Reset Password",
           $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");

        //if(response == "Success")
        //{
          return RedirectToAction("Index");
      //  }
      }


      return View(newuser);
    }

  }
}