using E4S.Data;
using E4S.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E4S.Controllers
{
  [Authorize]
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
      var orgId = getOrg();

      var org = _context.Organisations.Where(x => x.Id == orgId).FirstOrDefault();

      return View(org);
    }

    public IActionResult Branch()
    {
      var orgId = getOrg();

      var branchList = _context.Branches.Where(x => x.OrganisationId == orgId).ToList();

      return View(branchList);
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

      if (organisation != null)
      {

        _context.Update(organisation);
        _context.SaveChanges();

        return RedirectToAction("Index");
      }

      return View();
    }
    public IActionResult createbranch()
    {
        return View();
    }

    [HttpPost]
    public IActionResult createbranch(Branch branch)
    {
      var orgId = getOrg();

      if (branch == null)
      {
        return View();
      }

      branch.OrganisationId = orgId;
      branch.Id = Guid.NewGuid();

      _context.Add(branch);
      _context.SaveChanges();

      return RedirectToAction("Branch");
    }

  }
}
