using E4S.Models.HumanResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.ViewModel
{
  public class AppraisalCategoryEdit
  {
    public AppraisalCategory AppraisalCategory { get; set; }
    public List<AppraisalKPI> AppraisalKPIs { get; set; }
  }
}
