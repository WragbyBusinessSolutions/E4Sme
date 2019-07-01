using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.HumanResource
{
  public class Salary : BaseClass
  {
    public Guid Id { get; set; }
    public Guid EmployeeDetailId { get; set; }
    public virtual EmployeeDetail EmployeeDetail { get; set; }


    public string SalaryComponent { get; set; }
    public string PayFrequency { get; set; }
    public string Currency { get; set; }
    public float Amount { get; set; }
    public string Comment { get; set; }

    public bool IsActive { get; set; }

  }
}
