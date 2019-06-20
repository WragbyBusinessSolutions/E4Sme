using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models
{
  public class Branch : BaseClass
  {
    public Guid Id { get; set; }
    public string BranchName { get; set; }
    public string Comment { get; set; }
    public bool IsActive { get; set; }
  }
}
