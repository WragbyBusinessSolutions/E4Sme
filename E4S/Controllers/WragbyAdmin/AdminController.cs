using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace E4S.Controllers.WragbyAdmin
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult CompanyListing()
        {
            return View();
        }

        public IActionResult UpdateOrganizationDetails()
        {
            return View();
        }

        public IActionResult Support()
        {
            return View();
        }

        public IActionResult SupportDetails()
        {
            return View();
        }

        public IActionResult License()
        {
            return View();
        }

        public IActionResult AddLicense()
        {
            return View();
        }

        public IActionResult ViewLicense()
        {
            return View();
        }
    }
}