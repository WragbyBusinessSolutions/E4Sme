using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.HumanResource
{
  public class AppraisalKPI : BaseClass
  {
    public Guid Id { get; set; }
    public Guid AppraisalCategoryId { get; set; }
    
    public string KPI { get; set; }
    public float Weight { get; set; }
                                    
  }
}
