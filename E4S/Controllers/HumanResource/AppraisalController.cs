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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace E4S.Controllers.HumanResource
{
  [Authorize]
    public class AppraisalController : Controller
    {
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AppraisalController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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
      var orgId = getOrg();

      var appraisal = _context.Appraisals.Where(x => x.OrganisationId == orgId).ToList();

      return View(appraisal);
        }


        public IActionResult AppraisalReport()
        {
      var orgId = getOrg();

      var appraisal = _context.Appraisals.Where(x => x.OrganisationId == orgId).ToList();

      return View(appraisal);


        }


        public IActionResult ViewAppraisalDetails(Guid id)
        {
      var orgId = getOrg();

      var appEmpResult = _context.AppraisalEmployeeResults.Where(x => x.AppraisalId == id).Include(x => x.Appraisal).Include(x => x.EmployeeDetail).ToList();

      return View(appEmpResult);
        }


        public IActionResult CatgoryList()
        {
      var orgId = getOrg();

      var appCategory = _context.AppraisalCategories.Where(x => x.OrganisationId == orgId).ToList();

      return View(appCategory);
        }
    public IActionResult AddCategory()
    {
      var orgId = getOrg();


      return View();

    }
    [HttpPost]
    public IActionResult AddCategory(AppraisalCategory appraisalCategory)
    {
      var orgId = getOrg();

      appraisalCategory.OrganisationId = orgId;
      appraisalCategory.Id = Guid.NewGuid();


      try
      {
        _context.Add(appraisalCategory);
        _context.SaveChanges();
      }
      catch
      {

      }

      return RedirectToAction("CatgoryList");
    }



    public IActionResult EditCategory(Guid id)
    {
      var orgId = getOrg();

      AppraisalCategoryEdit apeVM = new AppraisalCategoryEdit();

      var category = _context.AppraisalCategories.Where(x => x.Id == id).FirstOrDefault();
      var kpi = _context.AppraisalKPIs.Where(x => x.AppraisalCategoryId == id).ToList();


      apeVM.AppraisalCategory = category;
      apeVM.AppraisalKPIs = kpi;

      return View(apeVM);

    }


    [HttpPost]
    public async Task<IActionResult> AddKPI([FromBody]AppraisalKPI kpi)
    {
      if (kpi == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      var orgId = getOrg();

      kpi.Id = Guid.NewGuid();
      kpi.OrganisationId = orgId;

      var kpis = _context.AppraisalKPIs.Where(x => x.AppraisalCategoryId == kpi.AppraisalCategoryId).ToList().Sum(x => x.Weight) + kpi.Weight;

      if (kpis > 100)
      {
        return Json(new
        {
          msg = "Exceed"
        });

      }

      try
      {
        _context.Add(kpi);
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
    public async Task<IActionResult> AddTemplate([FromBody]AppraisalTemplate temp)
    {
      if (temp == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      var orgId = getOrg();

      temp.Id = Guid.NewGuid();
      temp.OrganisationId = orgId;


      try
      {
        _context.Add(temp);
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


    public IActionResult VeiwCatgory()
        {
      var orgId = getOrg();

      return View();
        }
        public IActionResult Template()
        {
      var orgId = getOrg();

      List<TemplateListViewModel> tlVM = new List<TemplateListViewModel>();

      TemplateListViewModel temp;

      var temps = _context.AppraisalTemplates.Where(x => x.OrganisationId == orgId).ToList();

      foreach (var item in temps)
      {
        temp = new TemplateListViewModel();
        var appTemp = _context.AppraisalTemplateCategories.Where(x => x.AppraisalTemplateId == item.Id).ToList();

        temp.AppraisalTemplate = item;
        temp.NoOfCategory = appTemp.Count();
        temp.TotalWeight = appTemp.Sum(x => x.Weight);

        tlVM.Add(temp);
      }

      return View(tlVM);
        }

    [HttpPost]
    public async Task<IActionResult> AddAppTempCat([FromBody]AppraisalTemplateCategory appTemp)
    {
      if (appTemp == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      var appTemps = _context.AppraisalTemplateCategories.Where(x => x.AppraisalTemplateId == appTemp.AppraisalTemplateId).ToList().Sum(x => x.Weight) + appTemp.Weight;

      if (appTemps > 100)
      {
        return Json(new
        {
          msg = "Exceed"
        });

      }

      var orgId = getOrg();

      appTemp.Id = Guid.NewGuid();
      appTemp.OrganisationId = orgId;


      try
      {
        _context.Add(appTemp);
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

    

    public IActionResult AddTemplate()
    {
      var orgId = getOrg();

      return View();
    }

    public IActionResult EditTemplate(Guid id)
    {
      var orgId = getOrg();

      AppraisalTemplateEdit apeVM = new AppraisalTemplateEdit();

      var appTemp = _context.AppraisalTemplates.Where(x => x.Id == id).FirstOrDefault();
      var appTempCat = _context.AppraisalTemplateCategories.Where(x => x.AppraisalTemplateId == appTemp.Id).Include(x => x.AppraisalCategory).ToList();

      apeVM.AppraisalTemplate = appTemp;
      apeVM.Categories = appTempCat;
      ViewData["AppraisalCategory"] = new SelectList(_context.AppraisalCategories.Where(x => x.OrganisationId == orgId), "Id", "Category");



      return View(apeVM);
    }

    public IActionResult ViewTemplate(Guid id)
    {
      var orgId = getOrg();

      AppraisalViewTemplateViewModel avtVM = new AppraisalViewTemplateViewModel();

      List<AppCat> appCat = new List<AppCat>();
      List<AppraisalKPI> kPIs;
      AppCat sAppCat;

      var temp = _context.AppraisalTemplates.Where(x => x.Id == id).FirstOrDefault();
      var appCats = _context.AppraisalTemplateCategories.Where(x => x.AppraisalTemplateId == id).Include(x => x.AppraisalCategory).ToList();
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

        
        ace.AppraisalKPIs = kPIs;
        sAppCat.AppraisalCategoryEdit = ace;

        appCat.Add(sAppCat);
      }

      avtVM.AppCat = appCat;
      return View(avtVM);
    }


    public IActionResult StartAppraisal()
    {
      var orgId = getOrg();



      return View();
    }

    [HttpPost]
    public IActionResult StartAppraisal(Appraisal appraisal)
    {
      var orgId = getOrg();

      if (appraisal == null)
      {
        return View();
      }

      appraisal.Id = Guid.NewGuid();
      appraisal.OrganisationId = orgId;
      appraisal.IsActive = true;


      try
      {
        _context.Add(appraisal);
        _context.SaveChanges();

        return RedirectToAction("Index");

      }
      catch
      {

      }
      return View();

    }

    public IActionResult OngoingAppraisals()
    {
      var orgId = getOrg();

      var appraisal = _context.Appraisals.Where(x => x.OrganisationId == orgId).ToList();

      return View(appraisal);
    }

    public IActionResult OngoingAppraisal(Guid id)
    {
      var orgId = getOrg();
      OngoingAppraisalViewModel oAVM = new OngoingAppraisalViewModel();
      List<EmployeeAppraisal> allEmployeeApp = new List<EmployeeAppraisal>();
      List<EmployeeAppraisal> removeEmployee = new List<EmployeeAppraisal>();
      EmployeeAppraisal singleEmployeApp;


      oAVM.Appraisal = _context.Appraisals.Where(x => x.Id == id).FirstOrDefault();

      var allEmployee = _context.EmployeeDetails.Where(x => x.OrganisationId == orgId).ToList();
      var allSupervisor = _context.AssignedSupervisors.Where(x => x.OrganisationId == orgId).ToList();
      var allJobs = _context.Jobs.Where(x => x.OrganisationId == orgId).Include(c => c.Department).ToList();

      oAVM.AppraisalAssignedTemplates = _context.AppraisalAssignedTemplates.Where(x => x.OrganisationId == orgId).Where(x => x.AppraisalId == id).Include(x => x.AppraisalTemplate).Include(x => x.EmployeeDetail).ToList();

      foreach (var item in allEmployee)
      {
        singleEmployeApp = new EmployeeAppraisal();

        singleEmployeApp.EmployeeDetail = item;
        singleEmployeApp.Job = allJobs.Where(x => x.EmployeeDetailId == item.Id).FirstOrDefault();
        //singleEmployeApp.AssignedSupervisor = allEmployee.Where(x => x.Id == allSupervisor.Where(c => c.EmployeeDetailId == item.Id).FirstOrDefault().SupervisorId).FirstOrDefault();
        if (singleEmployeApp.Job != null)
        {
          if (oAVM.AppraisalAssignedTemplates.FindAll(x => x.EmployeeDetail == item).Count() < 1)
          {
            allEmployeeApp.Add(singleEmployeApp);

          }

        }



      }


      oAVM.EmployeeAppraisalList = allEmployeeApp.OrderBy(x => x.Job.Department.DepartmentName).ToList();

      oAVM.Jobs = allJobs;

      ViewData["Templates"] = new SelectList(_context.AppraisalTemplates.Where(x => x.OrganisationId == orgId), "Id", "Template");



      return View(oAVM);
    }

    [HttpPost]
    public IActionResult AssignTemp([FromBody]AppraisalAssignedTemplate assignTemp)
    {
      var orgId = getOrg();

      if (assignTemp == null)
      {
        return View();
      }

      assignTemp.Id = Guid.NewGuid();
      assignTemp.OrganisationId = orgId;
      //appraisal.IsActive = true;


      try
      {
        _context.Add(assignTemp);
        _context.SaveChanges();

        return Json(new
        {
          msg = "Success"
        });

      }
      catch
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