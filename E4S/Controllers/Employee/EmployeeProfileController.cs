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
using E4S.Services;
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
    private readonly IEmailSender _emailSender;

    public EmployeeProfileController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
    {
      _context = context;
      _userManager = userManager;
      _emailSender = emailSender;
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

      try
      {
        var job = _context.Jobs.Where(x => x.EmployeeDetailId == userDetails.Id).Include(x => x.JobTitle).Include(x => x.Department).FirstOrDefault();

        edVM.JobTitle = job.JobTitle.JobTitleName;
        edVM.Department = job.Department.DepartmentName;
      }
      catch
      {

      }


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


        [HttpPost]
        public async Task<IActionResult> editEmergencyContact([FromBody]PostEmergencyContact postEmergencyContact)
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
            var employeeDetails = await _context.EmployeeDetails.Where(x => x.UserId == Guid.Parse(userId)).FirstOrDefaultAsync();

            try
            {
                
                var EmployEmergCon = _context.EmergencyContacts.Where(x => x.Id == Guid.Parse(postEmergencyContact.AId)).FirstOrDefault();
                EmployEmergCon.Name = postEmergencyContact.Name;
                EmployEmergCon.Relationship = postEmergencyContact.Relationship;
                EmployEmergCon.HomeTelephone = postEmergencyContact.HomeTelephone;
                EmployEmergCon.Address = postEmergencyContact.Address;
                EmployEmergCon.OrganisationId = orgId;
                EmployEmergCon.EmployeeDetailId = employeeDetails.Id;
                
               
                _context.Update(EmployEmergCon);
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

    // Ednf of Edit for Emergency Contact

    //Delete Emergency Contact

    private bool EmergencyContactExists(Guid id)
    {
      return _context.EmergencyContacts.Any(e => e.Id == id);
    }


    [HttpPost]
    public IActionResult DeleteEmergencyContact([FromBody]string emergencyContactId)
    {
      if (emergencyContactId == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      try
      {
        var emergencyContact = _context.EmergencyContacts.SingleOrDefault(m => m.Id == Guid.Parse(emergencyContactId));
        _context.EmergencyContacts.Remove(emergencyContact);
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

    // end of emergency contacts

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

    public async Task<IActionResult> PostDependent([FromBody]PostDependents postDependents)
    {
      if (postDependents == null)
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
          Name = postDependents.Name,
          Relationship = postDependents.Relationship,
          Address = postDependents.Address,
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


        // Edit the Dependents

        [HttpPost]
        public async Task<IActionResult> editDependents([FromBody]PostDependents postDependents)
        {
            if (postDependents == null)
            {
                return Json(new
                {
                    msg = "No Data"
                }
               );
            }

            var orgId = getOrg();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var employeeDetails = await _context.EmployeeDetails.Where(x => x.UserId == Guid.Parse(userId)).FirstOrDefaultAsync();

            try
            {

                var EmployDependt = _context.Dependants.Where(x => x.Id == Guid.Parse(postDependents.AId)).FirstOrDefault();
                EmployDependt.Name = postDependents.Name;
                EmployDependt.Relationship = postDependents.Relationship;
                EmployDependt.Address = postDependents.Address;
                EmployDependt.OrganisationId = orgId;
                EmployDependt.EmployeeDetailId = employeeDetails.Id;


                _context.Update(EmployDependt);
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

    // Ednf of Edit for Dependents

    //delete dependents

    private bool DependentExists(Guid id)
    {
      return _context.Dependants.Any(e => e.Id == id);
    }


    [HttpPost]
    public IActionResult DeleteDependent([FromBody]string dependentId)
    {
      if (dependentId == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      try
      {
        var dependent = _context.Dependants.SingleOrDefault(m => m.Id == Guid.Parse(dependentId));
        _context.Dependants.Remove(dependent);
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

    public IActionResult Jobs()
        {
      var orgId = getOrg();
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var employeeDetails = _context.EmployeeDetails.Where(x => x.UserId == Guid.Parse(userId)).FirstOrDefault();

      var empJob = _context.Jobs
        .Where(x => x.EmployeeDetailId == employeeDetails.Id)
        .Include(x => x.JobTitle)
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

    private bool SkillsExists(Guid id)
    {
      return _context.Skills.Any(e => e.Id == id);
    }


    [HttpPost]
    public IActionResult DeleteSkills([FromBody]string skillId)
    {
      if (skillId == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      try
      {
        var skill = _context.Skills.SingleOrDefault(m => m.Id == Guid.Parse(skillId));
        _context.Skills.Remove(skill);
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


    private bool RecordsExists(Guid id)
    {
      return _context.InstitutionQualifications.Any(e => e.Id == id);
    }


    [HttpPost]
    public IActionResult DeleteRecords([FromBody]string institutionId)
    {
      if (institutionId == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      try
      {
        var institution = _context.InstitutionQualifications.SingleOrDefault(m => m.Id == Guid.Parse(institutionId));
        _context.InstitutionQualifications.Remove(institution);
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

    private bool WorkExperienceExists(Guid id)
    {
      return _context.WorkExperiences.Any(e => e.Id == id);
    }


    [HttpPost]
    public IActionResult DeleteWorkExperience([FromBody]string companyId)
    {
      if (companyId == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      try
      {
        var workExperience = _context.WorkExperiences.SingleOrDefault(m => m.Id == Guid.Parse(companyId));
        _context.WorkExperiences.Remove(workExperience);
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

      var orgId = getOrg();
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var employeeDetails = _context.EmployeeDetails.Where(x => x.UserId == Guid.Parse(userId)).FirstOrDefault();

      var employeeAsset = _context.OrganisationAssets.Where(x => x.EmployeeDetailId == employeeDetails.Id).ToList();

      return View(employeeAsset);
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
            ViewData["LeaveTitle"] = new SelectList(_context.LeaveConfigurations.Where(x => x.OrganisationId == orgId), "Id", "LeaveTitle");


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

      if(postLeave.StartDate <= DateTime.Now)
      {
        return Json(new
        {
          msg = "You cant not back date leave date, check/adjust the dates."
        });
      }
      else if(postLeave.StartDate >= postLeave.EndDate)
      {
        return Json(new
        {
          msg = "End date is earlier than start date. Kindly adjust dates."
        });

      }

      var orgId = getOrg();
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var employeeDetails = _context.EmployeeDetails.Where(x => x.UserId == Guid.Parse(userId)).FirstOrDefault();

      var leaveDetails = _context.LeaveConfigurations.Where(x => x.OrganisationId == orgId).Where(x => x.Id == Guid.Parse(postLeave.LeaveTitle)).FirstOrDefault();
      var organisationDetails = _context.Organisations.Where(x => x.Id == orgId).FirstOrDefault();

      int days = int.Parse((postLeave.EndDate.Date - postLeave.StartDate.Date).TotalDays.ToString());
      int weekendCount = 0;
      for (DateTime date = postLeave.StartDate; date <= postLeave.EndDate; date = date.AddDays(1))
      {
        if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
          weekendCount++;
      }

      int leaveDays = days - weekendCount;

      if(leaveDetails.MaxDuration < leaveDays)
      {
        return Json(new
        {
          msg = "You have exceeded the number of leave days. Kindly adjust dates."
        });
      }


      try
      {
        Leave leave = new Leave()
        {
          Id = Guid.NewGuid(),
          LeaveTitle = leaveDetails.LeaveTitle,
          LeaveConfigId = leaveDetails.Id,
          Description = postLeave.Description,
          StartDate = postLeave.StartDate,
          EndDate = postLeave.EndDate,
          OrganisationId = orgId,
          Status = "Pending",
          EmployeeDetailId = employeeDetails.Id,
          CalculatedDays = leaveDays,

        };

        _context.Add(leave);
        _context.SaveChanges();

         var user_response = _emailSender.SendGridLeaveApplicationAsync(employeeDetails.Email, "Leave Application Request", "/EmployeeProfile/Leave", employeeDetails.FirstName, "leaveApplication", organisationDetails.OrganisationName, leave);


        leave.EmployeeDetail = employeeDetails;

        var hrs = _context.Users.Where(x => x.OrganisationId == orgId).Where(x => x.UserRole == "Human Resources").ToList();

        foreach (var item in hrs)
        {
          var response = _emailSender.SendGridLeaveRequestAsync(item.Email, "Pending Leave Request",
          "/Leaves", item.FirstName, "leaveRequest", organisationDetails.OrganisationName, leave);

        }


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

    private bool LeaveExists(Guid id)
    {
      return _context.Leaves.Any(e => e.Id == id);
    }


    [HttpPost]
    public IActionResult DeleteLeave([FromBody]string leaveId)
    {
      if (leaveId == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      try
      {
        var leave = _context.Leaves.SingleOrDefault(m => m.Id == Guid.Parse(leaveId));
        _context.Leaves.Remove(leave);
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





    public IActionResult Performance()
    {
      var orgId = getOrg();
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var employeeDetails = _context.EmployeeDetails.Where(x => x.UserId == Guid.Parse(userId)).FirstOrDefault();

      EmployeePerformanceViewModel epVM = new EmployeePerformanceViewModel();

      var empTemplate = _context.AppraisalAssignedTemplates.Where(x => x.OrganisationId == orgId).Where(x => x.EmployeeDetailId == employeeDetails.Id).Where(x => x.Status != "Completed").Include(x => x.AppraisalTemplate).Include(x => x.Appraisal).ToList();
      epVM.AppraisalAssignedTemplates = empTemplate;

      

      return View(epVM);
    }

    public IActionResult AppraisalTemplate(string id)
    {
      var orgId = getOrg();

      var idsplit = id.Split(',');
      Guid ids = Guid.Parse(idsplit[0]);
      Guid appId = Guid.Parse(idsplit[1]);

      AppraisalViewTemplateViewModel avtVM = new AppraisalViewTemplateViewModel();

      List<AppCat> appCat = new List<AppCat>();
      List<AppraisalKPI> kPIs;
      AppCat sAppCat;

      List<Guid> kpiId = new List<Guid>();

      var temp = _context.AppraisalTemplates.Where(x => x.Id == ids).FirstOrDefault();
      var appCats = _context.AppraisalTemplateCategories.Where(x => x.AppraisalTemplateId == ids).Include(x => x.AppraisalCategory).ToList();
      var kpi = _context.AppraisalKPIs.Where(x => x.OrganisationId == orgId);
      avtVM.TemplateName = temp.Template;


      AppraisalCategoryEdit ace;

      foreach (var item in appCats)
      {

        sAppCat = new AppCat();
        ace = new AppraisalCategoryEdit();
        kPIs = new List<AppraisalKPI>();

        sAppCat.AppraisalTemplateCategory = item;
        ace.AppraisalCategory = item.AppraisalCategory;
        kPIs = kpi.Where(x => x.AppraisalCategoryId == item.AppraisalCategoryId).ToList();

        kpiId.AddRange(kPIs.Select(x => x.Id));

        ace.AppraisalKPIs = kPIs;
        sAppCat.AppraisalCategoryEdit = ace;

        appCat.Add(sAppCat);
      }

      avtVM.AppCat = appCat;
      avtVM.kpiId = kpiId;
      avtVM.AppTemId = appId;
      return View(avtVM);

    }

    [HttpPost]
    public async Task<IActionResult> EmpAppResult([FromBody]AppraisalResponse empAppResult)
    {
      if (empAppResult == null)
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

      var appsKPI = _context.AppraisalKPIs.Where(x => x.OrganisationId == orgId).ToList();
      var appsCat = _context.AppraisalTemplateCategories.Where(x => x.OrganisationId == orgId).ToList();

      AppraisalEmployeeResult appEmployeeResult = new AppraisalEmployeeResult()
      {
        Id = Guid.NewGuid(),
        AppraisalId = empAppResult.AppraisalId,
        EmployeeDetailId = employeeDetails.Id,
        DateSubmited = DateTime.Now,
        OrganisationId = orgId
      };

      List<KPIEmployeeResult> KPIListResult = new List<KPIEmployeeResult>();
      KPIEmployeeResult singleKPI;
      float point = 0;
      float points = 0;

      foreach (var item in empAppResult.KPIs)
      {
        singleKPI = new KPIEmployeeResult()
        {
          Id = Guid.NewGuid(),
          AppraisalId = appEmployeeResult.AppraisalId,
          EmployeeDetailId = appEmployeeResult.EmployeeDetailId,
          AppraisalEmployeeResultId = appEmployeeResult.Id,
          KPIId = item.KPIId,
          KPISelection = item.KPIResult,
          OrganisationId = orgId
        };

        var kpi = appsKPI.Where(x => x.Id == singleKPI.KPIId).FirstOrDefault();
        var cat = appsCat.Where(x => x.AppraisalCategoryId == kpi.AppraisalCategoryId).FirstOrDefault();

        point = point + (kpi.Weight * Int32.Parse(singleKPI.KPISelection) * 20 * cat.Weight);
        points = points + (kpi.Weight * cat.Weight);

        KPIListResult.Add(singleKPI);
      }

      appEmployeeResult.AppraisalScore = point / points;




      try
      {

        _context.Add(appEmployeeResult);
        _context.SaveChanges();

        _context.AddRange(KPIListResult);
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

    [HttpPost]
    public async Task<IActionResult> FinishAppResult([FromBody]AppraisalResponse empAppResult)
    {
      if (empAppResult == null)
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

      var appsKPI = _context.AppraisalKPIs.Where(x => x.OrganisationId == orgId).ToList();
      var appsCat = _context.AppraisalTemplateCategories.Where(x => x.OrganisationId == orgId).ToList();

      var appExisit = _context.AppraisalEmployeeResults.Where(x => x.AppraisalId == empAppResult.AppraisalId).Where(x => x.EmployeeDetailId == employeeDetails.Id).FirstOrDefault();
      float point = 0;
      float points = 0;


      if (appExisit != null)
      {
        var kpiResults = _context.KPIEmployeeResults.Where(x => x.AppraisalEmployeeResultId == appExisit.Id).Where(x => x.EmployeeDetailId == employeeDetails.Id).ToList();

        foreach (var item in empAppResult.KPIs)
        {
          var editKPI = kpiResults.Where(x => x.KPIId == item.KPIId).FirstOrDefault();

          var kpi = appsKPI.Where(x => x.Id == editKPI.KPIId).FirstOrDefault();
          var cat = appsCat.Where(x => x.AppraisalCategoryId == kpi.AppraisalCategoryId).FirstOrDefault();
          editKPI.KPISelection = item.KPIResult;

          point = point + (kpi.Weight * Int32.Parse(editKPI.KPISelection) * 20 * cat.Weight);
          points = points + (kpi.Weight * cat.Weight);

          _context.KPIEmployeeResults.Update(editKPI);
          _context.SaveChanges();
        }


        appExisit.Status = "Finished";
        appExisit.AppraisalScore = point / points;

        _context.AppraisalEmployeeResults.Update(appExisit);
        _context.SaveChanges();

        return Json(new
        {
          msg = "Success"
        }
);

      }



      AppraisalEmployeeResult appEmployeeResult = new AppraisalEmployeeResult()
      {
        Id = Guid.NewGuid(),
        AppraisalId = empAppResult.AppraisalId,
        EmployeeDetailId = employeeDetails.Id,
        DateSubmited = DateTime.Now,
        OrganisationId = orgId,
        Status = "Finished"
      };

      List<KPIEmployeeResult> KPIListResult = new List<KPIEmployeeResult>();
      KPIEmployeeResult singleKPI;
      point = 0;
      points = 0;

      foreach (var item in empAppResult.KPIs)
      {
        singleKPI = new KPIEmployeeResult()
        {
          Id = Guid.NewGuid(),
          AppraisalId = appEmployeeResult.AppraisalId,
          EmployeeDetailId = appEmployeeResult.EmployeeDetailId,
          AppraisalEmployeeResultId = appEmployeeResult.Id,
          KPIId = item.KPIId,
          KPISelection = item.KPIResult,
          OrganisationId = orgId
        };

        var kpi = appsKPI.Where(x => x.Id == singleKPI.KPIId).FirstOrDefault();
        var cat = appsCat.Where(x => x.AppraisalCategoryId == kpi.AppraisalCategoryId).FirstOrDefault();

        point = point + (kpi.Weight * Int32.Parse(singleKPI.KPISelection) * 20 * cat.Weight);
        points = points + (kpi.Weight * cat.Weight);

        KPIListResult.Add(singleKPI);
      }

      appEmployeeResult.AppraisalScore = point / points;




      try
      {

        _context.Add(appEmployeeResult);
        _context.SaveChanges();

        _context.AddRange(KPIListResult);
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