using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.ViewModel.AccountVM
{
  public class ProfitLossViewModel
  {
    public float TotalIncome { get; set; }
    public float CostOfGoods { get; set; }
    public float GrossProfit { get; set; }
    public float OperatingExpenses { get; set; }
    public float NetProfit { get; set; }
  }
}
