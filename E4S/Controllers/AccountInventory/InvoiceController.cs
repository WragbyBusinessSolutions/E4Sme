using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using E4S.Data;
using E4S.Models;
using E4S.Models.AccountInventory;
using E4S.ViewModel.AccountVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
      var orgId = getOrg();
      var allInvoices = _context.InvoiceRecords.Where(x => x.OrganisationId == orgId).ToList();
      var customer = _context.Customers.Where(x => x.OrganisationId == orgId).ToList();

      List<InvoiceListViewModel> listIVM = new List<InvoiceListViewModel>();
      InvoiceListViewModel iVM;

      foreach (var item in allInvoices)
      {
        iVM = new InvoiceListViewModel();
        iVM.InvoiceRecord = item;
        iVM.Customer = customer.Where(x => x.Id == item.CustomerId).FirstOrDefault();

        listIVM.Add(iVM);
      }

      return View(listIVM);
        }

        public IActionResult AddInvoice(Guid? id)
        {
      var orgId = getOrg();
      var inv = _context.InvoiceRecords.Where(x => x.OrganisationId == orgId).OrderByDescending(x => x.DateCreated).ToList();

      if (id == null)
      {
        InvoiceRecord ir = new InvoiceRecord()
        {
          Id = Guid.NewGuid(),
          OrganisationId = orgId
        };

        if (inv.Count > 0)
        {
          ir.InvoiceNo = inv.FirstOrDefault().InvoiceNo + 1;
        }
        else
        {
          ir.InvoiceNo = 1;
        }

        _context.Add(ir);
        _context.SaveChanges();

        id = ir.Id;

        return RedirectToAction("AddInvoice", new { id = ir.Id });

      }


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

    [Produces("application/json")]
    [HttpGet("search")]
    [Route("/api/Invoice/Product")]
    public async Task<IActionResult> SearchProduct()
    {
      var orgId = getOrg();

      try
      {
        string term = HttpContext.Request.Query["term"].ToString();
        var names = _context.StockRecords.Where(x => x.OrganisationId == orgId).Where(p => p.ProductService.ProductServiceName.Contains(term)).Select(p => p.ProductService.ProductServiceName).ToList();
        return Ok(names);
      }
      catch
      {
        return BadRequest();
      }

    }


    [HttpPost]
    public IActionResult GetProduct([FromBody]AutoCus tag)
    {
      var orgId = getOrg();

      var produc = _context.StockRecords.Where(x => x.OrganisationId == orgId).Where(x => x.ProductService.ProductServiceName == tag.Name).Include(x => x.ProductService).FirstOrDefault();

      return Json(produc);

    }



  }
}