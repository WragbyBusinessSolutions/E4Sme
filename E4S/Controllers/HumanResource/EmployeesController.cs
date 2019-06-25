using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace E4S.Controllers.HumanResource
{
  [Authorize]
    public class EmployeesController : Controller
    {
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender _emailSender;

    public EmployeesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
    {
      _context = context;
      _userManager = userManager;
      _emailSender = emailSender;

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

      var employeeListDb = _context.EmployeeDetails.Where(x => x.OrganisationId == orgId).ToList();
      List<EmployeeListViewModel> employeeList = new List<EmployeeListViewModel>();

      EmployeeListViewModel singleEmployee;

      foreach (var item in employeeListDb)
      {
        try
        {
          var empDetails = _context.Jobs.Where(x => x.EmployeeDetailId == item.Id).Include(x => x.JobTitle).Include(x => x.Department).Include(x => x.EmploymentStatus).FirstOrDefault();
          singleEmployee = new EmployeeListViewModel()
          {
            Id = item.Id,
            EmployeeName = item.FirstName + " " + item.LastName,
            Email = item.Email,
            Department = empDetails.Department.DepartmentName,
            EmployeeStatus = empDetails.EmploymentStatus.EmploymentStatusName,
            JobTitle = empDetails.JobTitle.JobTitleName,
            Supervisor = ""
          };

        }
        catch
        {
          var empDetails = _context.Jobs.Where(x => x.EmployeeDetailId == item.Id).FirstOrDefault();
          singleEmployee = new EmployeeListViewModel()
          {
            Id = item.Id,
            EmployeeName = item.FirstName + " " + item.LastName,
            Email = item.Email,
            Department = "--Not Assigned--",
            EmployeeStatus = "--Not Assigned--",
            JobTitle = "--Not Assigned--",
            Supervisor = ""
          };

        }
        //var empDetails = _context.Jobs.Where(x => x.EmployeeDetailId == item.Id).Include(x => x.JobTitle).Include(x => x.Department).Include(x => x.EmploymentStatus).FirstOrDefault();
        //singleEmployee = new EmployeeListViewModel()
        //{
        //  Id = item.Id,
        //  EmployeeName = item.FirstName + " " + item.LastName,
        //  Department = empDetails.Department.DepartmentName,
        //  EmployeeStatus = empDetails.EmploymentStatus.EmploymentStatusName,
        //  JobTitle = empDetails.JobTitle.JobTitleName,
        //  Supervisor = ""
        //};

        employeeList.Add(singleEmployee);
      }



      return View(employeeList);
        }
    
        public IActionResult AddEmployee()
        {
            return View();
        }

    [HttpPost]
    public async Task<IActionResult> UploadCSV(IFormFile file)
    {
      if (file == null || file.Length == 0)
        return Content("file not selected");

      var path = Path.Combine(
                  Directory.GetCurrentDirectory(), "wwwroot",
                  file.FileName);

      var orgId = getOrg();
      var organisationDetails = _context.Organisations.Where(x => x.Id == orgId).FirstOrDefault();

      using (var stream = new FileStream(path, FileMode.Create))
      {
        await file.CopyToAsync(stream);
      }

      string csvData = System.IO.File.ReadAllText(path);
      Guid userId;

      //Execute a loop over the rows.
      foreach (string row in csvData.Split("\r\n"))
      {
        if (!string.IsNullOrEmpty(row))
        {
          userId = Guid.NewGuid();
          int noOfEmployee = _context.Users.Where(x => x.OrganisationId == orgId).Count();

          EmployeeDetail empDetail = new EmployeeDetail()
          {
            Id = Guid.NewGuid(),
            FirstName = row.Split(',')[0],
            LastName = row.Split(',')[1],
            Email = row.Split(',')[2],
            OrganisationId = orgId,
            EmployeeId = organisationDetails.OrganisationPrefix + (noOfEmployee + 1).ToString(),
            UserId = userId
          };

          var user = new ApplicationUser
          {
            Id = userId.ToString(),
            UserName = empDetail.Email,
            Email = empDetail.Email,
            OrganisationId = orgId,
            UserRole = "Employee",
            EmployeeName = empDetail.LastName + " " + empDetail.FirstName,
          };

          var result = await _userManager.CreateAsync(user);
          if (result.Succeeded)
          {
            await _userManager.AddToRoleAsync(user, user.UserRole);

            var Email = user.Email;
            string Code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, Code, Request.Scheme);

            var response = _emailSender.SendGridEmailAsync(user.Email, "Reset Password",
               $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");

            //var response = _emailSender.GmailSendEmail(user.Email, callbackUrl, user.UserRole);

            _context.Add(empDetail);
            _context.SaveChanges();


          }


        }
        }
      return RedirectToAction("Index");
    }

        public IActionResult EmployeeDetails(Guid id)
        {
      var singleEmployee = _context.EmployeeDetails.Where(x => x.Id == id).FirstOrDefault();
      var orgId = getOrg();
      EmployeeDetailsViewModel employeeDetailsVM = new EmployeeDetailsViewModel();
      employeeDetailsVM.Id = id;
      employeeDetailsVM.EmployeeDetail = singleEmployee;
      employeeDetailsVM.ContactDetail = _context.ContactDetails.Where(x => x.EmployeeDetailId == id).FirstOrDefault();
      employeeDetailsVM.EmergencyContacts = _context.EmergencyContacts.Where(x => x.EmployeeDetailId == id).ToList();
      employeeDetailsVM.Dependants = _context.Dependants.Where(x => x.EmployeeDetailId == id).ToList();


      var salaryEmployee = _context.Salaries.Where(x => x.EmployeeDetailId == singleEmployee.Id).FirstOrDefault();
      var jobEmployee = _context.Jobs.Where(x => x.EmployeeDetailId == id).FirstOrDefault();

      if (jobEmployee != null)
      {
        employeeDetailsVM.BranchId = jobEmployee.BranchId;
        employeeDetailsVM.JobTitleId = jobEmployee.JobTitleId;
        employeeDetailsVM.DepartmentId = jobEmployee.DepartmentId;
        employeeDetailsVM.EmploymentStatusId = jobEmployee.EmploymentStatusId;
        employeeDetailsVM.JobCategoryId = jobEmployee.JobCategoryId;
        employeeDetailsVM.JoinDate = jobEmployee.JoinedDate;
        employeeDetailsVM.StartDate = jobEmployee.StartDate;
        employeeDetailsVM.EndDate = jobEmployee.EndDate;
        employeeDetailsVM.ContractDetails = jobEmployee.ContractDetail;

      }

      if (salaryEmployee != null)
      {
        employeeDetailsVM.Amount = salaryEmployee.Amount;
        employeeDetailsVM.PayGradeId = salaryEmployee.PayGradeId;
        employeeDetailsVM.PayFrequency = salaryEmployee.PayFrequency;
        employeeDetailsVM.Comments = salaryEmployee.Comment;
        employeeDetailsVM.Currency = salaryEmployee.Currency;
      }

      ViewData["JobTitle"] = new SelectList(_context.JobTitles.Where(x => x.OrganisationId == orgId) , "Id", "JobTitleName", employeeDetailsVM.JobTitleId);
      ViewData["EmploymentStatus"] = new SelectList(_context.EmploymentStatuses.Where(x => x.OrganisationId == orgId), "Id", "EmploymentStatusName", employeeDetailsVM.EmploymentStatusId);
      ViewData["Department"] = new SelectList(_context.Departments.Where(x => x.OrganisationId == orgId), "Id", "DepartmentName", employeeDetailsVM.DepartmentId);
      ViewData["JobCategory"] = new SelectList(_context.JobCategories.Where(x => x.OrganisationId == orgId), "Id", "JobCategoryName", employeeDetailsVM.JobCategoryId);
      ViewData["Branch"] = new SelectList(_context.Branches.Where(x => x.OrganisationId == orgId), "Id", "BranchName", employeeDetailsVM.BranchId);
      ViewData["PayGrade"] = new SelectList(_context.PayGrades.Where(x => x.OrganisationId == orgId), "Id", "PayGradeName", employeeDetailsVM.PayGradeId);

      return View(employeeDetailsVM);
        }

    [HttpPost]
    public async Task<IActionResult> SaveSalary([FromBody]PostSalary postSalary)
    {
      if (postSalary == null)
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

      var employeeSalary = _context.Salaries.FirstOrDefault(x => x.EmployeeDetailId == postSalary.EmployeeId);

      if (employeeSalary != null)
      {
        employeeSalary.OrganisationId = orgId;
        employeeSalary.PayFrequency = postSalary.PayFrequency;
        employeeSalary.Amount = postSalary.Amount;
        employeeSalary.Currency = postSalary.Currency;
        employeeSalary.PayGradeId = postSalary.PayGrade;
        employeeSalary.Comment = postSalary.Comments;

        _context.Update(employeeSalary);
        await _context.SaveChangesAsync();

        return Json(new
        {
          msg = "Success"
        });

      }


      try
      {
        var newEmployeeSalary = new Salary()
        {
          Id = Guid.NewGuid(),
          Amount = postSalary.Amount,
          OrganisationId = orgId,
          PayGradeId = postSalary.PayGrade,
          EmployeeDetailId = postSalary.EmployeeId,
          Currency = postSalary.Currency,
          PayFrequency = postSalary.PayFrequency,
          Comment = postSalary.Comments,
          IsActive = true
          
        };

        _context.Add(newEmployeeSalary);
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
    public async Task<IActionResult> SaveJob([FromBody]PostJob postJob)
    {
      if (postJob == null)
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

      var employeeJob = _context.Jobs.FirstOrDefault(x => x.EmployeeDetailId == postJob.EmployeeId);

      if (employeeJob != null)
      {
        employeeJob.OrganisationId = orgId;
        employeeJob.BranchId = postJob.BranchId;
        employeeJob.JobTitleId = postJob.JobTitleId;
        employeeJob.DepartmentId = postJob.DepartmentId;
        employeeJob.EmploymentStatusId = postJob.EmploymentStatusId;
        employeeJob.JobCategoryId = postJob.JobCategoryId;
        employeeJob.JoinedDate = postJob.JoinDate;
        employeeJob.StartDate = postJob.StartDate;
        employeeJob.EndDate = postJob.EndDate;
        employeeJob.ContractDetail = postJob.ContractDetails;

        _context.Update(employeeJob);
        await _context.SaveChangesAsync();

        return Json(new
        {
          msg = "Success"
        });

      }


      try
      {
        var newEmployeeJob = new Job()
        {
          Id = Guid.NewGuid(),
          OrganisationId = orgId,
          EmployeeDetailId = postJob.EmployeeId,
          BranchId = postJob.BranchId,
          ContractDetail = postJob.ContractDetails,
          DepartmentId = postJob.DepartmentId,
          EmploymentStatusId = postJob.EmploymentStatusId,
          EndDate = postJob.EndDate,
          JobCategoryId = postJob.JobCategoryId, 
          JobTitleId = postJob.JobTitleId,
          JoinedDate = postJob.JoinDate,
          StartDate = postJob.StartDate,

          IsActive = true

        };

        _context.Add(newEmployeeJob);
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

    [HttpGet]
    public async Task<IActionResult> EmployeeInfo (Guid id)
    {
      if (id == null)
      {
        return NotFound();
      }
      return View();
    }

        [HttpPost]
        public async Task<IActionResult> postNewEmployee([FromBody]PostNewEmployee postNewEmployee)
        {
      if (postNewEmployee == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      var orgId = getOrg();
      var organisationDetails = _context.Organisations.Where(x => x.Id == orgId).FirstOrDefault();
      int noOfEmployee = _context.Users.Where(x => x.OrganisationId == orgId).Count();

      try
      {
        var userId = Guid.NewGuid();
        EmployeeDetail newEmployee = new EmployeeDetail()
        {
          Id = Guid.NewGuid(),
          FirstName = postNewEmployee.FirstName,
          LastName = postNewEmployee.LastName,
          Email = postNewEmployee.PersonalEmail,
          PhoneNumber = postNewEmployee.PhoneNumber,
          EmployeeId = organisationDetails.OrganisationPrefix + (noOfEmployee + 1).ToString(),
          OrganisationId = orgId,
          UserId = userId
        };

        _context.Add(newEmployee);
        _context.SaveChanges();

        var user = new ApplicationUser
        {
          Id = userId.ToString(),
          UserName = newEmployee.Email,
          Email = newEmployee.Email,
          OrganisationId = orgId,
          PhoneNumber = newEmployee.PhoneNumber,
          UserRole = "Employee",
          EmployeeName = newEmployee.LastName + " " + newEmployee.FirstName,
        };

        var result = await _userManager.CreateAsync(user);
        if (result.Succeeded)
        {
          await _userManager.AddToRoleAsync(user, user.UserRole);

          var Email = user.Email;
          string Code = await _userManager.GeneratePasswordResetTokenAsync(user);
          var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, Code, Request.Scheme);

          var response = _emailSender.SendGridEmailAsync(user.Email, "Reset Password",
             $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");

          //var response = _emailSender.GmailSendEmail(user.Email, callbackUrl, user.UserRole);


          return Json(new
          {
            msg = "Success"
          }
         );


        }
      }
      catch(Exception ee)
      {

      }


      return Json(new
      {
        msg = "Fail"
      }
     );

    }
  }
}