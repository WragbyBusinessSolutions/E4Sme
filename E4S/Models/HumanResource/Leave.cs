using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.HumanResource
{
  public class Leave : BaseClass
  {
    public Guid Id { get; set; }
    public Guid EmployeeDetailId { get; set; }
    public virtual EmployeeDetail EmployeeDetail { get; set; }

    public string LeaveTitle { get; set; }
    public string Description { get; set; }
    public string Comment { get; set; }


    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; }
    public DateTime ApproveDate { get; set; }

  }
}
