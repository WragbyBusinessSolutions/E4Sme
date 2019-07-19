using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using E4S.Data;
using E4S.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E4S.Controllers.HumanResource
{
    public class AppraisalController : Controller
    {
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AppraisalController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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

      return View();
        }

        public IActionResult CatgoryList()
        {
      var orgId = getOrg();

      return View();
        }
    public IActionResult AddCategory()
    {
      var orgId = getOrg();


      return View();

    }


    public IActionResult EditCategory()
    {
      var orgId = getOrg();


      return View();

    }
    public IActionResult VeiwCatgory()
        {
      var orgId = getOrg();

      return View();
        }
        public IActionResult Template()
        {
      var orgId = getOrg();

      return View();
        }

    public IActionResult AddTemplate()
    {
      var orgId = getOrg();

      return View();
    }


    public IActionResult ViewTemplate()
        {
      var orgId = getOrg();

      return View();
        }
    }
}