using MauiBudgetApp.Data;
using MauiBudgetApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBudgetApp.Services
{
    public class PayItemService
    {
        List<PayItem> payItems = new();
        private int itemCount = 0;
        public int ItemCount
        {
            get => this.itemCount;
            set
            {
                if (this.itemCount == value)
                {
                    return;
                }

                var count = Task.Run(async () =>
                {
                    var items = await this.GetPayItemsAsync();
                    return items.Count;
                });

                this.itemCount = count.Result;
            }
        }

        public decimal GetTotalFor(List<PayItem> payItems)
        {
            if (payItems.Count > 0)
            {
                var total = payItems.Sum(i => i.Amount);
                return total;
            }
            else
            {
                return 0;
            }
        }

        public decimal GetLeftToPay(List<PayItem> payItems)
        {
            if (!payItems.All(i => i.IsExpense))
            {
                return 0;
            }

            var toPayAmount = this.GetTotalFor(payItems);
            var paidAmount = payItems.Where(i => i.IsPaid).ToList().Sum(a => a.Amount);

            return toPayAmount - paidAmount;
        }

        public async Task<List<PayItem>> GetExpenseItemsAsync()
        {
            var items = await this.GetPayItemsAsync();
            return items.Where<PayItem>(i => i.IsExpense).ToList();
        }

        public async Task<List<PayItem>> GetIncomeItemsAsync()
        {
            var items = await this.GetPayItemsAsync();
            return items.Where<PayItem>(i => i.IsIncome).ToList();
        }

        private async Task<List<PayItem>> GetPayItemsAsync()
        {
            PayItemItemDatabase database = await PayItemItemDatabase.Instance;
            this.payItems = await database.GetItemsAsync();
            return this.payItems;
        }
    }
}
