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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E4S.Controllers.AccountInventory
{
  [Authorize]
  public class ProductController : Controller
    {

    [TempData]
    public string StatusMessage { get; set; }

    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProductController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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
    [Route("/api/Vendor/search")]
    public async Task<IActionResult> Search()
    {
      var orgId = getOrg();

      try
      {
        string term = HttpContext.Request.Query["term"].ToString();
        var names = _context.Vendors.Where(x => x.OrganisationId == orgId).Where(p => p.VendorName.Contains(term)).Select(p => p.VendorName).ToList();
        return Ok(names);
      }
      catch
      {
        return BadRequest();
      }

    }

    // GET: /<controller>/
    public IActionResult Index()
        {
      var orgId = getOrg();

      var products = _context.StockRecords.Where(x => x.OrganisationId == orgId).Include(x => x.ProductService).ToList();

      ViewData["Vendors"] = new SelectList(_context.Vendors.Where(x => x.OrganisationId == orgId), "Id", "VendorName");
      return View(products);
        }

    [HttpPost]
    public async Task<IActionResult> AddNewProduct([FromBody]ProductService product)
    {
      var orgId = getOrg();

      if (product != null)
      {
        product.Id = Guid.NewGuid();
        product.OrganisationId = orgId;


        try
        {
          _context.Add(product);
          await _context.SaveChangesAsync();

          StockRecord newStock = new StockRecord()
          {
            Id = Guid.NewGuid(),
            ProductServiceId = product.Id,
            AllTimeQuantity = 0,
            QuantitySold = 0,
            QuantityRemain = 0,
            OrganisationId = orgId
          };

          _context.Add(newStock);
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
