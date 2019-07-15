using E4S.Models.HumanResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.ViewModel
{
  public class AdditionDeductionViewModel
  {
    public List<Addition> Additions { get; set; }

    public string AId { get; set; }
    public string AdditionName { get; set; }

    public string Description { get; set; }

    public List<Deduction> Deductions { get; set; }

    public string FId { get; set; }

    public string DeductionName { get; set; }

    public string DDescription { get; set; }

  }
}
