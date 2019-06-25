using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using E4S.Data;
using E4S.Models;
using E4S.Models.HumanResource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace E4S.Controllers.HumanResource
{
  [Authorize]
    public class RecruitmentsController : Controller
    {

    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public RecruitmentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
      _context = context;
      _userManager = userManager;

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

      return View(_context.Vacancies.Where(x => x.OrganisationId == orgId).Include(x => x.JobTitle).ToList());
    }
    public IActionResult Candidates()
    {
      return View();
    }

    public IActionResult AddVacancy()
    {
      var orgId = getOrg();

      ViewData["JobTitleId"] = new SelectList(_context.JobTitles.Where(x => x.OrganisationId == orgId), "Id", "JobTitleName");// SelectList. _context.JobTitles.Where(x => x.OrganisationId == orgId).ToList()
      ViewData["HiringManager"] = new SelectList(_context.EmployeeDetails.Where(x => x.OrganisationId == orgId), "LastName", "LastName");// SelectList. _context.JobTitles.Where(x => x.OrganisationId == orgId).ToList()


      return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddVacancy(Vacancy vacancy)
    {
      if (vacancy != null)
      {
        var orgId = getOrg();

        vacancy.Id = Guid.NewGuid();
        vacancy.OrganisationId = orgId;
        try
        {
          _context.Add(vacancy);
          await _context.SaveChangesAsync();

          return RedirectToAction("Index");

        }
        catch
        {

        }


      }

      return View();

    }
  }
}