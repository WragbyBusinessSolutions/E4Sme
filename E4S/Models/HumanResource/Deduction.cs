using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.HumanResource
{
  public class Deduction : BaseClass
  {
    public Guid Id { get; set; }
    public string DeductionType { get; set; }
    public string Description { get; set; }
    public string Others { get; set; }
    public bool Status { get; set; }

  }
}
