using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.HumanResource
{
  public class Addition : BaseClass
  {
    public Guid Id { get; set; }
    public string AdditionType { get; set; }
    public string Description { get; set; }
    public string Others { get; set; }
    public bool Status { get; set; }

  }
}
