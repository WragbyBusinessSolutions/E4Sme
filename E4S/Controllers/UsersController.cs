using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using E4S.Data;
using E4S.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E4S.Controllers
{
  [Authorize]
  public class UsersController : Controller
    {
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
      _context = context;

    }
    private Guid getOrg()
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var orgId = _context.Users.Where(x => x.Id == userId).FirstOrDefault().OrganisationId;

      return orgId;
    }

    public IActionResult Index()
    {
      var orgId = getOrg();

      var userList = _context.Users.Where(x => x.OrganisationId == orgId).ToList();

      return View(userList);
    }

    public IActionResult AddUser()
    {
      return View();
    }

    [HttpPost]
    public IActionResult AddUser(UsersViewModel user)
    {
      var orgId = getOrg();


      return View();
    }

  }
}