using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.WragbyAdmin
{
  public class Ticket : BaseClass
  {
    public Guid Id { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public int SupportId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public string Severity { get; set; }
    public string Status { get; set; }
    public string ImageUrl { get; set; }


    public virtual Organisation Organisation { get; set; }

  }
}
