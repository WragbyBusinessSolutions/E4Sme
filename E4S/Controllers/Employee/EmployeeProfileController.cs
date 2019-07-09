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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
      var orgId = getOrg();

      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

      var userDetails = _context.EmployeeDetails.Where(x => x.UserId == Guid.Parse(userId)).FirstOrDefault();

      EmployeeDashboardViewModel edVM = new EmployeeDashboardViewModel();
      edVM.FirstName = userDetails.FirstName;
      edVM.LastName = userDetails.LastName;
      edVM.ImageURL = userDetails.ImageUrl;

      var job = _context.Jobs.Where(x => x.EmployeeDetailId == userDetails.Id).Include(x => x.JobTitle).Include(x => x.Department).FirstOrDefault();

      edVM.JobTitle = job.JobTitle.JobTitleName;
      edVM.Department = job.Department.DepartmentName;



      return View(edVM);
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


    public IActionResult PersonalDetails()
        {
      var orgId = getOrg();
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

      var employeeDetails = _context.EmployeeDetails.Where(x => x.UserId == Guid.Parse(userId)).FirstOrDefault();

          return View(employeeDetails);
        }

    public async Task<IActionResult> UploadProfile(IFormFile file)
    {
      if (file == null || file.Length == 0)
        return Content("file not selected");

      //var path = Path.Combine(
      //            Directory.GetCurrentDirectory(), "wwwroot",
      //            file.FileName);
      var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "employeeImage", file.FileName);
      var path2 = Path.Combine("images", "employeeImage", file.FileName);

      var orgId = getOrg();
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var employeeDetails = _context.EmployeeDetails.Where(x => x.UserId == Guid.Parse(userId)).FirstOrDefault();


      using (var stream = new FileStream(path, FileMode.Create))
      {
        await file.CopyToAsync(stream);
        employeeDetails.ImageUrl = path2;

        _context.Update(employeeDetails);
        _context.SaveChanges();

      }

      return RedirectToAction("PersonalDetails");
    }


    [HttpPost]
    public async Task<IActionResult> PersonalDetails(EmployeeDetail employeeDetail)
    {
      if(employeeDetail == null)
      {
        return NotFound();
      }

      var orgId = getOrg();
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      employeeDetail.UserId = Guid.Parse(userId);
      employeeDetail.OrganisationId = orgId;

      var empDetails = _context.EmployeeDetails.Where(x => x.Id == employeeDetail.Id).FirstOrDefault();

      empDetails.FirstName = employeeDetail.FirstName;
      empDetails.MiddleName = employeeDetail.MiddleName;
      empDetails.LastName = employeeDetail.LastName;
      empDetails.OtherId = employeeDetail.OtherId;
      empDetails.Gender = employeeDetail.Gender;
      empDetails.MaritalStatus = employeeDetail.MaritalStatus;

      empDetails.DateOfBirth = employeeDetail.DateOfBirth;


      _context.Update(empDetails);
      await _context.SaveChangesAsync();


      return View(empDetails);
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
      //contactDetail.EmployeeDetail = null;

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

      var orgId = getOrg();
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var employeeDetails = _context.EmployeeDetails.Where(x => x.UserId == Guid.Parse(userId)).FirstOrDefault();

      var empSalary = _context.Salaries.Where(x => x.EmployeeDetailId == employeeDetails.Id).FirstOrDefault();
      return View(empSalary);
        }

        public IActionResult Qualification()
        {
      var orgId = getOrg();
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var employeeDetails = _context.EmployeeDetails.Where(x => x.UserId == Guid.Parse(userId)).FirstOrDefault();
      var employeeQualification = _context.InstitutionQualifications.Where(x => x.EmployeeDetailId == employeeDetails.Id).ToList();
      var employeeSkill = _context.Skills.Where(x => x.EmployeeDetailId == employeeDetails.Id).ToList();
      var employeeWorkExperience = _context.WorkExperiences.Where(x => x.EmployeeDetailId == employeeDetails.Id).ToList();


      EmployeeQualificationViewModel EQVM = new EmployeeQualificationViewModel();
      EQVM.InstitutionQualifications = employeeQualification;
      EQVM.Skills = employeeSkill;
      EQVM.WorkExperiences = employeeWorkExperience;


      return View(EQVM);
        }

    public async Task<IActionResult> skillsUpload(PostSkill postSkill)
    {
      if(postSkill == null)
      {
        return NoContent();
      }

      var orgId = getOrg();
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var employeeDetails = _context.EmployeeDetails.Where(x => x.UserId == Guid.Parse(userId)).FirstOrDefault();


      try
      {
        Skill skill = new Skill()
        {
          Id = Guid.NewGuid(),
          EmployeeDetailId = employeeDetails.Id,
          SkillName = postSkill.qSkill,
          Description = postSkill.qDescription,
          YearsOfExperience = postSkill.qYearOfExperience,
          OrganisationId = orgId

        };

        _context.Add(skill);
        _context.SaveChanges();


      }
      catch
      {

      }


      return RedirectToAction("Qualification");


    }

    [HttpPost]
    public async Task<IActionResult> editSkills([FromBody]PostSkill postSkill)
    {
      if (postSkill == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      var orgId = getOrg();
      var organisationDetails = await _context.Organisations.Where(x => x.Id == orgId).FirstOrDefaultAsync();


      //bool isAssign = true;

      //if (postNewDepartment. == Guid.Empty)
      //{
      //    isAssign = false;
      //}

      try
      {

        var orgSkill = _context.Skills.Where(x => x.Id == Guid.Parse(postSkill.AId)).FirstOrDefault();
        orgSkill.SkillName = postSkill.qSkill;
        orgSkill.Description = postSkill.qDescription;
        orgSkill.YearsOfExperience = postSkill.qYearOfExperience;


        _context.Update(orgSkill);
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


    public async Task<IActionResult> editRecords([FromBody]PostQualification postQualification)
    {
      if (postQualification == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      var orgId = getOrg();
      var organisationDetails = await _context.Organisations.Where(x => x.Id == orgId).FirstOrDefaultAsync();


      //bool isAssign = true;

      //if (postNewDepartment. == Guid.Empty)
      //{
      //    isAssign = false;
      //}

      try
      {

        var orgRecord = _context.InstitutionQualifications.Where(x => x.Id == Guid.Parse(postQualification.AId)).FirstOrDefault();
        orgRecord.Degree = postQualification.Degree;
        orgRecord.Grade = postQualification.Grade;
        orgRecord.Institution = postQualification.Institution;
        orgRecord.CourseOfStudy = postQualification.CourseOfStudy;
        orgRecord.YearCompleted = postQualification.YearCompleted;
        orgRecord.ImageURL = postQualification.ImageUrl;


        _context.Update(orgRecord);
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


    public async Task<IActionResult> editWorkExperience([FromBody]PostWorkExperience postWorkExperience)
    {
      if (postWorkExperience == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      var orgId = getOrg();
      var organisationDetails = await _context.Organisations.Where(x => x.Id == orgId).FirstOrDefaultAsync();


      //bool isAssign = true;

      //if (postNewDepartment. == Guid.Empty)
      //{
      //    isAssign = false;
      //}

      try
      {

        var orgWork = _context.WorkExperiences.Where(x => x.Id == Guid.Parse(postWorkExperience.AId)).FirstOrDefault();
        orgWork.Organisation = postWorkExperience.WOrganisation;
        orgWork.JobTitle = postWorkExperience.WJobTitle;
        orgWork.StartDate = postWorkExperience.WStartDate;
        orgWork.EndDate = postWorkExperience.WEndDate;
        orgWork.Comment = postWorkExperience.WComment;


        _context.Update(orgWork);
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



    public async Task<IActionResult> workUpload(PostWorkExperience postWorkExperience)
    {
      if (postWorkExperience == null)
      {
        return NoContent();
      }

      var orgId = getOrg();
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var employeeDetails = _context.EmployeeDetails.Where(x => x.UserId == Guid.Parse(userId)).FirstOrDefault();


      try
      {
        WorkExperience workExperience = new WorkExperience()
        {
          Id = Guid.NewGuid(),
          EmployeeDetailId = employeeDetails.Id,
          Organisation = postWorkExperience.WOrganisation,
          JobTitle = postWorkExperience.WJobTitle,
          StartDate = postWorkExperience.WStartDate,
          EndDate = postWorkExperience.WEndDate,
          Comment = postWorkExperience.WComment,
          OrganisationId = orgId

        };

        _context.Add(workExperience);
        _context.SaveChanges();


      }
      catch
      {

      }


      return RedirectToAction("Qualification");


    }


    public async Task<IActionResult> UploadDocument(IFormFile file, PostQualification postQualification)
    {
      //if (file == null || file.Length == 0)
      //  return Content("file not selected");

      //var path = Path.Combine(
      //            Directory.GetCurrentDirectory(), "wwwroot",
      //            file.FileName);
      //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "qualificationImage", file.FileName);
      //var path2 = Path.Combine("images", "qualificationImage", file.FileName);

      var orgId = getOrg();
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var employeeDetails = _context.EmployeeDetails.Where(x => x.UserId == Guid.Parse(userId)).FirstOrDefault();

      InstitutionQualification institutionQualification = new InstitutionQualification()
      {
        Id = Guid.NewGuid(),
        OrganisationId = orgId,
        EmployeeDetailId = employeeDetails.Id,
        Degree = postQualification.Degree,
        Grade = postQualification.Grade,
        CourseOfStudy = postQualification.CourseOfStudy,
        Institution = postQualification.Institution,
        YearCompleted = postQualification.YearCompleted,
        ImageURL = postQualification.ImageUrl,

      };


      //using (var stream = new FileStream(path, FileMode.Create))
      //{
      //  await file.CopyToAsync(stream);
      // // employeeDetails.ImageUrl = path2;

      //}

      institutionQualification.ImageURL = postQualification.ImageUrl;


      _context.Add(institutionQualification);
      _context.SaveChanges();


      return RedirectToAction("Qualification");
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
      ViewData["LeaveTitle"] = new SelectList(_context.LeaveConfigurations.Where(x => x.OrganisationId == orgId), "LeaveTitle", "LeaveTitle");


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