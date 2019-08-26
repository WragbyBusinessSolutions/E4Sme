using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using E4S.Data;
using E4S.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E4S.Controllers.WragbyAdmin
{
  [Authorize]
  public class SupportController : Controller
    {

    [TempData]
    public string StatusMessage { get; set; }

    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public SupportController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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

    //Damilare
//================= User Method Starts Here =========================//
    public IActionResult Index()
        {
            return View();
        }

    public IActionResult NewTicket()
    {
      return View();
    }
    [HttpPost]
    public IActionResult CreateTicket()
    {
      return View();
    }

    public IActionResult ViewTicket()
    {
      return View();
    }
    [HttpPost]
    public IActionResult ReplyTicket()
    {
      return View();
    }

    //================= User Method Ends Here =========================//


      //Temofe
    //================= Admin Method Starts Here =======================//

    public IActionResult AllTickets()
    {
      return View();

    }

    public IActionResult AdminViewTicket()
    {
      return View();
    }
    [HttpPost]
    public IActionResult AdminReplyTicket()
    {
      return View();
    }
    [HttpPost]
    public IActionResult AdminCloseTicket()
    {
      return View();
    }
    public IActionResult AdminDeleteTicket()
    {
      return View();
    }

    //================= Admin Method Ends Here =======================//

  }
}