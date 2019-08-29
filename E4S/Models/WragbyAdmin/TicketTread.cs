using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.WragbyAdmin
{
  public class TicketTread : BaseClass
  {
    public Guid Id { get; set; }

    public Guid TicketId { get; set; }
    public virtual Ticket Ticket { get; set; }
    public string Thread { get; set; }
    public string ImageUrl { get; set; }
    public string ResponseType { get; set; }

  }
}
