using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.HumanResource
{
  public class AppraisalEmployeeResult : BaseClass
  {
    public Guid Id { get; set; }
    public Guid AppraisalId { get; set; }
    public virtual Appraisal Appraisal { get; set; }

    public Guid EmployeeDetailId { get; set; }
    public virtual EmployeeDetail EmployeeDetail { get; set; }

    public float AppraisalScore { get; set; }
    public string EmployeePerformanceReview { get; set; }
    public DateTime DateSubmited { get; set; }
    public string Status { get; set; }
  }
}
