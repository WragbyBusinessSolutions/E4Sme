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
    public class VendorsController : Controller
    {
    [TempData]
    public string StatusMessage { get; set; }

    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public VendorsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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
      return View(_context.Vendors.Where(x => x.OrganisationId == orgId).ToList());
    }

    public IActionResult AddVendor()
    { 
      var orgId = getOrg();

      return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddVendor(Vendor vendor)
    {
      var orgId = getOrg();

      if (ModelState.IsValid)
      {
        vendor.Id = Guid.NewGuid();
        vendor.OrganisationId = orgId;


        _context.Add(vendor);
        await _context.SaveChangesAsync();

        StatusMessage = "New Vendor successfully created.";
        return RedirectToAction(nameof(Index));
      }

      StatusMessage = "Error! Check fields...";
      ViewData["StatusMessage"] = StatusMessage;

      return View(vendor);
    }
    public async Task<IActionResult> EditVendor(Guid id)
    {
      var orgId = getOrg();

      if (id == null)
      {
        return NotFound();
      }

      var vendor = await _context.Vendors.FindAsync(id);
      if (vendor == null)
      {
        return NotFound();
      }
      return View(vendor);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditVendor(Vendor vendor)
    {
      var orgId = getOrg();

      if (vendor.Id == null)
      {
        return NotFound();
      }
      vendor.OrganisationId = orgId;

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(vendor);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!CustomerExists(vendor.Id))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        StatusMessage = "Vendor record updated.";

        return RedirectToAction(nameof(Index));
      }
      return View(vendor);
    }


    private bool CustomerExists(Guid id)
    {
      return _context.Vendors.Any(e => e.Id == id);
    }



  }
}