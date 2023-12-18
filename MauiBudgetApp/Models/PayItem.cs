using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBudgetApp.Models
{
    public abstract class PayItem : IPayItem
    {
        private string amountText;
        private decimal amount;
        public PayItem() { }

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Amount 
        {
            get => amount;
            set
            {
                this.amountText = $"£{value}";
                amount = value;
            } 
        }

        public string AmountText => this.amountText;

        public bool IsVisible { get; set; } = false;
    }
}
