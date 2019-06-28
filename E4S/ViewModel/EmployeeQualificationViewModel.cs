using E4S.Models.HumanResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.ViewModel
{
  public class EmployeeQualificationViewModel
  {
    public List<InstitutionQualification> InstitutionQualifications { get; set; }
    public List<Skill> Skills { get; set; }
    public List<WorkExperience> WorkExperiences { get; set; }
  }
}
