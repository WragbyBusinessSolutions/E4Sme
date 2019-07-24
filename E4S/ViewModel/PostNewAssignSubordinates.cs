using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.ViewModel
{
    public class PostNewAssignSubordinates
    {
        public string AId { get; set; }
        public Guid EmployeeDetailsId { get; set; }
        public string ReportMethod { get; set; }
        public bool Autolist { get; set; }
    }
}
