using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.ViewModel
{
  public class EmployeeListViewModel
  {
    public Guid Id { get; set; }
    public string EmployeeName { get; set; }
    public string Email { get; set; }

    public string JobTitle { get; set; }
    public string EmployeeStatus { get; set; }
    public string Department { get; set; }
    public string Supervisor { get; set; }

  }
}
