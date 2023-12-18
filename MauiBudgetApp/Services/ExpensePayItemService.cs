using MauiBudgetApp.Data;
using MauiBudgetApp.Models;

namespace MauiBudgetApp.Services
{
    public class ExpensePayItemService : PayItemService
    {
        private List<ExpenseItem> expenseItems;
        public ExpensePayItemService()
        {
            this.expenseItems = Task.Run(() => this.GetExpenseItemsAsync()).Result;
        }

        public async Task<List<ExpenseItem>> GetExpenseItemsAsync()
        {
            var items = this.expenseItems ?? await this.Database.GetExpenseItemsAsync();
            return items;
        }

        public override async Task<decimal> GetTotalFor()
        {
            var items = this.expenseItems ?? await this.GetExpenseItemsAsync();
            var total = items.Sum(e => e.Amount);
            return total;
        }


        public async Task<decimal> GetLeftToPay()
        {
            var items = this.expenseItems ?? await this.GetExpenseItemsAsync();

            if (items.Count > 0)
            {
                var toPayAmount = await this.GetTotalFor();

                var paidAmount = items.Where(e => e.IsPaid)
                                      .Sum(a => a.Amount);

                return toPayAmount - paidAmount;
            }
            else
            {
                return 0;
            }
        }


        public async Task<bool> SaveItemAsync(ExpenseItem item)
        {
            if (item == null)
            {
                return false;
            }

            try
            {
                var res = await this.Database.SaveItemAsync(item) == 1;

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.CompletedTask;
                return false;
            }
        }
    }
}
