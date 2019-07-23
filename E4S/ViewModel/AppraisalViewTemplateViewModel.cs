using E4S.Models.HumanResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.ViewModel
{
  public class AppraisalViewTemplateViewModel
  {
    public Guid Id { get; set; }
    public string TemplateName { get; set; }
    public List<AppCat> AppCat { get; set; }
  }

  public class AppCat
  {
    public AppraisalCategoryEdit AppraisalCategoryEdit { get; set; }
    public AppraisalTemplateCategory AppraisalTemplateCategory { get; set; }
  }

}
