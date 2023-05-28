using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBudgetApp.Models
{
    public class PayItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public bool IsIncome { get; set; } = false;
        public bool IsExpense { get; set; } = true;
        public bool IsPaid { get; set; } = false;

        public bool IsVisible { get; set; } = false;
    }
}
