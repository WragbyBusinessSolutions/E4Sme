using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using E4S.Data;
using E4S.Models;
using E4S.Models.HumanResource;
using E4S.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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


        //[HttpPost]
        //public void Upload()
        //{
        //    for (int i = 0; i < Request.Files.Count; i++)
        //    {
        //        var file = Request.Files[i];

        //        var fileName = Path.GetFileName(file.FileName);

        //        var path = Path.Combine(Server.MapPath("~/App_Data/"), fileName);
        //        file.SaveAs(path);
        //    }

        //}

        //[HttpPost]
        //public async Task<IActionResult> UploadLogo(string name, Organisation organisation)
        //{
        //    var newFileName = string.Empty;

        //    if (HttpContext.Request.Form.Files != null)
        //    {
        //        var fileName = string.Empty;
        //        string PathDB = string.Empty;

        //        var files = HttpContext.Request.Form.Files;

        //        //var organisation = new Organisation();

        //        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);



        //        foreach (var file in files)
        //        {
        //            if (file.Length > 0)
        //            {
        //                //Getting FileName
        //                fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

        //                /// fileName = userId  + "logo";
        //                //Assigning Unique Filename (Guid)
        //                var myUniqueFileName = Convert.ToString(Guid.NewGuid());

        //                //Getting file Extension
        //                var FileExtension = Path.GetExtension(fileName);

        //                // concating  FileName + FileExtension
        //                newFileName = userId + FileExtension;

        //                // Combines two strings into a path.
        //                fileName = Path.Combine(_env.WebRootPath, "demoImages") + $@"\{newFileName}";

        //                // if you want to store path of folder in database
        //                PathDB = "demoImages/" + newFileName;

        //                using (FileStream fs = System.IO.File.Create(fileName))
        //                {
        //                    file.CopyTo(fs);
        //                    fs.Flush();
        //                }

        //                var organ = _context.employeeDetail.Where(x => x.UserId == Guid.Parse(userId)).FirstOrDefault();
        //                organ.ImageUrl = PathDB;

        //                _context.Update(organ);
        //                await _context.SaveChangesAsync();

        //                var test = organ;
        //                var another = test;



        //            }
        //        }


        //    }
        //    return View(nameof(Edit));
        //}

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
      var orgId = getOrg();
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var employeeDetails = _context.EmployeeDetails.Where(x => x.UserId == Guid.Parse(userId)).FirstOrDefault();

      var empJob = _context.Jobs
        .Where(x => x.EmployeeDetailId == employeeDetails.Id)
        .Include(x => x.Branch)
        .Include(x => x.JobCategory)
        .Include(x => x.JobTitle)
        .Include(x => x.EmploymentStatus)
        .Include(x => x.Department)
        .FirstOrDefault();
      return View(empJob);
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
      var orgId = getOrg();
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

      var leaveList = _context.Leaves.Where(x => x.EmployeeDetail.UserId == Guid.Parse(userId)).ToList();
      return View(leaveList);
        }

    [HttpPost]
    public async Task<IActionResult> PostNewLeave([FromBody]PostNewLeave postLeave)
    {
      if (postLeave == null)
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
        Leave leave = new Leave()
        {
          Id = Guid.NewGuid(),
          LeaveTitle = postLeave.LeaveTitle,
          Description = postLeave.Description,
          StartDate = postLeave.StartDate,
          EndDate = postLeave.EndDate,
          OrganisationId = orgId,
          Status = "Pending",
          EmployeeDetailId = employeeDetails.Id

        };

        _context.Add(leave);
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

  }
}