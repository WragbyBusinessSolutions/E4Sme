﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace E4S.Controllers.Employee
{
    public class EmployeeProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PersonalDetails()
        {
          return View();
        }

        public IActionResult ContactDetails()
        {
          return View();
        }

        public IActionResult EmergencyContacts()
        {
          return View();
        }

        public IActionResult Dependents()
        {
            return View();
        }

        public IActionResult jobs()
        {
            return View();
        }

        public IActionResult Salary()
        {
            return View();
        }

        public IActionResult Qualification()
        {
            return View();
        }

    public IActionResult EmployeeAssets()
    {
      return View();
    }

    public IActionResult Appraisal()
    {
      return View();
    }
  }
}