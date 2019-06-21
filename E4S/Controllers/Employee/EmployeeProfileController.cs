using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using E4S.Data;
using E4S.Models;
using E4S.Models.HumanResource;
using E4S.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E4S.Controllers.Employee
{
  [Authorize]
    public class EmployeeProfileController : Controller
    {
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public EmployeeProfileController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
      _context = context;
      _userManager = userManager;

    }

    public IActionResult Index()
        {
            return View();
        }

    private Guid getOrg()
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var orgId = _context.Users.Where(x => x.Id == userId).FirstOrDefault().OrganisationId;

      return orgId;
    }


    public IActionResult PersonalDetails()
        {
      var orgId = getOrg();
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

      var employeeDetails = _context.EmployeeDetails.Where(x => x.UserId == Guid.Parse(userId)).FirstOrDefault();

          return View(employeeDetails);
        }

    [HttpPost]
    public async Task<IActionResult> PersonalDetails(EmployeeDetail employeeDetail)
    {
      if(employeeDetail == null)
      {
        return NotFound();
      }

      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      employeeDetail.UserId = Guid.Parse(userId);
      _context.Update(employeeDetail);
      await _context.SaveChangesAsync();


      return View(employeeDetail);
    }

        public IActionResult ContactDetails()
        {

      var orgId = getOrg();
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var employeeDetails = _context.EmployeeDetails.Where(x => x.UserId == Guid.Parse(userId)).FirstOrDefault();

      var contactDetails = _context.ContactDetails.Where(x => x.EmployeeDetailId == employeeDetails.Id).FirstOrDefault();

      if (contactDetails == null)
      {
        contactDetails = new ContactDetail()
        {
          Id = Guid.NewGuid(),
          EmployeeDetailId = employeeDetails.Id,
          OrganisationId = orgId,
          IsActive = true
        };
        _context.Add(contactDetails);
        _context.SaveChanges();

        return View(contactDetails);
      }


      return View(contactDetails);
        }

    [HttpPost]
    public async Task<IActionResult> ContactDetails(ContactDetail contactDetail)
    {
      if (contactDetail == null)
      {
        return NotFound();
      }
      var orgId = getOrg();
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var employeeDetails = _context.EmployeeDetails.Where(x => x.UserId == Guid.Parse(userId)).FirstOrDefault();

      contactDetail.EmployeeDetailId = employeeDetails.Id;
      contactDetail.OrganisationId = orgId;
      contactDetail.IsActive = true;

      _context.Update(contactDetail);
      await _context.SaveChangesAsync();

      return View();
    }


    public IActionResult EmergencyContacts()
        {

      var orgId = getOrg();
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var employeeDetails = _context.EmployeeDetails.Where(x => x.UserId == Guid.Parse(userId)).FirstOrDefault();

      var emergencyContacts = _context.EmergencyContacts.Where(x => x.EmployeeDetailId == employeeDetails.Id).ToList();

      if (emergencyContacts == null)
      {
        return View();
      }

      return View(emergencyContacts);
        }

    [HttpPost]
    public async Task<IActionResult> PostEmergencyContact([FromBody]PostEmergencyContact postEmergencyContact)
    {
      if (postEmergencyContact == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      var orgId = getOrg();
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var employeeDetails = _context.EmployeeDetails.Where(x => x.UserId == Guid.Parse(userId)).FirstOrDefault();

      try
      {
        EmergencyContact emergencyContact = new EmergencyContact()
        {
          Id = Guid.NewGuid(),
          Name = postEmergencyContact.Name,
          Relationship = postEmergencyContact.Relationship,
          HomeTelephone = postEmergencyContact.HomeTelephone,
          Address = postEmergencyContact.Address,
          OrganisationId = orgId,
          EmployeeDetailId = employeeDetails.Id,
          
        };

        _context.Add(emergencyContact);
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

    public IActionResult Dependents()
        {

      var orgId = getOrg();
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var employeeDetails = _context.EmployeeDetails.Where(x => x.UserId == Guid.Parse(userId)).FirstOrDefault();

      var dependants = _context.Dependants.Where(x => x.EmployeeDetailId == employeeDetails.Id).ToList();

      if (dependants == null)
      {
        return View();
      }

      return View(dependants);

        }

    public async Task<IActionResult> PostDependent([FromBody]PostEmergencyContact postEmergencyContact)
    {
      if (postEmergencyContact == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      var orgId = getOrg();
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var employeeDetails = _context.EmployeeDetails.Where(x => x.UserId == Guid.Parse(userId)).FirstOrDefault();

      try
      {
        Dependant dependant = new Dependant()
        {
          Id = Guid.NewGuid(),
          Name = postEmergencyContact.Name,
          Relationship = postEmergencyContact.Relationship,
          Address = postEmergencyContact.Address,
          OrganisationId = orgId,
          EmployeeDetailId = employeeDetails.Id,

        };

        _context.Add(dependant);
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


    public IActionResult Jobs()
        {
            return View();
        }

        public IActionResult Salary()
        {
            return View();
        }

        public IActionResult Qualification()
        {
            return View();
        }

        public IActionResult EmployeeAssets()
        {
            return View();
        }

        public IActionResult Appraisal()
        {
            return View();
        }

        public IActionResult Leave()
        {
            return View();
        }
    }
}