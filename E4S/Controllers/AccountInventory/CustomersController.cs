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
  public class CustomersController : Controller
    {
    [TempData]
    public string StatusMessage { get; set; }

    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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


      ViewData["StatusMessage"] = StatusMessage;
      return View(_context.Customers.Where(x => x.OrganisationId == orgId).ToList());
        }

        public IActionResult AddCustomer()
        {
      var orgId = getOrg();

      return View();
        }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddCustomer(Customer customer)
    {
      var orgId = getOrg();

      if (ModelState.IsValid)
      {
        customer.Id = Guid.NewGuid();
        customer.OrganisationId = orgId;
        

        _context.Add(customer);
        await _context.SaveChangesAsync();

        StatusMessage = "New Customer successfully created.";
        return RedirectToAction(nameof(Index));
      }

      StatusMessage = "Error! Check fields...";
      ViewData["StatusMessage"] = StatusMessage;

      return View(customer);
    }

    public async Task<IActionResult> EditCustomer(Guid id)
    {
      var orgId = getOrg();

      if (id == null)
      {
        return NotFound();
      }

      var customer = await _context.Customers.FindAsync(id);
      if (customer == null)
      {
        return NotFound();
      }
      return View(customer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCustomer(Customer customer)
    {
      if (customer.Id == null)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(customer);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!CustomerExists(customer.Id))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        StatusMessage = "Customer record updated.";

        return RedirectToAction(nameof(Index));
      }
      return View(customer);
    }


    private bool CustomerExists(Guid id)
    {
      return _context.Customers.Any(e => e.Id == id);
    }

        // Ednf of Edit for Job Title



        [HttpPost]
        public IActionResult DeleteCustomer([FromBody]string customerId)
        {
            if (customerId == null)
            {
                return Json(new
                {
                    msg = "No Data"
                }
               );
            }

            try
            {
                var customr = _context.Customers.SingleOrDefault(m => m.Id == Guid.Parse(customerId));
                _context.Customers.Remove(customr);
                _context.SaveChanges();

                return Json(new
                {
                    msg = "Success"
                });

            }
            catch
            {

            }

            return Json(new
            {
                msg = "Fail"
            });


        }

    [AllowAnonymous]
    public IActionResult ViewInvoice(Guid? id)
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

        return RedirectToAction("ViewInvoice", new { id = ir.Id });

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


  }
}