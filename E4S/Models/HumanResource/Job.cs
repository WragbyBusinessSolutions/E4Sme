using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.HumanResource
{
  public class Job : BaseClass
  {
    public Guid Id { get; set; }
    public Guid EmployeeDetailId { get; set; }
    public virtual EmployeeDetail EmployeeDetail { get; set; }

    public Guid JobTitleId { get; set; }
    public virtual JobTitle JobTitle { get; set; }

    public string JobSpecification { get; set; } 

    public Guid EmploymentStatusId { get; set; }
    //public virtual EmploymentStatus EmploymentStatus { get; set; }

    public Guid JobCategoryId { get; set; }
    //public virtual JobCategory JobCategory { get; set; }

    public DateTime JoinedDate { get; set; }

    public Guid BranchId { get; set; }
    //public virtual Branch Branch { get; set; }

    public Guid DepartmentId { get; set; }
    public virtual Department Department { get; set; }

    public string ContractDetail { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }

  }
}
