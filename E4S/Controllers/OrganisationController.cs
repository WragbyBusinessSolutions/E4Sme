using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Controllers
{
  public class OrganisationController : Controller
  {
    public OrganisationController()
    {

    }

    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Branch()
    {
      return View();
    }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult createbranch()
        {
            return View();
        }
    }
}
