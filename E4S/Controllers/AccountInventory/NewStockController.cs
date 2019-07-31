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

namespace E4S.Controllers.AccountInventory
{
  [Authorize]
  public class NewStockController : Controller
    {
    [TempData]
    public string StatusMessage { get; set; }

    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public NewStockController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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

      var newStocks = _context.NewInventories.Where(x => x.OrganisationId == orgId).Include(x => x.ProductService).OrderByDescending(x => x.DateCreated).ToList();
      ViewData["Products"] = new SelectList(_context.ProductServices.Where(x => x.OrganisationId == orgId), "Id", "ProductServiceName");

      return View(newStocks);
    }

    [HttpPost]
    public async Task<IActionResult> AddNewStock([FromBody]NewInventory newInventory)
    {
      var orgId = getOrg();

      if (newInventory != null)
      {
        var stockRecord = _context.StockRecords.Where(x => x.ProductServiceId == newInventory.ProductServiceId).FirstOrDefault();

        newInventory.Id = Guid.NewGuid();
        newInventory.OrganisationId = orgId;


        try
        {
          _context.Add(newInventory);
          await _context.SaveChangesAsync();

          float remainQuantity = stockRecord.QuantityRemain;
          float unitPrice = stockRecord.CostPrice;
          float totalprice = remainQuantity * unitPrice;

          stockRecord.QuantityRemain = remainQuantity + newInventory.Quantity;
          stockRecord.AllTimeQuantity = stockRecord.AllTimeQuantity + newInventory.Quantity;
          stockRecord.CostPrice = (totalprice + (newInventory.TotalPrice)) / stockRecord.QuantityRemain;

          _context.Update(stockRecord);
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