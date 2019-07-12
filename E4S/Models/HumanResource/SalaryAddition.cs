using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.HumanResource
{
  public class SalaryAddition : BaseClass
  {
    public Guid Id { get; set; }
    public Guid AdditionId { get; set; }
    public virtual Addition Addition { get; set; }

    public Guid EmployeeDetailId { get; set; }
    public virtual EmployeeDetail EmployeeDetail { get; set; }

    public string Description { get; set; }
    public DateTime Date { get; set; }
  }
}
