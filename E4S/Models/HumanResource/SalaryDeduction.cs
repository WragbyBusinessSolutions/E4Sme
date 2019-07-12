using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.HumanResource
{
  public class SalaryDeduction : BaseClass
  {
    public Guid Id { get; set; }
    public Guid DeductionId { get; set; }
    public virtual Deduction Deduction { get; set; }

    public Guid EmployeeDetailId { get; set; }
    public virtual EmployeeDetail EmployeeDetail { get; set; }

    public string Description { get; set; }
    public DateTime Date { get; set; }

  }
}
