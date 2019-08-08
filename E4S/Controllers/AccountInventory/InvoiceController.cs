﻿using System;
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

      ViewData["StatusMessage"] = StatusMessage;
      return View(listIVM.OrderByDescending(x => x.InvoiceRecord.DateCreated));
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

      var cInvoiceRecord = inv.Where(x => x.Id == id).FirstOrDefault();

      InvoiceViewModel iVM = new InvoiceViewModel();

      iVM.Id = cInvoiceRecord.Id;
      iVM.InvoiceNo = cInvoiceRecord.InvoiceNo;
      iVM.InoviceTitle = cInvoiceRecord.InvoiceTitle;
      iVM.Customer = _context.Customers.Where(x => x.Id == cInvoiceRecord.CustomerId).FirstOrDefault();
      iVM.Organisation = _context.Organisations.Where(x => x.Id == orgId).FirstOrDefault();
      iVM.Tax = cInvoiceRecord.Tax;
      iVM.SubTotal = cInvoiceRecord.SubTotal;
      iVM.Total = cInvoiceRecord.Total;
      iVM.InvoiceItems = _context.InvoiceItems.Where(x => x.InvoiceRecordId == id).Include(x => x.ProductService).ToList();
      iVM.DateCreated = cInvoiceRecord.DateCreated;



      return View(iVM);
        }

    public IActionResult InvoiceDone(Guid id)
    {
      var orgId = getOrg();
      var inv = _context.InvoiceRecords.Where(x => x.Id == id).FirstOrDefault();

      var debit = _context.Transactions.Where(x => x.TransactionId == inv.Id).FirstOrDefault();

      if (debit != null)
      {
        debit.Amount = inv.Total;

        _context.Update(debit);
        _context.SaveChanges();

      }
      else
      {
        Transaction tDebit = new Transaction()
        {
          Id = Guid.NewGuid(),
          TransactionType = "IN",
          TransactionId = inv.Id,
          DebitCredit = "D",
          TransactionDetails = "Invoice - No" + inv.InvoiceNo, // + " " + operatingExpense.Description,
          Amount = inv.Total,
          OrganisationId = orgId
        };

        _context.Add(tDebit);
        _context.SaveChanges();

      }

      StatusMessage = "Invoice saved and processed.";
      return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody]InvoiceItem invoiceItem)
    {
      var orgId = getOrg();

      if (invoiceItem != null)
      {

        invoiceItem.Id = Guid.NewGuid();
        invoiceItem.OrganisationId = orgId;

        invoiceItem.TotalCost = invoiceItem.UnitCost * invoiceItem.Quantity;


        try
        {
          _context.Add(invoiceItem);
          _context.SaveChanges();

          var items = _context.InvoiceItems.Where(x => x.InvoiceRecordId == invoiceItem.InvoiceRecordId).ToList();
          var qr = _context.InvoiceRecords.Where(x => x.Id == invoiceItem.InvoiceRecordId).FirstOrDefault();

          qr.SubTotal = items.Sum(x => x.TotalCost);
          qr.Tax = qr.SubTotal * 0.05f;
          qr.Total = qr.SubTotal + qr.Tax;

          _context.Update(qr);
          _context.SaveChanges();

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

    [HttpPost]
    public IActionResult GetCustomer([FromBody]AutoCus tag)
    {
      var orgId = getOrg();

      var customer = _context.Customers.Where(x => x.OrganisationId == orgId).Where(x => x.CustomerName == tag.Name).FirstOrDefault();
      if (customer != null)
      {
        var quoterec = _context.InvoiceRecords.Where(x => x.Id == tag.QuoteId).FirstOrDefault();

        quoterec.CustomerId = customer.Id;

        //quoterec.QuoteNo = 0;

        _context.Update(quoterec);
        _context.SaveChanges();
      }

      return Json(customer);

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