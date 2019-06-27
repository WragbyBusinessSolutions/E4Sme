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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E4S.Controllers.HumanResource
{
  public class CompanyAssetsController : Controller
  {

    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CompanyAssetsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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
      var ordId = getOrg();
      ViewData["EmployeeDetails"] = new SelectList(_context.EmployeeDetails.Where(x => x.OrganisationId == ordId), "Id", "LastName");
      List<OrganisationAsset> orgAss = new List<OrganisationAsset>();

      var orgAssetList = _context.OrganisationAssets.Where(x => x.OrganisationId == ordId).ToList();

      foreach (var item in orgAssetList)
      {
        var employee = _context.EmployeeDetails.Where(x => x.Id == item.EmployeeDetailId).FirstOrDefault();

        if (employee != null)
        {
          item.EmployeeName = employee.FirstName + " " + employee.LastName;
        }

        orgAss.Add(item);

      }

      return View(orgAss);
    }

    [HttpPost]
    public async Task<IActionResult> postCompanyAssets([FromBody]PostOrganisationAsset postOrganisationAsset)
    {
      if (postOrganisationAsset == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      var orgId = getOrg();
      bool isAssign = true;

      if (postOrganisationAsset.EmployeeDetailId == Guid.Empty)
      {
        isAssign = false;
      }

      try
      {
        OrganisationAsset orgAsset = new OrganisationAsset()
        {
          Id = Guid.NewGuid(),
          DeviceName = postOrganisationAsset.DeviceName,
          Brand = postOrganisationAsset.Brand,
          Model = postOrganisationAsset.Model,
          SerialNumber = postOrganisationAsset.SerialNumber,
          OrganisationId = orgId,
          IsAssigned = isAssign,
          EmployeeDetailId = postOrganisationAsset.EmployeeDetailId,

        };

        _context.Add(orgAsset);
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
    public async Task<IActionResult> editCompanyAssets([FromBody]PostOrganisationAsset postOrganisationAsset)
    {
      if (postOrganisationAsset == null)
      {
        return Json(new
        {
          msg = "No Data"
        }
       );
      }

      var orgId = getOrg();
      bool isAssign = true;

      if (postOrganisationAsset.EmployeeDetailId == Guid.Empty)
      {
        isAssign = false;
      }

      try
      {

        var orgAsset = _context.OrganisationAssets.Where(x => x.Id == Guid.Parse(postOrganisationAsset.AId)).FirstOrDefault();
        orgAsset.DeviceName = postOrganisationAsset.DeviceName;
        orgAsset.Brand = postOrganisationAsset.Brand;
        orgAsset.Model = postOrganisationAsset.Model;
        orgAsset.SerialNumber = postOrganisationAsset.SerialNumber;



        orgAsset.EmployeeDetailId = postOrganisationAsset.EmployeeDetailId;

        _context.Update(orgAsset);
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