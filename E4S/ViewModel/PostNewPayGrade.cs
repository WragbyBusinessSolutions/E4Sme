using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.ViewModel
{
    public class PostNewPayGrade
    {
        public string PayGradeName { get; set; }
        public string Currency { get; set; }
        public float MinimumSalary { get; set; }
        public float MaximumSalary { get; set; }
    }
}
