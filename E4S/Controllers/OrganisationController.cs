using E4S.Data;
using E4S.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
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

      //var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      //var orgId = _context.Users.Where(x => x.Id == userId).FirstOrDefault().OrganisationId;
      //var org = _context.Organisations.Where(x => x.Id == orgId).FirstOrDefault();


    }

    public void orgDetails()
    {
      var orgdetails = _context.Organisations.Where(x => x.Id == getOrg()).FirstOrDefault();
      ViewData["OrganisationName"] = orgdetails.OrganisationName;
      ViewData["OrganisationImage"] = orgdetails.ImageUrl;
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

    public async Task<IActionResult> UploadProfile(IFormFile file)
    {
      if (file == null || file.Length == 0)
        return Content("file not selected");

      //var path = Path.Combine(
      //            Directory.GetCurrentDirectory(), "wwwroot",
      //            file.FileName);
      var filename = Guid.NewGuid().ToString() + file.FileName;
      var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "employeeImage", filename);
      var path2 = Path.Combine("images", "employeeImage", filename);

      var orgId = getOrg();
      var org = _context.Organisations.Where(x => x.OrganisationId == orgId).FirstOrDefault();

      
      //var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      //var employeeDetails = _context.EmployeeDetails.Where(x => x.UserId == Guid.Parse(userId)).FirstOrDefault();
      

      using (var stream = new FileStream(path, FileMode.Create))
      {
        await file.CopyToAsync(stream);
        org.ImageUrl = path2;

        _context.Update(org);
        _context.SaveChanges();

      }

      return RedirectToAction("Index");
    }

    public IActionResult Index()
    {
      orgDetails();
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
      var orgId = getOrg();

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
