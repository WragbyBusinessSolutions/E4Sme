using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.HumanResource
{
  public class KPIEmployeeResult : BaseClass
  {
    public Guid Id { get; set; }
    public Guid AppraisalId { get; set; }
    public Guid EmployeeDetailId { get; set; }
    public Guid AppraisalEmployeeResultId { get; set; }

    public Guid KPIId { get; set; }
    public string KPISelection { get; set; }

    public string Status { get; set; }
  }
}
