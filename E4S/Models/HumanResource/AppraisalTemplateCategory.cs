using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.HumanResource
{
  public class AppraisalTemplateCategory : BaseClass
  {
    public Guid Id { get; set; }
    public Guid AppraisalTemplateId { get; set; }
    public Guid AppraisalCategoryId { get; set; }
    public virtual AppraisalCategory AppraisalCategory { get; set; }
    public string AssignTo { get; set; }
    public float Weight { get; set; }

  }

}
