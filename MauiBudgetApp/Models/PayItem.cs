using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBudgetApp.Models
{
    public class PayItem : IPayItem
    {
        private string amountText;
        private decimal amount;
        public PayItem() { }

        public PayItem(bool isExpense = true)
        {
            if (isExpense)
            {
                IsIncome = !isExpense;
                IsExpense = isExpense;
            }
            else
            {
                IsExpense = isExpense;
                IsIncome = !isExpense;
            }         
        }

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Amount 
        {
            get => amount;
            set
            {
                amountText = $"£{value}";
                amount = value;
            } 
        }

        public string AmountText => amountText;
        public bool IsIncome { get; set; } = false;
        public bool IsExpense { get; set; }
        public bool IsPaid { get; set; } = false;

        public bool IsVisible { get; set; } = false;
    }
}
