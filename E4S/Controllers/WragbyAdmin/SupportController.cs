using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using E4S.Data;
using E4S.Models;
using E4S.Models.WragbyAdmin;
using E4S.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace E4S.Controllers.WragbyAdmin
{
  [Authorize]
  public class SupportController : Controller
  {

    [TempData]
    public string StatusMessage { get; set; }

    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public SupportController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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

    //Damilare
    //================= User Method Starts Here =========================//
    public IActionResult Index()
    {
      var orgId = getOrg();
      var support = _context.Tickets.Where(x => x.OrganisationId == orgId).ToList();

      return View(support);
    }



    [HttpPost]
    public async Task<IActionResult> NewTicket([FromBody]PostNewTicket postNewTicket)
    {
      if (postNewTicket == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      var orgId = getOrg();
      var organisationDetails = _context.Organisations.Where(x => x.Id == orgId).FirstOrDefault();
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

      try
      {
        Ticket newTicket = new Ticket()
        {
          Id = Guid.NewGuid(),
          UserId = Guid.Parse(userId),
          OrganisationId = orgId,
          Title = postNewTicket.Title,
          Severity = postNewTicket.Severity,
          Description = postNewTicket.Description,
          SupportId = int.Parse(DateTime.Now.ToString("yyyymmddss"))
        };

        _context.Add(newTicket);
        _context.SaveChanges();


        return Json(new
        {
          msg = "Success"
        }
     );
      }
      catch (Exception ee)
      {

      }

      return Json(
      new
      {
        msg = "Fail"
      });
    }

    public IActionResult CreateTicket()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateTicket(Ticket ticket)
    {

      if (ModelState.IsValid)
      {
        var tick = new Ticket()
        {
          Id = Guid.NewGuid(),
          //SupportId = ticket.SupportId,
          Title = ticket.Title,
          Severity = ticket.Severity,
          Description = ticket.Description,
          Status = ticket.Status,
          ImageUrl = ticket.ImageUrl,


        };


        _context.Add(tick);
        await _context.SaveChangesAsync();

      }
      ModelState.Clear();
      return View(ticket);
    }



    //public IActionResult ViewTicket()
    //    {
    //      return View();
    //    }

    //public IActionResult ViewTicket()
    //{
    //  return View();
    //}

    //public async Task<IActionResult> ViewTicket(Guid? id)
    //{
    //  return View();
    //}

    public async Task<IActionResult> ViewTicket(Guid id)
    {
      if (id == null)
      {
        return NotFound();
      }
      var ticketDetails = await _context.TicketTreads.Include(x => x.Ticket).SingleOrDefaultAsync(i => i.Id == id);
      //var ticketDetails = await _context.Tickets.Include(x => x.Organisation).SingleOrDefaultAsync(i => i.Id == id);


      return View(ticketDetails);
    }



    //[HttpPost]
    //public IActionResult ReplyTicket()
    //{
    //  return View();
    //}


    [HttpPost]
    public IActionResult ReplyTicket([Bind("id", "Thread")]TicketTread ticketTread)
    {

      var orgId = getOrg();
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;


      if (ModelState.IsValid)
      {
        var tickthr = new TicketTread()
        {
          Id = Guid.NewGuid(),
          TicketId = ticketTread.Id,
          Thread = ticketTread.Thread,
          ResponseType = userId,
          OrganisationId = orgId,


      };

     
      _context.Add(tickthr);
      _context.SaveChangesAsync();

      //return RedirectToAction(nameof(Index));
    }

      ModelState.Clear();
      return View(ticketTread);
    }
    

    //================= User Method Ends Here =========================//




































    //Temofe
    //================= Admin Method Starts Here =======================//

    public IActionResult AllTickets()
    {

        var allOrgTicket = _context.Tickets.Include(x => x.Organisation ).ToList();

        return View(allOrgTicket);

    }

    public async Task<IActionResult> AdminViewTicket(Guid id)
    {
      if (id == null)
      {
        return NotFound();
      }
      var ticketDetails = await _context.Tickets.Include(x => x.Organisation).SingleOrDefaultAsync(i => i.Id == id);


      return View(ticketDetails);
    }
    [HttpPost]
    public IActionResult AdminReplyTicket([Bind("id", "Thread")]TicketTread ticketTread)
    {
        
        var orgId = getOrg();
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

           
        if (ModelState.IsValid)
        {
            ticketTread.Id = Guid.NewGuid();          
            ticketTread.OrganisationId = orgId;


            _context.Add(ticketTread);
            _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(ticketTread);

    }


    [HttpPost]
    public IActionResult AdminCloseTicket()
    {
      return View();
    }
    public IActionResult AdminDeleteTicket()
    {
      return View();
    }

    //================= Admin Method Ends Here =======================//

  }
}