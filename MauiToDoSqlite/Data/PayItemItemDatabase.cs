using MauiBudgetApp.Models;
using SQLite;

namespace MauiBudgetApp.Data
{
    public class PayItemItemDatabase
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<PayItemItemDatabase> Instance =
            new AsyncLazy<PayItemItemDatabase>(async () =>
            
            {
                var instance = new PayItemItemDatabase();
                try
                {
                    CreateTableResult expense = await Database.CreateTableAsync<PayItem>();
                }
                catch (Exception ex)
                {
                    // Need to log the error and tell user about it
                    Console.WriteLine(ex.Message);
                }
                
                return instance;
            });

        public PayItemItemDatabase()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        public Task<List<PayItem>> GetItemsAsync()
        {
            return Database.Table<PayItem>().ToListAsync();
        }

        public async Task<List<PayItem>> GetExpenseItems()
        {
            try
            {
                return await Database.Table<PayItem>().Where(i => i.IsExpense).ToListAsync();
            }
            catch (Exception)
            {
                return await ReturnEmptyList();
            }
        }

        public async Task<List<PayItem>> GetIncomeItems()
        {
            try
            {
                return await Database.Table<PayItem>().Where(i => i.IsIncome).ToListAsync();
            }
            catch (Exception)
            {

                return await ReturnEmptyList();
            }
        }

        public Task<PayItem> GetItemAsync(int id)
        {
            return Database.Table<PayItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(PayItem item)
        {
            if (item.ID != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                item.IsVisible = true;
                return Database.InsertAsync(item);
            }
        }

        public async Task<int> DeleteItemAsync(PayItem item)
        {
            try
            {
                return await Database.DeleteAsync(item);
            }
            catch (Exception ex)
            {
                await Task.CompletedTask;
                return 0;
            }

        }

        private async Task<List<PayItem>> ReturnEmptyList()
        {
            await Task.CompletedTask;
            List<PayItem> empty = new List<PayItem>();
            return empty;
        }
    }
}
