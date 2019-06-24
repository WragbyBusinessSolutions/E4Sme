using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.ViewModel
{
  public class PostJob
  {
    public Guid EmployeeId { get; set; }
    public Guid JobTitleId { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid EmploymentStatusId { get; set; }
    public Guid JobCategoryId { get; set; }
    public DateTime JoinDate { get; set; }
    public Guid BranchId { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string ContractDetails { get; set; }

  }
}
