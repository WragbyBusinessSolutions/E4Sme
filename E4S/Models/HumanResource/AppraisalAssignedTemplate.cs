using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.HumanResource
{
  public class AppraisalAssignedTemplate : BaseClass
  {
    public Guid Id { get; set; }

    public Guid AppraisalId { get; set; }
    public Guid EmployeeDetailId { get; set; }
    public virtual EmployeeDetail EmployeeDetail { get; set; }

    public Guid AppraisalTemplateId { get; set; }
    public virtual AppraisalTemplate AppraisalTemplate { get; set; }
  }
}
