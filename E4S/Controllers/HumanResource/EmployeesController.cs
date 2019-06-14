using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace E4S.Controllers.HumanResource
{
    public class EmployeesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    
        public IActionResult AddEmployee()
        {
            return View();
        }

        public IActionResult EmployeeDetails()
        {
          return View();
        }
    }
}