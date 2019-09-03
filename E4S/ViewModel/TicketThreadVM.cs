using E4S.Models.WragbyAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.ViewModel
{
  public class TicketThreadVM
  {
    public Guid Id { get; set; }
    public List <TicketTread> TicketTread { get; set; }
    public List <Ticket> Ticket { get; set; }
    public Guid TicketId { get; set; }
    public string Thread { get; set; }
   //public string ImageUrl { get; set; }
    public string ResponseType { get; set; }

  }
}
