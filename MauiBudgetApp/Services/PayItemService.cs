using MauiBudgetApp.Data;
using MauiBudgetApp.Models;

namespace MauiBudgetApp.Services
{
    public class PayItemService
    {
        private PayItemItemDatabase database;

        public PayItemService()
        {
            this.database = Task.Run(async () => await PayItemItemDatabase.Instance).Result;
        }

        public PayItemItemDatabase Database => this.database;

        public virtual async Task<decimal> GetTotalFor() 
        {
            await Task.CompletedTask;
            return 0;
        }
    }
}
