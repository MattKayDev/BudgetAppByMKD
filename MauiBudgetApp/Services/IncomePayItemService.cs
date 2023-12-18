using MauiBudgetApp.Data;
using MauiBudgetApp.Models;

namespace MauiBudgetApp.Services
{
    public class IncomePayItemService : PayItemService
    {
        private List<IncomeItem> incomeItems;
        public IncomePayItemService()
        {
            this.incomeItems = Task.Run(() => this.GetIncomeItemsAsync()).Result;
        }

        public async Task<List<IncomeItem>> GetIncomeItemsAsync()
        {
            var items = await this.Database.GetIncomeItemsAsync();
            return items;
        }

        public override async Task<decimal> GetTotalFor()
        {
            var items = incomeItems ?? await this.GetIncomeItemsAsync();
            var total = items.Sum(e => e.Amount);
            return total;
        }


        public async Task<bool> SaveItemAsync(IncomeItem item)
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
