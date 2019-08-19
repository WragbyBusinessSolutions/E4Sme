using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.ViewModel.AccountVM
{
  public class DashboardACViewModel
  {
    public int Customers { get; set; }
    public float Sales { get; set; }
    public float Expenses { get; set; }

    public List<ExpenseBD> ExpenseChart { get; set; }
    public List<ProfitLossBD> ProfitLossChart { get; set; }
  }

  public class ExpenseBD
  {
    public string ExpenseName { get; set; }
    public float Amount { get; set; }
  }

  public class ProfitLossBD
  {
    public string Date { get; set; }
    public float Income { get; set; }
    public float Expenses { get; set; }
  }
}
