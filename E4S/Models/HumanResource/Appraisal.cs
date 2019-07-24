using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.HumanResource
{
  public class Appraisal : BaseClass
  {
    public Guid Id { get; set; }
    public string AppraisalTitle { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string Description { get; set; }
    public bool IsActive { get; set; }
  }
}
