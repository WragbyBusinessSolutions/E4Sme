using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using E4S.Data;
using E4S.Models;
using E4S.Models.HumanResource;
using E4S.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E4S.Controllers.HumanResource
{
  public class JobsController : Controller
  {
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

   

    public JobsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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
      return View();
    }

    public IActionResult JobTitle()
    {
      var orgId = getOrg();
      var jobTitles = _context.JobTitles.Where(x => x.OrganisationId == orgId).ToList();

            return View(jobTitles);
    }

    [HttpPost]
    public async Task<IActionResult> PostNewJobTitle([FromBody]PostNewJobTitle postNewJobTitle)
    {
      if (postNewJobTitle == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      var orgId = getOrg();
      var organisationDetails = await _context.Organisations.Where(x => x.Id == orgId).FirstOrDefaultAsync();
      int noOfEmployee = _context.Users.Where(x => x.OrganisationId == orgId).Count();

      try
      {
        JobTitle newJobTitle = new JobTitle()
        {
          Id = Guid.NewGuid(),
          JobTitleName = postNewJobTitle.JobTitle,
          Description = postNewJobTitle.Description,
          Note = postNewJobTitle.Note,
          OrganisationId = orgId
        };

        _context.Add(newJobTitle);
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

        // Edit the Job Title

        [HttpPost]
        public async Task<IActionResult> editJobTitle([FromBody]PostNewJobTitle postNewJobTitle)
        {
            if (postNewJobTitle == null)
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

                var orgJobTitle = _context.JobTitles.Where(x => x.Id == Guid.Parse(postNewJobTitle.AId)).FirstOrDefault();
                orgJobTitle.JobTitleName = postNewJobTitle.JobTitle;
                
                _context.Update(orgJobTitle);
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



    // Ednf of Edit for Job Title

    [HttpPost]
    public async Task<IActionResult> DelJobTitle([FromBody]string jobId)
    {
      if (jobId == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }


      //var orgId = getOrg();
      //var organisationDetails = await _context.Organisations.Where(x => x.Id == orgId).FirstOrDefaultAsync();


      //bool isAssign = true;

      //if (postNewDepartment. == Guid.Empty)
      //{
      //    isAssign = false;
      //}

      try
      {

        //var orgJobTitle = _context.JobTitles.Where(x => x.Id == Guid.Parse(postNewJobTitle.AId)).FirstOrDefault();
        //orgJobTitle.JobTitleName = postNewJobTitle.JobTitle;

        var jobTitle = await _context.JobTitles.FindAsync(Guid.Parse(jobId));
        _context.JobTitles.Remove(jobTitle);
        await _context.SaveChangesAsync();


        return Json(new
        {
          msg = "Success"
        });
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



    public IActionResult EmploymentStatus()
    {
      var orgId = getOrg();
      var employmentStatuses = _context.EmploymentStatuses.Where(x => x.OrganisationId == orgId).ToList();

      return View(employmentStatuses);
    }

    [HttpPost]
    public async Task<IActionResult> PostNewEmploymentStatus([FromBody]PostNewEmploymentStatus postNewEmploymentStatus)
    {
      if (postNewEmploymentStatus == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      var orgId = getOrg();
      var organisationDetails = await _context.Organisations.Where(x => x.Id == orgId).FirstOrDefaultAsync();
      int noOfEmployee = _context.Users.Where(x => x.OrganisationId == orgId).Count();

      try
      {
        EmploymentStatus newEmploymentStatus = new EmploymentStatus()
        {
          Id = Guid.NewGuid(),
          EmploymentStatusName = postNewEmploymentStatus.EmploymentStatusName,
          Description = postNewEmploymentStatus.Description,
          OrganisationId = orgId
        };

        _context.Add(newEmploymentStatus);
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


        // Edit the Department

        [HttpPost]
        public async Task<IActionResult> editEmploymentStatus([FromBody]PostNewEmploymentStatus postNewEmploymentStatus)
        {
            if (postNewEmploymentStatus == null)
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

                var orgEmployType = _context.EmploymentStatuses.Where(x => x.Id == Guid.Parse(postNewEmploymentStatus.AId)).FirstOrDefault();
                orgEmployType.EmploymentStatusName = postNewEmploymentStatus.EmploymentStatusName;
                orgEmployType.Description = postNewEmploymentStatus.Description;


                _context.Update(orgEmployType);
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

        // Ednf of Edit for Department


        public IActionResult JobCategory()
    {
      var orgId = getOrg();
      var jobCategory = _context.JobCategories.Where(x => x.OrganisationId == orgId).ToList();

      return View(jobCategory);
    }

    [HttpPost]
    public async Task<IActionResult> PostNewJobCategory([FromBody]PostNewJobCategory postNewJobCategory)
    {
      if (postNewJobCategory == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      var orgId = getOrg();
      var organisationDetails = await _context.Organisations.Where(x => x.Id == orgId).FirstOrDefaultAsync();
      int noOfEmployee = _context.Users.Where(x => x.OrganisationId == orgId).Count();

      try
      {
        JobCategory newJobCategory = new JobCategory()
        {
          Id = Guid.NewGuid(),
          JobCategoryName = postNewJobCategory.JobCategory,
          Description = postNewJobCategory.Description,
          OrganisationId = orgId
        };

        _context.Add(newJobCategory);
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


        // Edit the Job Cartegory

        [HttpPost]
        public async Task<IActionResult> editJobCategory([FromBody]PostNewJobCategory postNewJobCategory)
        {
            if (postNewJobCategory == null)
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

                var orgJobCat = _context.JobCategories.Where(x => x.Id == Guid.Parse(postNewJobCategory.AId)).FirstOrDefault();
                orgJobCat.JobCategoryName = postNewJobCategory.JobCategory;
                orgJobCat.Description = postNewJobCategory.Description;


                _context.Update(orgJobCat);
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

        // Ednf of Edit for Category


        // Delete of Edit for Category
        [HttpPost]
        public JsonResult Delete(int Id)
        {
            try
            {
                //ClientManagement cdb = new ClientManagement();
                //if (cdb.DeleteClient(id))
                //{
                //    TempData["Message"] = "Client Deleted Successfully";
                //}
                //Session.Abandon();
                return Json("Delete");
            }
            catch
            {
                return Json("Error");
            }

        }

            // Delete of Edit for Category



        public IActionResult PayGrade()
        {
            var orgId = getOrg();
            var payGrades = _context.PayGrades.Where(x => x.OrganisationId == orgId).ToList();

            return View(payGrades);
        }

    [HttpPost]
    public async Task<IActionResult> PostNewPayGrade([FromBody]PostNewPayGrade postNewPayGrade)
    {
      if (postNewPayGrade == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      var orgId = getOrg();
      var organisationDetails = await _context.Organisations.Where(x => x.Id == orgId).FirstOrDefaultAsync();
      int noOfEmployee = _context.Users.Where(x => x.OrganisationId == orgId).Count();

      try
      {
        PayGrade newPayGrade = new PayGrade()
        {
          Id = Guid.NewGuid(),
          PayGradeName = postNewPayGrade.PayGradeName,
          Currency = postNewPayGrade.Currency,
          MinimumSalary = postNewPayGrade.MinimumSalary,
          MaximumSalary = postNewPayGrade.MaximumSalary,
          OrganisationId = orgId
        };

        _context.Add(newPayGrade);
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

    public IActionResult Department()
    {
      var orgId = getOrg();
      var department = _context.Departments.Where(x => x.OrganisationId == orgId).ToList();

      return View(department);
    }

    [HttpPost]
    public async Task<IActionResult> Department([FromBody]PostNewDepartment postNewDepartment)
    {
      if (postNewDepartment == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      var orgId = getOrg();
      var organisationDetails = await _context.Organisations.Where(x => x.Id == orgId).FirstOrDefaultAsync();
      int noOfEmployee = _context.Users.Where(x => x.OrganisationId == orgId).Count();

      try
      {
        Department newDepartment = new Department()
        {
          Id = Guid.NewGuid(),
          DepartmentName = postNewDepartment.DepartmentName,
          Description = postNewDepartment.Description,
          OrganisationId = orgId
        };

        _context.Add(newDepartment);
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


        // Edit the Department

        [HttpPost]
        public async Task<IActionResult> editDepartment([FromBody]PostNewDepartment postNewDepartment)
        {
            if (postNewDepartment == null)
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

                var orgDept = _context.Departments.Where(x => x.Id == Guid.Parse(postNewDepartment.AId)).FirstOrDefault();
                orgDept.DepartmentName = postNewDepartment.DepartmentName;
                orgDept.Description = postNewDepartment.Description;
               

                _context.Update(orgDept);
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

        // Ednf of Edit for Department



        public IActionResult LeaveConfiguration()
    {
      var orgId = getOrg();
      var LeaveConfiguration = _context.LeaveConfigurations.Where(x => x.OrganisationId == orgId).ToList();

      return View(LeaveConfiguration);
    }

    //[HttpPost]
    //public async Task<IActionResult> LeaveConfiguration(LeaveConfiguration leaveConfiguration)
    //{
    //  var orgId = getOrg();

    //  if (leaveConfiguration != null)
    //  {
    //    leaveConfiguration.OrganisationId = orgId;
    //    leaveConfiguration.Id = Guid.NewGuid();

    //    _context.Add(leaveConfiguration);
    //    await _context.SaveChangesAsync();

    //    return RedirectToAction("LeaveConfiguration");

    //  }


    //  return RedirectToAction("LeaveConfiguration");
    //}

  
  [HttpPost]
  public async Task<IActionResult> LeaveConfiguration([FromBody]PostNewLeaveConfiguration postNewLeaveConfiguration)
  {
    if (postNewLeaveConfiguration == null)
    {
      return Json(new
      {
        msg = "No Data"
      }
     );
    }

    var orgId = getOrg();
    var organisationDetails = await _context.Organisations.Where(x => x.Id == orgId).FirstOrDefaultAsync();
    int noOfEmployee = _context.Users.Where(x => x.OrganisationId == orgId).Count();

    try
    {
      LeaveConfiguration newLeaveConfiguration = new LeaveConfiguration()
      {
        Id = Guid.NewGuid(),
        LeaveTitle = postNewLeaveConfiguration.LeaveTitle,
        Description = postNewLeaveConfiguration.Description,
        MaxDuration = postNewLeaveConfiguration.MaxDuration,
        MaxApplication = postNewLeaveConfiguration.MaxApplication,
        OrganisationId = orgId
      };

      _context.Add(newLeaveConfiguration);
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
        // Edit the LeaveConfiguration

        [HttpPost]
        public async Task<IActionResult> editLeaveConfiguration([FromBody]PostNewLeaveConfiguration postNewLeaveConfiguration)
        {
            if (postNewLeaveConfiguration == null)
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

                var orgLeaveCon = _context.LeaveConfigurations.Where(x => x.Id == Guid.Parse(postNewLeaveConfiguration.AId)).FirstOrDefault();
                orgLeaveCon.LeaveTitle = postNewLeaveConfiguration.LeaveTitle;
                orgLeaveCon.MaxDuration = postNewLeaveConfiguration.MaxDuration;
                orgLeaveCon.Description = postNewLeaveConfiguration.Description;


                _context.Update(orgLeaveCon);
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

    // Ednf of Edit for Leave Configuration

    [HttpPost]
    public async Task<IActionResult> DelLeaveConfig([FromBody]string LeaveId)
    {
      if (LeaveId == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }


      //var orgId = getOrg();
      //var organisationDetails = await _context.Organisations.Where(x => x.Id == orgId).FirstOrDefaultAsync();


      //bool isAssign = true;

      //if (postNewDepartment. == Guid.Empty)
      //{
      //    isAssign = false;
      //}

      try
      {

        //var orgJobTitle = _context.JobTitles.Where(x => x.Id == Guid.Parse(postNewJobTitle.AId)).FirstOrDefault();
        //orgJobTitle.JobTitleName = postNewJobTitle.JobTitle;

        var LeaveTitle = await _context.LeaveConfigurations.FindAsync(Guid.Parse(LeaveId));
        _context.LeaveConfigurations.Remove(LeaveTitle);
        var LeaveDuration = await _context.LeaveConfigurations.FindAsync(Guid.Parse(LeaveId));
        _context.LeaveConfigurations.Remove(LeaveDuration);
        var description = await _context.LeaveConfigurations.FindAsync(Guid.Parse(LeaveId));
        _context.LeaveConfigurations.Remove(description);


        await _context.SaveChangesAsync();


        return Json(new
        {
          msg = "Success"
        });
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
