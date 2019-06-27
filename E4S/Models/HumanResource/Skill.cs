using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.HumanResource
{
  public class Skill : BaseClass
  {
    public Guid Id { get; set; }
    public Guid EmployeeDetailId { get; set; }
    public virtual EmployeeDetail EmployeeDetail { get; set; }


    public string SkillName { get; set; }
    public string Description { get; set; }
    public string YearsOfExperience { get; set; }

  }
}
