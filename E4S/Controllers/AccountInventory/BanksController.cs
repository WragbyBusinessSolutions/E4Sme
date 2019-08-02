using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using E4S.Data;
using E4S.Models;
using E4S.Models.AccountInventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E4S.Controllers.AccountInventory
{
  [Authorize]
  public class BanksController : Controller
    {
    [TempData]
    public string StatusMessage { get; set; }

    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public BanksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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

      var accountBank = _context.AccountBanks.Where(x => x.OrganisationId == orgId).OrderByDescending(x => x.DateCreated).ToList();
      return View(accountBank);
        }

    [HttpPost]
    public async Task<IActionResult> AddAccount([FromBody]AccountBank accountBank)
    {
      var orgId = getOrg();

      if (accountBank != null)
      {
        accountBank.Id = Guid.NewGuid();
        accountBank.OrganisationId = orgId;


        try
        {
          _context.Add(accountBank);
          await _context.SaveChangesAsync();

          return Json(new
          {
            msg = "Success"
          });

        }
        catch
        {
          return Json(new
          {
            msg = "Failed"
          });

        }


        //StatusMessage = "New Vendor successfully created.";
      }

      //StatusMessage = "Error! Check fields...";
      //ViewData["StatusMessage"] = StatusMessage;
      return Json(new
      {
        msg = "No Data"
      });

    }



  }
}