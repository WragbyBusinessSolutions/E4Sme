using E4S.Data;
using E4S.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E4S.Controllers
{
  public class OrganisationController : Controller
  {
    private readonly ApplicationDbContext _context;

    public OrganisationController(ApplicationDbContext context)
    {
      _context = context;

    }
    private Guid getOrg()
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var orgId = _context.Users.Where(x => x.Id == userId).FirstOrDefault().OrganisationId;

      return orgId;
    }

    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Branch()
    {
      return View();
    }

    public IActionResult Edit()
    {
      var orgId = getOrg();

      var org = _context.Organisations.Where(x => x.Id == orgId).FirstOrDefault();

      if (org != null)
      {
        return View(org);
      }

       return View();
    }


    [HttpPost]
    public IActionResult Edit(Organisation organisation)
    {

      return View();
    }
    public IActionResult createbranch()
    {
        return View();
    }
    }
}
