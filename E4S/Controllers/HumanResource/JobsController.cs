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

namespace E4S.Controllers.HumanResource
{
    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager )
        {
            _context = context;
            _userManager = userManager;

        }

        private Guid getOrg()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var orgId = _context.Users.Where(x => x.Id == userId).FirstOrDefault().OrganisationId;

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
            var organisationDetails = _context.Organisations.Where(x => x.Id == orgId).FirstOrDefault();
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
            var organisationDetails = _context.Organisations.Where(x => x.Id == orgId).FirstOrDefault();
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

        public IActionResult JobCategory()
        {
          return View();
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
            var organisationDetails = _context.Organisations.Where(x => x.Id == orgId).FirstOrDefault();
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

        public IActionResult PayGrade()
        {           

            return View();
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
            var organisationDetails = _context.Organisations.Where(x => x.Id == orgId).FirstOrDefault();
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
    }
}