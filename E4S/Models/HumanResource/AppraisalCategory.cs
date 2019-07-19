using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.HumanResource
{
  public class AppraisalCategory : BaseClass
  {
    public Guid Id { get; set; }
    public string Category { get; set; }
    public bool PreFilled { get; set; }
  }
}
