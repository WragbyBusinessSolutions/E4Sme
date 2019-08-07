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

namespace E4S.Controllers.AccountInventory
{
  [Authorize]

  public class InvoiceController : Controller
    {
    [TempData]
    public string StatusMessage { get; set; }

    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public InvoiceController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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
            return View();
        }

        public IActionResult AddInvoice()
        {
      var orgId = getOrg();

      return View();
        }

    [Produces("application/json")]
    [HttpGet("search")]
    [Route("/api/Invoice/Search")]
    public async Task<IActionResult> Search()
    {
      var orgId = getOrg();

      try
      {
        string term = HttpContext.Request.Query["term"].ToString();
        var names = _context.ProductServices.Where(x => x.OrganisationId == orgId).Where(p => p.ProductServiceName.Contains(term)).Select(p => p.ProductServiceName).ToList();
        return Ok(names);
      }
      catch
      {
        return BadRequest();
      }

    }

    [Produces("application/json")]
    [HttpGet("search")]
    [Route("/api/Invoice/Customers")]
    public async Task<IActionResult> SearchCustomers()
    {
      var orgId = getOrg();

      try
      {
        string term = HttpContext.Request.Query["term"].ToString();
        var names = _context.Customers.Where(x => x.OrganisationId == orgId).Where(p => p.CustomerName.Contains(term)).Select(p => p.CustomerName).ToList();
        return Ok(names);
      }
      catch
      {
        return BadRequest();
      }

    }

  }
}