using E4S.Models;
using E4S.Models.HumanResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.ViewModel
{
  public class ReportViewModel
  {
    public EmployeeDetail EmployeeDetails { get; set; }
    public Job Job { get; set; }
    public Salary Salary { get; set; }
    public ContactDetail ContactDetail { get; set; }


  }
}
