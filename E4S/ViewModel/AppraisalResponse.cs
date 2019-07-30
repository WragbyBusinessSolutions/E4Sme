using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.ViewModel
{
  public class AppraisalResponse
  {
    public Guid AppraisalId { get; set; }
    public List<AppraisalResponseKPI> KPIs { get; set; }
    public string EmpReview { get; set; }
  }

  public class AppraisalResponseKPI
  {
    public Guid KPIId;
    public string KPIResult;
  }
}
