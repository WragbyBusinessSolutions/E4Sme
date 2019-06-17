using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace E4S.Controllers.HumanResource
{
    public class JobsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    public IActionResult JobTitle()
    {
      return View();
    }

    public IActionResult EmploymentStatus()
    {
      return View();
    }
    public IActionResult JobCategory()
    {
      return View();
    }
    public IActionResult PayGrade()
    {
      return View();
    }

  }
}