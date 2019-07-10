﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
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
using OfficeOpenXml;

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

      var orgdetails = _context.Organisations.Where(x => x.Id == orgId).FirstOrDefault();
      ViewData["OrganisationName"] = orgdetails.OrganisationName;
      ViewData["OrganisationImage"] = orgdetails.ImageUrl;

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
          var empDetails = _context.Jobs.Where(x => x.EmployeeDetailId == item.Id).Include(x => x.JobTitle).Include(x => x.Department).FirstOrDefault();
          singleEmployee = new EmployeeListViewModel()
          {
            Id = item.Id,
            EmployeeName = item.FirstName + " " + item.LastName,
            Email = item.Email,
            Department = empDetails.Department.DepartmentName,
            //EmployeeStatus = _context.EmploymentStatuses.Where(x => x.Id == empDetails.EmploymentStatusId).FirstOrDefault().EmploymentStatusName,
            JobTitle = empDetails.JobTitle.JobTitleName,
            Supervisor = "",
            IsActive = item.IsActive,
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
            //EmployeeStatus = "--Not Assigned--",
            JobTitle = "--Not Assigned--",
            Supervisor = "",
            IsActive = item.IsActive

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
      var orgId = getOrg();
      ViewData["JobTitle"] = new SelectList(_context.JobTitles.Where(x => x.OrganisationId == orgId), "Id", "JobTitleName");
      ViewData["EmploymentStatus"] = new SelectList(_context.EmploymentStatuses.Where(x => x.OrganisationId == orgId), "Id", "EmploymentStatusName");
      ViewData["Department"] = new SelectList(_context.Departments.Where(x => x.OrganisationId == orgId), "Id", "DepartmentName");
      ViewData["JobCategory"] = new SelectList(_context.JobCategories.Where(x => x.OrganisationId == orgId), "Id", "JobCategoryName");
      ViewData["Branch"] = new SelectList(_context.Branches.Where(x => x.OrganisationId == orgId), "Id", "BranchName");
      ViewData["PayGrade"] = new SelectList(_context.PayGrades.Where(x => x.OrganisationId == orgId), "Id", "PayGradeName");

      return View();
        }

    [HttpPost]
    public async Task<IActionResult> AddNewEmployee(IFormFile wizardpicture, PostNewEmployeeWizard postNewEmployee)
    {
      if(postNewEmployee == null)
      {
        return NotFound();
      }

      var orgId = getOrg();
      var organisationDetails = _context.Organisations.Where(x => x.Id == orgId).FirstOrDefault();
      int noOfEmployee = _context.Users.Where(x => x.OrganisationId == orgId).Count();
      var userId = Guid.NewGuid();

      EmployeeDetail newEmployee = new EmployeeDetail()
      {
        Id = Guid.NewGuid(),
        FirstName = postNewEmployee.FirstName,
        LastName = postNewEmployee.LastName,
        Email = postNewEmployee.Email,
        EmployeeId = organisationDetails.OrganisationPrefix + (noOfEmployee + 1).ToString(),
        OrganisationId = orgId,
        UserId = userId,
        DateOfBirth = postNewEmployee.DateOfBirth,
        MaritalStatus = postNewEmployee.MaritalStatus
      };

      //_context.Add(newEmployee);
      //_context.SaveChanges();

      var user = new ApplicationUser
      {
        Id = userId.ToString(),
        UserName = newEmployee.Email,
        Email = newEmployee.Email,
        OrganisationId = orgId,
        PhoneNumber = newEmployee.PhoneNumber,
        UserRole = "Employee",
        EmployeeName = newEmployee.LastName + " " + newEmployee.FirstName,
        FirstName = postNewEmployee.FirstName,
        LastName = postNewEmployee.LastName,
        OrganisationName = organisationDetails.OrganisationName
      };

      var result = await _userManager.CreateAsync(user);
      if (result.Succeeded)
      {
        await _userManager.AddToRoleAsync(user, user.UserRole);

        var Email = user.Email;
        string Code = await _userManager.GeneratePasswordResetTokenAsync(user);
        var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, Code, Request.Scheme);

        var response = _emailSender.SendGridEmailAsync(user.Email, "Set Your Password",
           callbackUrl, organisationDetails.OrganisationName, user.FirstName, "setPassword");


        _context.Add(newEmployee);
        _context.SaveChanges();

        Job employeeJon = new Job()
        {
          Id = Guid.NewGuid(),
          BranchId = postNewEmployee.Branch,
          JobCategoryId = postNewEmployee.JobCategory,
          JobTitleId = postNewEmployee.JobTitle,
          DepartmentId = postNewEmployee.Department,
          OrganisationId = orgId,
          EmployeeDetailId = newEmployee.Id,
          JoinedDate = postNewEmployee.JoinedDate,
          StartDate = postNewEmployee.StartDate,
          EmploymentStatusId = postNewEmployee.EmploymentStatus,

        };

        Salary employeeSal = new Salary()
        {
          Id = Guid.NewGuid(),
          Amount = postNewEmployee.Amount,
          PayFrequency = postNewEmployee.PayFrequency,
          OrganisationId = orgId,
          EmployeeDetailId = newEmployee.Id,
          BankName = postNewEmployee.BankName,
          AccountName = postNewEmployee.AccountName,
          AccountNo = postNewEmployee.AccountNo

        };



        try
        {
          _context.Add(employeeJon);
          _context.SaveChanges();

          _context.Add(employeeSal);
          _context.SaveChanges();
        }
        catch
        {

        }


      }
      else
      {
        return NotFound();
      }


      return RedirectToAction("Index");
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

            var response = _emailSender.SendGridEmailAsync(user.Email, "Set Password",
               callbackUrl, organisationDetails.OrganisationName, user.FirstName, "setPassword");

            //var response = _emailSender.GmailSendEmail(user.Email, callbackUrl, user.UserRole);

            _context.Add(empDetail);
            _context.SaveChanges();


          }


        }
        }
      return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> UploadEmployeeCSV(IFormFile file, CancellationToken cancellationToken)
    {
      if (file == null || file.Length == 0)
        return Content("file not selected");

      var path = Path.Combine(
                  Directory.GetCurrentDirectory(), "wwwroot",
                  file.FileName);

      var orgId = getOrg();
      var organisationDetails = _context.Organisations.Where(x => x.Id == orgId).FirstOrDefault();

      if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
      {
        return Json("File not supported");
      }
      List<NewEmployeeImport> newEmployeeList = new List<NewEmployeeImport>();
      NewEmployeeImport newEmployee;

      using (var strea = new MemoryStream())
      {
        await file.CopyToAsync(strea, cancellationToken);

        using (var package = new ExcelPackage(strea))
        {
          ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
          var rowCount = workSheet.Dimension.Rows;

          for (int row = 2; row <= rowCount; row++)
          {
            try
            {
              newEmployee = new NewEmployeeImport()
              {
                FirstName = workSheet.Cells[row, 1].Value.ToString(),
                LastName = workSheet.Cells[row, 2].Value.ToString(),
                EmployeeEmail = workSheet.Cells[row, 3].Value.ToString(),
                Gender = workSheet.Cells[row, 4].Value.ToString(),
                MaritalStatus = workSheet.Cells[row, 5].Value.ToString(),
                JobTitle = workSheet.Cells[row, 6].Value.ToString(),
                Department = workSheet.Cells[row, 7].Value.ToString(),
                PayFrequency = workSheet.Cells[row, 8].Value.ToString(),
                Amount = float.Parse(workSheet.Cells[row, 9].Value.ToString())
              };

            }
            catch
            {
              newEmployee = new NewEmployeeImport()
              {
                FirstName = workSheet.Cells[row, 1].Value.ToString(),
                LastName = workSheet.Cells[row, 2].Value.ToString(),
                EmployeeEmail = workSheet.Cells[row, 3].Value.ToString(),
                Gender = workSheet.Cells[row, 4].Value.ToString(),
                MaritalStatus = workSheet.Cells[row, 5].Value.ToString(),
                JobTitle = workSheet.Cells[row, 6].Value.ToString(),
                Department = workSheet.Cells[row, 7].Value.ToString(),
              };

              }

            newEmployeeList.Add(newEmployee);

          }

        }
      }





      List<NewEmployeeImport> successfullyEmployeeList = new List<NewEmployeeImport>();
      List<NewEmployeeImport> unsuccessfullyEmployeeList = new List<NewEmployeeImport>();
      EmployeeDetail empDetail;
      //var organisationDetails = _context.Organisations.Where(x => x.Id == orgId).FirstOrDefault();

      foreach (var item in newEmployeeList)
      {
        int noOfEmployee = _context.Users.Where(x => x.OrganisationId == orgId).Count();
        var userId = Guid.NewGuid();

        empDetail = new EmployeeDetail()
        {
          Id = Guid.NewGuid(),
          FirstName = item.FirstName,
          LastName = item.LastName,
          Email = item.EmployeeEmail,
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
          FirstName = empDetail.FirstName,
          LastName = empDetail.LastName
        };

        var result = await _userManager.CreateAsync(user);

        if (result.Succeeded)
        {
          await _userManager.AddToRoleAsync(user, user.UserRole);

          //var Email = user.Email;
          string Code = await _userManager.GeneratePasswordResetTokenAsync(user);
          var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, Code, Request.Scheme);

          var response = _emailSender.SendGridEmailAsync(user.Email, "Create Password", callbackUrl, organisationDetails.OrganisationName, user.EmployeeName, "setPassword");

          //var response = _emailSender.GmailSendEmail(user.Email, callbackUrl, user.UserRole);

          _context.Add(empDetail);
          _context.SaveChanges();

          successfullyEmployeeList.Add(item);


          // Job Creation
          Job employeeJob = new Job();

          if (item.JobTitle != null || item.JobTitle != "")
          {

            var isJobTitle = _context.JobTitles.Where(x => x.OrganisationId == orgId).Where(x => x.JobTitleName.ToLower() == item.JobTitle.ToLower()).FirstOrDefault();
            if (isJobTitle == null)
            {
              JobTitle newJobTitle = new JobTitle()
              {
                OrganisationId = orgId,
                JobTitleName = item.JobTitle,
                Id = Guid.NewGuid()

              };

              _context.Add(newJobTitle);
              _context.SaveChanges();

              employeeJob.JobTitleId = newJobTitle.Id;
            }
            else
            {
              employeeJob.JobTitleId = isJobTitle.Id;
            }
          }

          if (item.Department != null || item.Department != "")
          {
            var isDepartment = _context.Departments.Where(x => x.OrganisationId == orgId).Where(x => x.DepartmentName.ToLower() == item.Department.ToLower()).FirstOrDefault();
            if (isDepartment == null)
            {
              Department newDepartment = new Department()
              {
                OrganisationId = orgId,
                DepartmentName = item.Department,
                Id = Guid.NewGuid()

              };

              _context.Add(newDepartment);
              _context.SaveChanges();

              employeeJob.DepartmentId = newDepartment.Id;
            }
            else
            {
              employeeJob.DepartmentId = isDepartment.Id;

            }
          }

          employeeJob.Id = Guid.NewGuid();
          employeeJob.OrganisationId = orgId;
          employeeJob.EmployeeDetailId = empDetail.Id;
          try
          {
            _context.Add(employeeJob);
            _context.SaveChanges();
          }
          catch
          {

          }


          Salary empSalary = new Salary()
          {
            PayFrequency = item.PayFrequency,
            Amount = item.Amount,
            Id = Guid.NewGuid(),
            EmployeeDetailId = empDetail.Id,
            OrganisationId = orgId,

          };

          try
          {
            _context.Add(empSalary);
            _context.SaveChanges();
          }
          catch
          {

          }

        }
        else
        {
          unsuccessfullyEmployeeList.Add(item);

        }


      }




      // Tolu Code
      //using (var stream = new FileStream(path, FileMode.Create))
      //{
      //  await file.CopyToAsync(stream);
      //}


      //string csvData = System.IO.File.ReadAllText(path);
      //List<NewEmployeeImport> newEmployeeList = new List<NewEmployeeImport>();


      //var ep = new ExcelPackage(new FileInfo(path));
      //var ws = ep.Workbook.Worksheets[0];

      //for (int rw = 2; rw <= ws.Dimension.End.Row; rw++)
      //{


      //  if (ws.Cells[rw, 1].Value != null)
      //  {
      //    try
      //    {
      //      newEmployee = new NewEmployeeImport()
      //      {
      //        FirstName = ws.Cells[rw, 1].Value.ToString(),
      //        MiddleName = ws.Cells[rw, 2].Value.ToString(),
      //        LastName = ws.Cells[rw, 3].Value.ToString(),
      //        EmployeeEmail = ws.Cells[rw, 4].Value.ToString(),
      //        EmployeeId = ws.Cells[rw, 5].Value.ToString(),
      //        Gender = ws.Cells[rw, 6].Value.ToString(),
      //        MaritalStatus = ws.Cells[rw, 7].Value.ToString(),
      //        ContactAddress = ws.Cells[rw, 8].Value.ToString(),
      //        DateOfBirth = DateTime.Parse(ws.Cells[rw, 9].Value.ToString()),
      //        JobTitle = ws.Cells[rw, 10].Value.ToString(),
      //        Department = ws.Cells[rw, 11].Value.ToString(),
      //        EmploymentStatus = ws.Cells[rw, 12].Value.ToString(),
      //        JobCategory = ws.Cells[rw, 13].Value.ToString(),
      //        Branch = ws.Cells[rw, 15].Value.ToString(),
      //        JoinedDate = DateTime.Parse(ws.Cells[rw, 14].Value.ToString()),
      //        StartDate = DateTime.Parse(ws.Cells[rw, 16].Value.ToString()),
      //        EndDate = DateTime.Parse(ws.Cells[rw, 17].Value.ToString()),
      //        ContractDetail = ws.Cells[rw, 18].Value.ToString(),
      //        PayGrade = ws.Cells[rw, 19].Value.ToString(),
      //        PayFrequency = ws.Cells[rw, 20].Value.ToString(),
      //        Amount = float.Parse(ws.Cells[rw, 21].Value.ToString()),
      //        Currency = ws.Cells[rw, 22].Value.ToString(),
      //        Comments = ws.Cells[rw, 23].Value.ToString(),
      //        BankName = ws.Cells[rw, 24].Value.ToString(),
      //        AccointNo = ws.Cells[rw, 25].Value.ToString(),
      //        AccountName = ws.Cells[rw, 26].Value.ToString(),
      //      };

      //    }
      //    catch
      //    {
      //      newEmployee = new NewEmployeeImport()
      //      {
      //        FirstName = ws.Cells[rw, 1].Value.ToString(),
      //        MiddleName = ws.Cells[rw, 2].Value.ToString(),
      //        LastName = ws.Cells[rw, 3].Value.ToString(),
      //        EmployeeEmail = ws.Cells[rw, 4].Value.ToString(),
      //        EmployeeId = ws.Cells[rw, 5].Value.ToString(),
      //        Gender = ws.Cells[rw, 6].Value.ToString(),
      //        MaritalStatus = ws.Cells[rw, 7].Value.ToString(),
      //        ContactAddress = ws.Cells[rw, 8].Value.ToString(),
      //        //DateOfBirth = DateTime.Parse(ws.Cells[rw, 9].Value.ToString()),
      //        JobTitle = ws.Cells[rw, 10].Value.ToString(),
      //        Department = ws.Cells[rw, 11].Value.ToString(),
      //        EmploymentStatus = ws.Cells[rw, 12].Value.ToString(),
      //        JobCategory = ws.Cells[rw, 13].Value.ToString(),
      //        Branch = ws.Cells[rw, 15].Value.ToString(),
      //        //JoinedDate = DateTime.Parse(ws.Cells[rw, 14].Value.ToString()),
      //        //StartDate = DateTime.Parse(ws.Cells[rw, 16].Value.ToString()),
      //        //EndDate = DateTime.Parse(ws.Cells[rw, 17].Value.ToString()),
      //        ContractDetail = ws.Cells[rw, 18].Value.ToString(),
      //        PayGrade = ws.Cells[rw, 19].Value.ToString(),
      //        PayFrequency = ws.Cells[rw, 20].Value.ToString(),
      //        //Amount = float.Parse(ws.Cells[rw, 21].Value.ToString()),
      //        Currency = ws.Cells[rw, 22].Value.ToString(),
      //        Comments = ws.Cells[rw, 23].Value.ToString(),
      //        BankName = ws.Cells[rw, 24].Value.ToString(),
      //        AccointNo = ws.Cells[rw, 25].Value.ToString(),
      //        AccountName = ws.Cells[rw, 26].Value.ToString(),
      //      };

      //    }

      //    newEmployeeList.Add(newEmployee);

      //  }

      //}






      // Old Code

      //Execute a loop over the rows.
      //foreach (string row in csvData.Split("\r\n"))
      //{
      //  if (!string.IsNullOrEmpty(row))
      //  {
      //    userId = Guid.NewGuid();
      //    int noOfEmployee = _context.Users.Where(x => x.OrganisationId == orgId).Count();
      //    NewEmployeeImport newEmployee ;

      //    try
      //    {
      //      newEmployee = new NewEmployeeImport()
      //      {
      //        FirstName = row.Split(',')[0],
      //        MiddleName = row.Split(',')[1],
      //        LastName = row.Split(',')[2],
      //        EmployeeEmail = row.Split(',')[3],
      //        EmployeeId = row.Split(',')[4],
      //        Gender = row.Split(',')[5],
      //        MaritalStatus = row.Split(',')[6],
      //        ContactAddress = row.Split(',')[7],
      //        DateOfBirth = DateTime.Parse(row.Split(',')[8]),
      //        JobTitle = row.Split(',')[9],
      //        Department = row.Split(',')[10],
      //        EmploymentStatus = row.Split(',')[11],
      //        JobCategory = row.Split(',')[12],
      //        Branch = row.Split(',')[14],
      //        JoinedDate = DateTime.Parse(row.Split(',')[13]),
      //        StartDate = DateTime.Parse(row.Split(',')[15]),
      //        EndDate = DateTime.Parse(row.Split(',')[16]),
      //        ContractDetail = row.Split(',')[17],
      //        PayGrade = row.Split(',')[18],
      //        PayFrequency = row.Split(',')[19],
      //        Amount = float.Parse(row.Split(',')[20]),
      //        Currency = row.Split(',')[21],
      //        Comments = row.Split(',')[22],
      //        BankName = row.Split(',')[23],
      //        AccointNo = row.Split(',')[24],
      //        AccountName = row.Split(',')[25],
      //      };

      //    }
      //    catch
      //    {
      //      newEmployee = new NewEmployeeImport()
      //      {
      //        FirstName = row.Split(',')[0],
      //        MiddleName = row.Split(',')[1],
      //        LastName = row.Split(',')[2],
      //        EmployeeEmail = row.Split(',')[3],
      //        EmployeeId = row.Split(',')[4],
      //        Gender = row.Split(',')[5],
      //        MaritalStatus = row.Split(',')[6],
      //        ContactAddress = row.Split(',')[7],
      //        //DateOfBirth = DateTime.Parse(row.Split(',')[8]),
      //        JobTitle = row.Split(',')[9],
      //        Department = row.Split(',')[10],
      //        EmploymentStatus = row.Split(',')[11],
      //        JobCategory = row.Split(',')[12],
      //        Branch = row.Split(',')[14],
      //        //JoinedDate = DateTime.Parse(row.Split(',')[13]),
      //        //StartDate = DateTime.Parse(row.Split(',')[15]),
      //        //EndDate = DateTime.Parse(row.Split(',')[16]),
      //        ContractDetail = row.Split(',')[17],
      //        PayGrade = row.Split(',')[18],
      //        PayFrequency = row.Split(',')[19],
      //        //Amount = float.Parse(row.Split(',')[20]),
      //        Currency = row.Split(',')[21],
      //        Comments = row.Split(',')[22],
      //        BankName = row.Split(',')[23],
      //        AccointNo = row.Split(',')[24],
      //        AccountName = row.Split(',')[25],
      //      };

      //    }


      //    if(newEmployee.FirstName != "FIRSTNAME")
      //    {
      //      newEmployeeList.Add(newEmployee);
      //    }


      //    //EmployeeDetail empDetail = new EmployeeDetail()
      //    //{
      //    //  Id = Guid.NewGuid(),
      //    //  FirstName = row.Split(',')[0],
      //    //  LastName = row.Split(',')[1],
      //    //  Email = row.Split(',')[2],
      //    //  OrganisationId = orgId,
      //    //  EmployeeId = organisationDetails.OrganisationPrefix + (noOfEmployee + 1).ToString(),
      //    //  UserId = userId
      //    //};

      //    //var user = new ApplicationUser
      //    //{
      //    //  Id = userId.ToString(),
      //    //  UserName = empDetail.Email,
      //    //  Email = empDetail.Email,
      //    //  OrganisationId = orgId,
      //    //  UserRole = "Employee",
      //    //  EmployeeName = empDetail.LastName + " " + empDetail.FirstName,
      //    //};

      //    //var result = await _userManager.CreateAsync(user);
      //    //if (result.Succeeded)
      //    //{
      //    //  await _userManager.AddToRoleAsync(user, user.UserRole);

      //    //  var Email = user.Email;
      //    //  string Code = await _userManager.GeneratePasswordResetTokenAsync(user);
      //    //  var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, Code, Request.Scheme);

      //    //  var response = _emailSender.SendGridEmailAsync(user.Email, "Set Password",
      //    //     callbackUrl, user.FirstName, "setPassword");

      //    //  //var response = _emailSender.GmailSendEmail(user.Email, callbackUrl, user.UserRole);

      //    //  _context.Add(empDetail);
      //    //  _context.SaveChanges();


      //    //}


      //  }
      //}

      //try
      //{
      //  System.IO.File.Delete(path);
      //}
      //catch
      //{

      //}

      //var unsuccessful = SaveEmployeeDetailsAsync(newEmployeeList);

      return View(newEmployeeList);
    }

    private async Task<List<NewEmployeeImport>> SaveEmployeeDetailsAsync(List<NewEmployeeImport> newEmployeeList)
    {
      var orgId = getOrg();

      List<NewEmployeeImport> successfullyEmployeeList = new List<NewEmployeeImport>();
      List<NewEmployeeImport> unsuccessfullyEmployeeList = new List<NewEmployeeImport>();
      EmployeeDetail empDetail;
      var organisationDetails = _context.Organisations.Where(x => x.Id == orgId).FirstOrDefault();

      foreach (var item in newEmployeeList)
      {
        int noOfEmployee = _context.Users.Where(x => x.OrganisationId == orgId).Count();
        var userId = Guid.NewGuid();

        empDetail = new EmployeeDetail()
        {
          Id = Guid.NewGuid(),
          FirstName = item.FirstName,
          LastName = item.LastName,
          Email = item.EmployeeEmail,
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
          FirstName = empDetail.FirstName,
          LastName = empDetail.LastName
        };

        var result = await _userManager.CreateAsync(user);

        if (result.Succeeded)
        {
          await _userManager.AddToRoleAsync(user, user.UserRole);

          var Email = user.Email;
          string Code = await _userManager.GeneratePasswordResetTokenAsync(user);
          var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, Code, Request.Scheme);


          //var response = _emailSender.GmailSendEmail(user.Email, callbackUrl, user.UserRole);

          _context.Add(empDetail);
          _context.SaveChanges();

          successfullyEmployeeList.Add(item);

          Job employeeJob = new Job();

          if (item.JobTitle != null || item.JobTitle != "")
          {

            var isJobTitle = _context.JobTitles.Where(x => x.OrganisationId == orgId).Where(x => x.JobTitleName.ToLower() == item.JobTitle.ToLower()).FirstOrDefault();
            if(isJobTitle == null)
            {
              JobTitle newJobTitle = new JobTitle()
              {
                OrganisationId = orgId,
                JobTitleName = item.JobTitle,
                Id = Guid.NewGuid()

              };

              _context.Add(newJobTitle);
              _context.SaveChanges();

              employeeJob.JobTitleId = newJobTitle.Id;
            }
            else
            {
              employeeJob.JobTitleId = isJobTitle.Id;
            }
          }

          if (item.Department != null || item.Department != "")
          {
            var isDepartment = _context.Departments.Where(x => x.OrganisationId == orgId).Where(x => x.DepartmentName.ToLower() == item.Department.ToLower()).FirstOrDefault();
            if (isDepartment == null)
            {
              Department newDepartment = new Department()
              {
                OrganisationId = orgId,
                DepartmentName = item.Department,
                Id = Guid.NewGuid()

              };

              _context.Add(newDepartment);
              _context.SaveChanges();

              employeeJob.DepartmentId = newDepartment.Id;
            }
            else
            {
              employeeJob.DepartmentId = isDepartment.Id;

            }
          }


          employeeJob.Id = Guid.NewGuid();
          employeeJob.OrganisationId = orgId;
          employeeJob.EmployeeDetailId = empDetail.Id;
          //employeeJob.StartDate = item.StartDate;
          //employeeJob.JoinedDate = item.JoinedDate;
          //employeeJob.EndDate = item.EndDate;

          try
          {
            _context.Add(employeeJob);
            _context.SaveChanges();
          }
          catch
          {

          }

          //Salary empSalary = new Salary()
          //{
          //  PayFrequency = item.PayFrequency,
          //  Amount = item.Amount,
          //  Currency = item.Currency,
          //  Id = Guid.NewGuid(),
          //  EmployeeDetailId = empDetail.Id,
          //  OrganisationId = orgId,
          //  AccountName = item.AccountName,
          //  BankName = item.BankName,
          //  AccountNo = item.AccointNo

          //};

          //try
          //{
          //  _context.Add(empSalary);
          //  _context.SaveChanges();
          //}
          //catch
          //{

          //}

        //  await _emailSender.SendGridEmailAsync(user.Email, "Set Password", callbackUrl, organisationDetails.OrganisationName, user.FirstName, "setPassword");




        }
        else
        {
          unsuccessfullyEmployeeList.Add(item);
          
        }


      }


















      return unsuccessfullyEmployeeList;
    }


    public IActionResult CheckData()
    {

      return View();
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
        //employeeDetailsVM.PayGradeId = salaryEmployee.PayGradeId;
        employeeDetailsVM.PayFrequency = salaryEmployee.PayFrequency;
        employeeDetailsVM.Comments = salaryEmployee.Comment;
        employeeDetailsVM.Currency = salaryEmployee.Currency;
        employeeDetailsVM.AccountNo = salaryEmployee.AccountNo;
        employeeDetailsVM.BankName = salaryEmployee.BankName;
        employeeDetailsVM.AccountName = salaryEmployee.AccountName;
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
    public async Task<IActionResult> UpdatePersonalDetail([FromBody]PostPersonalDetail postPersonalDetail)
    {
      if (postPersonalDetail == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      var orgId = getOrg();
      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var employeeDetails = _context.EmployeeDetails.Where(x => x.Id == postPersonalDetail.Id).FirstOrDefault();


      if (employeeDetails != null)
      {
        try
        {
          employeeDetails.FirstName = postPersonalDetail.FirstName;
          employeeDetails.MiddleName = postPersonalDetail.MiddleName;
          employeeDetails.LastName = postPersonalDetail.LastName;
          employeeDetails.OtherId = postPersonalDetail.OtherId;
          employeeDetails.EmployeeId = postPersonalDetail.EmployeeId;
          employeeDetails.Gender = postPersonalDetail.Gender;
          employeeDetails.MaritalStatus = postPersonalDetail.MaritalStatus;
          employeeDetails.DateOfBirth = postPersonalDetail.DateofBirth;
          employeeDetails.PhoneNumber = postPersonalDetail.Telephone;

          _context.Update(employeeDetails);
          await _context.SaveChangesAsync();

          return Json(new
          {
            msg = "Success"
          });

        }
        catch
        {

        }

      }


     // try
     // {
     //   var newEmployeeSalary = new Salary()
     //   {
     //     Id = Guid.NewGuid(),
     //     Amount = postSalary.Amount,
     //     OrganisationId = orgId,
     //     EmployeeDetailId = postSalary.EmployeeId,
     //     Currency = postSalary.Currency,
     //     PayFrequency = postSalary.PayFrequency,
     //     Comment = postSalary.Comments,
     //     IsActive = true

     //   };

     //   _context.Add(newEmployeeSalary);
     //   _context.SaveChanges();


     //   return Json(new
     //   {
     //     msg = "Success"
     //   }
     //);
     // }
     // catch (Exception ee)
     // {

     // }

      return Json(
      new
      {
        msg = "Fail"
      });
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
        //employeeSalary.PayGradeId = postSalary.PayGrade;
        employeeSalary.Comment = postSalary.Comments;
        employeeSalary.BankName = postSalary.BankName;
        employeeSalary.AccountName = postSalary.AccountName;
        employeeSalary.AccountNo = postSalary.AccountNo;


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
          EmployeeDetailId = postSalary.EmployeeId,
          Currency = postSalary.Currency,
          PayFrequency = postSalary.PayFrequency,
          Comment = postSalary.Comments,
          IsActive = true,
          BankName = postSalary.BankName,
          AccountName = postSalary.AccountName,
          AccountNo = postSalary.AccountNo,

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

          var response = _emailSender.SendGridEmailAsync(user.Email, "Set Password",
             callbackUrl, organisationDetails.OrganisationName, user.FirstName, "setPassword");

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