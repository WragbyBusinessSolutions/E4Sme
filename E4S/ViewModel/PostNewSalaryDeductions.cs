using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.ViewModel
{
    public class PostNewSalaryDeductions
    {
        public string AId { get; set; }
        public Guid EmployeeDetailsId { get; set; }
        public Guid DeductionId { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public float Amount { get; set; }
    }
}
