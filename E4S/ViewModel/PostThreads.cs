using E4S.Models.WragbyAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.ViewModel
{
    public class PostThreads
    {
        public Guid Id { get; set; }
       
        public string Thread { get; set; }

        public List<TicketTread> TicketTread { get; set; }
        public Ticket Ticket { get; set; }

    }
}
