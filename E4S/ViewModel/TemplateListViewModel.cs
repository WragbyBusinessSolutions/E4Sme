using E4S.Models.HumanResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.ViewModel
{
  public class TemplateListViewModel
  {
    public AppraisalTemplate AppraisalTemplate { get; set; }
    public int NoOfCategory { get; set; }
    public float TotalWeight { get; set; }
  }
}
