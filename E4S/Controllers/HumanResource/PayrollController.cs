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

namespace E4S.Controllers.HumanResource
{
    [Authorize]
    public class PayrollController : Controller
    {

    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public PayrollController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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

        [Produces("application/json")]
        [HttpGet("search")]
        [Route("/api/Payroll/search")]
        public async Task<IActionResult> Search()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var orgId = _context.Users.Where(x => x.Id == userId).FirstOrDefault().OrganisationId;


            var otheruser =  _context.Organisations.Where(x => x.Id == orgId).FirstOrDefault();

            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var names = _context.EmployeeDetails.Where(x => x.OrganisationId == otheruser.Id).Where(p => p.FirstName.Contains(term)).Select(p => p.FirstName).ToList();
                return Ok(names);
            }
            catch
            {
                return BadRequest();
            }
        }

        public IActionResult Index()
        {
          var orgId = getOrg();

          return View();
        }
        public IActionResult SalaryAdditions()
        {
          var orgId = getOrg();

          return View();
        }
        public IActionResult SalaryDeductions()
        {
          var orgId = getOrg();

          return View();
        }

    }
}