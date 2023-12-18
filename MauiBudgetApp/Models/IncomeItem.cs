using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBudgetApp.Models
{
    public class IncomeItem : PayItem
    {
        public IncomeItem()
        {
        }

        string DatePaid { get; set; }
    }
}
