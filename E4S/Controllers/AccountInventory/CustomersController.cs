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

      return View();
    }


  }
}