﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace E4S.Controllers.HumanResource
{
    public class PayrollController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    public IActionResult SalaryAdditions()
    {
      return View();
    }
    public IActionResult SalaryDeductions()
    {
      return View();
    }

  }
}