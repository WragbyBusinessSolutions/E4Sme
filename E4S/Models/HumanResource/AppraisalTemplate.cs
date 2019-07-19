using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.HumanResource
{
  public class AppraisalTemplate : BaseClass
  {
    public Guid Id { get; set; }
    public string Template { get; set; }
  }
}
